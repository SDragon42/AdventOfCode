using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Common.Extensions;
using NUnit.Framework;

namespace AdventOfCode.CSharp.Year2015
{
    [TestFixture]
    internal class Day07 : TestBase
    {
        private const int DAY = 7;



        private class InputActions
        {
            public InputActions(string actions, string toWire)
            {
                Values = actions.Split(' ');
                Wire = toWire;
            }

            public string[] Values { get; private set; }
            public string Wire { get; private set; }
        }

        private (List<InputActions> input, int? expected) GetTestData(int part, string inputName)
        {
            var splitter1 = new string[] { " -> " };

            var input = Input.ReadLines(DAY, inputName)
                .Select(l => l.Split(splitter1, 2, StringSplitOptions.None))
                .Select(p => new InputActions(p[0], p[1]))
                .ToList();

            var expected = Input.ReadText(DAY, $"{inputName}-answer{part}")
                ?.ToInt32();

            return (input, expected);
        }



        [TestCase(1, "example1", "d", 72)]
        [TestCase(1, "example1", "e", 507)]
        [TestCase(1, "example1", "f", 492)]
        [TestCase(1, "example1", "g", 114)]
        [TestCase(1, "example1", "h", 65412)]
        [TestCase(1, "example1", "i", 65079)]
        [TestCase(1, "example1", "x", 123)]
        [TestCase(1, "example1", "y", 456)]
        public void Part1Examples(int part, string inputName, string signalWire, int expected)
        {
            var (input, _) = GetTestData(part, inputName);

            var result = GetSignalOnWire(input, signalWire);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }

        [TestCase(1, "input", "a")]
        public void Part1(int part, string inputName, string signalWire)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = GetSignalOnWire(input, signalWire);
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }



        [TestCase(2, "input", "a", 3176)]
        public void Part2(int part, string inputName, string signalWire, int bWireSignal)
        {
            var (input, expected) = GetTestData(part, inputName);

            var result = GetSignalOnWire(input, signalWire, ("b", (ushort)bWireSignal));
            Output($"Answer: {result}");
            Assert.AreEqual(expected, result);
        }



        private ushort GetSignalOnWire(List<InputActions> input, string signalWire, (string wire, ushort signal)? overrideSignal = null)
        {
            var circuit = new Circuit(input);
            //circuit.LogOutput += (s, text) => Output(text);

            if (overrideSignal.HasValue)
            {
                var (ovWire, ovSignal) = overrideSignal.Value;
                circuit.SetWireSignal(ovWire, ovSignal);
            }

            var result = circuit.GetWireSignal(signalWire);
            return result;
        }



        private class Circuit
        {
            private readonly Dictionary<string, Func<ushort>> _circuitDict = new Dictionary<string, Func<ushort>>();
            private readonly Dictionary<string, ushort> _knownValuesDict = new Dictionary<string, ushort>();


            public Circuit(IList<InputActions> input)
            {
                _circuitDict.Clear();
                _knownValuesDict.Clear();

                foreach (var inst in input)
                {
                    switch (inst.Values.Length)
                    {
                        case 1: // Set Signal on wire
                            _circuitDict[inst.Wire] = GetAction_Set(inst.Values[0]);
                            break;

                        case 2: // NOT operations
                            _circuitDict[inst.Wire] = GetAction_Not(inst.Values[1]);
                            break;

                        case 3: // Other operations
                            switch (inst.Values[1])
                            {
                                case "AND":
                                    _circuitDict[inst.Wire] = GetAction_And(inst.Values[0], inst.Values[2]);
                                    break;

                                case "OR":
                                    _circuitDict[inst.Wire] = GetAction_Or(inst.Values[0], inst.Values[2]);
                                    break;

                                case "LSHIFT":
                                    _circuitDict[inst.Wire] = GetAction_LeftShift(inst.Values[0], inst.Values[2]);
                                    break;

                                case "RSHIFT":
                                    _circuitDict[inst.Wire] = GetAction_RightShift(inst.Values[0], inst.Values[2]);
                                    break;

                                default:
                                    throw new Exception("Unrecognized command.");
                            }
                            break;

                        default: throw new Exception("Unhandled num actions.");
                    }
                }
            }


            public event EventHandler<string> LogOutput;


            private void Output(string text)
            {
                LogOutput?.Invoke(this, text);
            }



            public ushort GetWireSignal(string wire)
            {
                Output("  -> " + wire);

                if (_knownValuesDict.ContainsKey(wire))
                    return _knownValuesDict[wire];

                ushort value;

                if (ushort.TryParse(wire, out value))
                {
                    _knownValuesDict.Add(wire, value);
                    return value;
                }

                value = _circuitDict[wire]();
                _knownValuesDict.Add(wire, value);
                return value;
            }
            public void SetWireSignal(string wire, ushort signal)
            {
                _circuitDict[wire] = GetAction_Set(signal.ToString());
            }

            

            private Func<ushort> GetAction_Set(string source)
            {
                return () => GetWireSignal(source);
            }

            private Func<ushort> GetAction_Not(string source)
            {
                return () => (ushort)~GetWireSignal(source);
            }

            private Func<ushort> GetAction_And(string source1, string source2)
            {
                Func<ushort> part1 = () => GetWireSignal(source1);
                Func<ushort> part2 = () => GetWireSignal(source2);

                return () => (ushort)(part1() & part2());
            }

            private Func<ushort> GetAction_Or(string source1, string source2)
            {
                Func<ushort> part1 = () => GetWireSignal(source1);
                Func<ushort> part2 = () => GetWireSignal(source2);

                return () => (ushort)(part1() | part2());
            }

            private Func<ushort> GetAction_LeftShift(string source1, string source2)
            {
                Func<ushort> part1 = () => GetWireSignal(source1);
                Func<ushort> part2 = () => GetWireSignal(source2);

                return () => (ushort)(part1() << part2());
            }

            private Func<ushort> GetAction_RightShift(string source1, string source2)
            {
                Func<ushort> part1 = () => GetWireSignal(source1);
                Func<ushort> part2 = () => GetWireSignal(source2);

                return () => (ushort)(part1() >> part2());
            }
            
        }

    }
}
