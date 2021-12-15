namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/17
/// </summary>
class Day17 : PuzzleBase
{
    const int DAY = 17;

    readonly bool DISPLAY = false;

    public override IEnumerable<string> SolvePuzzle()
    {
        yield return "Day 17: Set and Forget";
        yield return string.Empty;

        yield return RunExample(() => " Ex. 1) " + RunPart1Example(GetMapDataFromInput(1, "example1")));
        yield return RunProblem(() => "Part 1) " + RunPart1(GetPuzzleData(1, "input")));

        yield return string.Empty;

        yield return RunProblem(() => "Part 2) " + RunPart2(GetPuzzleData(2, "input")));
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
    public (List<List<char>>, long?) GetMapDataFromInput(int part, string name)
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


    void Add2Map(long value, List<List<char>> map)
    {
        switch (value)
        {
            case Code_NewLine:
                map.Add(new List<char>());
                break;
            default:
                var c = Convert.ToChar(value);
                var line = GetLastLine();
                line.Add(c);
                break;
        }

        List<char> GetLastLine()
        {
            var line = map.LastOrDefault();
            if (line == null)
            {
                line = new List<char>();
                map.Add(line);
            }
            return line;
        }
    }

    void ShowOutput(long value)
    {
        if (!DISPLAY)
            return;
        switch (value)
        {
            case Code_NewLine:
                Console.WriteLine();
                break;
            default:
                var c = Convert.ToChar(value);
                Console.Write(c);
                Console.Write(' ');
                break;
        }
    }

    string RunPart1(InputAnswer puzzleData)
    {
        var map = new List<List<char>>();

        var bot = new IntCode(puzzleData.Code);
        bot.Output += (s, e) => Add2Map(e.OutputValue, map);
        bot.Output += (s, e) => ShowOutput(e.OutputValue);
        bot.Run();

        var intersections = MarkIntersections(map);

        var answer = intersections
            .Select(p => p.X * p.Y)
            .Sum();
        return Helper.GetPuzzleResultText($"What is the sum of the alignment parameters? {answer}", answer, puzzleData.ExpectedAnswer);
    }
    string RunPart1Example((List<List<char>> Map, long? ExpectedAnswer) data)
    {
        var intersections = MarkIntersections(data.Map);

        var answer = intersections
                .Select(p => p.X * p.Y)
                .Sum();
        return Helper.GetPuzzleResultText($"What is the sum of the alignment parameters? {answer}", answer, data.ExpectedAnswer);
    }


    string RunPart2(InputAnswer puzzleData)
    {
        var answer = 0L;
        var map = new List<List<char>>();

        var bot = new IntCode(puzzleData.Code);
        bot.Output += (s, e) =>
        {
            if (e.OutputValue < 256)
                ShowOutput(e.OutputValue);
            else
                answer = e.OutputValue;
        };
        bot.Poke(0, 2); // Wake up bot

        var movementCode =
            "A,A,B,C,A,C,B,C,A,B\n" +
            "L,4,L,10,L,6\n" +
            "L,6,L,4,R,8,R,8\n" +
            "L,6,R,8,L,10,L,8,L,8\n" +
            "n\n";
        var input = movementCode.ToCharArray()
            .Select(x => (long)(int)x)
            .ToArray();

        bot.AddInput(input);
        bot.Run();


        return Helper.GetPuzzleResultText($"How much dust does the vacuum robot report it has collected? {answer}", answer, puzzleData.ExpectedAnswer);
    }



    const char Char_Scaffolding = '#';
    const char Char_Intersection = 'O';

    const long Code_NewLine = 10;

    private List<List<char>> GetMapData(IList<long> code)
    {
        var line = new List<char>();
        var map = new List<List<char>>();

        var bot = new IntCode(code);
        bot.Output += (s, e) => Add2Map(e.OutputValue);
        bot.Run();

        return map;

        void Add2Map(long value)
        {
            if (value != Code_NewLine)
            {
                var c = Convert.ToChar(value);
                line.Add(c);
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
            if (loc.Y < 0 || loc.Y >= map.Count)
                return false;
            if (loc.X < 0 || loc.X >= map[loc.Y].Count)
                return false;
            var tile = map[loc.Y][loc.X];
            var result = (tile == Char_Scaffolding || tile == Char_Intersection);
            return result;
        }
    }




}

