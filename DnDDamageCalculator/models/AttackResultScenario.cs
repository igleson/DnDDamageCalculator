using DnDDamageCalculator.Utils;

namespace DnDDamageCalculator.Models;

public record AttackResult(IEnumerable<HitResult> HitHistory, IEnumerable<Dice> DamageDices, int DamageModifier, double Probability)
{
    public static readonly AttackResult Initial = new([], Enumerable.Empty<Dice>(), 0, 1);
}

public enum HitResult
{
    Miss, Hit, CriticalHit
}