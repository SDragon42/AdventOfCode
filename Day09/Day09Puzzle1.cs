using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent_of_Code.Day09
{
    /*
    --- Day 9: Sensor Boost ---
    You've just said goodbye to the rebooted rover and left Mars when you receive a faint distress signal coming from the asteroid belt. It must be the Ceres monitoring station!

    In order to lock on to the signal, you'll need to boost your sensors. The Elves send up the latest BOOST program - Basic Operation Of System Test.

    While BOOST (your puzzle input) is capable of boosting your sensors, for tenuous safety reasons, it refuses to do so until the computer it runs on passes some checks to demonstrate it is a complete Intcode computer.

    Your existing Intcode computer is missing one key feature: it needs support for parameters in relative mode.

    Parameters in mode 2, relative mode, behave very similarly to parameters in position mode: the parameter is interpreted as a position. Like position mode, parameters in relative mode can be read from or written to.

    The important difference is that relative mode parameters don't count from address 0. Instead, they count from a value called the relative base. The relative base starts at 0.

    The address a relative mode parameter refers to is itself plus the current relative base. When the relative base is 0, relative mode parameters and position mode parameters with the same value refer to the same address.

    For example, given a relative base of 50, a relative mode parameter of -7 refers to memory address 50 + -7 = 43.

    The relative base is modified with the relative base offset instruction:

    Opcode 9 adjusts the relative base by the value of its only parameter. The relative base increases (or decreases, if the value is negative) by the value of the parameter.
    For example, if the relative base is 2000, then after the instruction 109,19, the relative base would be 2019. If the next instruction were 204,-34, then the value at address 1985 would be output.

    Your Intcode computer will also need a few other capabilities:

    The computer's available memory should be much larger than the initial program. Memory beyond the initial program starts with the value 0 and can be read or written like any other memory. (It is invalid to try to access memory at a negative address, though.)
    The computer should have support for large numbers. Some instructions near the beginning of the BOOST program will verify this capability.
    Here are some example programs that use these features:

    109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99 takes no input and produces a copy of itself as output.
    1102,34915192,34915192,7,4,7,99,0 should output a 16-digit number.
    104,1125899906842624,99 should output the large number in the middle.
    The BOOST program will ask for a single input; run it in test mode by providing it the value 1. It will perform a series of checks on each opcode, output any opcodes (and the associated parameter modes) that seem to be functioning incorrectly, and finally output a BOOST keycode.

    Once your Intcode computer is fully functional, the BOOST program should report no malfunctioning opcodes when run in test mode; it should only output a single value, the BOOST keycode. What BOOST keycode does it produce?
     */
    class Day09Puzzle1 : IPuzzle
    {
        public Day09Puzzle1()
        {
            // Live data
            IntCodeMemory = new long[] { 1102, 34463338, 34463338, 63, 1007, 63, 34463338, 63, 1005, 63, 53, 1102, 3, 1, 1000, 109, 988, 209, 12, 9, 1000, 209, 6, 209, 3, 203, 0, 1008, 1000, 1, 63, 1005, 63, 65, 1008, 1000, 2, 63, 1005, 63, 904, 1008, 1000, 0, 63, 1005, 63, 58, 4, 25, 104, 0, 99, 4, 0, 104, 0, 99, 4, 17, 104, 0, 99, 0, 0, 1101, 0, 34, 1006, 1101, 0, 689, 1022, 1102, 27, 1, 1018, 1102, 1, 38, 1010, 1102, 1, 31, 1012, 1101, 20, 0, 1015, 1102, 1, 791, 1026, 1102, 0, 1, 1020, 1101, 24, 0, 1000, 1101, 0, 682, 1023, 1101, 788, 0, 1027, 1101, 0, 37, 1005, 1102, 21, 1, 1011, 1102, 1, 28, 1002, 1101, 0, 529, 1024, 1101, 39, 0, 1017, 1102, 30, 1, 1013, 1101, 0, 23, 1003, 1102, 524, 1, 1025, 1101, 32, 0, 1007, 1102, 25, 1, 1008, 1101, 29, 0, 1001, 1101, 33, 0, 1016, 1101, 410, 0, 1029, 1101, 419, 0, 1028, 1101, 22, 0, 1014, 1102, 26, 1, 1019, 1102, 1, 35, 1009, 1102, 36, 1, 1004, 1102, 1, 1, 1021, 109, 11, 2107, 22, -8, 63, 1005, 63, 199, 4, 187, 1106, 0, 203, 1001, 64, 1, 64, 1002, 64, 2, 64, 109, 2, 21108, 40, 40, -2, 1005, 1011, 221, 4, 209, 1106, 0, 225, 1001, 64, 1, 64, 1002, 64, 2, 64, 109, 13, 21102, 41, 1, -7, 1008, 1019, 41, 63, 1005, 63, 251, 4, 231, 1001, 64, 1, 64, 1106, 0, 251, 1002, 64, 2, 64, 109, -19, 1202, 1, 1, 63, 1008, 63, 26, 63, 1005, 63, 271, 1105, 1, 277, 4, 257, 1001, 64, 1, 64, 1002, 64, 2, 64, 109, 7, 2101, 0, -6, 63, 1008, 63, 24, 63, 1005, 63, 297, 1106, 0, 303, 4, 283, 1001, 64, 1, 64, 1002, 64, 2, 64, 109, 7, 1205, -1, 315, 1105, 1, 321, 4, 309, 1001, 64, 1, 64, 1002, 64, 2, 64, 109, -11, 21107, 42, 41, 0, 1005, 1010, 341, 1001, 64, 1, 64, 1106, 0, 343, 4, 327, 1002, 64, 2, 64, 109, -8, 1207, 6, 24, 63, 1005, 63, 363, 1001, 64, 1, 64, 1106, 0, 365, 4, 349, 1002, 64, 2, 64, 109, 11, 1206, 8, 381, 1001, 64, 1, 64, 1106, 0, 383, 4, 371, 1002, 64, 2, 64, 109, 4, 1205, 4, 401, 4, 389, 1001, 64, 1, 64, 1105, 1, 401, 1002, 64, 2, 64, 109, 14, 2106, 0, -3, 4, 407, 1001, 64, 1, 64, 1106, 0, 419, 1002, 64, 2, 64, 109, -33, 1202, 3, 1, 63, 1008, 63, 29, 63, 1005, 63, 445, 4, 425, 1001, 64, 1, 64, 1105, 1, 445, 1002, 64, 2, 64, 109, -5, 2102, 1, 7, 63, 1008, 63, 25, 63, 1005, 63, 465, 1105, 1, 471, 4, 451, 1001, 64, 1, 64, 1002, 64, 2, 64, 109, 11, 21107, 43, 44, 7, 1005, 1011, 489, 4, 477, 1105, 1, 493, 1001, 64, 1, 64, 1002, 64, 2, 64, 109, -3, 1208, 8, 35, 63, 1005, 63, 511, 4, 499, 1105, 1, 515, 1001, 64, 1, 64, 1002, 64, 2, 64, 109, 25, 2105, 1, -2, 4, 521, 1106, 0, 533, 1001, 64, 1, 64, 1002, 64, 2, 64, 109, -8, 21108, 44, 47, -8, 1005, 1010, 549, 1106, 0, 555, 4, 539, 1001, 64, 1, 64, 1002, 64, 2, 64, 109, -19, 1207, 7, 35, 63, 1005, 63, 577, 4, 561, 1001, 64, 1, 64, 1106, 0, 577, 1002, 64, 2, 64, 109, 2, 2108, 32, 0, 63, 1005, 63, 597, 1001, 64, 1, 64, 1106, 0, 599, 4, 583, 1002, 64, 2, 64, 109, 13, 2101, 0, -7, 63, 1008, 63, 32, 63, 1005, 63, 625, 4, 605, 1001, 64, 1, 64, 1105, 1, 625, 1002, 64, 2, 64, 109, -13, 2107, 24, 2, 63, 1005, 63, 645, 1001, 64, 1, 64, 1106, 0, 647, 4, 631, 1002, 64, 2, 64, 109, 18, 21101, 45, 0, -4, 1008, 1015, 43, 63, 1005, 63, 671, 1001, 64, 1, 64, 1105, 1, 673, 4, 653, 1002, 64, 2, 64, 109, -6, 2105, 1, 10, 1001, 64, 1, 64, 1105, 1, 691, 4, 679, 1002, 64, 2, 64, 109, 1, 1208, -6, 23, 63, 1005, 63, 707, 1105, 1, 713, 4, 697, 1001, 64, 1, 64, 1002, 64, 2, 64, 109, -2, 1206, 8, 731, 4, 719, 1001, 64, 1, 64, 1106, 0, 731, 1002, 64, 2, 64, 109, -7, 21102, 46, 1, 5, 1008, 1010, 43, 63, 1005, 63, 751, 1106, 0, 757, 4, 737, 1001, 64, 1, 64, 1002, 64, 2, 64, 109, -9, 2108, 24, 4, 63, 1005, 63, 779, 4, 763, 1001, 64, 1, 64, 1106, 0, 779, 1002, 64, 2, 64, 109, 38, 2106, 0, -7, 1106, 0, 797, 4, 785, 1001, 64, 1, 64, 1002, 64, 2, 64, 109, -27, 2102, 1, -6, 63, 1008, 63, 29, 63, 1005, 63, 819, 4, 803, 1105, 1, 823, 1001, 64, 1, 64, 1002, 64, 2, 64, 109, 1, 21101, 47, 0, 7, 1008, 1015, 47, 63, 1005, 63, 845, 4, 829, 1105, 1, 849, 1001, 64, 1, 64, 1002, 64, 2, 64, 109, -11, 1201, 5, 0, 63, 1008, 63, 31, 63, 1005, 63, 869, 1106, 0, 875, 4, 855, 1001, 64, 1, 64, 1002, 64, 2, 64, 109, 5, 1201, 4, 0, 63, 1008, 63, 34, 63, 1005, 63, 901, 4, 881, 1001, 64, 1, 64, 1105, 1, 901, 4, 64, 99, 21102, 27, 1, 1, 21101, 915, 0, 0, 1105, 1, 922, 21201, 1, 58905, 1, 204, 1, 99, 109, 3, 1207, -2, 3, 63, 1005, 63, 964, 21201, -2, -1, 1, 21101, 0, 942, 0, 1106, 0, 922, 22101, 0, 1, -1, 21201, -2, -3, 1, 21102, 1, 957, 0, 1106, 0, 922, 22201, 1, -1, -2, 1106, 0, 968, 22102, 1, -2, -2, 109, -3, 2106, 0, 0 };

            //IntCodeMemory = new long[] { 109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99 };
            //IntCodeMemory = new long[] { 1102, 34915192, 34915192, 7, 4, 7, 99, 0 };
            //IntCodeMemory = new long[] { 104, 1125899906842624, 99 };
        }

        readonly IReadOnlyList<long> IntCodeMemory = new long[] { };


        public void Run()
        {
            Console.WriteLine("--- Day 9: Sensor Boost ---");

            var outputList = new List<long>();
            var computer = new IntCodeV3(IntCodeMemory);
            computer.OnOutput += (s, e) => outputList.Add(e.OutputValue);
            computer.Init(IntCodeMemory);
            computer.AddInput(1L);

            Console.WriteLine();
            do
            {
                computer.Run();
                if (computer.State == IntCodeState.NeedsInput)
                {
                    Console.Write("Input Value: ");
                    var result = Console.ReadLine();
                    computer.AddInput(Convert.ToInt64(result));
                }
            } while (computer.State == IntCodeState.NeedsInput);

            Console.WriteLine($"Output: {string.Join(',', outputList)}");
            Console.WriteLine($"\t{(outputList.FirstOrDefault() == 3280416268 ? "CORRECT" : "You did something wrong.")}");

            Console.WriteLine();
        }

    }
}
