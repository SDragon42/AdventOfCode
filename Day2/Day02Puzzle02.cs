using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent_of_Code.Day2
{
    class Day02Puzzle02 : IPuzzle
    {
        /*
        --- Part Two ---
        "Good, the new computer seems to be working correctly! Keep it nearby during this mission - you'll 
        probably use it again. Real Intcode computers support many more features than your new one, but 
        we'll let you know what they are as you need them."

        "However, your current priority should be to complete your gravity assist around the Moon. For this 
        mission to succeed, we should settle on some terminology for the parts you've already built."

        Intcode programs are given as a list of integers; these values are used as the initial state for 
        the computer's memory. When you run an Intcode program, make sure to start by initializing memory 
        to the program's values. A position in memory is called an address (for example, the first value 
        in memory is at "address 0").

        Opcodes (like 1, 2, or 99) mark the beginning of an instruction. The values used immediately after 
        an opcode, if any, are called the instruction's parameters. For example, in the instruction 1,2,3,4, 
        1 is the opcode; 2, 3, and 4 are the parameters. The instruction 99 contains only an opcode and has 
        no parameters.

        The address of the current instruction is called the instruction pointer; it starts at 0. After an 
        instruction finishes, the instruction pointer increases by the number of values in the instruction; 
        until you add more instructions to the computer, this is always 4 (1 opcode + 3 parameters) for the 
        add and multiply instructions. (The halt instruction would increase the instruction pointer by 1, 
        but it halts the program instead.)

        "With terminology out of the way, we're ready to proceed. To complete the gravity assist, you need 
        to determine what pair of inputs produces the output 19690720."

        The inputs should still be provided to the program by replacing the values at addresses 1 and 2, 
        just like before. In this program, the value placed in address 1 is called the noun, and the value 
        placed in address 2 is called the verb. Each of the two input values will be between 0 and 99, 
        inclusive.

        Once the program has halted, its output is available at address 0, also just like before. Each time 
        you try a pair of inputs, make sure you first reset the computer's memory to the values in the program 
        (your puzzle input) - in other words, don't reuse memory from a previous attempt.

        Find the input noun and verb that cause the program to produce the output 19690720. 
        What is 100 * noun + verb? (For example, if noun=12 and verb=2, the answer would be 1202.)
        */

        public Day02Puzzle02()
        {
            InitialValues = new int[] {
            1, 0, 0, 3,
            1, 1, 2, 3,
            1, 3, 4, 3,
            1, 5, 0, 3,
            2, 13, 1, 19,
            1, 10, 19, 23,
            1, 23, 9, 27,
            1, 5, 27, 31,
            2, 31, 13, 35,
            1, 35, 5, 39,
            1, 39, 5, 43,
            2, 13, 43, 47,
            2, 47, 10, 51,
            1, 51, 6, 55,
            2, 55, 9, 59,
            1, 59, 5, 63,
            1, 63, 13, 67,
            2, 67, 6, 71,
            1, 71, 5, 75,
            1, 75, 5, 79,
            1, 79, 9, 83,
            1, 10, 83, 87,
            1, 87, 10, 91,
            1, 91, 9, 95,
            1, 10, 95, 99,
            1, 10, 99, 103,
            2, 103, 10, 107,
            1, 107, 9, 111,
            2, 6, 111, 115,
            1, 5, 115, 119,
            2, 119, 13, 123,
            1, 6, 123, 127,
            2, 9, 127, 131,
            1, 131, 5, 135,
            1, 135, 13, 139,
            1, 139, 10, 143,
            1, 2, 143, 147,
            1, 147, 10, 0,
            99,
            2, 0, 14, 0 };
        }

        const int DesiredResult = 19690720;

        readonly IReadOnlyList<int> InitialValues;

        //int[] input;

        const int Step = 4;

        const int OpCode_Add = 1;
        const int OpCode_Multiply = 2;
        const int OpCode_Finished = 99;

        public void Run()
        {
            Console.WriteLine("--- Part Two ---");

            var noun = 0;
            var verb = 0;
            var found = false;

            while (noun <= 99 && !found)
            {
                while (verb <= 99 && !found)
                {
                    var runResult = RunOpCode(noun, verb);
                    if (runResult == DesiredResult)
                        found = true;
                    if (!found)
                        verb++;
                }
                if (!found) 
                {
                    noun++;
                    verb = 0; 
                }
            }

            if (found)
            {
                var result = 100 * noun + verb;
                Console.WriteLine($"result is: {result}");
            }
            else
            {
                Console.WriteLine($"result is: NOT FOUND!");
            }

            Console.WriteLine();
        }


        private int RunOpCode(int noun, int verb)
        {
            var input = InitialValues.ToArray();
            input[1] = noun;
            input[2] = verb;

            var position = 0;
            var keepRunning = true;
            ShowInput();
            while (keepRunning)
            {
                var opCode = input[position];

                switch (opCode)
                {
                    case OpCode_Add:
                        RunOp(input, position, OpAdd);
                        break;
                    case OpCode_Multiply:
                        RunOp(input, position, OpMultiply);
                        break;
                    case OpCode_Finished:
                        keepRunning = false;
                        break;
                    default:
                        Console.WriteLine("Something went Wrong!");
                        return -1;
                }
                position += Step;
            }

            return input[0];
        }

        private int OpAdd(int value1, int value2) => value1 + value2;
        private int OpMultiply(int value1, int value2) => value1 * value2;

        private void RunOp(int[] input, int index, Func<int, int, int> operation)
        {
            var value1 = input[input[index + 1]];
            var value2 = input[input[index + 2]];
            input[input[index + 3]] = operation(value1, value2);
        }

        private void ShowInput()
        {
            //Console.WriteLine($"Input: {string.Join(',', input)}");
        }
    }
}
