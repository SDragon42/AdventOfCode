namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/17
/// </summary>
class Day17 : PuzzleBase
{
    const int DAY = 17;


    public override IEnumerable<string> SolvePuzzle()
    {
        yield return "Day 17: Set and Forget";
        yield return string.Empty;

        yield return RunExample(() => " Ex. 1) " + RunPart1Example(GetMapData(1, "example1")));
        yield return RunProblem(() => "Part 1) " + RunPart1(GetPuzzleData(1, "input")));

        //yield return string.Empty;

        //yield return RunExample(() => " Ex. 1) " + RunPart1(GetPuzzleData(2, "example1")));
        //yield return RunProblem(() => "Part 2) " + RunPart1(GetPuzzleData(2, "input")));
    }


    class InputAnswer : IntCodeInputAnswer<long?> { }
    InputAnswer GetPuzzleData(int part, string name)
    {
        var result = new InputAnswer()
        {
            Input = InputHelper.LoadInputFile(DAY, name).ToList(),
            ExpectedAnswer = InputHelper.LoadAnswerFile(DAY, part, name)?.FirstOrDefault()?.ToInt64()
        };
        return result;
    }
    public (List<List<char>>, long?) GetMapData(int part, string name)
    {
        var map = new List<List<char>>();
        var lines = InputHelper.LoadInputFile(DAY, name);
        foreach (var line in lines)
        {
            var t = line.Select(c => c).ToList();
            map.Add(t);
        }

        return (
            map,
            InputHelper.LoadAnswerFile(DAY, part, name)?.FirstOrDefault()?.ToInt64()
        );
    }


    string RunPart1(InputAnswer puzzleData)
    {
        var map = GetMapData(puzzleData.Code);
        //ShowMap(map);
        var intersections = MarkIntersections(map);
        
        var answer = intersections
            .Select(p => p.X * p.Y)
            .Sum();
        return Helper.GetPuzzleResultText($"What is the sum of the alignment parameters? {answer}", answer, puzzleData.ExpectedAnswer);
    }
    string RunPart1Example((List<List<char>> Map, long? ExpectedAnswer) data)
    {
        var intersections = MarkIntersections(data.Map);

        //ShowMap(data.Map);
        var answer = intersections
                .Select(p => p.X * p.Y)
                .Sum();
        return Helper.GetPuzzleResultText($"What is the sum of the alignment parameters? {answer}", answer, data.ExpectedAnswer);
    }


    string RunPart2(InputAnswer puzzleData)
    {
        var answer = 0;
        return Helper.GetPuzzleResultText($": {answer}", answer, puzzleData.ExpectedAnswer);
    }



    //const long Code_Scaffolding = 35;
    const char Char_Scaffolding = '#';
    const char Char_Space = '.';
    const char Char_Intersection = 'O';
    const char Char_Unknown = '?';

    const char Char_BotDirU = '^';
    const char Char_BotDirR = '>';
    const char Char_BotDirD = 'V';
    const char Char_BotDirL = '<';

    private char GetTileData(long value) => value switch
    {
        35 => Char_Scaffolding,
        46 => Char_Space,
        //10 => "\n",
        _ => Char_Unknown
        //_ => throw new ApplicationException("Unrecognized value")
    };

    private List<List<char>> GetMapData(IList<long> code)
    {
        List<char> line = new List<char>();
        var map = new List<List<char>>();

        var ascii = new IntCode(code);
        ascii.Output += (s, e) => Add2Map(e.OutputValue);

        ascii.Run();

        return map;

        void Add2Map(long numValue)
        {
            if (numValue != 10)
            {
                line.Add(GetTileData(numValue));
                return;
            }
            if (line.Count > 0)
                map.Add(line);
            line = new List<char>();
        }
    }

    private void ShowMap(List<List<char>> map)
    {
        foreach (var line in map)
        {
            var text = string.Join("", line);
            Console.WriteLine(text);
        }
    }

    private List<Point> MarkIntersections(List<List<char>> map)
    {
        var offsets = new Point[] {
            new Point(0, -1),
            new Point(1, 0),
            new Point(0, 1),
            new Point(-1, 0),
        };

        var intersections = new List<Point>();

        for (var y = 0; y < map.Count; y++)
        {
            for (var x = 0; x < map[y].Count; x++)
            {
                var tile = map[y][x];
                if (tile != Char_Scaffolding)
                    continue;

                var SidesWithScaffolding = offsets
                    .Select(o => hasScaffolding(new Point(x, y), o))
                    .Where(o => o)
                    .ToList();
                if (SidesWithScaffolding.Count() >= 3)
                {
                    map[y][x] = Char_Intersection;
                    intersections.Add(new Point(x, y));
                }
            }
        }

        return intersections;

        bool hasScaffolding(Point location, Point shift)
        {
            var loc = location;
            loc.Offset(shift);
            if (loc.X < 0 || loc.X >= map[0].Count)
                return false;
            if (loc.Y < 0 || loc.Y >= map.Count)
                return false;
            var tile = map[loc.Y][loc.X];
            var result = (tile == Char_Scaffolding || tile == Char_Intersection);
            return result;
        }
    }




}

