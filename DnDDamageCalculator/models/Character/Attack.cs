using DnDDamageCalculator.Models.Combat;
using DnDDamageCalculator.Utils;

namespace DnDDamageCalculator.Models.Character;

public record Attack(
    int AttackMod,
    IEnumerable<Dice> WeaponDamage,
    int DamageMod,
    int CritRange,
    double AttackPerc = 1,
    IWeaponMastery? Mastery = null
)
{
    public AttackResult[] GenerateAttackResults(CombatConfiguration combatConfiguration, Feat[] feats)
    {
        var necessaryRollToHit = combatConfiguration.targetAC - AttackMod;
        var necessaryRollToCrit = int.Max(CritRange, necessaryRollToHit);

        var d20Distribution = AttackHasAdvantage(combatConfiguration)
            ? DiceDistributions.D20WithAdvantage
            : DiceDistributions.D20;

        var percToMiss = 0.0;
        var percToHit = 0.0;
        var percToCrit = 0.0;

        foreach (var (value, probability) in d20Distribution)
        {
            if (value == 1)
            {
                percToMiss += probability;
                continue;
            }

            if (value == 20)
            {
                percToCrit += probability;
                continue;
            }

            if (value < necessaryRollToHit)
            {
                percToMiss += probability;
                continue;
            }

            if (value >= necessaryRollToHit && value < necessaryRollToCrit)
            {
                percToHit += probability;
                continue;
            }

            if (value >= necessaryRollToCrit)
            {
                percToCrit += probability;
                continue;
            }
        }

        AttackResult[] missResults =
        [
            new AttackResult([HitResult.Miss], [], CalculateMissDamage(Mastery), percToMiss * AttackPerc,
                combatConfiguration.effects with { Vexed = [false] }),
            new AttackResult([HitResult.NonAttack], [], CalculateMissDamage(Mastery), percToMiss * (1 - AttackPerc),
                combatConfiguration.effects with { Vexed = [false] })
        ];
        var hitResults =
            GenerateWeaponMasteryResults(
                    new AttackResult([HitResult.Hit], WeaponDamage, DamageMod, percToHit * AttackPerc,
                        new AttackEffects()),
                    Mastery).Concat(
                [
                    new AttackResult([HitResult.NonAttack], [], 0, percToHit * (1 - AttackPerc), new AttackEffects())
                ])
                .SelectMany(result => ProcessFeats(result, feats));
        var critResult =
            GenerateWeaponMasteryResults(
                    new AttackResult([HitResult.CriticalHit], WeaponDamage.Concat(WeaponDamage), DamageMod,
                        percToCrit * AttackPerc, new AttackEffects()), Mastery).Concat(
                [
                    new AttackResult([HitResult.NonAttack], [], 0,
                        percToCrit * (1 - AttackPerc), new AttackEffects())
                ])
                .SelectMany(result => ProcessFeats(result, feats));

        return critResult.Concat(hitResults).Concat(missResults).ToArray();
    }

    private static bool AttackHasAdvantage(CombatConfiguration configuration)
    {
        return configuration.hasAdvantageOnAttacks
               || configuration.effects.Toppled.LastOrDefault()
               || configuration.effects.Vexed.LastOrDefault();
    }

    private static int CalculateMissDamage(IWeaponMastery? mastery)
    {
        if (mastery is Graze(var grazeDamage))
        {
            return grazeDamage;
        }

        return 0;
    }

    private static AttackResult[] GenerateWeaponMasteryResults(AttackResult result, IWeaponMastery? mastery)
    {
        if (mastery is null) return [result];
        return mastery switch
        {
            Topple { SuccessPerc: var topplePerc } =>
            [
                result with
                {
                    Probability = result.Probability * topplePerc,
                    AttackEffects = result.AttackEffects with { Toppled = [true] }
                },
                result with
                {
                    Probability = result.Probability * (1 - topplePerc)
                }
            ],
            Vex =>
            [
                result with
                {
                    Probability = result.Probability,
                    AttackEffects = result.AttackEffects with { Vexed = [true] }
                }
            ],
            _ => [result]
        };
    }

    private static AttackResult[] ProcessFeats(AttackResult result, Feat[] feats)
    {
        if (feats.Length == 0) return [result];
        return feats.SelectMany<Feat, AttackResult>(feat => feat switch
        {
            ShieldMaster { TopplePerc: var perc } when !result.AttackEffects.ShieldMasterUsed &&
                                                       !result.AttackEffects.EnemyIsCurrentlyToppled() =>
            [
                result with
                {
                    Probability = result.Probability * perc,
                    AttackEffects = result.AttackEffects with
                    {
                        Toppled = result.AttackEffects.Toppled.Concat([true]),
                        ShieldMasterUsed = true
                    }
                },
                result with
                {
                    Probability = result.Probability * (1 - perc),
                    AttackEffects = result.AttackEffects with
                    {
                        Toppled = result.AttackEffects.Toppled.Concat([false]),
                        ShieldMasterUsed = true
                    }
                }
            ],
            _ => [result]
        }).ToArray();
    }
}

public interface IWeaponMastery;

public record Topple(double SuccessPerc) : IWeaponMastery;

public record Graze(int GrazeDamage) : IWeaponMastery;

public record Vex : IWeaponMastery;