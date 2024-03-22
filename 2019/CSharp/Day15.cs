namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/15
/// </summary>
public class Day15 : TestBase
{
    public Day15(ITestOutputHelper output) : base(output, 15) { }


    private (List<long>, long?) GetTestData(string name, int part)
    {
        var input = InputHelper.ReadLines(DAY, name)
            .First()
            .Split(',')
            .Select(v => v.ToInt64())
            .ToList();

        var expected = InputHelper.ReadLines(DAY, $"{name}-answer{part}")
            ?.FirstOrDefault()
            ?.ToInt64();

        return (input, expected);
    }


    [Fact]
    public void Part1()
    {
        var (input, expected) = GetTestData("input", 1);

        var result = RunPart1(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Part2()
    {
        var (input, expected) = GetTestData("input", 2);

        var result = RunPart2(input);

        Assert.Equal(expected, result);
    }




    readonly Map map = new();
    readonly Stack<Point> stepStack = new();
    RepairDroid robot;

    
    long RunPart1(List<long> code)
    {
        map.Clear();
        stepStack.Clear();
        var stepCount = 0L;

        robot = new RepairDroid(code, map);
        robot.ReportStatus += (s, e) =>
        {
            if (e.StatusCode != RepairDroidStatusCode.HitWall)
                stepStack.Push(robot.LastLocation);
            if (e.StatusCode == RepairDroidStatusCode.MovedOneStep_AtDestination)
                stepCount = stepStack.Count;
        };
        robot.RequestMovement += (s, e) =>
        {
            var dir = map.GetNextDirection(robot.Location, stepStack);
            e.Direction = dir;
        };

        try
        {
            robot.Start();
        }
        catch (Exception) { }

        return stepCount;
    }


    long RunPart2(List<long> code)
    {
        RunPart1(code);
        var minutes = 0L;

        var start = map.MapData
            .Where(kv => kv.Value == MapLegend.O2Generator)
            .Select(kv => kv.Key)
            .First();

        map.Set(start, MapLegend.Oxygen);

        var hallTiles = map.MapData
            .Where(kv => kv.Value == MapLegend.Hall);
        var tilesToFill = hallTiles
            .Where(kv => NextToOxygen(kv.Key))
            .Select(kv => kv.Key);

        while (hallTiles.Any())
        {
            minutes++;
            var tmp = tilesToFill.ToList();
            foreach (var p in tmp)
                map.Set(p, MapLegend.Oxygen);
        }

        return minutes;
    }

    bool NextToOxygen(Point location)
    {
        var test = map.Offsets
            .Select(kv => Point.Add(location, kv.Value))
            .Any(p => map.MapData[p] == MapLegend.Oxygen);
        return test;
    }



    enum RepairDroidStatusCode
    {
        HitWall = 0,
        MovedOneStep = 1,
        MovedOneStep_AtDestination = 2,
    }
    enum Direction
    {
        North = 1,
        South = 2,
        West = 3,
        East = 4,
    }


    static class MapLegend
    {
        public const string Droid = "D";
        public const string Hall = ".";
        public const string Wall = "#";
        public const string Start = "S";
        public const string O2Generator = "@";
        public const string Oxygen = "O";
    }


    class Map
    {
        private readonly Dictionary<Point, string> mapData = new();
        public IDictionary<Point, string> MapData => mapData;

        private readonly Dictionary<Direction, Size> offsets = new()
        {
            { Direction.North, new Size(0, 1) },
            { Direction.East, new Size(1, 0) },
            { Direction.South, new Size(0, -1) },
            { Direction.West, new Size(-1, 0) },
        };
        public IDictionary<Direction, Size> Offsets => offsets;


        public void Clear()
        {
            mapData.Clear();
        }

        public void Set(Point location, string tile)
        {
            if (!mapData.ContainsKey(location))
                mapData.Add(location, tile);
            else
                mapData[location] = tile;
        }

        readonly Size MinArea = new Size(20, 20);
        public string RenderMap()
        {
            return Helper.DrawPointGrid2D(mapData, minArea: MinArea);
        }

        internal Direction GetNextDirection(Point location, Stack<Point> stepStack)
        {
            var aa = offsets
                .Select(kv => new { dir = kv.Key, loc = location + kv.Value })
                .Where(o => mapData.ContainsKey(o.loc) == false)
                .FirstOrDefault();
            if (aa != null)
                return aa.dir;

            while (stepStack.Count > 0)
            {
                var last = stepStack.Pop();
                var bb = offsets
                    .Select(kv => new { dir = kv.Key, loc = location + kv.Value })
                    .Where(o => o.loc == last)
                    .FirstOrDefault();
                if (bb != null)
                    return bb.dir;
            }

            throw new Exception("No direction found!!");
        }
    }


    class RepairDroid
    {
        private readonly IntCode cpu;
        private readonly Map map;

        private Point startPoint = new Point(0, 0);
        private Point? o2Location = default(Point?);

        public RepairDroid(IEnumerable<long> code, Map map)
        {
            cpu = new IntCode(code);
            cpu.Output += Cpu_OnOutput;
            MoveDirection = Direction.North;
            LastMoveDirection = Direction.North;
            this.map = map;
        }

        

        public Point Location { get; private set; }

        public Direction MoveDirection { get; private set; }
        public Direction LastMoveDirection { get; private set; }
        public Point LastLocation { get; private set; }

        public event EventHandler<ReportStatusEventArgs> ReportStatus;
        public event EventHandler<RequestMovementDirEventArgs> RequestMovement;



        public void Start()
        {
            while (cpu.State != IntCodeState.Finished)// && o2Location.HasValue == false)
            {
                cpu.Run();
                if (cpu.State == IntCodeState.NeedsInput)
                {
                    var args = new RequestMovementDirEventArgs(LastMoveDirection);
                    RequestMovement(this, args);
                    LastMoveDirection = args.Direction;
                    AddInput(args.Direction);
                }
            }
        }

        public void AddInput(Direction direction)
        {
            cpu.AddInput((long)direction);
            MoveDirection = direction;
        }


        public Point CalcNewPosition(Direction direction)
        {
            return Point.Add(Location, map.Offsets[direction]);
        }

        void MoveToNewPosition(Direction direction)
        {
            Location = CalcNewPosition(direction);
        }


        void Cpu_OnOutput(object sender, IntCodeOutputEventArgs e)
        {
            var code = (RepairDroidStatusCode)e.OutputValue;
            var args = new ReportStatusEventArgs(code);

            switch (code)
            {
                case RepairDroidStatusCode.HitWall:
                    map.Set(CalcNewPosition(LastMoveDirection), MapLegend.Wall);
                    break;
                case RepairDroidStatusCode.MovedOneStep:
                    MoveToNewPosition(LastMoveDirection);
                    var lastTile = MapLegend.Hall;
                    if (LastLocation == startPoint)
                        lastTile = MapLegend.Start;
                    if (LastLocation == o2Location)
                        lastTile = MapLegend.O2Generator;
                    map.Set(LastLocation, lastTile);
                    map.Set(Location, MapLegend.Droid);
                    break;
                case RepairDroidStatusCode.MovedOneStep_AtDestination:
                    MoveToNewPosition(LastMoveDirection);
                    map.Set(LastLocation, MapLegend.Hall);
                    map.Set(Location, MapLegend.O2Generator);
                    o2Location = Location;
                    break;
            }

            LastLocation = Location;
            LastMoveDirection = MoveDirection;
            ReportStatus(this, args);
        }

    }


    class ReportStatusEventArgs : EventArgs
    {
        public ReportStatusEventArgs(RepairDroidStatusCode statusCode) : base()
        {
            StatusCode = statusCode;
        }

        public RepairDroidStatusCode StatusCode { get; private set; }
    }


    class RequestMovementDirEventArgs : EventArgs
    {
        public RequestMovementDirEventArgs() : this(Direction.North) { }
        public RequestMovementDirEventArgs(Direction direction) : base()
        {
            Direction = direction;
        }

        public Direction Direction { get; set; } = Direction.North;
    }

}
