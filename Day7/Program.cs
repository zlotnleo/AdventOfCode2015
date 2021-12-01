using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Day7
{
    class Program
    {
        private interface IOperand
        {
        }

        private struct ConstantOperand : IOperand
        {
            public ushort value;

            public ConstantOperand(ushort value)
            {
                this.value = value;
            }
        }

        private struct WireOperand : IOperand
        {
            public string wire;

            public WireOperand(string wire)
            {
                this.wire = wire;
            }
        }

        private interface IOperation
        {
        }

        private enum BinaryOperator
        {
            And,
            Or,
            RShift,
            LShift
        }

        private struct BinaryOperation : IOperation
        {
            public BinaryOperator op;
            public IOperand operand1;
            public IOperand operand2;
        }

        private enum UnaryOperator
        {
            Not
        }

        private struct UnaryOperation : IOperation
        {
            public UnaryOperator op;
            public IOperand operand;
        }

        private struct AssignmentOperation : IOperation
        {
            public IOperand operand;
        }

        private static IOperand GetOperand(string value) =>
            ushort.TryParse(value, out var numValue)
                ? new ConstantOperand(numValue)
                : new WireOperand(value);

        private static Dictionary<string, IOperation> ParseCircuit(IEnumerable<string> instructions)
        {
            var binaryOpRegex = new Regex("(\\d+|[a-z]+) ([A-Z]+) (\\d+|[a-z]+) -> ([a-z]+)", RegexOptions.Compiled);
            var unaryOpRegex = new Regex("([A-Z]+) (\\d+|[a-z]+) -> ([a-z]+)", RegexOptions.Compiled);
            var assignRegex = new Regex("(\\d+|[a-z]+) -> ([a-z]+)", RegexOptions.Compiled);

            var circuit = new Dictionary<string, IOperation>();
            foreach (var instruction in instructions)
            {
                var binaryMatch = binaryOpRegex.Match(instruction);
                if (binaryMatch.Success)
                {
                    circuit[binaryMatch.Groups[4].Value] = new BinaryOperation
                    {
                        operand1 = GetOperand(binaryMatch.Groups[1].Value),
                        operand2 = GetOperand(binaryMatch.Groups[3].Value),
                        op = binaryMatch.Groups[2].Value switch
                        {
                            "AND" => BinaryOperator.And,
                            "OR" => BinaryOperator.Or,
                            "RSHIFT" => BinaryOperator.RShift,
                            "LSHIFT" => BinaryOperator.LShift,
                            _ => throw new ArgumentOutOfRangeException(nameof(instructions))
                        }
                    };
                    continue;
                }

                var unaryMatch = unaryOpRegex.Match(instruction);
                if (unaryMatch.Success)
                {
                    circuit[unaryMatch.Groups[3].Value] = new UnaryOperation
                    {
                        operand = GetOperand(unaryMatch.Groups[2].Value),
                        op = unaryMatch.Groups[1].Value switch
                        {
                            "NOT" => UnaryOperator.Not,
                            _ => throw new ArgumentOutOfRangeException(nameof(instructions))
                        }
                    };
                    continue;
                }

                var assignMatch = assignRegex.Match(instruction);
                if (assignMatch.Success)
                {
                    circuit[assignMatch.Groups[2].Value] = new AssignmentOperation
                    {
                        operand = GetOperand(assignMatch.Groups[1].Value)
                    };
                }
            }

            return circuit;
        }

        private static ushort GetSignalOnA(Dictionary<string, IOperation> circuit)
        {
            var cache = new Dictionary<string, ushort>();

            ushort EvalOperand(IOperand operand) =>
                operand switch
                {
                    ConstantOperand constantOperand => constantOperand.value,
                    WireOperand wireOperand => Eval(wireOperand.wire)
                };

            ushort Eval(string wire)
            {
                if (cache.TryGetValue(wire, out var value))
                {
                    return value;
                }

                var computedValue = circuit[wire] switch
                {
                    BinaryOperation binaryOp => binaryOp.op switch
                    {
                        BinaryOperator.And => (ushort) (EvalOperand(binaryOp.operand1) & EvalOperand(binaryOp.operand2)),
                        BinaryOperator.Or => (ushort) (EvalOperand(binaryOp.operand1) | EvalOperand(binaryOp.operand2)),
                        BinaryOperator.RShift => (ushort) (EvalOperand(binaryOp.operand1) >> EvalOperand(binaryOp.operand2)),
                        BinaryOperator.LShift => (ushort) (EvalOperand(binaryOp.operand1) << EvalOperand(binaryOp.operand2)),
                    },
                    UnaryOperation unaryOp => unaryOp.op switch
                    {
                        UnaryOperator.Not => (ushort) ~EvalOperand(unaryOp.operand),
                    },
                    AssignmentOperation assignmentOp => EvalOperand(assignmentOp.operand)
                };

                cache[wire] = computedValue;
                return computedValue;
            }

            return Eval("a");
        }

        public static void Main()
        {
            var circuit = ParseCircuit(File.ReadAllLines("input.txt"));
            var signalOnA = GetSignalOnA(circuit);
            Console.WriteLine(signalOnA);

            circuit["b"] = new AssignmentOperation
            {
                operand = new ConstantOperand(signalOnA)
            };
            Console.WriteLine(GetSignalOnA(circuit));
        }
    }
}
