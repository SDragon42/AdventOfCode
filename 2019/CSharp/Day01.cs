//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using AdventOfCode.CSharp.Common;

//namespace AdventOfCode.CSharp.Year2019
//{
//    class Day01
//    {
//        static void Main(string[] args)
//        {
//            var me = new Day01();

//            Console.WriteLine("--- Day 1: The Tyranny of the Rocket Equation ---");
//            // me.RunPart1(InputHelper.LoadLinesAsArray<long>("example101"), 2);
//            // me.RunPart1(InputHelper.LoadLinesAsArray<long>("example102"), 2);
//            // me.RunPart1(InputHelper.LoadLinesAsArray<long>("example103"), 654);
//            // me.RunPart1(InputHelper.LoadLinesAsArray<long>("example104"), 33583);
//            me.RunPart1(InputHelper.LoadLines<long>("input").ToArray(), 3373568);

//            Console.WriteLine("--- Part Two ---");
//            // me.RunPart2(InputHelper.LoadLinesAsArray<long>("example102"), 2);
//            // me.RunPart2(InputHelper.LoadLinesAsArray<long>("example103"), 966);
//            me.RunPart2(InputHelper.LoadLines<long>("input").ToArray(), 5057481);

//        }

//        long CalcFuel(long mass)
//        {
//            return Convert.ToInt32(Math.Truncate(mass / 3.0)) - 2;
//        }

//        long CalcTotalFuel(long mass)
//        {
//            var fuelMass = CalcFuel(mass);
//            if (fuelMass <= 0L)
//                return 0L;
//            return fuelMass + CalcTotalFuel(fuelMass);
//        }

//        void ShowInput(long[] input)
//        {
//            foreach (var item in input)
//                Console.WriteLine($"{item,10:N0}");
//        }

//        public void RunPart1(long[] input, long? correctAnswer = null)
//        {
//            //ShowInput(input);
//            var totalFuel = input.Sum(moduleMass => CalcFuel(moduleMass));

//            PuzzleHelper.ShowPuzzleResult($"Total Fuel needed: {totalFuel}", totalFuel, correctAnswer);
//        }

//        public void RunPart2(long[] input, long? correctAnswer = null)
//        {
//            var totalFuel = input.Sum(moduleMass => CalcTotalFuel(moduleMass));
//            var fuelNeeded = totalFuel;

//            PuzzleHelper.ShowPuzzleResult($"Total Fuel needed: {totalFuel}", totalFuel, correctAnswer);
//        }
//    }
//}
