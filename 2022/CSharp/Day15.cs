using Newtonsoft.Json.Linq;

namespace AdventOfCode.CSharp.Year2022;
/// <summary>
/// I used this example for day 15 to figure out Part 2:
/// https://gist.github.com/nathanbelles/30adf58fe199759590bf41dc7a471d01
/// Their code is still faster by far.
/// </summary>
public class Day15_Beacon_Exclusion_Zone
{
    private const int DAY = 15;

    private readonly ITestOutputHelper output;
    public Day15_Beacon_Exclusion_Zone(ITestOutputHelper output) => this.output = output;



    private (List<SensorBeaconInfo> input, long? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadLines(DAY, inputName)
            .Select(ParseInput)
            .ToList();

        var expected = TestServices.Input.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt64();

        return (input, expected);
    }

    private Regex inputRegex = new Regex("Sensor at x=(?<x>.*), y=(?<y>.*): closest beacon is at x=(?<x2>.*), y=(?<y2>.*)", RegexOptions.Compiled);
    private SensorBeaconInfo ParseInput(string instruction)
    {
        var match = inputRegex.Match(instruction);
        if (!match.Success)
            throw new ApplicationException("Input line does not match the pattern");

        var sensor = new Point(
            (int)Convert.ChangeType(match.Groups[1].Value, typeof(int)),
            (int)Convert.ChangeType(match.Groups[2].Value, typeof(int)));
        
        var beacon = new Point(
            (int)Convert.ChangeType(match.Groups[3].Value, typeof(int)),
            (int)Convert.ChangeType(match.Groups[4].Value, typeof(int)));
        
        return new SensorBeaconInfo() {
            Sensor = sensor,
            Beacon = beacon,
            Distance = CalcManhattenDistance(sensor, beacon)
        };
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
        var (x1, x2) = GetMinMaxValues(input.Select(l => (l.Sensor.X, l.Distance)));
        var startPoint = new Point(x1, yPosition);
        var endPoint = new Point(x2, yPosition);

        var numPointsNotPresent = GetAllPointsBetween(startPoint, endPoint)
            .Where(p => PointIsInDetectionRange(input, p))
            .Count();
        return numPointsNotPresent;

        bool PointIsInDetectionRange(List<SensorBeaconInfo> input, Point point)
        {
            return input
                .Where(l => l.Beacon != point)
                .Select(l => new { l.Sensor, l.Distance, myDist = CalcManhattenDistance(l.Sensor, point) })
                .Any(l => l.Distance >= l.myDist);
        }
    }

    private long GetTuningFreqencyOfDistressBeacon(List<SensorBeaconInfo> input, int lowerValue, int upperValue)
    {
        var (x1, x2) = GetMinMaxValues(input.Select(l => (l.Sensor.X, l.Distance)));
        x1 = Math.Clamp(x1, lowerValue, upperValue);
        x2 = Math.Clamp(x2, lowerValue, upperValue);
        var startPoint = new Point(x1, lowerValue);
        var endPoint = new Point(x2, upperValue);

        var list1 = GetAllPointsToCheck(input, startPoint, endPoint, lowerValue, upperValue);
        var list2 = list1.Where(IsPossibleBeaconLocation).ToList();

        var DistressBeaconLocation = list2.First();

        var tuningFrequency = (DistressBeaconLocation.X * 4000000L) + DistressBeaconLocation.Y;
        return tuningFrequency;

        bool IsPossibleBeaconLocation(Point point)
        {
            return input
                .Select(l => new { l.Sensor, l.Distance, myDist = CalcManhattenDistance(l.Sensor, point) })
                .All(l => l.Distance < l.myDist);
        }
    }

    private static (int min, int max) GetMinMaxValues(IEnumerable<(int value, int distance)> input)
    {
        var allXindexes = input
            .Select(i => i.value - i.distance)
            .Union(input.Select(i => i.value + i.distance))
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

    private static HashSet<Point> GetAllPointsToCheck(List<SensorBeaconInfo> input, Point start, Point end, int lowerValue, int upperValue)
    {
        var toCheck = new HashSet<Point>();

        for (int i = 0; i < input.Count; i++)
        {
            var circle = input[i];

            for (int j = 0; j < input.Count; j++)
            {
                if (i == j)
                    continue;

                var circle2 = input[j];
                
                if (CalcManhattenDistance(circle.Sensor, circle2.Sensor) != circle.Distance + circle2.Distance + 2)
                    continue;

                var endY = Math.Max(circle.Sensor.X + circle.Distance, circle2.Sensor.X + circle2.Distance);
                var startY = Math.Max(circle.Sensor.X - circle.Distance, circle2.Sensor.X - circle2.Distance);

                var startX = Math.Max(circle.Sensor.X - circle.Distance, circle2.Sensor.X - circle2.Distance);
                var endX = Math.Max(circle.Sensor.X + circle.Distance, circle2.Sensor.X + circle2.Distance);


                for (var y = startY; y < endY; y++)
                {
                    var xOffset = circle.Distance + 1 - Math.Abs(y - circle.Sensor.Y);
                    var x1 = circle.Sensor.X + xOffset;
                    var x2 = circle.Sensor.X - xOffset;

                    if (InValidRange(x1, startX, endX))
                        toCheck.Add(new Point(x1, y));

                    if (InValidRange(x2, startX, endX))
                        toCheck.Add(new Point(x2, y));
                }
            }
        }

        return toCheck;

        bool InValidRange(int value, int min, int max)
        {
            return (value >= lowerValue && value <= upperValue && value >= min && value <= max);
        }
    }


    private static int CalcManhattenDistance(Point a, Point b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }





    private record SensorBeaconInfo
    {
        public Point Sensor { get; init; }
        public Point Beacon { get; init; }
        public int Distance { get; init; }
    }
}