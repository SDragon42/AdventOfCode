namespace AdventOfCode.CSharp.Year2022;

public class Day13_Distress_Signal
{
    private const int DAY = 13;

    private readonly ITestOutputHelper output;
    public Day13_Distress_Signal(ITestOutputHelper output) => this.output = output;



    private (List<string> input, int? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToList();

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

        var value = GetSumOfRightPairIndexes(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetDecoderKey(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private int GetSumOfRightPairIndexes(List<string> packets)
    {
        var result = GetPacketPairs(packets)
            .Where(pair => Compare(pair.left, pair.right) < 0)
            .Select(pair => pair.index)
            .Sum();

        return result;
    }

    private IEnumerable<(string left, string right, int index)> GetPacketPairs(List<string> packets)
    {
        var index = 1;
        var enumerator = packets.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var left = enumerator.Current;
            enumerator.MoveNext();
            var right = enumerator.Current;

            yield return (left, right, index++);
        }
    }

    private int GetDecoderKey(List<string> packets)
    {
        const string divider1 = "[[2]]";
        const string divider2 = "[[6]]";

        packets.Add(divider1);
        packets.Add(divider2);

        packets.Sort(Compare);

        var idx1 = packets.IndexOf(divider1) + 1;
        var idx2 = packets.IndexOf(divider2) + 1;

        var result = idx1 * idx2;
        return result;
    }


    private int Compare(string a, string b)
    {
        return CompareJson(
            JsonSerializer.Deserialize<JsonElement>(a),
            JsonSerializer.Deserialize<JsonElement>(b));
    }

    private int CompareJson(JsonElement a, JsonElement b)
    {
        switch (a.ValueKind, b.ValueKind)
        {
            case (JsonValueKind.Number, JsonValueKind.Number):
                return a.GetInt32() - b.GetInt32();

            case (JsonValueKind.Number, JsonValueKind.Array):
                return CompareJson(
                    JsonSerializer.Deserialize<JsonElement>("[" + a.GetRawText() + "]"),
                    b);

            case (JsonValueKind.Array, JsonValueKind.Number):
                return CompareJson(
                    a,
                    JsonSerializer.Deserialize<JsonElement>("[" + b.GetRawText() + "]"));

            case (JsonValueKind.Array, JsonValueKind.Array):
                var e1 = a.EnumerateArray();
                var e2 = b.EnumerateArray();

                while (e1.MoveNext() && e2.MoveNext())
                {
                    var result = CompareJson(e1.Current, e2.Current);
                    if (result != 0)
                        return result;
                }
                return a.GetArrayLength() - b.GetArrayLength();

            default:
                throw new ApplicationException("Unexpected JsonValueKind type");
        }
    }

}