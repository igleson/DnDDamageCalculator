namespace DnDDamageCalculator.Models.Combat;

public record CombatConfiguration(int TargetAc, AttackEffects Effects, bool HasAdvantageOnAttacks = false);