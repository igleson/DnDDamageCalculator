using DnDDamageCalculator.Models.Combat;
using DnDDamageCalculator.Utils;

namespace DnDDamageCalculator.Models.Character;

public record Attack(int attackMod, IEnumerable<Dice> weaponDamage, int damageMod, int critRange)
{
    public AttackResult[] GenerateAttackResults(CombatConfiguration combatConfiguration)
    {
        var necessaryRollToHit = combatConfiguration.targetAC - attackMod;
        var necessaryRollToCrit = int.Max(critRange, necessaryRollToHit);

        var d20Distribution = DiceDistributions.D20;

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

        var missResult =
            new AttackResult([HitResult.Miss], [], 0, percToMiss);
        var hitResult =
            new AttackResult([HitResult.Hit], weaponDamage, damageMod, percToHit);
        var critResult =
            new AttackResult([HitResult.CriticalHit], weaponDamage.Concat(weaponDamage), damageMod, percToCrit);


        return [missResult, hitResult, critResult];
    }
}