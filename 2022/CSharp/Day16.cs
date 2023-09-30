using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text;
using Newtonsoft.Json.Linq;

namespace AdventOfCode.CSharp.Year2022;

using FloydWarshallMapType = IDictionary<string, IDictionary<string, float>>;

public class Day16_Proboscidea_Volcanium
{
    private const int DAY = 16;

    private readonly ITestOutputHelper output;
    public Day16_Proboscidea_Volcanium(ITestOutputHelper output) => this.output = output;



    private (IDictionary<string, ValveState> input, int? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName)
            .Select(ParseInputLine)
            .ToDictionary(k => k.Name, v => v);

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (input, expected);
    }

    private readonly Regex inputRegex = new Regex(
        @"^Valve (?<name>.*) has flow rate=(?<flowRate>.*); tunnels? leads? to valves? (?<connectedValves>.*)$",
        RegexOptions.Compiled);
    private ValveState ParseInputLine(string line)
    {
        var match = inputRegex.Match(line);
        if (!match.Success)
            throw new ApplicationException($"Input line does not match the pattern:  {line}");

        var valve = new ValveState()
        {
            Name = match.Groups["name"].Value,
            FlowRate = (int)Convert.ChangeType(match.Groups["flowRate"].Value, typeof(int)),
            ToValues = match.Groups["connectedValves"].Value.Split(", ")
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


    private int GetMostPreasureReleased(IDictionary<string, ValveState> allValves, int numberOfMinutes)
    {
        var valveDict = allValves.ToDictionary(v => v.Key, v => v.Value.ToValues);
        var fwMap = BuildFloydWarshallMap(valveDict);

        var valvesWithFlow = allValves
            .Where(v => v.Value.FlowRate > 0)
            .ToDictionary(k => k.Key, k => k.Value);

        var i = 0;
        var I = valvesWithFlow.Keys.ToDictionary(k => k, v => 1<<i);

        //RenderMap(fwMap);

        var result = Visit("AA", numberOfMinutes, 0, 0, new Dictionary<int, float>(), 
            valvesWithFlow, fwMap, I);
        var result2 = result.Values.Max();
        return Convert.ToInt32(result2);
    }

    private static string RenderMap(FloydWarshallMapType map)
    {
        var sb = new StringBuilder();
        var keys = map.Keys;

        foreach (var title in keys)
        {
            sb.Append($"\t{title}");
        }
        sb.AppendLine();

        foreach (var row in keys)
        {
            sb.Append(row);
            foreach (var col in keys)
            {
                sb.Append($"\t{map[row][col]:N0}");
            }

            sb.AppendLine();
        }
        var result = sb.ToString();
        return result;
    }

    private IDictionary<int, float> Visit(string v, float budget, int state, float flow, IDictionary<int, float> answer,
        IDictionary<string, ValveState> F,
        FloydWarshallMapType T,
        IDictionary<string, int> I)
    {
        answer.TryGetValue(state, out var current);
        answer[state] = float.Max(current, flow);

        foreach(var u in F.Keys)
        {
            var newBudget = budget - T[v][u] - 1;
            var cond1 = (I[u] & state);
            var cond2 = (newBudget <= 0);
            if (cond1 > 0 || cond2)
            //if ((I[u] & state) > 0 || (newBudget <= 0))
                continue;
            Visit(u,
                newBudget,
                state | I[u],
                flow + newBudget * F[u].FlowRate,
                answer,
                F, T, I);
        }

        return answer;
    }

    private FloydWarshallMapType BuildFloydWarshallMap(IDictionary<string, string[]> G) 
    {
        var map = new Dictionary<string, IDictionary<string, float>>();

        foreach (var x in G.Keys)
        {
            var xDict = new Dictionary<string, float>();
            map.Add(x, xDict);
            foreach (var y in G.Keys)
            {
                var value = G[x].Contains(y)
                    ? 1
                    : float.PositiveInfinity;
                xDict.Add(y, value);
            }
        }

        // calculate map
        foreach (var k in map.Keys)
        {
            foreach (var i in map.Keys)
            {
                foreach (var j in map.Keys)
                {
                    map[i][j] = float.Min(
                        map[i][j],
                        map[i][k] + map[k][j]);
                }
            }
        }

        return map;
    }



    //private int GetMostPreasureReleased(IDictionary<string, ValveState> allValves, int numberOfMinutes)
    //{
    //    var dist = GetShortestDistBetweenValves(allValves);

    //    //var indexMap = allValves
    //    //    .OrderBy(kvp => kvp.Key)
    //    //    .Select((kvp, index) => (kvp.Key, index))
    //    //    .ToDictionary(aa => aa.Key, aa => aa.index);

    //    //var valvesToOpen = allValves
    //    //    .Where(kv => kv.Value.FlowRate > 0)
    //    //    .ToDictionary(k => k.Key, v => v.Value);

    //    var releasedPresure = new Dictionary<string, int>();
    //    //releasedPresure.Add("AA", 0);

    //    while (numberOfMinutes-- > 0)
    //    {

    //        foreach (var key in releasedPresure.Keys)
    //            releasedPresure[key] += allValves[key].FlowRate;
    //    }

    //    var result = releasedPresure.Values.Sum();
    //    return result;
    //}

    //private IDictionary<(string, string), int> GetShortestDistBetweenValves(IDictionary<string, ValveState> allValves)
    //{
    //    var dist = new Dictionary<(string, string), int>();

    //    foreach (var i in allValves.Keys)
    //    {
    //        foreach (var j in allValves.Keys)
    //        {
    //            var visited = new HashSet<string>();
    //            var q = new Queue<(string node, int l)>();
    //            q.Enqueue((i, 0));

    //            while (q.Count > 0)
    //            {
    //                var cur = q.Dequeue();
    //                if (cur.node == j)
    //                {
    //                    dist[(i, j)] = cur.l;
    //                    break;
    //                }

    //                foreach (var n in allValves[cur.node].ToValues)
    //                {
    //                    if (visited.Add(n))
    //                        q.Enqueue((n, cur.l + 1));
    //                }
    //            }
    //        }
    //    }

    //    return dist;
    //}


    class ValveState
    {
        public required string Name { get; init; }
        public required int FlowRate { get; init; }
        public bool IsOpen { get; set; } = false;

        public required string[] ToValues { get; init; }
    }


}