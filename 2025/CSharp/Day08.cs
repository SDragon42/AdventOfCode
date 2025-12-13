using Point3D = (int x, int y, int z);

namespace AdventOfCode.CSharp.Year2025;

public class Day08(ITestOutputHelper output)
{
    private const int DAY = 8;

    private (Point3D[] input, long? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadLines(DAY, inputName)
                                      .Select(l => l.Split(',').Select(int.Parse).ToArray())
                                      .Select(arr => (x: arr[0], y: arr[1], z: arr[2]))
                                      .ToArray();

        var expected = TestServices.Input.ReadText(DAY, $"{inputName}-answer{part}")
                                         ?.ToInt64();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1", 10)]
    [InlineData(1, "input", 1000)]
    public void Part1(int part, string inputName, int maxConnections)
    {
        var (junctionBoxes, expected) = GetTestData(part, inputName);

        var distanceMap = BuildDistanceMap(junctionBoxes).OrderBy(d => d.Distance).ToArray();
        var circuits = junctionBoxes.Select(p => new List<Point3D> { p }).ToList();
        ConnectCircuits(distanceMap, circuits, stopCondition: () =>
        {
            maxConnections--;
            return maxConnections <= 0;
        });
        var value = circuits.OrderByDescending(c => c.Count)
                            .Take(3)
                            .Select(c => c.Count)
                            .Aggregate(1L, (a, b) => a * b);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (junctionBoxes, expected) = GetTestData(part, inputName);

        var distanceMap = BuildDistanceMap(junctionBoxes).OrderBy(d => d.Distance).ToArray();
        var circuits = junctionBoxes.Select(p => new List<Point3D> { p }).ToList();
        Point3D p1 = default;
        Point3D p2 = default;
        ConnectCircuits(distanceMap, circuits, onMergedCircuits: (point1, point2, currentCircuits) =>
        {
            if (currentCircuits.Count == 1)
            {
                p1 = point1;
                p2 = point2;
            }
        });
        var value = (long)p1.x * p2.x;

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }




    private IEnumerable<(double Distance, Point3D P1, Point3D P2)> BuildDistanceMap(Point3D[] junctionBoxes)
    {
        for (int i = 0; i < junctionBoxes.Length; i++)
        {
            for (int j = i + 1; j < junctionBoxes.Length; j++)
            {
                var dist = GetStraitLineDistance(junctionBoxes[i], junctionBoxes[j]);
                yield return (dist, junctionBoxes[i], junctionBoxes[j]);
            }
        }
    }

    private void ConnectCircuits((double Distance, Point3D P1, Point3D P2)[] distanceMap, List<List<Point3D>> circuits, 
                                 Func<bool>? stopCondition = null, 
                                 Action<Point3D, Point3D, List<List<Point3D>>>? onMergedCircuits = null)
    {
        foreach (var (distance, p1, p2) in distanceMap)
        {
            var circuit1 = circuits.FirstOrDefault(c => c.Contains(p1));
            var circuit2 = circuits.FirstOrDefault(c => c.Contains(p2));

            if (circuit1 is null && circuit2 is null)
            {
                circuits.Add([p1, p2]); // make a new circuit
            }
            else if (circuit1 is not null && circuit2 is null)
            {
                circuit1.Add(p2); // add to existing circuit
            }
            else if (circuit1 is null && circuit2 is not null)
            {
                circuit2.Add(p1); // add to existing circuit
            }
            else if (circuit1 is not null && circuit2 is not null && circuit1 != circuit2)
            {
                circuit1.AddRange(circuit2);
                circuits.Remove(circuit2);
                onMergedCircuits?.Invoke(p1, p2, circuits);
            }

            var stopConditionMet = stopCondition?.Invoke() ?? false;
            if (stopConditionMet)
            {
                break;
            }
        }
    }

    private double GetStraitLineDistance(Point3D p1, Point3D p2)
        => Math.Sqrt(Math.Pow(p1.x - p2.x, 2) +
                     Math.Pow(p1.y - p2.y, 2) +
                     Math.Pow(p1.z - p2.z, 2));
}
