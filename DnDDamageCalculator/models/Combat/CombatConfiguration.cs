namespace DnDDamageCalculator.Models.Combat;

public record CombatConfiguration(int targetAC, EnemyEffects effects, bool hasAdvantageOnAttacks = false);