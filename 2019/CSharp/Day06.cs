namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/6
/// </summary>
class Day06 : PuzzleBase
{
    const int DAY = 6;


    public override IEnumerable<string> SolvePuzzle()
    {
        yield return "Day 6: Universal Orbit Map";

        yield return string.Empty;
        yield return RunExample(Example1);
        yield return Run(Part1);

        yield return string.Empty;
        yield return RunExample(Example2);
        yield return Run(Part2);
    }

    string Example1() => " Ex. 1) " + RunPart1(GetPuzzleData(1, "example1"));
    string Part1() => "Part 1) " + RunPart1(GetPuzzleData(1, "input"));

    string Example2() => " Ex. 2) " + RunPart2(GetPuzzleData(2, "example2"));
    string Part2() => "Part 2) " + RunPart2(GetPuzzleData(2, "input"));


    class InputAnswer : InputAnswer<List<string>, int?>
    {
        public InputAnswer(List<string> input, int? expectedAnswer)
        {
            Input = input;
            ExpectedAnswer = expectedAnswer;

            // Built Orbit Tree
            var orbitMap = Input
                .Select(m => m.Split(')'))
                .Select(p => new { centerOfMass = p[0], satalite = p[1] })
                .ToList();

            var coms = orbitMap.Select(m => m.centerOfMass);
            var sats = orbitMap.Select(m => m.satalite);
            UniqueBodies = coms.Union(sats)
                .Distinct()
                .ToDictionary(key => key, centerOfMass => new OrbitPair(centerOfMass));

            foreach (var orbit in orbitMap)
            {
                var comObj = UniqueBodies[orbit.centerOfMass];
                var orbitObj = UniqueBodies[orbit.satalite];
                orbitObj.Orbits = comObj;
            }
        }

        public Dictionary<string, OrbitPair> UniqueBodies;
    }
    InputAnswer GetPuzzleData(int part, string name)
    {
        var result = new InputAnswer(
            InputHelper.LoadInputFile(DAY, name).ToList(),
            InputHelper.LoadAnswerFile(DAY, part, name)?.FirstOrDefault()?.ToInt32()
            );

        return result;
    }






    string RunPart1(InputAnswer puzzleData)
    {
        var result = CountAllOrbits(puzzleData.UniqueBodies);

        return Helper.GetPuzzleResultText($"The total number of Direct and Indirect orbits: {result}", result, puzzleData.ExpectedAnswer);
    }

    string RunPart2(InputAnswer puzzleData)
    {
        var meToCommonCOM = GetPathToUniversalCOM(puzzleData.UniqueBodies["YOU"].Orbits).ToList();
        var destToCommonCOM = GetPathToUniversalCOM(puzzleData.UniqueBodies["SAN"].Orbits).ToList();

        RemoveSharedCOMs(meToCommonCOM, destToCommonCOM);

        var result = meToCommonCOM.Count + destToCommonCOM.Count;

        return Helper.GetPuzzleResultText($"The minimum # of orbit transfers required: {result}", result, puzzleData.ExpectedAnswer);
    }



    int CountAllOrbits(Dictionary<string, OrbitPair> uniqueBodies)
    {
        var result = uniqueBodies.Keys.Sum(key => CountChain(uniqueBodies[key]));
        return result;
    }

    int CountChain(OrbitPair item)
    {
        if (item.Orbits == null)
            return 0;
        return 1 + CountChain(item.Orbits);
    }


    IEnumerable<OrbitPair> GetPathToUniversalCOM(OrbitPair obj)
    {
        while (obj != null)
        {
            yield return obj;
            obj = obj.Orbits;
        }
    }

    void RemoveSharedCOMs(List<OrbitPair> meToCommonCOM, List<OrbitPair> destToCommonCOM)
    {
        while (true)
        {
            var last1 = meToCommonCOM.LastOrDefault();
            var last2 = destToCommonCOM.LastOrDefault();

            if (last1 != last2)
                return;

            meToCommonCOM.Remove(last1);
            destToCommonCOM.Remove(last2);
        }
    }


    class OrbitPair
    {
        public OrbitPair(string centerOfMass)
        {
            CenterOfMass = centerOfMass;
        }

        public string CenterOfMass { get; private set; }

        public OrbitPair Orbits { get; set; }
    }

}
