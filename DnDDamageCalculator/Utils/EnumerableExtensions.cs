using DnDDamageCalculator.Models;

namespace DnDDamageCalculator.Utils;

public static class EnumerableExtensions
{
    public static IEnumerable<AttackResult> AggregateSimilarResult(this IEnumerable<AttackResult> results)
    {
        var dict = new Dictionary<int, IList<AttackResult>>();

        foreach (var result in results)
        {
            var key = result.GetHashCode();
            if (dict.TryGetValue(key, out var currentList))
            {
                currentList.Add(result);
            }
            else
            {
                dict[key] = [result];
            }
        }

        return dict.Select(pair =>
            pair.Value.First() with { Probability = pair.Value.Select(result => result.Probability).Sum() });
    }

    public static IEnumerable<AttackResult> AggregateSimilarDamage(this IEnumerable<AttackResult> results)
    {
        var dict = new Dictionary<int, IList<AttackResult>>();

        foreach (var result in results)
        {
            var key = result.GetHashCodeForJustDamage();
            if (dict.TryGetValue(key, out var currentList))
            {
                currentList.Add(result);
            }
            else
            {
                dict[key] = [result];
            }
        }

        return dict.Select(pair =>
            pair.Value.First() with { Probability = pair.Value.Select(result => result.Probability).Sum() });
    }
}