using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent_of_Code.Day07
{
    /*
    --- Part Two ---
    It's no good - in this configuration, the amplifiers can't generate a large enough output signal to produce the thrust you'll need. The Elves quickly talk you through rewiring the amplifiers into a feedback loop:

          O-------O  O-------O  O-------O  O-------O  O-------O
    0 -+->| Amp A |->| Amp B |->| Amp C |->| Amp D |->| Amp E |-.
       |  O-------O  O-------O  O-------O  O-------O  O-------O |
       |                                                        |
       '--------------------------------------------------------+
                                                                |
                                                                v
                                                         (to thrusters)
    Most of the amplifiers are connected as they were before; amplifier A's output is connected to amplifier B's input, and so on. However, the output from amplifier E is now connected into amplifier A's input. This creates the feedback loop: the signal will be sent through the amplifiers many times.

    In feedback loop mode, the amplifiers need totally different phase settings: integers from 5 to 9, again each used exactly once. These settings will cause the Amplifier Controller Software to repeatedly take input and produce output many times before halting. Provide each amplifier its phase setting at its first input instruction; all further input/output instructions are for signals.

    Don't restart the Amplifier Controller Software on any amplifier during this process. Each one should continue receiving and sending signals until it halts.

    All signals sent or received in this process will be between pairs of amplifiers except the very first signal and the very last signal. To start the process, a 0 signal is sent to amplifier A's input exactly once.

    Eventually, the software on the amplifiers will halt after they have processed the final loop. When this happens, the last output signal from amplifier E is sent to the thrusters. Your job is to find the largest output signal that can be sent to the thrusters using the new phase settings and feedback loop arrangement.

    Here are some example programs:

    Max thruster signal 139629729 (from phase setting sequence 9,8,7,6,5):

    3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,
    27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5
    Max thruster signal 18216 (from phase setting sequence 9,7,8,5,6):

    3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,
    -5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,
    53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10
    Try every combination of the new phase settings on the amplifier feedback loop. What is the highest signal that can be sent to the thrusters?
    */
    class Day07Puzzle2 : IPuzzle
    {
        public Day07Puzzle2()
        {
            // Main
            IntCodeMemory = new int[] { 3, 8, 1001, 8, 10, 8, 105, 1, 0, 0, 21, 30, 47, 64, 81, 98, 179, 260, 341, 422, 99999, 3, 9, 1001, 9, 5, 9, 4, 9, 99, 3, 9, 1002, 9, 5, 9, 101, 4, 9, 9, 102, 2, 9, 9, 4, 9, 99, 3, 9, 102, 3, 9, 9, 101, 2, 9, 9, 1002, 9, 3, 9, 4, 9, 99, 3, 9, 1001, 9, 5, 9, 1002, 9, 3, 9, 1001, 9, 3, 9, 4, 9, 99, 3, 9, 1002, 9, 3, 9, 101, 2, 9, 9, 102, 5, 9, 9, 4, 9, 99, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 99, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 99, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 99, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 99, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 99 };

            // P2 Test 1
            IntCodeMemory = new int[] { 3, 26, 1001, 26, -4, 26, 3, 27, 1002, 27, 2, 27, 1, 27, 26, 27, 4, 27, 1001, 28, -1, 28, 1005, 28, 6, 99, 0, 0, 5 };
            PhaseSetting = new int[] { 9, 8, 7, 6, 5 };
            ExpectedResult = 139629729;
        }

        readonly IReadOnlyList<int> IntCodeMemory = new int[] { };
        readonly IReadOnlyList<int> PhaseSetting = new int[] { };
        readonly int ExpectedResult = 0;


        public void Run()
        {
            Console.WriteLine("--- Day 7: Amplification Circuit (Part 2) ---");

            var maxThrust = 0;
            var phaseWithMax = default(int[]);

            foreach (var phase in GetAllPhaseSettings(5, 6, 7, 8, 9))
            {
                var foundThrust = RunPhase(phase);

                if (foundThrust > maxThrust)
                {
                    maxThrust = foundThrust;
                    phaseWithMax = phase;
                }
            }

            Console.WriteLine($"Phase Sequence: [{string.Join(',', phaseWithMax)}]   Max Thrust: {maxThrust}");

            var expectedResult = (ExpectedResult > 0) ? ExpectedResult : 45730;
            Console.WriteLine("    " + (expectedResult == maxThrust ? "CORRECT" : "You done it wrong!"));
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

        int RunPhase(int[] phase)
        {
            var currentSignal = 0;

            var amp1 = new IntCodeV2(IntCodeMemory.ToArray());
            amp1.Input += (s, e) => { };
            amp1.Output += (s, e) => { };

            var amp2 = new IntCodeV2(IntCodeMemory.ToArray());
            amp2.Input += (s, e) => { };
            amp2.Output += (s, e) => { };

            var amp3 = new IntCodeV2(IntCodeMemory.ToArray());
            amp3.Input += (s, e) => { };
            amp3.Output += (s, e) => { };

            var amp4 = new IntCodeV2(IntCodeMemory.ToArray());
            amp4.Input += (s, e) => { };
            amp4.Output += (s, e) => { };

            var amp5 = new IntCodeV2(IntCodeMemory.ToArray());
            amp5.Input += (s, e) => { };
            amp5.Output += (s, e) => { };

            amp1.Run();
            //amp1.Run(phase[0], amp5.OutputValues.FirstOrDefault());
            //amp2.Run(phase[1], amp1.OutputValues.FirstOrDefault());
            //amp3.Run(phase[2], amp2.OutputValues.FirstOrDefault());
            //amp4.Run(phase[3], amp3.OutputValues.FirstOrDefault());
            //amp5.Run(phase[4], amp4.OutputValues.FirstOrDefault());

            //var value2 = amp5.OutputValues.FirstOrDefault();


            return currentSignal;
        }

    }
}
