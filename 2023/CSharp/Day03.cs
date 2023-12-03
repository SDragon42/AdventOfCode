namespace AdventOfCode.CSharp.Year2023;

public class Day03
{
    private const int DAY = 3;

    private readonly ITestOutputHelper output;
    public Day03(ITestOutputHelper output) => this.output = output;



    private (char[][] input, int? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName)
            .Select(l => l.ToArray())
            .ToArray();

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetSumOfPartNumbers(input);

        output.WriteLine($"Answer: {value}");
        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetSumOfGearRatios(input);

        output.WriteLine($"Answer: {value}");
        Assert.Equal(expected, value);
    }



    private int GetSumOfPartNumbers(char[][] input)
    {
        var symbolPositions = GetSymbols(input).ToArray();
        var partNumbers = GetPartNumbers(input).ToArray();

        var result = partNumbers
            .Where(pn => IsNextTo(pn, symbolPositions))
            .ToArray()
            .Sum(pn => pn.PartNumber);

        return result;
    }

    private int GetSumOfGearRatios(char[][] input)
    {
        var gearPositions = GetSymbols(input)
            .Where(s => s.Symbol == '*')
            .ToArray();
        var partNumbers = GetPartNumbers(input).ToArray();

        var gearsAndParts = partNumbers
            .Select(pn => (pn, gear: GetAdjacentSymbol(pn, gearPositions)))
            .Where(z => z.gear != null)
            .ToArray();

        var r1 = gearsAndParts
            .GroupBy(
                f => f.gear!,
                v => v.pn
                )
            ;
        var points = r1.Select(a => a.Key).ToArray();
        var blah = r1.Select(a => (a.Key, a.Select(z => z.PartNumber).ToArray()))
            .ToDictionary()
            ;

        var result = points
            .Select(p => blah[p])
            .Where(z => z.Length == 2)
            .Select(z => z[0] * z[1])
            .Sum();

        return result;
    }

    private IEnumerable<SymbolData> GetSymbols(char[][] input)
    {
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                var symbol = input[y][x];
                if (symbol != '.' && !char.IsDigit(symbol))
                {
                    yield return new SymbolData(symbol, new Point(x, y));
                }
            }
        }
    }

    private IEnumerable<PnData> GetPartNumbers(char[][] input)
    {
        for (var y = 0; y < input.Length; y++)
        {
            var pnStart = -1;
            var pnText = string.Empty;
            for (var x = 0; x < input[y].Length; x++)
            {
                if (char.IsDigit(input[y][x]))
                {
                    if (pnStart < 0)
                        pnStart = x;
                    pnText += input[y][x];
                    continue;
                }

                if (pnText.Length > 0)
                {
                    yield return MakeReturn(pnStart, y, x, pnText);
                    pnStart = -1;
                    pnText = string.Empty;
                }
            }

            if (pnText.Length > 0)
            {
                yield return MakeReturn(pnStart, y, input[y].Length - 1, pnText);
            }
        }

        PnData MakeReturn(int xStart, int y, int xEnd, string value)
        {
            return new PnData(
                value.ToInt32(),
                new Rectangle(xStart, y, xEnd - xStart, 1)
            );
        }
    }

    private bool IsNextTo(PnData pn, IEnumerable<SymbolData> symbolPositions)
    {
        var newRect = Rectangle.Inflate(pn.Rect, 1, 1);

        var result = symbolPositions
            .Select(s => s.Position)
            .Any(newRect.Contains);

        return result;
    }

    private SymbolData? GetAdjacentSymbol(PnData pn, SymbolData[] symbolPositions)
    {
        var newRect = Rectangle.Inflate(pn.Rect, 1, 1);

        var result = symbolPositions
            .Where(s => newRect.Contains(s.Position))
            .FirstOrDefault();

        return result;
    }



    private record PnData(int PartNumber, Rectangle Rect);

    private record SymbolData(char Symbol, Point Position);

}