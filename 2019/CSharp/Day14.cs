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
    /// <remarks>
    /// used https://asmartins.com/blog/rocket-fuel/ to solve this puzzle.
    /// </remarks>
    class Day14 : PuzzleBase
    {
        const string FUEL = "FUEL";
        const string ORE = "ORE";

        public override IEnumerable<string> SolvePuzzle()
        {
            yield return "Day 14: Space Stoichiometry";

            yield return RunExample(Example1);
            yield return RunExample(Example2);
            yield return RunExample(Example3);
            yield return RunExample(Example4);
            yield return RunExample(Example5);
            yield return Run(Part1);

            yield return string.Empty;
            //yield return RunExample(Example1P2);
            //yield return RunExample(Example2P2);
            //yield return Run(Part2);
        }

        string Example1() => " Ex. 1) " + RunPart1(GetPuzzleData(1, "example1"));
        string Example2() => " Ex. 2) " + RunPart1(GetPuzzleData(1, "example2"));
        string Example3() => " Ex. 3) " + RunPart1(GetPuzzleData(1, "example3"));
        string Example4() => " Ex. 4) " + RunPart1(GetPuzzleData(1, "example4"));
        string Example5() => " Ex. 5) " + RunPart1(GetPuzzleData(1, "example5"));
        string Part1() => "Part 1) " + RunPart1(GetPuzzleData(1, "input"));

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


        readonly StringBuilder Log = new StringBuilder();

        string RunPart1(InputAnswer puzzleData)
        {
            Log.Clear();
            var reactions = BuildReactionChains(puzzleData.Input);
            var orderedChems = SetTopologicalOrder(reactions);
            
            var oreNeeded = CalcOreNeeded_Topological(FUEL, 1, reactions, orderedChems);
            
            Log.Clear();
            Log.AppendLine(Helper.GetPuzzleResultText($"Minimum amount of ORE needed for 1 Fuel: {oreNeeded}", oreNeeded, puzzleData.ExpectedAnswer));
            return Log.ToString().Trim();
        }
        IList<string> SetTopologicalOrder(IDictionary<string, ChemReaction> reactions)
        {
            //OrderedChemicals.Clear();
            var orderedChems = new List<string>();
            var visitedChems = new List<string>();

            DepthFirstSearch(FUEL);
            orderedChems.Reverse();
            orderedChems.Add(ORE);
            //Log.AppendLine("ORDER: " + string.Join(" -> ", OrderedChems));

            return orderedChems;

            void DepthFirstSearch(string chemName)
            {
                visitedChems.Add(chemName);
                if (chemName == ORE)
                    return;
                var ingredients = reactions[chemName];

                foreach (var reactant in ingredients.Reactants)
                {
                    if (!visitedChems.Contains(reactant.Product.Name))
                        DepthFirstSearch(reactant.Product.Name);
                }

                orderedChems.Add(chemName);
            }
        }
        private int CalcOreNeeded_Topological(string chemName, int amountNeeded, IDictionary<string, ChemReaction> reactions, IList<string> orderedChems)
        {
            //Log.AppendLine("-");

            var needs = new List<Chemical>();
            needs.Add(new Chemical(chemName, amountNeeded));

            var oreRequired = 0;
            var iterations = 0;

            while (needs.Count > 0)
            {
                iterations++;

                //Log.Append(
                //    $"{iterations,2}: " +
                //    $"{string.Join(',', needs.Select(r => r.ToString().Replace(" ", ""))),8}" +
                //    " -> ");

                var t = orderedChems[0];
                orderedChems.RemoveAt(0);

                var chemical = needs.Where(a => a.Name == t).First();
                var qty_required = chemical.Quantity;
                needs.Remove(chemical);

                var x = reactions[chemical.Name];
                var qty_produced = x.Product.Quantity;
                var ingredients = x.Reactants;
                var n = (int)Math.Ceiling((decimal)qty_required / qty_produced);

                foreach (var ingredient in ingredients)
                {
                    var n2 = ingredient.Product.Quantity * n;
                    if (ingredient.Product.Name == ORE)
                    {
                        oreRequired += n2;
                        continue;
                    }

                    var tmp = needs
                        .Where(i => i.Name == ingredient.Product.Name)
                        .FirstOrDefault();

                    if (tmp == null)
                    {
                        tmp = new Chemical(ingredient.Product.Name, 0);
                        needs.Add(tmp);
                    }

                    tmp.Quantity += n2;
                }

                //Log.AppendLine(
                //    $"{string.Join(',', needs.Select(r => r.ToString().Replace(" ", ""))),-8}" +
                //    $"{string.Join(',', Stock.Where(s => s.Value > 0).Select(s => $"{s.Value}{s.Key}")),-8}" +
                //    $"{oreRequired,4}");
            }
            return oreRequired;
        }

        IDictionary<string, ChemReaction> BuildReactionChains(IEnumerable<string> inputLines)
        {
            var reactions = new Dictionary<string, ChemReaction>();
            foreach (var line in inputLines)
            {
                var parts = line.Split("=>", 2);
                var endNode = MakeChemReaction(parts[1].Trim());

                var neededParts = parts[0].Trim().Split(',');
                foreach (var rec in neededParts)
                    endNode.Reactants.Add(MakeChemReaction(rec.Trim()));

                reactions.Add(endNode.Product.Name, endNode);
            }

            return reactions;
        }
        ChemReaction MakeChemReaction(string chemInfo)
        {
            var parts = chemInfo.Split(" ", 2);
            var chem = new Chemical(parts[1].Trim(), Convert.ToInt32(parts[0].Trim()));
            var item = new ChemReaction(chem);
            return item;
        }

        IEnumerable<string> DisplayReachionChains(IDictionary<string, ChemReaction> reactions)
        {
            foreach (var key in reactions.Keys)
            {
                var rec = reactions[key];

                var rNeeds = rec.Reactants.Select(r => r.ToString());
                yield return $"{string.Join(", ", rNeeds)} => {rec}";
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
