using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day16
{
    class Program
    {
        private static List<(int, Dictionary<string, int>)> ParseAunts(IEnumerable<string> lines)
        {
            var auntRegex = new Regex("Sue (\\d+): (?:(\\w+): (\\d+)(?:, )?)*", RegexOptions.Compiled);
            return lines.Select(line => auntRegex.Match(line))
                .Where(match => match.Success)
                .Select(match => (
                    int.Parse(match.Groups[1].Value),
                    match.Groups[2].Captures.Zip(match.Groups[3].Captures)
                        .ToDictionary(
                            zip => zip.First.Value,
                            zip => int.Parse(zip.Second.Value)
                        )
                ))
                .ToList();
        }

        private static int GetAuntNumber(
            IEnumerable<(int num, Dictionary<string, int> facts)> aunts,
            IEnumerable<(string property, Func<int, bool> predicate)> filters
        ) => filters.Aggregate(aunts, (currentAunts, currentFilter) =>
            currentAunts.Where(aunt =>
                !aunt.facts.TryGetValue(currentFilter.property, out var auntValue)
                || currentFilter.predicate(auntValue)
            )
        ).Single().num;

        private static int Part1(IEnumerable<(int, Dictionary<string, int>)> aunts)
        {
            var filters = new (string, Func<int, bool>)[]
            {
                ("children", v => v == 3),
                ("cats", v => v == 7),
                ("samoyeds", v => v == 2),
                ("pomeranians", v => v == 3),
                ("akitas", v => v == 0),
                ("vizslas", v => v == 0),
                ("goldfish", v => v == 5),
                ("trees", v => v == 3),
                ("cars", v => v == 2),
                ("perfumes", v => v == 1)
            };

            return GetAuntNumber(aunts, filters);
        }

        private static int Part2(IEnumerable<(int, Dictionary<string, int>)> aunts)
        {
            var filters = new (string, Func<int, bool>)[]
            {
                ("children", v => v == 3),
                ("cats", v => v > 7),
                ("samoyeds", v => v == 2),
                ("pomeranians", v => v < 3),
                ("akitas", v => v == 0),
                ("vizslas", v => v == 0),
                ("goldfish", v => v < 5),
                ("trees", v => v > 3),
                ("cars", v => v == 2),
                ("perfumes", v => v == 1)
            };

            return GetAuntNumber(aunts, filters);
        }

        public static void Main()
        {
            var aunts = ParseAunts(File.ReadAllLines("input.txt"));
            Console.WriteLine(Part1(aunts));
            Console.WriteLine(Part2(aunts));
        }
    }
}
