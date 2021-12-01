using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day24
{
    class Program
    {
        private static List<List<int>> GetSubsetsSubsetSum(IReadOnlyList<int> numbers, int goal, bool onlyShortestSubsets)
        {
            var arr = Enumerable.Range(0, numbers.Count).Select(_ =>
                Enumerable.Range(0, goal).Select(_ =>
                    new List<List<int>>()
                ).ToArray()
            ).ToArray();

            for (var t = 1; t <= goal; t++)
            {
                if (numbers[0] == t)
                {
                    arr[0][t - 1].Add(new List<int> {numbers[0]});
                }
            }

            for (var i = 1; i < numbers.Count; i++)
            {
                for (var t = 1; t <= goal; t++)
                {
                    var number = numbers[i];
                    var subsets = arr[i - 1][t - 1]
                        .Concat(number == t
                            ? new List<List<int>> {new() {number}}
                            : Enumerable.Empty<List<int>>()
                        )
                        .Concat(number < t
                            ? arr[i - 1][t - numbers[i] - 1].Select(l => l.Append(number).ToList())
                            : Enumerable.Empty<List<int>>()
                        )
                        .ToList();

                    if (!onlyShortestSubsets || subsets.Count == 0)
                    {
                        arr[i][t - 1] = subsets;
                    }
                    else
                    {
                        var minLength = subsets.Select(subset => subset.Count).Min();
                        arr[i][t - 1] = subsets.Where(subset => subset.Count == minLength).ToList();
                    }
                }
            }

            return arr[^1][^1];
        }

        private static bool CheckRemainingDivisibleIntoGroups(IReadOnlyList<int> remaining, int target, int groups)
        {
            if (groups == 1)
            {
                return remaining.Sum() == target;
            }

            var subsets = GetSubsetsSubsetSum(remaining, target, false);
            return subsets.Any(subset => CheckRemainingDivisibleIntoGroups(remaining.Except(subset).ToList(), target, groups - 1));
        }

        private static long Solve(IReadOnlyList<int> weights, int groups)
        {
            var target = weights.Sum() / groups;
            var shortestSubsets = GetSubsetsSubsetSum(weights, target, true);
            return shortestSubsets.Select(subset => (subset, entropy: subset.Aggregate(1L, (acc, x) => acc * x)))
                .OrderBy(t => t.entropy)
                .First(t => CheckRemainingDivisibleIntoGroups(weights.Except(t.subset).ToList(), target, groups - 1))
                .entropy;
        }

        public static void Main()
        {
            var weights = File.ReadAllLines("input.txt").Select(int.Parse).ToArray();
            Console.WriteLine(Solve(weights, 3));
            Console.WriteLine(Solve(weights, 4));
        }
    }
}
