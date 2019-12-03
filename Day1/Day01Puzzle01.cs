using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent_of_Code.Day1
{
    class Day01Puzzle01 : IPuzzle
    {
        /*
        --- Day 1: The Tyranny of the Rocket Equation ---
        Santa has become stranded at the edge of the Solar System while delivering presents to other planets! To accurately calculate his position in space, safely align his warp drive, and return to Earth in time to save Christmas, he needs you to bring him measurements from fifty stars.

        Collect stars by solving puzzles. Two puzzles will be made available on each day in the Advent calendar; the second puzzle is unlocked when you complete the first. Each puzzle grants one star. Good luck!

        The Elves quickly load you into a spacecraft and prepare to launch.

        At the first Go / No Go poll, every Elf is Go until the Fuel Counter-Upper. They haven't determined the amount of fuel required yet.

        Fuel required to launch a given module is based on its mass. Specifically, to find the fuel required for a module, take its mass, divide by three, round down, and subtract 2.

        For example:

        For a mass of 12, divide by 3 and round down to get 4, then subtract 2 to get 2.
        For a mass of 14, dividing by 3 and rounding down still yields 4, so the fuel required is also 2.
        For a mass of 1969, the fuel required is 654.
        For a mass of 100756, the fuel required is 33583.
        The Fuel Counter-Upper needs to know the total fuel requirement. To find it, individually calculate the fuel needed for the mass of each module (your puzzle input), then add together all the fuel values.

        What is the sum of the fuel requirements for all of the modules on your spacecraft?
        */
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
            Console.WriteLine("--- Day 1: The Tyranny of the Rocket Equation ---");

            var totalFuel = input.Sum(moduleMass => CalcFuel(moduleMass));

            Console.WriteLine($"Total Fuel needed: {totalFuel}");
            Console.WriteLine();
        }

        long CalcFuel(long mass)
        {
            return Convert.ToInt32(Math.Truncate(mass / 3.0)) - 2;
        }
    }
}
