using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day14
{
    class Program
    {
        private class Reindeer
        {
            public int speed;
            public int flyTime;
            public int restTime;

            public int GetDistanceAtTime(int t)
            {
                var fullCycles = t / (flyTime + restTime);
                var remainingTime = t % (flyTime + restTime);
                return fullCycles * speed * flyTime + speed * Math.Min(flyTime, remainingTime);
            }
        }

        private const int TotalTime = 2503;

        private static List<Reindeer> ParseReindeerStats(IEnumerable<string> lines)
        {
            var statsRegex = new Regex("\\w+ can fly (\\d+) km/s for (\\d+) seconds, but then must rest for (\\d+) seconds.", RegexOptions.Compiled);
            return lines.Select(line => statsRegex.Match(line))
                .Where(match => match.Success)
                .Select(match => new Reindeer
                {
                    speed = int.Parse(match.Groups[1].Value),
                    flyTime = int.Parse(match.Groups[2].Value),
                    restTime = int.Parse(match.Groups[3].Value)
                })
                .ToList();
        }

        private static int Part1(IEnumerable<Reindeer> reindeer) =>
            reindeer.Select(r => r.GetDistanceAtTime(TotalTime)).Max();

        private static int Part2(List<Reindeer> reindeer)
        {
            var scores = Enumerable.Repeat(0, reindeer.Count).ToArray();
            for (var time = 1; time <= TotalTime; time++)
            {
                var distances = reindeer.Select(r => r.GetDistanceAtTime(time)).ToList();
                var maxDistance = distances.Max();
                for (var i = 0; i < distances.Count; i++)
                {
                    if (distances[i] == maxDistance)
                    {
                        scores[i]++;
                    }
                }
            }

            return scores.Max();
        }

        public static void Main()
        {
            var reindeer = ParseReindeerStats(File.ReadAllLines("input.txt"));
            Console.WriteLine(Part1(reindeer));
            Console.WriteLine(Part2(reindeer));
        }
    }
}
