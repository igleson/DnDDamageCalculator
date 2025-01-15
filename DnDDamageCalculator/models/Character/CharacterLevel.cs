using DnDDamageCalculator.Models.Combat;
using DnDDamageCalculator.Utils;

namespace DnDDamageCalculator.Models.Character;

public record CharacterLevel(int LevelNumber, IEnumerable<Attack> Attacks, IList<CharacterFeature> Features)
{
    public IEnumerable<AttackResult> GenerateResults(CombatConfiguration initialCombatConfiguration)
    {
        IEnumerable<(AttackResult previousResult, CombatConfiguration configuration)> currentScenarios =
            [(AttackResult.Initial, initialCombatConfiguration)];

        foreach (var attack in Attacks)
        {
            var newScenariosPair =
                currentScenarios
                    .Select<(AttackResult previousScenario, CombatConfiguration configuration), (AttackResult
                        previousScenario,
                        IEnumerable<AttackResult> newResults
                        )>(pair => (pair.previousScenario, attack.GenerateAttackResults(pair.configuration, Features)));

            currentScenarios = newScenariosPair.SelectMany(
                pair =>
                    pair.newResults.Select(newScenario =>
                        (AggregateConsecutiveAttacks(pair.previousScenario, newScenario),
                            initialCombatConfiguration with { Effects = newScenario.AttackEffects })
                    )
            );
        }

        return currentScenarios.Select(pair => pair.previousResult);
    }

    private static AttackResult AggregateConsecutiveAttacks(
        AttackResult scenario1,
        AttackResult scenario2)
    {
        return new AttackResult(
            HitHistory: scenario1.HitHistory.Concat(scenario2.HitHistory),
            DamageDices: scenario1.DamageDices.Concat(scenario2.DamageDices),
            DamageModifier: scenario1.DamageModifier + scenario2.DamageModifier,
            Probability: scenario1.Probability * scenario2.Probability,
            AttackEffects: AggregateEffects(scenario1.AttackEffects, scenario2.AttackEffects));
    }

    private static AttackEffects AggregateEffects(AttackEffects effects1, AttackEffects effects2)
    {
        return new AttackEffects
        {
            Toppled = effects1.Toppled.Concat(effects2.Toppled),
            Vexed = effects1.Vexed.Concat(effects2.Vexed)
        };
    }
}

public interface CharacterFeature;

public record ShieldMasterFeat(double TopplePerc) : CharacterFeature;

public record HeroicWarriorFeature : CharacterFeature;