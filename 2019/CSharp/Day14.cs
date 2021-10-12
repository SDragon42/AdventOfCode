using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.CSharp.Common;

namespace AdventOfCode.CSharp.Year2019
{
    /// <summary>
    /// https://adventofcode.com/2019/day/14
    /// </summary>
    class Day14 : PuzzleBase
    {
        public override IEnumerable<string> SolvePuzzle()
        {
            yield return "Day 14: Space Stoichiometry";

            yield return string.Empty;
            yield return RunExample(Example1);
            yield return RunExample(Example2);
            yield return RunExample(Example3);
            yield return RunExample(Example4);
            yield return RunExample(Example5);
            //yield return Run(Part1);

            yield return string.Empty;
            //yield return RunExample(Example1P2);
            //yield return RunExample(Example2P2);
            //yield return Run(Part2);
        }

        string Example1() => "\r\n\r\n\r\n Ex. 1) " + RunPart1(GetPuzzleData(1, "example1"));
        string Example2() => "\r\n\r\n\r\n Ex. 2) " + RunPart1(GetPuzzleData(1, "example2"));
        string Example3() => "\r\n\r\n\r\n Ex. 3) " + RunPart1(GetPuzzleData(1, "example3"));
        string Example4() => "\r\n\r\n\r\n Ex. 4) " + RunPart1(GetPuzzleData(1, "example4"));
        string Example5() => "\r\n\r\n\r\n Ex. 5) " + RunPart1(GetPuzzleData(1, "example5"));
        //string Example2() => " Ex. 2) " + RunPart1(GetPuzzleData(1, "example2"), 100);
        //string Part1() => "Part 1) " + RunPart1(GetPuzzleData(1, "input"), 1000);

        //string Example1P2() => " Ex. 1) " + RunPart2(GetPuzzleData(2, "example1"));
        //string Example2P2() => " Ex. 2) " + RunPart2(GetPuzzleData(2, "example2"));
        //string Part2() => "Part 2) " + RunPart2(GetPuzzleData(2, "input"));



        class InputAnswer : InputAnswer<List<string>, long?> { }
        InputAnswer GetPuzzleData(int part, string name)
        {
            const int DAY = 14;

            var result = new InputAnswer()
            {
                Input = InputHelper.LoadInputFile(DAY, name).ToList(),
                ExpectedAnswer = InputHelper.LoadAnswerFile(DAY, part, name)?.FirstOrDefault()?.ToInt64()
            };
            return result;
        }


        readonly Dictionary<string, ChemReaction> ReactionChains = new Dictionary<string, ChemReaction>();
        readonly Dictionary<string, Chemical> ExcessChems = new Dictionary<string, Chemical>();



        string RunPart1(InputAnswer puzzleData)
        {
            ReactionChains.Clear();
            ExcessChems.Clear();
            BuildReactionChains(puzzleData.Input);

            var sb = new StringBuilder();

            sb.AppendLine("REACTION CHAINS");

            DisplayReachionChains()
                .ForEach(l => sb.AppendLine(l));

            ////Console.WriteLine("--- Raw Data ---".PadRight(50, '-'));
            ////rawData.ForEach(d => Console.WriteLine(d));
            var oreNeeded = CalcOreNeeded("FUEL", 1);

            //Console.WriteLine($"ORE needed: {oreNeeded}");
            //if (oreNeeded == CorrectAnswer)
            //    Console.WriteLine("\tCorrect");
            //else
            //    Console.WriteLine($"  Expected: {CorrectAnswer}");
            //Console.WriteLine();

            sb.AppendLine(Helper.GetPuzzleResultText($"Number of block tiles are on the screen: {oreNeeded}", oreNeeded, puzzleData.ExpectedAnswer));

            return sb.ToString();
        }

        void BuildReactionChains(IEnumerable<string> inputLines)
        {
            foreach (var line in inputLines)
            {
                var parts = line.Split("=>", 2);
                var endNode = MakeChemReaction(parts[1].Trim());

                var neededParts = parts[0].Trim().Split(',');
                foreach (var rec in neededParts)
                    endNode.Reactants.Add(MakeChemReaction(rec.Trim()));

                ReactionChains.Add(endNode.Product.Name, endNode);
            }

            ChemReaction MakeChemReaction(string chemInfo)
            {
                var parts = chemInfo.Split(" ", 2);
                var chem = new Chemical(parts[1].Trim(), Convert.ToInt32(parts[0].Trim()));
                var item = new ChemReaction(chem);
                return item;
            }
        }




        //readonly IReadOnlyList<string> rawData;

        //readonly IDictionary<string, Chemical> ExcessChems = new Dictionary<string, Chemical>();
        //readonly int CorrectAnswer;


        private int CalcOreNeeded(string chemical, int amountNeeded)
        {
            if (chemical == "ORE")
                return amountNeeded;

            var oreNeeded = 0;
            var chain = ReactionChains[chemical];
            var chem = chain.Product;
            var gotten = 0;

            //var t2 = GetExcessChemical(chemical);
            //if (t2.Quantity >= amountNeeded)
            //{
            //    t2.Quantity -= amountNeeded;
            //    return 0;
            //}

            for (int l = 1; l <= amountNeeded; l += chem.Quantity)
            {
                foreach (var neededChem in chain.Reactants)
                {
                    var t = GetExcessChemical(neededChem.Product.Name);
                    if (t.Quantity >= neededChem.Product.Quantity)
                        t.Quantity -= neededChem.Product.Quantity;
                    else
                        oreNeeded += CalcOreNeeded(neededChem.Product.Name, neededChem.Product.Quantity);
                }
                gotten += chem.Quantity;
            }

            var excess = gotten - amountNeeded;
            if (excess > 0)
            {
                var t = GetExcessChemical(chemical);
                t.Quantity += excess;
            }
            return oreNeeded;
        }

        Chemical GetExcessChemical(string chemicalName)
        {
            if (!ExcessChems.ContainsKey(chemicalName))
                ExcessChems.Add(chemicalName, new Chemical(chemicalName, 0));
            return ExcessChems[chemicalName];
        }


        IEnumerable<string> DisplayReachionChains()
        {
            foreach (var key in ReactionChains.Keys)
            {
                var rec = ReactionChains[key];

                var rneeds = rec.Reactants.Select(r => r.ToString());
                yield return $"{string.Join(", ", rneeds)} => {rec}";
            }
        }






        class Chemical
        {
            public Chemical(string element, int amount)
            {
                Name = element;
                Quantity = amount;
            }

            public string Name { get; private set; }
            public int Quantity { get; set; }

            public override string ToString()
            {
                return $"{Quantity} {Name}";
            }
        }


        class ChemReaction
        {
            public ChemReaction(Chemical chem)
            {
                Product = chem;
                Reactants = new List<ChemReaction>();
            }

            public Chemical Product { get; private set; }
            public List<ChemReaction> Reactants { get; private set; }

            public override string ToString()
            {
                return Product.ToString();
            }
        }

    }
}
