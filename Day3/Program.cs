using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day3
{
    class Program
    {
        private static void VisitHouses(IEnumerable<char> instructions, ISet<(int, int)> visited)
        {
            var coords = (x: 0, y: 0);
            visited.Add(coords);

            foreach (var instruction in instructions)
            {
                coords = instruction switch
                {
                    '^' => (coords.x, coords.y + 1),
                    'v' => (coords.x, coords.y - 1),
                    '>' => (coords.x + 1, coords.y),
                    '<' => (coords.x - 1, coords.y)
                };
                visited.Add(coords);
            }
        }

        private static int Part1(IEnumerable<char> instructions)
        {
            var houses = new HashSet<(int, int)>();
            VisitHouses(instructions, houses);
            return houses.Count;
        }

        private static int Part2(IList<char> instructions)
        {
            var instructionSets = new[]
            {
                new List<char>(),
                new List<char>()
            };
            for (var i = 0; i < instructions.Count; i++)
            {
                instructionSets[i % 2].Add(instructions[i]);
            }

            var houses = new HashSet<(int, int)>();
            VisitHouses(instructionSets[0], houses);
            VisitHouses(instructionSets[1], houses);
            return houses.Count;
        }

        public static void Main()
        {
            var validDirections = new[] {'^', 'v', '<', '>'};
            var instructions = File.ReadAllText("input.txt").Where(validDirections.Contains).ToArray();
            Console.WriteLine(Part1(instructions));
            Console.WriteLine(Part2(instructions));
        }
    }
}
