namespace AdventOfCode.CSharp.Year2023;



using Number = long;

file static class Extentions
{
    public static Number ToNumber(this string text) => Number.Parse(text);
}



public class Day05
{
    private const int DAY = 5;

    private readonly ITestOutputHelper output;
    public Day05(ITestOutputHelper output) => this.output = output;



    private class InputMaps
    {
        public InputMaps(IEnumerator<string> en)
        {
            var mapIndex = -1;
            while (en.MoveNext())
            {
                if (en.Current.Length == 0)
                    continue;

                if (en.Current.IndexOf(':') >= 0)
                {
                    mapIndex++;
                    _maps[mapIndex] = new MapData();
                    continue;
                }

                var nums = en.Current.Split(' ').Select(s => s.ToNumber()).ToArray();
                var mapRec = new RangeData(nums[0], nums[1], nums[2]);
                _maps[mapIndex].AddMap(mapRec);
            }

        }

        private readonly MapData[] _maps = new MapData[7];

        public MapData SeedToSoil => _maps[0];
        public MapData SoilToFertilizer => _maps[1];
        public MapData FertilizerToWater => _maps[2];
        public MapData WaterToLight => _maps[3];
        public MapData LightToTemperature => _maps[4];
        public MapData TemperatureToHumidity => _maps[5];
        public MapData HumidityToLocation => _maps[6];
    }

    private class MapData()
    {
        private readonly List<RangeData> _map = new();

        public void AddMap(RangeData map)
        {
            _map.Add(map);
        }

        public Number GetDestinationNumber(Number source)
        {
            var result = _map
                .Select(a => a.GetDestinationNum(source))
                .Where(a => a.HasValue)
                .FirstOrDefault();
            return result ?? source;
        }
    }

    private class RangeData
    {
        public Number DestinationStart { get; init; }
        public Number SourceStart { get; init; }
        public Number Length { get; init; }
        public Number Delta { get; init; }

        public RangeData(Number destinationStart, Number sourceStart, Number length)
        {
            this.DestinationStart = destinationStart;
            this.SourceStart = sourceStart;
            this.Length = length;

            //this.range = [sourceStart..(sourceStart+1)];
            this.Delta = sourceStart - destinationStart;
        }


        public Number? GetDestinationNum(Number sourceNum)
        {
            var offset = sourceNum - SourceStart;
            var value = DestinationStart + offset;
            var result = value >= DestinationStart && value < DestinationStart + Length ? value : (Number?)null;
            return result;
        }
    }


    private (Number[] seeds, InputMaps maps, Number? expected) GetTestData(int part, string inputName)
    {
        var lines = Services.Input.ReadLines(DAY, inputName);

        var en = lines.GetEnumerator();
        en.MoveNext();
        var seeds = en.Current.Split(' ').Skip(1).Select(s => s.ToNumber()).ToArray();

        var maps = new InputMaps(en);

        var expected = Services.Input.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToNumber();

        return (seeds, maps, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (seeds, maps, expected) = GetTestData(part, inputName);

        var value = GetTheClosestLocation(seeds, maps);

        output.WriteLine($"Answer: {value}");
        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(2, "example1")]
    //[InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (seeds, maps, expected) = GetTestData(part, inputName);

        var allSeeds = CalcAllSeeds(seeds);
        var value = GetTheClosestLocationWithCache(allSeeds, maps);

        output.WriteLine($"Answer: {value}");
        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(79, 81)]
    [InlineData(14, 14)]
    [InlineData(55, 57)]
    [InlineData(13, 13)]
    public void Test_SeedToSoilNum(Number seedNumber, Number expectedSoilNumber)
    {
        var (_, maps, _) = GetTestData(1, "example1");

        var soilNumber = maps.SeedToSoil.GetDestinationNumber(seedNumber);

        output.WriteLine($"Seed: {seedNumber}    Soil: {soilNumber}");
        Assert.Equal(expectedSoilNumber, soilNumber);
    }



    private Number GetTheClosestLocation(IEnumerable<Number> seeds, InputMaps maps)
    {
        var q = seeds
            .Select(seed => (seed, soil: maps.SeedToSoil.GetDestinationNumber(seed)))
            .Select(t => (t.seed, t.soil, fertilizer: maps.SoilToFertilizer.GetDestinationNumber(t.soil)))
            .Select(t => (t.seed, t.soil, t.fertilizer, water: maps.FertilizerToWater.GetDestinationNumber(t.fertilizer)))
            .Select(t => (t.seed, t.soil, t.fertilizer, t.water, light: maps.WaterToLight.GetDestinationNumber(t.water)))
            .Select(t => (t.seed, t.soil, t.fertilizer, t.water, t.light, temperature: maps.LightToTemperature.GetDestinationNumber(t.light)))
            .Select(t => (t.seed, t.soil, t.fertilizer, t.water, t.light, t.temperature, humidity: maps.TemperatureToHumidity.GetDestinationNumber(t.temperature)))
            .Select(t => (t.seed, t.soil, t.fertilizer, t.water, t.light, t.temperature, t.humidity, location: maps.HumidityToLocation.GetDestinationNumber(t.humidity)))
            .ToArray();

        var result = q.Select(t => t.location).Min();

        return result;
    }

    private Number GetTheClosestLocationWithCache(IEnumerable<Number> seeds, InputMaps maps)
    {
        var result = Number.MaxValue;
        var seedsArray = seeds.Distinct().ToArray();

        var history = new Dictionary<string, HashSet<Number>>()
        {
            { "seed", new HashSet<Number>() },
            { "soil", new HashSet<Number>() },
            { "fertilizer", new HashSet<Number>() },
            { "water", new HashSet<Number>() },
            { "light", new HashSet<Number>() },
            { "temperature", new HashSet<Number>() },
            { "humidity", new HashSet<Number>() },
        };

        foreach (var seed in seedsArray)
        {
            if (history["seed"].Contains(seed))
                continue;

            var soil = maps.SeedToSoil.GetDestinationNumber(seed);
            if (AddToHistory("soil", soil)) continue;

            var fertilizer = maps.SoilToFertilizer.GetDestinationNumber(soil);
            if (AddToHistory("fertilizer", fertilizer)) continue;

            var water = maps.FertilizerToWater.GetDestinationNumber(fertilizer);
            if (AddToHistory("water", water)) continue;

            var light = maps.WaterToLight.GetDestinationNumber(water);
            if (AddToHistory("light", light)) continue;

            var temperature = maps.LightToTemperature.GetDestinationNumber(light);
            if (AddToHistory("temperature", temperature)) continue;

            var humidity = maps.TemperatureToHumidity.GetDestinationNumber(temperature);
            if (AddToHistory("humidity", humidity)) continue;

            var location = maps.HumidityToLocation.GetDestinationNumber(humidity);

            result = Number.Min(result, location);
        }

        return result;

        bool AddToHistory(string key, Number value)
        {
            if (history[key].Contains(value))
                return true;
            history[key].Add(value);
            return false;
        }
    }

    private IEnumerable<Number> CalcAllSeeds(Number[] seeds)
    {
        var i = 0;
        while (i + 1 < seeds.Length)
        {
            var num = seeds[i];
            var end = seeds[i] + seeds[i + 1];

            while (num < end)
                yield return num++;

            i += 2;
        }
    }

}