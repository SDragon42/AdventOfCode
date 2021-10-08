using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdventOfCode.CSharp.Year2019.Day04
{
    /*
    --- Part Two ---
    An Elf just remembered one more important detail: the two adjacent matching 
    digits are not part of a larger group of matching digits.

    Given this additional criterion, but still ignoring the range rule, the 
    following are now true:
    - 112233 meets these criteria because the digits never decrease and all 
      repeated digits are exactly two digits long.
    - 123444 no longer meets the criteria (the repeated 44 is part of a 
      larger group of 444).
    - 111122 meets the criteria (even though 1 is repeated more than twice,
      it still contains a double 22).
    
    How many different passwords within the range given in your puzzle input 
    meet all of the criteria?
    */
    class Day04Puzzle2 : IPuzzle
    {

        public void Run()
        {
            Console.WriteLine("--- Day 4: Secure Container (Part 2) ---");

            TestValue(112233, true);
            TestValue(123444, false);
            TestValue(111122, true);

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
            Console.WriteLine($"Test: '{value}'  [{result,-5}]  {(result == expected ? "SUCCESS" : "FAILED")}");
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

            //- Two adjacent digits are the same(like 22 in 122345).
            //  the two adjacent matching digits are not part of a larger group of matching digits
            var any2AdjacentSame = Any2AdjacentDigitsTheSameExclusively(digits);

            //- Going from left to right, the digits never decrease; they only ever increase or stay the same(like 111123 or 135679).
            var neverDecreases = DigitsNeverDecrease(digits);

            return isSixDigits && isInRange && any2AdjacentSame && neverDecreases;
        }

        private bool DigitsNeverDecrease(int[] digits)
        {
            var neverDecreases = true;
            for (int i = 0; i < digits.Length - 1; i++)
            {
                neverDecreases &= digits[i] <= digits[i + 1];
            }
            return neverDecreases;
        }
        private bool Any2AdjacentDigitsTheSameExclusively(int[] digits)
        {
            var lastDigit = digits[0];
            var repeatCount = 0;
            var i = 1;
            while (i < digits.Length)
            {
                if (digits[i] == lastDigit)
                {
                    repeatCount++;
                }
                else
                {
                    if (repeatCount == 1) // 2 digits only
                        return true;
                    repeatCount = 0;
                }
                lastDigit = digits[i];
                i++;
            }
            return (repeatCount == 1);
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
