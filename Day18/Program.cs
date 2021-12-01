using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day18
{
    class Program
    {
        private static bool[][] ParseInput(IEnumerable<string> lines) =>
            lines.Select(line =>
                line.Select(c => c switch
                {
                    '#' => true,
                    '.' => false
                }).ToArray()
            ).ToArray();

        private static int CountTurnedOnNeighbours(bool[][] lights, int y, int x)
        {
            var count = 0;
            for (var i = y - 1; i <= y + 1; i++)
            {
                for (var j = x - 1; j <= x + 1; j++)
                {
                    if ((i != y || j != x)
                        && i >= 0 && i < lights.Length
                        && j >= 0 && j < lights[i].Length)
                    {
                        count += lights[i][j] ? 1 : 0;
                    }
                }
            }

            return count;
        }

        private static bool[][] Iterate(bool[][] lights) =>
            Enumerable.Range(0, lights.Length).Select(i =>
                Enumerable.Range(0, lights[i].Length).Select(j => lights[i][j] switch
                {
                    true => CountTurnedOnNeighbours(lights, i, j) is 2 or 3,
                    false => CountTurnedOnNeighbours(lights, i, j) is 3
                }).ToArray()
            ).ToArray();

        private static int Part1(bool[][] lights)
        {
            for (var i = 0; i < 100; i++)
            {
                lights = Iterate(lights);
            }

            return lights.SelectMany(ls => ls).Count(l => l);
        }

        private static int Part2(bool[][] lights)
        {
            lights[0][0] = lights[0][^1] = lights[^1][0] = lights[^1][^1] = true;
            for (var i = 0; i < 100; i++)
            {
                lights = Iterate(lights);
                lights[0][0] = lights[0][^1] = lights[^1][0] = lights[^1][^1] = true;
            }

            return lights.SelectMany(ls => ls).Count(l => l);
        }

        public static void Main()
        {
            var lights = ParseInput(File.ReadAllLines("input.txt"));
            Console.WriteLine(Part1(lights));
            Console.WriteLine(Part2(lights));
        }
    }
}
