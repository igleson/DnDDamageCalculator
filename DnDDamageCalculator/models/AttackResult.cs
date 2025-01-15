using System.Text;
using DnDDamageCalculator.Utils;

namespace DnDDamageCalculator.Models;

public record AttackResult(
    IEnumerable<HitResult> HitHistory,
    IEnumerable<Dice> DamageDices,
    int DamageModifier,
    double Probability,
    AttackEffects AttackEffects)
{
    public static readonly AttackResult Initial = new([], Enumerable.Empty<Dice>(), 0, 1, new AttackEffects());

    public bool LastAttackIsHit() =>
        HitHistory.Any() && (HitHistory.Last() == HitResult.Hit || HitHistory.Last() == HitResult.CriticalHit);

    public double AverageDamage()
    {
        var diceAverage = DamageDices.Select(dice =>
        {
            var a = dice.AverageDamage();
            return a;
        }).Sum();
        return (diceAverage + DamageModifier) * Probability;
    }

    protected virtual bool PrintMembers(StringBuilder stringBuilder)
    {
        // stringBuilder.Append($"HitHistory = [{string.Join(',', HitHistory.Select(hit => hit.ToString()))}], ");
        // stringBuilder.Append($"EnemyEffects = {EnemyEffects}");
        stringBuilder.Append($"Damage = {AverageDamage()}");
        return true;
    }
}

public enum HitResult
{
    Miss,
    Hit,
    CriticalHit,
    NonAttack
}

public record AttackEffects
{
    public IEnumerable<bool> Toppled { get; init; } = [false];

    public IEnumerable<bool> Vexed { get; init; } = [false];

    public IEnumerable<bool> ShieldMasterUsedHist { get; init; } = [];
    
    public IEnumerable<bool> HeroicWarriorUsedHist { get; init; } = [];

    public bool EnemyIsCurrentlyToppled() => Toppled.LastOrDefault();

    public bool HasShieldMasterBeenUsed() => ShieldMasterUsedHist.FirstOrDefault(used => used);
    
    public bool HasHeroicWarriorBeenUsed() => HeroicWarriorUsedHist.FirstOrDefault(used => used);

    protected virtual bool PrintMembers(StringBuilder stringBuilder)
    {
        stringBuilder.Append($"Toppled = [{string.Join(',', Toppled.Select(t => t.ToString()))}], ");
        stringBuilder.Append($"Vexed = [{string.Join(',', Vexed.Select(t => t.ToString()))}]");
        return true;
    }
}