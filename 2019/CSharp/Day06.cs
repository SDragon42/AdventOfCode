namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/6
/// </summary>
public class Day06
{

    public Dictionary<string, OrbitPair> BuildOrbitDictionary(IList<string> input)
    {
        var uniqueBodies = new Dictionary<string, OrbitPair>();

        // Built Orbit Tree
        var orbitMap = input
            .Select(m => m.Split(')'))
            .Select(p => new { centerOfMass = p[0], satalite = p[1] })
            .ToList();

        var coms = orbitMap.Select(m => m.centerOfMass);
        var sats = orbitMap.Select(m => m.satalite);
        uniqueBodies = coms.Union(sats)
            .Distinct()
            .ToDictionary(key => key, centerOfMass => new OrbitPair(centerOfMass));

        foreach (var orbit in orbitMap)
        {
            var comObj = uniqueBodies[orbit.centerOfMass];
            var orbitObj = uniqueBodies[orbit.satalite];
            orbitObj.Orbits = comObj;
        }

        return uniqueBodies;
    }



    public int RunPart1(IList<string> input)
    {
        var uniqueBodies = BuildOrbitDictionary(input);
        var result = CountAllOrbits(uniqueBodies);
        return result;
    }

    public int RunPart2(IList<string> input)
    {
        var uniqueBodies = BuildOrbitDictionary(input);
        var meToCommonCOM = GetPathToUniversalCOM(uniqueBodies["YOU"].Orbits).ToList();
        var destToCommonCOM = GetPathToUniversalCOM(uniqueBodies["SAN"].Orbits).ToList();

        RemoveSharedCOMs(meToCommonCOM, destToCommonCOM);

        var result = meToCommonCOM.Count + destToCommonCOM.Count;
        return result;
    }



    int CountAllOrbits(Dictionary<string, OrbitPair> uniqueBodies)
    {
        var result = uniqueBodies.Keys.Sum(key => CountChain(uniqueBodies[key]));
        return result;
    }

    int CountChain(OrbitPair item)
    {
        if (item.Orbits == null)
            return 0;
        return 1 + CountChain(item.Orbits);
    }


    IEnumerable<OrbitPair> GetPathToUniversalCOM(OrbitPair obj)
    {
        while (obj != null)
        {
            yield return obj;
            obj = obj.Orbits;
        }
    }

    void RemoveSharedCOMs(List<OrbitPair> meToCommonCOM, List<OrbitPair> destToCommonCOM)
    {
        while (true)
        {
            var last1 = meToCommonCOM.LastOrDefault();
            var last2 = destToCommonCOM.LastOrDefault();

            if (last1 != last2)
                return;

            meToCommonCOM.Remove(last1);
            destToCommonCOM.Remove(last2);
        }
    }


    public class OrbitPair
    {
        public OrbitPair(string centerOfMass)
        {
            CenterOfMass = centerOfMass;
        }

        public string CenterOfMass { get; private set; }

        public OrbitPair Orbits { get; set; }
    }

}
