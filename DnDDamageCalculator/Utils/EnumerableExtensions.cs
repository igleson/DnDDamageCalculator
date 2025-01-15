using DnDDamageCalculator.Models;

namespace DnDDamageCalculator.Utils;

public static class EnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
    {
        IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
        return sequences.Aggregate(
            emptyProduct,
            (accumulator, sequence) =>
                from accseq in accumulator
                from item in sequence
                select accseq.Concat(new[] { item })
        );
    }

    public static IEnumerable<AttackResult> AggregateSimilarResults(
        this IEnumerable<AttackResult> scenarios)
    {
        return scenarios
            .GroupBy(scenario => scenario.HitHistory)
            .Select(groupedScenarios =>
                new AttackResult(
                    groupedScenarios.Key,
                    groupedScenarios.SelectMany(scenario => scenario.DamageDices),
                    groupedScenarios.Select(scenario => scenario.DamageModifier).Sum(),
                    groupedScenarios.Sum(scenario => scenario.Probability)));
    }
}