namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/14
/// </summary>
/// <remarks>
/// used https://asmartins.com/blog/rocket-fuel/ to solve this puzzle.
/// </remarks>
class Day14 : PuzzleBase
{
    const int DAY = 14;

    const string FUEL = "FUEL";
    const string ORE = "ORE";

    const long TRILLION_ORE = 1000000000000;


    public override IEnumerable<string> SolvePuzzle()
    {
        yield return "Day 14: Space Stoichiometry";

        yield return RunExample(Example1);
        yield return RunExample(Example2);
        yield return RunExample(Example3);
        yield return RunExample(Example4);
        yield return RunExample(Example5);
        yield return RunProblem(Part1);

        yield return string.Empty;
        yield return RunExample(Example3P2);
        yield return RunExample(Example4P2);
        yield return RunExample(Example5P2);
        yield return RunProblem(Part2);
    }

    string Example1() => " Ex. 1) " + RunPart1(GetPuzzleData(1, "example1"));
    string Example2() => " Ex. 2) " + RunPart1(GetPuzzleData(1, "example2"));
    string Example3() => " Ex. 3) " + RunPart1(GetPuzzleData(1, "example3"));
    string Example4() => " Ex. 4) " + RunPart1(GetPuzzleData(1, "example4"));
    string Example5() => " Ex. 5) " + RunPart1(GetPuzzleData(1, "example5"));
    string Part1() => "Part 1) " + RunPart1(GetPuzzleData(1, "input"));

    string Example3P2() => " Ex. 3) " + RunPart2(GetPuzzleData(2, "example3"));
    string Example4P2() => " Ex. 4) " + RunPart2(GetPuzzleData(2, "example4"));
    string Example5P2() => " Ex. 5) " + RunPart2(GetPuzzleData(2, "example5"));
    string Part2() => "Part 2) " + RunPart2(GetPuzzleData(2, "input"));



    class InputAnswer : InputAnswer<List<string>, long?> { }
    InputAnswer GetPuzzleData(int part, string name)
    {
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

        Log.AppendLine(Helper.GetPuzzleResultText($"Minimum amount of ORE needed for 1 Fuel: {oreNeeded}", oreNeeded, puzzleData.ExpectedAnswer));
        return Log.ToString().Trim();
    }

    string RunPart2(InputAnswer puzzleData)
    {
        Log.Clear();
        var reactions = BuildReactionChains(puzzleData.Input);
        var orderedChems = SetTopologicalOrder(reactions);

        var fuelProduced = 1L;
        var args = (reactions, orderedChems);
        (var estimations, _) = EstimateFuelProduced(reactions, TRILLION_ORE, orderedChems);
        (long result, long i) = Bisect(f, TRILLION_ORE, estimations, args);

        long f(long x, IDictionary<string, ChemReaction> reactions, List<string> orderedChems)
        {
            var ore = CalcOreNeeded_Topological(FUEL, x, reactions, orderedChems.ToList());
            return ore;
        }

        fuelProduced = result;

        Log.AppendLine(Helper.GetPuzzleResultText($"Maximum Fuel Produced: {fuelProduced}", fuelProduced, puzzleData.ExpectedAnswer));
        return Log.ToString().Trim();
    }

    List<string> SetTopologicalOrder(IDictionary<string, ChemReaction> reactions)
    {
        var orderedChems = new List<string>();
        var visitedChems = new List<string>();

        DepthFirstSearch(FUEL);
        orderedChems.Reverse();
        orderedChems.Add(ORE);

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
    private long CalcOreNeeded_Topological(string chemName, long amountNeeded, IDictionary<string, ChemReaction> reactions, IList<string> orderedChems)
    {
        var needs = new List<Chemical>();
        needs.Add(new Chemical(chemName, amountNeeded));

        var oreRequired = 0L;
        var iterations = 0L;

        while (needs.Count > 0)
        {
            iterations++;

            var t = orderedChems[0];
            orderedChems.RemoveAt(0);

            var chemical = needs.Where(a => a.Name == t).First();
            var qty_required = chemical.Quantity;
            needs.Remove(chemical);

            var x = reactions[chemical.Name];
            var qty_produced = x.Product.Quantity;
            var ingredients = x.Reactants;
            var n = (long)Math.Ceiling((decimal)qty_required / qty_produced);

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



    ((long underEstimation, long overEstimation), long order) EstimateFuelProduced(IDictionary<string, ChemReaction> reactions, long ore_Qty, IReadOnlyList<string> orderedChems)
    {
        var ratio = CalcOreNeeded_Topological(FUEL, 1L, reactions, orderedChems.ToList());
        var underEstimation = ore_Qty / ratio;
        var order = Convert.ToInt64(Math.Log10(underEstimation));
        var (qty, ore) = DoIt(order);
        while (ore < ore_Qty)
        {
            order += 1;
            (qty, ore) = DoIt(order);
        }
        ratio = Convert.ToInt64(Math.Floor((ore * 1.0) / (qty * 1.0)));
        var overEstimation = ore_Qty / ratio;
        return ((underEstimation, overEstimation), order);

        (long, long) DoIt(long order)
        {
            var qty = Convert.ToInt64(Math.Pow(order, 10));
            var ore = CalcOreNeeded_Topological(FUEL, qty, reactions, orderedChems.ToList());
            return (qty, ore);
        }
    }


    (long result, long i) Bisect(
        Func<long, IDictionary<string, ChemReaction>, List<string>, long> f,
        long target,
        (long underEstimation, long overEstimation) interval,
        (IDictionary<string, ChemReaction> reactions, List<string> orderedChems) args)
    {
        var iterations = 0L;
        (long low, long high) = interval;
        while (low <= high)
        {
            iterations++;
            var mid = low + ((high - low) / 2);
            var f_mid = f(mid, args.reactions, args.orderedChems);
            if (f_mid == target)
                return (mid, iterations);
            if (f_mid < target)
                low = mid + 1;
            else
                high = mid - 1;
        }
        return (low - 1, iterations);
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
        public Chemical(string element, long amount)
        {
            Name = element;
            Quantity = amount;
        }

        public string Name { get; private set; }
        public long Quantity { get; set; }

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
