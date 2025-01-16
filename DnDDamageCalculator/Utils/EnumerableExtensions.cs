using DnDDamageCalculator.Models;

namespace DnDDamageCalculator.Utils;

public static class EnumerableExtensions
{
    public static IEnumerable<AttackResult> AggregateSimilar(this IEnumerable<AttackResult> results)
    {
        return results.GroupBy(result => result, AttackResult.SimilarResultComparer).Select(group =>
            group.First() with { Probability = group.Select(result => result.Probability).Sum() });
    }
}