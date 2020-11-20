using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Advent_of_Code.Day04
{
    /*
    --- Day 4: Secure Container ---
    You arrive at the Venus fuel depot only to discover it's protected by a 
    password. The Elves had written the password on a sticky note, but someone 
    threw it out.

    However, they do remember a few key facts about the password:
    - It is a six-digit number.
    - The value is within the range given in your puzzle input.
    - Two adjacent digits are the same (like 22 in 122345).
    - Going from left to right, the digits never decrease; they only ever 
      increase or stay the same (like 111123 or 135679).
    
    Other than the range rule, the following are true:

    - 111111 meets these criteria (double 11, never decreases).
    - 223450 does not meet these criteria (decreasing pair of digits 50).
    - 123789 does not meet these criteria (no double).

    How many different passwords within the range given in your puzzle input 
    meet these criteria?
    */
    class Day04Puzzle1 : IPuzzle
    {

        public void Run()
        {
            Console.WriteLine("--- Day 4: Secure Container ---");

            TestValue(111111, true);
            TestValue(223450, false);
            TestValue(123789, false);

            var numPossibilities = 0;
            var password = Day04Common.PasswordRangeMIN;
            while (password <= Day04Common.PasswordRangeMAX)
            {
                if (DoesMeetCriteria(password))
                    numPossibilities++;
                password++;
            }

            Console.WriteLine($"Number of Possible Passwords: {numPossibilities}");
            Console.WriteLine();
        }

        void TestValue(int value, bool expected)
        {
            var result = DoesMeetCriteria(value, true);
            Console.WriteLine($"Test: '{value}'  [{result}]  {(result == expected? "SUCCESS" : "FAILED")}");
        }


        private bool DoesMeetCriteria(int value, bool skipRange = false)
        {
            var digits = GetDigits(value);

            //-It is a six - digit number.
            var isSixDigits = (digits.Length == 6);

            //- The value is within the range given in your puzzle input.
            var isInRange = (Day04Common.PasswordRangeMIN <= value && value <= Day04Common.PasswordRangeMAX);
            if (skipRange)
                isInRange = true;

            var any2AdjacentSame = false;
            var neverDecreases = true;

            for (int i = 0; i < digits.Length - 1; i++)
            {
                any2AdjacentSame |= digits[i] == digits[i + 1];
                neverDecreases &= digits[i] <= digits[i + 1];
            }
            //- Two adjacent digits are the same(like 22 in 122345).


            //- Going from left to right, the digits never decrease; they only ever increase or stay the same(like 111123 or 135679).

            return isSixDigits && isInRange && any2AdjacentSame && neverDecreases;
        }

        private static int NumDigits(int value) => Convert.ToInt32(Math.Floor(Math.Log10(value) + 1));
        private static int[] GetDigits(int value)
        {
            var parts = new int[NumDigits(value)];
            for (var i = 0; i < parts.Length; i++)
                parts[i] = GetDigit(value, i + 1);
            return parts;
        }
        private static int GetDigit(int A1, int B1)
        {
            // =TRUNC($A1/POWER(10, 6 - $B1)) - TRUNC($A1/POWER(10, 6 - $B1 + 1)) * 10

            var result = Math.Truncate(A1 / Math.Pow(10, 6 - B1)) - (Math.Truncate(A1 / Math.Pow(10, 6 - B1 + 1)) * 10);
            return Convert.ToInt32(result);
        }

    }
}
