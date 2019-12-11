using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent_of_Code.Day07
{
    /*
    --- Day 7: Amplification Circuit ---
    Based on the navigational maps, you're going to need to send more power to your ship's thrusters to reach Santa in time. To do this, you'll need to configure a series of amplifiers already installed on the ship.

    There are five amplifiers connected in series; each one receives an input signal and produces an output signal. They are connected such that the first amplifier's output leads to the second amplifier's input, the second amplifier's output leads to the third amplifier's input, and so on. The first amplifier's input value is 0, and the last amplifier's output leads to your ship's thrusters.

        O-------O  O-------O  O-------O  O-------O  O-------O
    0 ->| Amp A |->| Amp B |->| Amp C |->| Amp D |->| Amp E |-> (to thrusters)
        O-------O  O-------O  O-------O  O-------O  O-------O
    The Elves have sent you some Amplifier Controller Software (your puzzle input), a program that should run on your existing Intcode computer. Each amplifier will need to run a copy of the program.

    When a copy of the program starts running on an amplifier, it will first use an input instruction to ask the amplifier for its current phase setting (an integer from 0 to 4). Each phase setting is used exactly once, but the Elves can't remember which amplifier needs which phase setting.

    The program will then call another input instruction to get the amplifier's input signal, compute the correct output signal, and supply it back to the amplifier with an output instruction. (If the amplifier has not yet received an input signal, it waits until one arrives.)

    Your job is to find the largest output signal that can be sent to the thrusters by trying every possible combination of phase settings on the amplifiers. Make sure that memory is not shared or reused between copies of the program.

    For example, suppose you want to try the phase setting sequence 3,1,2,4,0, which would mean setting amplifier A to phase setting 3, amplifier B to setting 1, C to 2, D to 4, and E to 0. Then, you could determine the output signal that gets sent from amplifier E to the thrusters with the following steps:

    Start the copy of the amplifier controller software that will run on amplifier A. At its first input instruction, provide it the amplifier's phase setting, 3. At its second input instruction, provide it the input signal, 0. After some calculations, it will use an output instruction to indicate the amplifier's output signal.
    Start the software for amplifier B. Provide it the phase setting (1) and then whatever output signal was produced from amplifier A. It will then produce a new output signal destined for amplifier C.
    Start the software for amplifier C, provide the phase setting (2) and the value from amplifier B, then collect its output signal.
    Run amplifier D's software, provide the phase setting (4) and input value, and collect its output signal.
    Run amplifier E's software, provide the phase setting (0) and input value, and collect its output signal.
    The final output signal from amplifier E would be sent to the thrusters. However, this phase setting sequence may not have been the best one; another sequence might have sent a higher signal to the thrusters.

    Here are some example programs:

    Max thruster signal 43210 (from phase setting sequence 4,3,2,1,0):

    3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0
    Max thruster signal 54321 (from phase setting sequence 0,1,2,3,4):

    3,23,3,24,1002,24,10,24,1002,23,-1,23,
    101,5,23,23,1,24,23,23,4,23,99,0,0
    Max thruster signal 65210 (from phase setting sequence 1,0,4,3,2):

    3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,
    1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0
    Try every combination of phase settings on the amplifiers. What is the highest signal that can be sent to the thrusters?
    */
    class Day07Puzzle1 : IPuzzle
    {
        public Day07Puzzle1()
        {
            // Main
            IntCodeMemory = new int[] { 3, 8, 1001, 8, 10, 8, 105, 1, 0, 0, 21, 30, 47, 64, 81, 98, 179, 260, 341, 422, 99999, 3, 9, 1001, 9, 5, 9, 4, 9, 99, 3, 9, 1002, 9, 5, 9, 101, 4, 9, 9, 102, 2, 9, 9, 4, 9, 99, 3, 9, 102, 3, 9, 9, 101, 2, 9, 9, 1002, 9, 3, 9, 4, 9, 99, 3, 9, 1001, 9, 5, 9, 1002, 9, 3, 9, 1001, 9, 3, 9, 4, 9, 99, 3, 9, 1002, 9, 3, 9, 101, 2, 9, 9, 102, 5, 9, 9, 4, 9, 99, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 99, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 99, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 99, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 99, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 99 };
            ExpectedResult = 45730;

            // Test 1
            //IntCodeMemory = new int[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 };
            //PhaseSetting = new int[] { 4, 3, 2, 1, 0 };
            //ExpectedResult = 43210;

            // Test 2
            //IntCodeMemory = new int[] { 3, 23, 3, 24, 1002, 24, 10, 24, 1002, 23, -1, 23, 101, 5, 23, 23, 1, 24, 23, 23, 4, 23, 99, 0, 0 };
            //PhaseSetting = new int[] { 0, 1, 2, 3, 4 };
            //ExpectedResult = 54321;

            // Test 3
            //IntCodeMemory = new int[] { 3, 31, 3, 32, 1002, 32, 10, 32, 1001, 31, -2, 31, 1007, 31, 0, 33, 1002, 33, 7, 33, 1, 33, 31, 31, 1, 32, 31, 31, 4, 31, 99, 0, 0, 0 };
            //PhaseSetting = new int[] { 1, 0, 4, 3, 2 };
            //ExpectedResult = 65210;
        }

        readonly IReadOnlyList<int> IntCodeMemory = new int[] { };
        readonly IReadOnlyList<int> PhaseSetting = new int[] { };
        readonly int ExpectedResult = 0;


        public void Run()
        {
            Console.WriteLine("--- Day 7: Amplification Circuit ---");

            var computer = new IntCode();
            var maxThrust = 0;
            var phaseWithMax = default(int[]);

            foreach (var phase in GetAllPhaseSettings(0, 1, 2, 3, 4))
            {
                var value2 = 0;
                foreach (var value1 in phase)
                {
                    computer.Init(IntCodeMemory.ToArray());
                    computer.Run(value1, value2);

                    value2 = computer.OutputValues.FirstOrDefault();
                }

                if (value2 > maxThrust)
                {
                    maxThrust = value2;
                    phaseWithMax = phase;
                }
            }

            Console.WriteLine($"Phase Sequence: [{string.Join(',', phaseWithMax)}]   Max Thrust: {maxThrust}");
            Console.WriteLine("    " + (ExpectedResult == maxThrust ? "CORRECT" : "You done it wrong!"));
            Console.WriteLine();
        }

        IEnumerable<int[]> GetAllPhaseSettings(params int[] sourceValues)
        {
            if (PhaseSetting.Count > 0)
            {
                yield return PhaseSetting.ToArray();
                yield break;
            }

            var result = Helper.GetPermutations(sourceValues);
            foreach (var item in result)
                yield return item.ToArray();
        }

    }
}
