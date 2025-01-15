namespace DnDDamageCalculator.Models.Combat;

public record CombatConfiguration(int targetAC, AttackEffects effects, bool hasAdvantageOnAttacks = false);