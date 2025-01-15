using System.Text;
using DnDDamageCalculator.Utils;

namespace DnDDamageCalculator.Models;

public record AttackResult(IEnumerable<HitResult> HitHistory, IEnumerable<Dice> DamageDices, int DamageModifier, double Probability, AttackEffects AttackEffects)
{
    public static readonly AttackResult Initial = new([], Enumerable.Empty<Dice>(), 0, 1, new AttackEffects());

    public double AverageDamage()
    {
        var diceAverage = DamageDices.Select(dice =>
        {
            var a = dice.AverageDamage();
            return a;
        }).Sum();
        return (diceAverage +DamageModifier)*Probability;
    }
    
    protected virtual bool PrintMembers(StringBuilder stringBuilder)
    {
        // stringBuilder.Append($"HitHistory = [{string.Join(',', HitHistory.Select(hit => hit.ToString()))}], ");
        stringBuilder.Append($"Damage = {AverageDamage()}");
        // stringBuilder.Append($"EnemyEffects = {EnemyEffects}");
        return true;
    }
}

public enum HitResult
{
    Miss, Hit, CriticalHit, NonAttack
}

public record AttackEffects
{
    public IEnumerable<bool> Toppled { get; init; } = [false];

    public IEnumerable<bool> Vexed { get; init; } = [false];

    public bool ShieldMasterUsed { get; init; } = false;

    public bool EnemyIsCurrentlyToppled() => Toppled.LastOrDefault();
    
    protected virtual bool PrintMembers(StringBuilder stringBuilder)
    {
        stringBuilder.Append($"Toppled = [{string.Join(',', Toppled.Select(t => t.ToString()))}], ");
        stringBuilder.Append($"Vexed = [{string.Join(',', Vexed.Select(t => t.ToString()))}]");
        return true;
    }
}