namespace AdventOfCode.CSharp.Year2022;

public class Day15_Beacon_Exclusion_Zone
{
    private const int DAY = 15;

    private readonly ITestOutputHelper output;
    public Day15_Beacon_Exclusion_Zone(ITestOutputHelper output) => this.output = output;



    private (List<SensorBeaconInfo> input, int? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName)
            .Select(ParseInput)
            .ToList();

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (input, expected);
    }

    private Regex inputRegex = new Regex("Sensor at x=(?<x>.*), y=(?<y>.*): closest beacon is at x=(?<x2>.*), y=(?<y2>.*)", RegexOptions.Compiled);
    private SensorBeaconInfo ParseInput(string instruction)
    {
        var match = inputRegex.Match(instruction);
        if (!match.Success)
            throw new ApplicationException("Input line does not match the pattern");
        var sensorPosition = new Point(
            (int)Convert.ChangeType(match.Groups[1].Value, typeof(int)),
            (int)Convert.ChangeType(match.Groups[2].Value, typeof(int)));
        var nearestBeacon = new Point(
            (int)Convert.ChangeType(match.Groups[3].Value, typeof(int)),
            (int)Convert.ChangeType(match.Groups[4].Value, typeof(int)));
        return new SensorBeaconInfo(sensorPosition, nearestBeacon);
    }



    [Theory]
    [InlineData(1, "example1", 10)]
    [InlineData(1, "input", 2000000)]
    public void Part1(int part, string inputName, int yPosition)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetHowManyPositionsCantContainBeacon(input, yPosition);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1", 0, 20)]
    [InlineData(2, "input", 0, 4000000)]
    public void Part2(int part, string inputName, int lowerValue, int upperValue)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetTuningFreqencyOfDistressBeacon(input, lowerValue, upperValue);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private int GetHowManyPositionsCantContainBeacon(List<SensorBeaconInfo> input, int yPosition)
    {
        var (x1, x2) = GetXMinMax(input.Select(l => (l.SensorLocation.X, l.Distance)));
        var startPoint = new Point(x1, yPosition);
        var endPoint = new Point(x2, yPosition);

        var numPointsNotPresent = GetAllPointsBetween(startPoint, endPoint)
            .Where(PointIsInDetectionRange)
            .Count();
        return numPointsNotPresent;

        bool PointIsInDetectionRange(Point point)
        {
            return input
                .Where(l => l.BeaconLocation != point)
                .Select(l => new { l.SensorLocation, l.Distance, myDist = l.DistanceFromSensor(point) })
                .Any(l => l.Distance >= l.myDist);
        }
    }

    private int GetTuningFreqencyOfDistressBeacon(List<SensorBeaconInfo> input, int lowerValue, int upperValue)
    {
        var (x1, x2) = GetXMinMax(input.Select(l => (l.SensorLocation.X, l.Distance)));
        x1 = Math.Clamp(x1, lowerValue, upperValue);
        x2 = Math.Clamp(x2, lowerValue, upperValue);
        var startPoint = new Point(x1, lowerValue);
        var endPoint = new Point(x2, upperValue);

        var DistressBeaconLocation = GetAllPointsBetween(startPoint, endPoint)
            .Where(IsPossibleBeaconLocation)
            .First();

        var tuningFrequency = (DistressBeaconLocation.X * 4000000) + DistressBeaconLocation.Y;
        return tuningFrequency;

        bool IsPossibleBeaconLocation(Point point)
        {
            return input
                .Select(l => new { l.SensorLocation, l.Distance, myDist = l.DistanceFromSensor(point) })
                .All(l => l.Distance < l.myDist);
        }
    }

    private static (int start, int end) GetXMinMax(IEnumerable<(int xPosition, int distance)> input)
    {
        var allXindexes = input
            .Select(i => i.xPosition - i.distance)
            .Union(input.Select(i => i.xPosition + i.distance))
            .ToList();

        return (
            allXindexes.Min(),
            allXindexes.Max()
        );
    }

    private static IEnumerable<Point> GetAllPointsBetween(Point start, Point end)
    {
        for (var y = start.Y; y <= end.Y; y++)
        {
            for (var x = start.X; x <= end.X; x++)
            {
                yield return new Point(x, y);
            }
        }
    }



    private record SensorBeaconInfo
    {
        public SensorBeaconInfo(Point sensorLocation, Point beaconLocation)
        {
            SensorLocation = sensorLocation;
            BeaconLocation = beaconLocation;
            Distance = CalcManhattenDistance(sensorLocation, beaconLocation);
        }


        public Point SensorLocation { get; private set; }
        public Point BeaconLocation { get; private set; }

        public int Distance { get; init; }

        private int CalcManhattenDistance(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        public int DistanceFromSensor(Point a)
        {
            return CalcManhattenDistance(a, SensorLocation);
        }
    }
}