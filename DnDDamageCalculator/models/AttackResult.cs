using System.Text;
using DnDDamageCalculator.Utils;

namespace DnDDamageCalculator.Models;

public record AttackResult(
    HitResult LastWas,
    DamageDices DamageDices,
    int DamageModifier,
    double Probability,
    AttackEffects AttackEffects)
{
    public static readonly AttackResult Initial = new(HitResult.Miss, DamageDices.Empty, 0, 1, new AttackEffects());

    public readonly bool LastAttackIsHit =
        LastWas is HitResult.Hit or HitResult.CriticalHit;

    protected virtual bool PrintMembers(StringBuilder stringBuilder)
    {
        stringBuilder.Append($"LastWasHit = {LastAttackIsHit}, ");
        stringBuilder.Append($"Damage = {DamageDices}+{DamageModifier} , ");
        stringBuilder.Append($"AttackEffects = {AttackEffects}, ");
        stringBuilder.Append($"Probability = {Probability * 100}");
        return true;
    }

    public virtual bool Equals(AttackResult? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return LastAttackIsHit == other.LastAttackIsHit
               && DamageDices.Equals(other.DamageDices)
               && DamageModifier == other.DamageModifier
               && AttackEffects.Equals(other.AttackEffects);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(LastAttackIsHit, DamageDices, DamageModifier, AttackEffects);
    }

    public int GetHashCodeForJustDamage()
    {
        return HashCode.Combine(DamageDices, DamageModifier);
    }
}

public enum HitResult
{
    Miss,
    Hit,
    CriticalHit
}

public record AttackEffects
{
    public bool Toppled { get; init; }

    public bool Vexed { get; init; }

    public bool ShieldMasterUsed { get; init; }

    public bool HeroicWarriorUsed { get; init; }

    private int HashCode = -1;

    protected virtual bool PrintMembers(StringBuilder stringBuilder)
    {
        stringBuilder.Append($"Toppled = {string.Join(',', Toppled.ToString())}, ");
        stringBuilder.Append($"Vexed = {string.Join(',', Vexed.ToString())}, ");
        stringBuilder.Append($"ShieldMaster = {string.Join(',', ShieldMasterUsed.ToString())}, ");
        stringBuilder.Append($"HeroicWarrior = {string.Join(',', HeroicWarriorUsed.ToString())}");
        return true;
    }

    public virtual bool Equals(AttackEffects? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Toppled == other.Toppled
               && Vexed == other.Vexed
               && ShieldMasterUsed == other.ShieldMasterUsed
               && HeroicWarriorUsed == other.HeroicWarriorUsed;
    }

    public override int GetHashCode()
    {
        if (HashCode != -1) return HashCode;
        HashCode = 0;
        if (Toppled) HashCode |= 1 << 0;
        if (Vexed) HashCode |= 1 << 1;
        if (ShieldMasterUsed) HashCode |= 1 << 2;
        if (HeroicWarriorUsed) HashCode |= 1 << 3;

        return HashCode;
    }
}