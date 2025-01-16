using DnDDamageCalculator.Models.Combat;
using DnDDamageCalculator.Utils;

namespace DnDDamageCalculator.Models.Character;

public record Attack(
    int AttackMod,
    DamageDices WeaponDamage,
    int DamageMod,
    int CritRange,
    double AttackPerc = 1,
    IWeaponMastery? Mastery = null
)
{
    public IEnumerable<AttackResult> GenerateAttackResults(CombatConfiguration combatConfiguration,
        IList<CharacterFeature> features)
    {
        var (missResults, hitResults, critResult) = ProcessBasicAttack(combatConfiguration);

        return missResults.Concat(critResult).Concat(hitResults)
            .SelectMany(result => ProcessFeatures(result, features, combatConfiguration)).AggregateSimilar();
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
            new AttackResult(HitResult.Miss, DamageDices.Empty, CalculateMissDamage(Mastery), percToMiss * AttackPerc,
                combatConfiguration.Effects with { Vexed = false }),
            new AttackResult(HitResult.NonAttack, DamageDices.Empty, CalculateMissDamage(Mastery), percToMiss * (1 - AttackPerc),
                combatConfiguration.Effects with { Vexed = false })
        ];
        var hitResults = GenerateWeaponMasteryResults(
            new AttackResult(HitResult.Hit, WeaponDamage, DamageMod, percToHit * AttackPerc,
                new AttackEffects()),
            Mastery).Concat(
        [
            new AttackResult(HitResult.NonAttack, DamageDices.Empty, 0, percToHit * (1 - AttackPerc), new AttackEffects())
        ]);
        var critResults = GenerateWeaponMasteryResults(
            new AttackResult(HitResult.CriticalHit, WeaponDamage.Sum(WeaponDamage), DamageMod,
                percToCrit * AttackPerc, new AttackEffects()), Mastery).Concat(
        [
            new AttackResult(HitResult.NonAttack, DamageDices.Empty, 0,
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
               || configuration.Effects.Toppled
               || configuration.Effects.Vexed;
    }

    private static int CalculateMissDamage(IWeaponMastery? mastery)
    {
        if (mastery is Graze(var grazeDamage))
        {
            return grazeDamage;
        }

        return 0;
    }

    private static IEnumerable<AttackResult> GenerateWeaponMasteryResults(AttackResult result, IWeaponMastery? mastery)
    {
        if (mastery is null) return [result];
        return mastery switch
        {
            Topple { SuccessPerc: var topplePerc } =>
            [
                result with
                {
                    Probability = result.Probability * topplePerc,
                    AttackEffects = result.AttackEffects with { Toppled = true }
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
                    AttackEffects = result.AttackEffects with { Vexed = true }
                }
            ],
            _ => [result]
        };
    }

    private IEnumerable<AttackResult> ProcessFeature(AttackResult result, CharacterFeature feature,
        CombatConfiguration combatConfiguration)
    {
        return feature switch
        {
            ShieldMasterFeat { TopplePerc: var perc } => ProcessShieldMasterFeat(result, perc),
            HeroicWarriorFeature => ProcessHeroicWarriorFeature(result, combatConfiguration),
            StudiedAttacks => ProcessStudiedAttacksFeature(result),
            BoonOfCombatProwess => ProcessBoonOfCombatProwess(result),
            _ => [result]
        };
    }

    private IEnumerable<AttackResult> ProcessFeatures(AttackResult result, IList<CharacterFeature> features,
        CombatConfiguration combatConfiguration)
    {
        if (!features.Any()) return [result];

        return features.Aggregate<CharacterFeature, IEnumerable<AttackResult>>([result],
            (results, feature) => results.SelectMany(res => ProcessFeature(res, feature, combatConfiguration)));
    }

    private IEnumerable<AttackResult> ProcessBoonOfCombatProwess(AttackResult result)
    {
        if (result.LastAttackIsHit) return [result];

        return GenerateWeaponMasteryResults(
            result with
            {
                LastWas = HitResult.Hit,
                DamageDices = WeaponDamage,
                DamageModifier = DamageMod
            },
            Mastery);
    }

    private static IEnumerable<AttackResult> ProcessStudiedAttacksFeature(AttackResult result)
    {
        if (result.LastAttackIsHit || result.AttackEffects.Vexed) return [result];
        return
        [
            result with
            {
                AttackEffects = result.AttackEffects with { Vexed = true }
            }
        ];
    }

    private IEnumerable<AttackResult> ProcessHeroicWarriorFeature(AttackResult result,
        CombatConfiguration combatConfiguration)
    {
        var lastWasHit = result.LastAttackIsHit;
        var usedHeroicWarrior = result.AttackEffects.HeroicWarriorUsed;
        if (usedHeroicWarrior || lastWasHit)
        {
            return
            [
                result with
                {
                    AttackEffects = result.AttackEffects with
                    {
                        HeroicWarriorUsed = false
                    }
                }
            ];
        }

        var (miss, hit, crit) = ProcessBasicAttack(combatConfiguration);
        return miss.Concat(hit).Concat(crit).Select(attackResult => attackResult with
            {
                Probability = attackResult.Probability * result.Probability,
                AttackEffects = attackResult.AttackEffects with
                {
                    HeroicWarriorUsed = true,
                }
            }
        );
    }

    private static IEnumerable<AttackResult> ProcessShieldMasterFeat(AttackResult result, double perc)
    {
        if (!result.LastAttackIsHit ||
            result.AttackEffects.ShieldMasterUsed ||
            result.AttackEffects.Toppled)
        {
            return [result];
        }

        return
        [
            result with
            {
                Probability = result.Probability * perc,
                AttackEffects = result.AttackEffects with
                {
                    Toppled = true,
                    ShieldMasterUsed = true
                }
            },
            result with
            {
                Probability = result.Probability * (1 - perc),
                AttackEffects = result.AttackEffects with
                {
                    Toppled = false,
                    ShieldMasterUsed = true
                }
            }
        ];
    }
}

public interface IWeaponMastery;

public record Topple(double SuccessPerc) : IWeaponMastery;

public record Graze(int GrazeDamage) : IWeaponMastery;

public record Vex : IWeaponMastery;