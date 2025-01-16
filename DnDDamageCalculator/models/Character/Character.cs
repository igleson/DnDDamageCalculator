using DnDDamageCalculator.Models.Combat;
using DnDDamageCalculator.Utils;

namespace DnDDamageCalculator.Models.Character;

public readonly struct CharacterCombat(
    IEnumerable<(CharacterLevel level, CombatConfiguration combatConfiguration)> levels)
{
    public IEnumerable<(int level, IEnumerable<AttackResult> results)> GenerateResults()
    {
        return levels
            .Select(tuple => (tuple.level.LevelNumber,
                tuple.level.GenerateResults(tuple.combatConfiguration).AggregateSimilarResult()));
    }
}