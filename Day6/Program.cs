using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day6
{
    class Program
    {
        private enum InstructionType
        {
            On,
            Off,
            Toggle
        }

        private struct Instruction
        {
            public InstructionType type;
            public (int x, int y) corner1;
            public (int x, int y) corner2;
        }

        private static IEnumerable<Instruction> ParseInstructions(IEnumerable<string> instructions)
        {
            var regex = new Regex("(turn on|turn off|toggle) (\\d+),(\\d+) through (\\d+),(\\d+)", RegexOptions.Compiled);
            return instructions
                .Select(instruction => regex.Match(instruction))
                .Where(match => match.Success)
                .Select(match => new Instruction
                {
                    type = match.Groups[1].Value switch
                    {
                        "turn on" => InstructionType.On,
                        "turn off" => InstructionType.Off,
                        "toggle" => InstructionType.Toggle,
                        _ => throw new ArgumentOutOfRangeException(nameof(instructions), "invalid instruction")
                    },
                    corner1 = (int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value)),
                    corner2 = (int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value))
                });
        }

        private static void ApplyInstructions(IEnumerable<Instruction> instructions, Action<InstructionType, int, int> apply)
        {
            foreach (var instruction in instructions)
            {
                for (var y = instruction.corner1.y; y <= instruction.corner2.y; y++)
                {
                    for (var x = instruction.corner1.x; x <= instruction.corner2.x; x++)
                    {
                        apply(instruction.type, x, y);
                    }
                }
            }
        }

        private static int Part1(IEnumerable<Instruction> instructions)
        {
            var lights = Enumerable.Range(0, 1000).Select(_ =>
                Enumerable.Repeat(false, 1000).ToArray()
            ).ToArray();

            ApplyInstructions(instructions, (type, x, y) =>
            {
                lights[x][y] = type switch
                {
                    InstructionType.On => true,
                    InstructionType.Off => false,
                    InstructionType.Toggle => !lights[x][y]
                };
            });

            return lights.SelectMany(row => row).Count(light => light);
        }

        private static int Part2(IEnumerable<Instruction> instructions)
        {
            var lights = Enumerable.Range(0, 1000).Select(_ =>
                Enumerable.Repeat(0, 1000).ToArray()
            ).ToArray();

            ApplyInstructions(instructions, (type, x, y) =>
            {
                lights[x][y] = type switch
                {
                    InstructionType.On => lights[x][y] + 1,
                    InstructionType.Off => Math.Max(lights[x][y] - 1, 0),
                    InstructionType.Toggle => lights[x][y] + 2
                };
            });

            return lights.SelectMany(row => row).Sum();
        }

        public static void Main()
        {
            var instructions = ParseInstructions(File.ReadAllLines("input.txt")).ToList();
            Console.WriteLine(Part1(instructions));
            Console.WriteLine(Part2(instructions));
        }
    }
}
