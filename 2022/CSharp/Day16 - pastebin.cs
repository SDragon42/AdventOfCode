using System.Collections.ObjectModel;

namespace AdventOfCode.CSharp.Year2022;

public class Day16_PasteBin
{
    private const int DAY = 16;

    private readonly ITestOutputHelper output;
    public Day16_PasteBin(ITestOutputHelper output) => this.output = output;



    private (IDictionary<string, Valves> input, int? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadLines(DAY, inputName)
            .Select(ParseInputLine)
            .ToDictionary(k => k.Name, v => v);

        var expected = TestServices.Input.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (input, expected);
    }

    private readonly Regex inputRegex = new Regex(
        @"^Valve (?<name>.*) has flow rate=(?<flowRate>.*); tunnels? leads? to valves? (?<connectedValves>.*)$",
        RegexOptions.Compiled);
    private Valves ParseInputLine(string line)
    {
        var match = inputRegex.Match(line);
        if (!match.Success)
            throw new ApplicationException($"Input line does not match the pattern:  {line}");

        var valve = new Valves()
        {
            Name = match.Groups["name"].Value,
            Rate = (int)Convert.ChangeType(match.Groups["flowRate"].Value, typeof(int)),
            connected = match.Groups["connectedValves"].Value.Split(", ")
        };

        return valve;
    }



    [Theory]
    [InlineData(1, "example1")]
    //[InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        // Show input was read correctly
        //foreach (var item in input)
        //    output.WriteLine($"{item.Value.Name}    {item.Value.FlowRate,2}    {string.Join(',', item.Value.ToValues)}");

        var value = GetMostPreasureReleased(input, 30);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    //[Theory]
    //[InlineData(2, "example1")]
    //[InlineData(2, "input")]
    //public void Part2(int part, string inputName)
    //{
    //    var (input, expected) = GetTestData(part, inputName);

    //    var value = GetMostPreasureReleased(input);

    //    output.WriteLine($"Answer: {value}");

    //    Assert.Equal(expected, value);
    //}



    private int GetMostPreasureReleased(IDictionary<string, Valves> allValves, int numberOfMinutes)
    {
        var dist = GetShortestDistBetweenValves(allValves);

        int max = 0;
        var curPath = new List<string>();
        var paths = new Dictionary<HashSet<string>, int>();
        int available = allValves.Where(v => v.Value.Rate > 0 && !v.Value.isOpen).Count();

        Backtrack(allValves, dist, numberOfMinutes, "AA", 0, 0, available, ref max, curPath, paths);

        return max;
    }


    private IDictionary<(string, string), int> GetShortestDistBetweenValves(IDictionary<string, Valves> allValves)
    {
        var dist = new Dictionary<(string, string), int>();

        foreach (var i in allValves.Keys)
        {
            foreach (var j in allValves.Keys)
            {
                var q = new Queue<(string node, int l)>();
                q.Enqueue((i, 0));

                var visited = new HashSet<string>();

                while (q.Count > 0)
                {
                    var cur = q.Dequeue();
                    if (cur.node == j)
                    {
                        dist[(i, j)] = cur.l;
                        break;
                    }

                    foreach (var n in allValves[cur.node].connected)
                    {
                        if (visited.Add(n))
                            q.Enqueue((n, cur.l + 1));
                    }
                }
            }
        }

        return dist;
    }

    void Backtrack(
        IDictionary<string, Valves> valves, 
        IDictionary<(string, string), int> dist, 
        int time, string cur, int pressure, int cumulpressure, int available, ref int max, 
        List<string> curPath, 
        Dictionary<HashSet<string>, int> paths)
    {
        if (time == 0 || available == 0)
        {
            max = Math.Max(max, cumulpressure + (pressure * time));
            return;
        }

        paths.Add(new HashSet<string>(curPath), cumulpressure + (pressure * time));

        foreach (var n in valves.Keys)
        {
            if (n == cur || valves[n].isOpen || valves[n].Rate == 0)
                continue;

            var ttg = dist[(n, cur)] + 1;
            if (ttg > time)
                ttg = time;

            valves[n].isOpen = true;
            curPath.Add(n);
            Backtrack(valves, dist, time - ttg, n, pressure + valves[n].Rate, cumulpressure + (pressure * ttg), available - 1, ref max, curPath, paths);
            curPath.RemoveAt(curPath.Count - 1);
            valves[n].isOpen = false;
        }
    }


    class Valves
    {
        public required string Name { get; init; }
        public required int Rate { get; init; }
        public required string[] connected { get; init; }

        public bool isOpen { get; set; } = false;
    }

}
