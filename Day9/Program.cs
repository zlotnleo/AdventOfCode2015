using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day9
{
    class Program
    {
        private static Dictionary<string, Dictionary<string, int>> ParseDistances(IEnumerable<string> lines)
        {
            var routeRegex = new Regex("(\\w+) to (\\w+) = (\\d+)", RegexOptions.Compiled);
            var graph = new Dictionary<string, Dictionary<string, int>>();

            Dictionary<string, int> GetDestinations(string from)
            {
                if (graph.TryGetValue(from, out var destList))
                {
                    return destList;
                }

                var destinations = new Dictionary<string, int>();
                graph[from] = destinations;
                return destinations;
            }

            foreach (var line in lines)
            {
                var match = routeRegex.Match(line);
                if (match.Success)
                {
                    var dist = int.Parse(match.Groups[3].Value);
                    GetDestinations(match.Groups[1].Value)[match.Groups[2].Value] = dist;
                    GetDestinations(match.Groups[2].Value)[match.Groups[1].Value] = dist;
                }
            }

            return graph;
        }

        private static ICollection<List<T>> GetPermutations<T>(ICollection<T> items) =>
            items.Count == 0
                ? new List<List<T>> {new()}
                : items.SelectMany((item, index) =>
                        GetPermutations(items.Take(index).Concat(items.Skip(index + 1)).ToList())
                            .Select(perm => perm.Prepend(item).ToList()))
                    .ToList();

        private static (int min, int max) Solve(Dictionary<string, Dictionary<string, int>> graph)
        {
            var permutations = GetPermutations(graph.Keys);
            var minimumDistance = int.MaxValue;
            var maximumDistance = 0;
            foreach (var order in permutations)
            {
                var distance = 0;
                var currentLocation = order[0];
                for(var i = 1; i < order.Count; i++)
                {
                    var nextLocation = order[i];
                    distance += graph[currentLocation][nextLocation];
                    currentLocation = nextLocation;
                }

                minimumDistance = Math.Min(minimumDistance, distance);
                maximumDistance = Math.Max(maximumDistance, distance);
            }

            return (minimumDistance, maximumDistance);
        }

        public static void Main()
        {
            var graph = ParseDistances(File.ReadAllLines("input.txt"));
            var (min, max) = Solve(graph);
            Console.WriteLine(min);
            Console.WriteLine(max);
        }
    }
}
