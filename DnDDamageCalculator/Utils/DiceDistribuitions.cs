global using Dice = System.Collections.Generic.IEnumerable<(int value, double probability)>;

namespace DnDDamageCalculator.Utils;

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
        (1, 1.0/12),
        (2, 1.0/12),
        (3, 1.0/12),
        (4, 1.0/12),
        (5, 1.0/12),
        (6, 1.0/12),
        (7, 1.0/12),
        (8, 1.0/12),
        (9, 1.0/12),
        (10, 1.0/12),
        (11, 1.0/12),
        (12, 1.0/12)
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
        (1, 1.0/6),
        (2, 1.0/6),
        (3, 1.0/6),
        (4, 1.0/6),
        (5, 1.0/6),
        (6, 1.0/6)
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

        return string.Join('+', groupedBySides.OrderBy(group => group.Key).Select(group => $"{group.Count()}d{group.Key}"));
    }
}