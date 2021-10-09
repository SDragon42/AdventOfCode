using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.CSharp.Common;

namespace AdventOfCode.CSharp.Year2019
{
    /// <summary>
    /// https://adventofcode.com/2019/day/1
    /// </summary>
    class Day01 : IPuzzle
    {
        public void Run()
        {
            Console.WriteLine("--- Day 1: The Tyranny of the Rocket Equation ---");
            //RunPart1(GetPuzzleData(1, "example1"));
            //RunPart1(GetPuzzleData(1, "example2"));
            //RunPart1(GetPuzzleData(1, "example3"));
            //RunPart1(GetPuzzleData(1, "example4"));
            RunPart1(GetPuzzleData(1, "input"));

            Console.WriteLine("--- Part Two ---");
            //RunPart2(GetPuzzleData(2, "example2"));
            //RunPart2(GetPuzzleData(2, "example3"));
            RunPart2(GetPuzzleData(2, "input"));
        }

        InputAnswer<List<int>, int?> GetPuzzleData(int part, string name)
        {
            var input = InputHelper.LoadInputFile(1, name)
                .Split("\r\n")
                .Select(l => l.ToInt32())
                .ToList();

            var answer = default(int?);
            try { answer = InputHelper.LoadAnswerFile(1, part, name).ToInt32(); }
            catch { answer = null; }
            
            var result = new InputAnswer<List<int>, int?>(input, answer);
            return result;
        }


        void RunPart1(InputAnswer<List<int>, int?> puzzleData)
        {
            var totalFuel = puzzleData.Input.Sum(CalcFuel);
            Helper.ShowPuzzleResult($"Total Fuel needed: {totalFuel}", totalFuel, puzzleData.ExpectedAnswer);
        }

        void RunPart2(InputAnswer<List<int>, int?> puzzleData)
        {
            var totalFuel = puzzleData.Input.Sum(CalcTotalFuel);
            Helper.ShowPuzzleResult($"Total Fuel needed: {totalFuel}", totalFuel, puzzleData.ExpectedAnswer);
        }


        /// <summary>
        /// Calculates the Fuel needed for the mass.
        /// </summary>
        /// <param name="mass"></param>
        /// <returns></returns>
        int CalcFuel(int mass)
        {
            return (mass / 3) - 2;
        }
        

        /// <summary>
        /// Calculates the fuel need to lift the mass, including the fuel.
        /// </summary>
        /// <param name="mass"></param>
        /// <returns></returns>
        int CalcTotalFuel(int mass)
        {
            var fuelMass = CalcFuel(mass);
            if (fuelMass <= 0L)
                return 0;
            return fuelMass + CalcTotalFuel(fuelMass);
        }

    }

}
