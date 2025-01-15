using DnDDamageCalculator.Models.Combat;
using DnDDamageCalculator.Utils;

namespace DnDDamageCalculator.Models.Character;

public record CharacterLevel(int levelNumber, Attack[] attacks)
{
    public AttackResult[] GenerateResults(CombatConfiguration combatConfiguration)
    {
        IEnumerable<AttackResult> currentScenarios = [AttackResult.Initial];

        foreach (var attack in attacks)
        {
            var newScenariosPair =
                currentScenarios
                    .Select<AttackResult, (AttackResult previousScenario,
                        IEnumerable<AttackResult> newScenarios
                        )>(previousScenarios =>
                    {
                        //Generate a new combatConfiguration based on the currentScenarios
                        return (previousScenarios, attack.GenerateAttackResults(combatConfiguration));
                    });

            currentScenarios = newScenariosPair.SelectMany(
                pair =>
                    pair.newScenarios.Select(newScenario =>
                        AggregateConsecutiveAttacks(pair.previousScenario, newScenario)
                    )
            );
        }

        return currentScenarios
            .ToArray();
    }

    private static AttackResult AggregateConsecutiveAttacks(
        AttackResult scenario1,
        AttackResult scenario2)
    {
        var newProbability = scenario1.Probability * scenario2.Probability;

        return new AttackResult(
            scenario1.HitHistory.Concat(scenario2.HitHistory),
            scenario1.DamageDices.Concat(scenario2.DamageDices),
            scenario1.DamageModifier + scenario2.DamageModifier,
            newProbability);
    }
}