namespace AdventOfCode.CSharp.Year2023;

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

                var nums = en.Current.Split(' ').Select(s => s.ToInt64()).ToArray();
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

        public long GetDestinationNumber(long source)
        {
            var result = _map
                .Select(a => a.GetDestinationNum(source))
                .Where(a => a.HasValue)
                .FirstOrDefault();
            return result ?? source;
        }
    }

    private class RangeData(long destinationStart, long sourceStart, long length)
    {
        public long? GetDestinationNum(long sourceNum)
        {
            var offset = sourceNum - sourceStart;
            var value = destinationStart + offset;
            var result = value >= destinationStart && value < destinationStart + length ? value : -1;
            return (result > 0) ? result : null;
        }
    }


    private (long[] seeds, InputMaps maps, long? expected) GetTestData(int part, string inputName)
    {
        var lines = InputHelper.ReadLines(DAY, inputName);

        var en = lines.GetEnumerator();
        en.MoveNext();
        var seeds = en.Current.Split(' ').Skip(1).Select(s => s.ToInt64()).ToArray();

        var maps = new InputMaps(en);

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt64();

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
    [InlineData(79, 81)]
    [InlineData(14, 14)]
    [InlineData(55, 57)]
    [InlineData(13, 13)]
    public void Test_SeedToSoilNum(long seedNumber, long expectedSoilNumber)
    {
        var (_, maps, _) = GetTestData(1, "example1");

        var soilNumber = maps.SeedToSoil.GetDestinationNumber(seedNumber);

        output.WriteLine($"Seed: {seedNumber}    Soil: {soilNumber}");
        Assert.Equal(expectedSoilNumber, soilNumber);
    }



    private long GetTheClosestLocation(IEnumerable<long> seeds, InputMaps maps)
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

}