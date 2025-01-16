using DnDDamageCalculator.Models;

namespace DnDDamageCalculator.Utils;

public static class EnumerableExtensions
{
    public static IEnumerable<AttackResult> AggregateSimilarResult(this IEnumerable<AttackResult> results)
    {
        return results.GroupBy(result => result, AttackResult.SameResultComparer).Select(group =>
            group.First() with { Probability = group.Select(result => result.Probability).Sum() });
    }
    
    public static IEnumerable<AttackResult> AggregateSimilarDamage(this IEnumerable<AttackResult> results)
    {
        return results.GroupBy(result => result, AttackResult.SameDamageComparer).Select(group =>
            group.First() with { Probability = group.Select(result => result.Probability).Sum() });
    }
}