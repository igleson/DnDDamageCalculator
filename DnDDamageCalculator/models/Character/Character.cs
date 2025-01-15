using DnDDamageCalculator.Models.Combat;

namespace DnDDamageCalculator.Models.Character;

public struct Character(IEnumerable<CharacterLevel> levels)
{
    public IEnumerable<(int level, IEnumerable<AttackResult> results)> GenerateResults(
        IEnumerable<CombatConfiguration> initialConfigurationByLevel)
    {
        var levelAndTarget = initialConfigurationByLevel.Zip(levels);
        return levelAndTarget
            .Select(tuple =>
            {
                var (combatConfiguration, level) = tuple;
                return (levelNumber: level.LevelNumber, level.GenerateResults(combatConfiguration));
            });
    }
}