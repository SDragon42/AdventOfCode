using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent_of_Code.IntCodeComputer;

namespace Advent_of_Code.Day05
{
    /*
    The air conditioner comes online! Its cold air feels good for a while, but then the TEST
    alarms start to go off. Since the air conditioner can't vent its heat anywhere but back into
    the spacecraft, it's actually making the air inside the ship warmer.

    Instead, you'll need to use the TEST to extend the thermal radiators. Fortunately, the
    diagnostic program (your puzzle input) is already equipped for this. Unfortunately, your
    Intcode computer is not.

    Your computer is only missing a few opcodes:

    Opcode 5 is jump-if-true: if the first parameter is non-zero, it sets the instruction pointer
    to the value from the second parameter. Otherwise, it does nothing.
    Opcode 6 is jump-if-false: if the first parameter is zero, it sets the instruction pointer to
    the value from the second parameter. Otherwise, it does nothing.
    Opcode 7 is less than: if the first parameter is less than the second parameter, it stores 1
    in the position given by the third parameter. Otherwise, it stores 0.
    Opcode 8 is equals: if the first parameter is equal to the second parameter, it stores 1 in
    the position given by the third parameter. Otherwise, it stores 0.
    Like all instructions, these instructions need to support parameter modes as described above.

    Normally, after an instruction is finished, the instruction pointer increases by the number of
    values in that instruction. However, if the instruction modifies the instruction pointer, that
    value is used and the instruction pointer is not automatically increased.

    For example, here are several programs that take one input, compare it to the value 8, and
    then produce one output:

    3,9,8,9,10,9,4,9,99,-1,8 - Using position mode, consider whether the input is equal to 8;
    output 1 (if it is) or 0 (if it is not).

    3,9,7,9,10,9,4,9,99,-1,8 - Using position mode, consider whether the input is less than 8;
    output 1 (if it is) or 0 (if it is not).

    3,3,1108,-1,8,3,4,3,99 - Using immediate mode, consider whether the input is equal to 8;
    output 1 (if it is) or 0 (if it is not).

    3,3,1107,-1,8,3,4,3,99 - Using immediate mode, consider whether the input is less than 8;
    output 1 (if it is) or 0 (if it is not).

    Here are some jump tests that take an input, then output 0 if the input was zero or 1 if the
    input was non-zero:

    3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9 (using position mode)
    3,3,1105,-1,9,1101,0,0,12,4,12,99,1 (using immediate mode)
    Here's a larger example:

    3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,
    1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,
    999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99
    The above example program uses an input instruction to ask for a single number. The program
    will then output 999 if the input value is below 8, output 1000 if the input value is equal
    to 8, or output 1001 if the input value is greater than 8.

    This time, when the TEST diagnostic program runs its input instruction to get the ID of the
    system to test, provide it 5, the ID for the ship's thermal radiator controller. This
    diagnostic test suite only outputs one number, the diagnostic code.

    What is the diagnostic code for system ID 5?
    */
    class Day05Puzzle2 : IPuzzle
    {
        public void Run()
        {
            Console.WriteLine("--- Day 5: Sunny with a Chance of Asteroids  (Part 2) ---");

            // Compare tests
            //Console.WriteLine("Test 1 ".PadRight(40, '-'));
            //RunProgram(new int[] { 3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8 }, input: 8, showMemory: true); // Input: =8  Output: 1  else 0

            //Console.WriteLine("Test 2 ".PadRight(40, '-'));
            //RunProgram(new int[] { 3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8 }, input: 5, showMemory: true); // Input: <8  Output: 1  else 0

            //Console.WriteLine("Test 3 ".PadRight(40, '-'));
            //RunProgram(new int[] { 3, 3, 1108, -1, 8, 3, 4, 3, 99 }, input: 8, showMemory: true); // Input: =8  Output: 1  else 0

            //Console.WriteLine("Test 4 ".PadRight(40, '-'));
            //RunProgram(new int[] { 3, 3, 1107, -1, 8, 3, 4, 3, 99 }, input: 5, showMemory: true); // Input: <8  Output: 1  else 0

            // Jump tests
            //RunProgram(new int[] { 3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9 }); // Input: =0  Output: 0  else 1
            //RunProgram(new int[] { 3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1 }); // Input: =0  Output: 0  else 1

            // Larger Example
            //RunProgram(new int[] { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 });
            /* Input: <8  Output: 999
             * Input: =8  Output: 1000
             * Input: >8  Output: 1001
             */

            RunProgram(Day05Common.IntCodeMemory.ToArray(), 5); // Input: 5   Last Output: 3419022
        }

        private static void RunProgram(int[] data, int? input = null, bool showMemory = false)
        {
            var computer = new IntCode(showMemory);
            computer.Init(data);
            if (showMemory)
                computer.ShowMemoryDump();

            if (input.HasValue)
                computer.Run(input.Value);
            else
                computer.Run();

            if (showMemory)
                computer.ShowMemoryDump();
        }
    }
}
