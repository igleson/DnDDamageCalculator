using DnDDamageCalculator.Models.Combat;
using DnDDamageCalculator.Utils;

namespace DnDDamageCalculator.Models.Character;

public record CharacterLevel(int levelNumber, Attack[] attacks)
{
    public AttackResult[] GenerateResults(CombatConfiguration initialCombatConfiguration)
    {
        IEnumerable<(AttackResult previousResult, CombatConfiguration configuration)> currentScenarios =
            [(AttackResult.Initial, initialCombatConfiguration)];

        foreach (var attack in attacks)
        {
            var newScenariosPair =
                currentScenarios
                    .Select<(AttackResult previousScenario, CombatConfiguration configuration), (AttackResult
                        previousScenario,
                        IEnumerable<AttackResult> newResults
                        )>(pair => (pair.previousScenario, attack.GenerateAttackResults(pair.configuration)));

            currentScenarios = newScenariosPair.SelectMany(
                pair =>
                    pair.newResults.Select(newScenario =>
                        (AggregateConsecutiveAttacks(pair.previousScenario, newScenario),
                            initialCombatConfiguration with { effects = newScenario.EnemyEffects })
                    )
            );
        }

        return currentScenarios.Select(pair => pair.previousResult)
            .ToArray();
    }

    private static AttackResult AggregateConsecutiveAttacks(
        AttackResult scenario1,
        AttackResult scenario2)
    {
        return scenario2 with
        {
            HitHistory = scenario1.HitHistory.Concat(scenario2.HitHistory),
            DamageDices = scenario1.DamageDices.Concat(scenario2.DamageDices),
            DamageModifier = scenario1.DamageModifier + scenario2.DamageModifier,
            Probability = scenario1.Probability * scenario2.Probability,
            EnemyEffects = AggregateEffects(scenario1.EnemyEffects, scenario2.EnemyEffects)
        };
    }

    private static EnemyEffects AggregateEffects(EnemyEffects effects1, EnemyEffects effects2)
    {
        return new EnemyEffects
        {
            Toppled = effects1.Toppled.Concat(effects2.Toppled),
            Vexed = effects1.Vexed.Concat(effects2.Vexed)
        };
    }
}