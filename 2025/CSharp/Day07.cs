namespace AdventOfCode.CSharp.Year2025;

public class Day07(ITestOutputHelper output)
{
    private const int DAY = 7;

    private (char[][] input, long? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadLines(DAY, inputName)
                                      .Select(l => l.ToCharArray())
                                      .ToArray();

        var expected = TestServices.Input.ReadText(DAY, $"{inputName}-answer{part}")
                                         ?.ToInt64();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (tachyonManifold, expected) = GetTestData(part, inputName);

        var xStart = GetStartPosition(tachyonManifold);
        var value = GetNumberOfTimeBeamIsSplit(tachyonManifold, xStart);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (tachyonManifold, expected) = GetTestData(part, inputName);

        var xStart = GetStartPosition(tachyonManifold);
        var value = GetNumberOfTimelinesTheParticleCouldTake(tachyonManifold, (xStart, 1));

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    const char START = 'S';
    const char EMPTY = '.';
    const char SPLITTER = '^';


    private int GetStartPosition(char[][] tachyonManifold) => tachyonManifold[0].IndexOf(START);
    
    public long GetNumberOfTimeBeamIsSplit(char[][] tachyonManifold, int xStart)
    {
        var splitCount = 0L;

        HashSet<int> beamIndexes = [xStart];
        var xMin = 0;
        var xMax = tachyonManifold[0].Length - 1;

        for (var y = 1; y < tachyonManifold.Length; y++)
        {
            var beams = beamIndexes.Select(x => x).ToArray();
            beamIndexes.Clear();

            foreach (var x in beams)
            {
                var c = tachyonManifold[y][x];
                switch (c)
                {
                    case SPLITTER:
                        if (x - 1 >= xMin) beamIndexes.Add(x - 1);
                        if (x + 1 >= xMin) beamIndexes.Add(x + 1);
                        splitCount++;
                        break;
                    case EMPTY:
                        beamIndexes.Add(x);
                        break;
                    default:
                        throw new Exception($"Unknown symbol [{c}] encountered in tachyon manifold.");
                }
            }
        }

        return splitCount;
    }


    public long GetNumberOfTimelinesTheParticleCouldTake(char[][] tachyonManifold, (int x, int y) position, Dictionary<(int x, int y), long> possibles = null!)
    {
        possibles ??= [];

        if (position.y >= tachyonManifold.Length)
        {
            return 1L;
        }

        if (possibles.TryGetValue(position, out var value))
        {
            return value;
        }

        var c = tachyonManifold[position.y][position.x];
        switch (c)
        {
            case SPLITTER:
                var count = GetNumberOfTimelinesTheParticleCouldTake(tachyonManifold, (position.x - 1, position.y + 1), possibles);
                count += GetNumberOfTimelinesTheParticleCouldTake(tachyonManifold, (position.x + 1, position.y + 1), possibles);

                possibles[position] = count;
                return count;
            case START:
            case EMPTY:
                return GetNumberOfTimelinesTheParticleCouldTake(tachyonManifold, (position.x, position.y + 1), possibles);
            default:
                throw new Exception($"Unknown symbol [{c}] encountered in tachyon manifold.");
        }

    }
}
