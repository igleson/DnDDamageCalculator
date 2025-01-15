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
    public AttackResult[] GenerateAttackResults(CombatConfiguration combatConfiguration, CharacterFeature[] features)
    {
        var (missResults, hitResults, critResult) = ProcessBasicAttack(combatConfiguration);

        return missResults.Concat(critResult).Concat(hitResults).SelectMany(result => ProcessFeatures(result, features))
            .ToArray();
    }

    private (IEnumerable<AttackResult> missResults, IEnumerable<AttackResult> hitResults, IEnumerable<AttackResult>
        critResults) ProcessBasicAttack(CombatConfiguration combatConfiguration)
    {
        var necessaryRollToHit = combatConfiguration.TargetAc - AttackMod;
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

        List<AttackResult> missResults =
        [
            new AttackResult([HitResult.Miss], [], CalculateMissDamage(Mastery), percToMiss * AttackPerc,
                combatConfiguration.Effects with { Vexed = [false] }),
            new AttackResult([HitResult.NonAttack], [], CalculateMissDamage(Mastery), percToMiss * (1 - AttackPerc),
                combatConfiguration.Effects with { Vexed = [false] })
        ];
        var hitResults = GenerateWeaponMasteryResults(
            new AttackResult([HitResult.Hit], WeaponDamage, DamageMod, percToHit * AttackPerc,
                new AttackEffects()),
            Mastery).Concat(
        [
            new AttackResult([HitResult.NonAttack], [], 0, percToHit * (1 - AttackPerc), new AttackEffects())
        ]);
        var critResults = GenerateWeaponMasteryResults(
            new AttackResult([HitResult.CriticalHit], WeaponDamage.Concat(WeaponDamage), DamageMod,
                percToCrit * AttackPerc, new AttackEffects()), Mastery).Concat(
        [
            new AttackResult([HitResult.NonAttack], [], 0,
                percToCrit * (1 - AttackPerc), new AttackEffects())
        ]);
        return (
            missResults.Where(result => result.Probability > 0),
            hitResults.Where(result => result.Probability > 0),
            critResults.Where(result => result.Probability > 0)
        );
    }

    private static bool AttackHasAdvantage(CombatConfiguration configuration)
    {
        return configuration.HasAdvantageOnAttacks
               || configuration.Effects.Toppled.LastOrDefault()
               || configuration.Effects.Vexed.LastOrDefault();
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

    private static IEnumerable<AttackResult> ProcessFeature(AttackResult result, CharacterFeature feature)
    {
        return feature switch
        {
            ShieldMasterFeat { TopplePerc: var perc } when result.LastAttackIsHit() &&
                                                           !result.AttackEffects.HasShieldMasterBeenUsed() &&
                                                           !result.AttackEffects.EnemyIsCurrentlyToppled() =>
                ProcessShieldMasterFeat(result, perc),
            _ => [result]
        };
    }

    private static IEnumerable<AttackResult> ProcessFeatures(AttackResult result, CharacterFeature[] features)
    {
        if (features.Length == 0) return [result];

        return features.Aggregate<CharacterFeature, IEnumerable<AttackResult>>([result],
            (results, feature) => results.SelectMany(res => ProcessFeature(res, feature)));
    }

    private static IEnumerable<AttackResult> ProcessShieldMasterFeat(AttackResult result, double perc)
    {
        return
        [
            result with
            {
                Probability = result.Probability * perc,
                AttackEffects = result.AttackEffects with
                {
                    Toppled = result.AttackEffects.Toppled.Concat([true]),
                    ShieldMasterUsedHist = result.AttackEffects.ShieldMasterUsedHist.Concat([true])
                }
            },
            result with
            {
                Probability = result.Probability * (1 - perc),
                AttackEffects = result.AttackEffects with
                {
                    Toppled = result.AttackEffects.Toppled.Concat([false]),
                    ShieldMasterUsedHist = result.AttackEffects.ShieldMasterUsedHist.Concat([true])
                }
            }
        ];
    }
}

public interface IWeaponMastery;

public record Topple(double SuccessPerc) : IWeaponMastery;

public record Graze(int GrazeDamage) : IWeaponMastery;

public record Vex : IWeaponMastery;