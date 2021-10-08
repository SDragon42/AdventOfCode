using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.CSharp.Year2019.Day01
{
    /*
    --- Part Two ---
    During the second Go / No Go poll, the Elf in charge of the Rocket Equation Double-Checker 
    stops the launch sequence. Apparently, you forgot to include additional fuel for the fuel 
    you just added.

    Fuel itself requires fuel just like a module - take its mass, divide by three, round down, 
    and subtract 2. However, that fuel also requires fuel, and that fuel requires fuel, and 
    so on. Any mass that would require negative fuel should instead be treated as if it 
    requires zero fuel; the remaining mass, if any, is instead handled by wishing really hard, 
    which has no mass and is outside the scope of this calculation.

    So, for each module mass, calculate its fuel and add it to the total. Then, treat the fuel 
    amount you just calculated as the input mass and repeat the process, continuing until a 
    fuel requirement is zero or negative. 
    
    For example:

    A module of mass 14 requires 2 fuel. This fuel requires no further fuel (2 divided by 3 
    and rounded down is 0, which would call for a negative fuel), so the total fuel required 
    is still just 2. At first, a module of mass 1969 requires 654 fuel. Then, this fuel 
    requires 216 more fuel (654 / 3 - 2). 216 then requires 70 more fuel, which requires 21 
    fuel, which requires 5 fuel, which requires no further fuel. So, the total fuel required 
    for a module of mass 1969 is 654 + 216 + 70 + 21 + 5 = 966.
    The fuel required by a module of mass 100756 and its fuel is: 33583 + 11192 + 3728 + 1240 
    + 411 + 135 + 43 + 12 + 2 = 50346.

    What is the sum of the fuel requirements for all of the modules on your spacecraft when 
    also taking into account the mass of the added fuel? (Calculate the fuel requirements for 
    each module separately, then add them all up at the end.)
    */
    class Day01Puzzle2 : IPuzzle
    {
        
        readonly long[] input = new long[] {
            51753,53456,128133,118219,136490,113318,134001,99775,84746,68712,
            104274,72530,101517,65533,98719,100215,144296,114166,62930,118494,
            144800,97604,112836,73074,62591,99278,94544,73035,146631,148331,
            144573,121956,94088,60092,126397,117285,122292,77413,145860,76968,
            64790,58479,94035,148441,78999,74329,145182,129497,106765,67046,
            58702,123344,63866,121123,126808,115190,86467,136359,148718,88225,
            126185,82745,142546,149788,137524,114690,68075,60244,127157,123719,
            87843,107714,51281,92123,146495,50761,130708,53103,75289,121675,
            61726,70674,101755,97736,100141,81085,55493,73071,85321,119965,
            147313,105201,107107,130007,67834,137614,140848,64038,106078,71447};

        public void Run()
        {
            Console.WriteLine("--- Day 1: The Tyranny of the Rocket Equation (Part 2) ---");

            var totalFuel = input.Sum(moduleMass => CalcFuel(moduleMass));

            Console.WriteLine($"Total Fuel needed: {totalFuel}");
            Console.WriteLine();
        }


        long CalcFuel(long mass)
        {
            var fuelmass = Convert.ToInt32(Math.Truncate(mass / 3.0)) - 2;
            if (fuelmass <= 0L)
                return 0L;
            return fuelmass + CalcFuel(fuelmass);
        }

    }
}
