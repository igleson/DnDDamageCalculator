global using Dice = System.Collections.Generic.IEnumerable<(int value, double probability)>;
using System.Text;
using Microsoft.AspNetCore.Components.CompilerServices;

namespace DnDDamageCalculator.Utils;

public record DamageDices(IDictionary<int, int> Dies)
{
    public static readonly DamageDices Empty = new(new Dictionary<int, int>());

    public IDictionary<int, int> Dies { get; } = Dies;

    private int? HashCode = null;

    protected virtual bool PrintMembers(StringBuilder stringBuilder)
    {
        // stringBuilder.Append($"HitHistory = [{string.Join(',', HitHistory.Select(hit => hit.ToString()))}], ");
        // stringBuilder.Append($"EnemyEffects = {EnemyEffects}");
        stringBuilder.Append('[');
        stringBuilder.Append($"{string.Join('+', Dies.Select(die => $"{die.Value}d{die.Key}"))}");
        stringBuilder.Append(']');

        return true;
    }

    public DamageDices Sum(DamageDices otherDice)
    {

        var newDies = new Dictionary<int, int>(int.Max(Dies.Count, otherDice.Dies.Count));
        
        foreach (var die in Dies)
        {
            newDies[die.Key] = die.Value;
        }

        foreach (var die in otherDice.Dies)
        {
            if (newDies.TryGetValue(die.Key, out var currentQuantity))
            {
                newDies[die.Key] = die.Value + currentQuantity;
            }
            else
            {
                newDies[die.Key] = die.Value;
            }
        }

        return new DamageDices(newDies);
    }

    public virtual bool Equals(DamageDices? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        if (Dies.Count != other.Dies.Count) return false;

        foreach (var die in Dies)
        {
            var otherHasDie = other.Dies.TryGetValue(die.Key, out var quantity);
            if (!otherHasDie) return false;
            if (quantity != die.Value) return false;
        }
        
        return true;
    }

    public override int GetHashCode()
    {
        if (HashCode.HasValue) return HashCode.Value;
        var generator = new HashCode();
        foreach (var (quantity, sides) in Dies)
        {
            generator.Add(quantity);
            generator.Add(sides);
        }

        HashCode = generator.ToHashCode();
        return HashCode.Value;
    }
}

public static class DiceDistributions
{
    public static readonly Dice D20 =
    [
        (1, 0.05),
        (2, 0.05),
        (3, 0.05),
        (4, 0.05),
        (5, 0.05),
        (6, 0.05),
        (7, 0.05),
        (8, 0.05),
        (9, 0.05),
        (10, 0.05),
        (11, 0.05),
        (12, 0.05),
        (13, 0.05),
        (14, 0.05),
        (15, 0.05),
        (16, 0.05),
        (17, 0.05),
        (18, 0.05),
        (19, 0.05),
        (20, 0.05)
    ];

    public static readonly Dice D20WithAdvantage =
    [
        (1, 0.0025),
        (2, 0.0075),
        (3, 0.0125),
        (4, 0.0175),
        (5, 0.0225),
        (6, 0.0275),
        (7, 0.0325),
        (8, 0.0375),
        (9, 0.0425),
        (10, 0.0475),
        (11, 0.0525),
        (12, 0.0575),
        (13, 0.0625),
        (14, 0.0675),
        (15, 0.0725),
        (16, 0.0775),
        (17, 0.0825),
        (18, 0.0875),
        (19, 0.0925),
        (20, 0.0975)
    ];

    public static readonly Dice D12 =
    [
        (1, 1.0 / 12),
        (2, 1.0 / 12),
        (3, 1.0 / 12),
        (4, 1.0 / 12),
        (5, 1.0 / 12),
        (6, 1.0 / 12),
        (7, 1.0 / 12),
        (8, 1.0 / 12),
        (9, 1.0 / 12),
        (10, 1.0 / 12),
        (11, 1.0 / 12),
        (12, 1.0 / 12)
    ];

    public static readonly Dice D10 =
    [
        (1, 0.1),
        (2, 0.1),
        (3, 0.1),
        (4, 0.1),
        (5, 0.1),
        (6, 0.1),
        (7, 0.1),
        (8, 0.1),
        (9, 0.1),
        (10, 0.1),
    ];

    public static readonly Dice D8 =
    [
        (1, 0.125),
        (2, 0.125),
        (3, 0.125),
        (4, 0.125),
        (5, 0.125),
        (6, 0.125),
        (7, 0.125),
        (8, 0.125),
    ];

    public static readonly Dice D6 =
    [
        (1, 1.0 / 6),
        (2, 1.0 / 6),
        (3, 1.0 / 6),
        (4, 1.0 / 6),
        (5, 1.0 / 6),
        (6, 1.0 / 6)
    ];

    public static readonly Dice D4 =
    [
        (1, 0.25),
        (2, 0.25),
        (3, 0.25),
        (4, 0.25)
    ];

    public static double AverageDamage(this Dice die)
    {
        return die.Select(dice => dice.value * dice.probability).Sum();
    }

    public static string DiesString(this IEnumerable<Dice> dies)
    {
        var groupedBySides = dies.GroupBy(die => die.Count());

        return string.Join('+',
            groupedBySides.OrderBy(group => group.Key).Select(group => $"{group.Count()}d{group.Key}"));
    }
}