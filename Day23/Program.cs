using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day23
{
    class Program
    {
        private static List<(string, char?, int?)> ParseInstructions(IEnumerable<string> lines) =>
            lines.Select(line => line[..3] switch
            {
                {} op and ("hlf" or "tpl" or "inc") => (op, line[4] as char?, null as int?),
                {} op and "jmp" => (op, null, int.Parse(line[4..])),
                {} op and ("jie" or "jio") => (op, line[4], int.Parse(line[7..])),
            }).ToList();

        private static long Solve(List<(string, char?, int?)> instructions, int initA)
        {
            var reg = new Dictionary<char, long> {{'a', initA}, {'b', 0}};
            var pc = 0;
            while (pc >= 0 && pc < instructions.Count)
            {
                switch (instructions[pc])
                {
                    case ("hlf", {} r, null):
                        reg[r] /= 2;
                        pc++;
                        break;
                    case ("tpl", {} r, null):
                        reg[r] *= 3;
                        pc++;
                        break;
                    case ("inc", {} r, null):
                        reg[r]++;
                        pc++;
                        break;
                    case ("jmp", null, {} offset):
                        pc += offset;
                        break;
                    case ("jie", {} r, {} offset) when reg[r] % 2 == 0:
                        pc += offset;
                        break;
                    case ("jie", _, _):
                        pc++;
                        break;
                    case ("jio", {} r, {} offset) when reg[r] == 1:
                        pc += offset;
                        break;
                    case ("jio", _, _):
                        pc++;
                        break;
                    default:
                        throw new Exception($"Invalid instruction {instructions[pc]}");
                }
            }

            return reg['b'];
        }

        public static void Main()
        {
            var instructions = ParseInstructions(File.ReadAllLines("input.txt"));
            Console.WriteLine(Solve(instructions, 0));
            Console.WriteLine(Solve(instructions, 1));
        }
    }
}
