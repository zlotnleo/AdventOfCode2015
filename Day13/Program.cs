using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day13
{
    class Program
    {
        private static Dictionary<string, Dictionary<string, int>> GetHappinessScores(IEnumerable<string> guestList)
        {
            var lineRegex = new Regex("(\\w+) would (lose|gain) (\\d+) happiness units by sitting next to (\\w+).", RegexOptions.Compiled);
            var happinessScores = new Dictionary<string, Dictionary<string, int>>();
            foreach (var line in guestList)
            {
                var match = lineRegex.Match(line);
                if (match.Success)
                {
                    if(!happinessScores.TryGetValue(match.Groups[1].Value, out var scores))
                    {
                        scores = happinessScores[match.Groups[1].Value] = new Dictionary<string, int>();
                    }

                    scores[match.Groups[4].Value] = int.Parse(match.Groups[3].Value) * match.Groups[2].Value switch
                    {
                        "lose" => -1,
                        "gain" => 1,
                        _ => 0
                    };
                }
            }

            return happinessScores;
        }

        private static ICollection<List<T>> GetPermutations<T>(ICollection<T> items) =>
            items.Count == 0
                ? new List<List<T>> {new()}
                : items.SelectMany((item, index) =>
                        GetPermutations(items.Take(index).Concat(items.Skip(index + 1)).ToList())
                            .Select(perm => perm.Prepend(item).ToList()))
                    .ToList();

        private static int Solve(Dictionary<string, Dictionary<string, int>> happinessScores)
        {
            var maxScore = int.MinValue;
            var seatingArrangements = GetPermutations(happinessScores.Keys);
            foreach (var seating in seatingArrangements)
            {
                var totalScore = 0;
                for (var i = 0; i < seating.Count; i++)
                {
                    totalScore += happinessScores[seating[i]][seating[(i + 1) % seating.Count]];
                    totalScore += happinessScores[seating[(i + 1) % seating.Count]][seating[i]];
                }

                maxScore = Math.Max(maxScore, totalScore);
            }

            return maxScore;
        }

        private static void AddMyself(Dictionary<string, Dictionary<string, int>> happinessScores)
        {
            const string myName = "SomeoneElse";
            happinessScores[myName] = new Dictionary<string, int>();

            var existingPeople = happinessScores.Keys;
            foreach (var existingPerson in existingPeople)
            {
                happinessScores[existingPerson][myName] = 0;
                happinessScores[myName][existingPerson] = 0;
            }
        }

        public static void Main()
        {
            var happinessScores = GetHappinessScores(File.ReadAllLines("input.txt"));
            Console.WriteLine(Solve(happinessScores));
            AddMyself(happinessScores);
            Console.WriteLine(Solve(happinessScores));
        }
    }
}
