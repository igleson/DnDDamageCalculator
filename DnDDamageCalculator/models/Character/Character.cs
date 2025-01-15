using DnDDamageCalculator.Models.Combat;

namespace DnDDamageCalculator.Models.Character;

public struct Character(CharacterLevel[] levels)
{
    public (int level, AttackResult[])[] GenerateResults(CombatConfiguration[] initialConfigurationByLevel)
    {
        var levelAndTarget = initialConfigurationByLevel.Zip(levels);
        return levelAndTarget
            .Select(tuple =>
            {
                var (combatConfiguration, level) = tuple;
                return (levelNumber: level.LevelNumber, level.GenerateResults(combatConfiguration));
            }).ToArray();
    }
}