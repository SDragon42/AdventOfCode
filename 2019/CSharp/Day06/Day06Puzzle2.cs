using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.CSharp.Year2019.Day06
{
    /*
    --- Part Two ---
    Now, you just need to figure out how many orbital transfers you (YOU) need to take to get to
    Santa (SAN).

    You start at the object YOU are orbiting; your destination is the object SAN is orbiting. 
    An orbital transfer lets you move from any object to an object orbiting or orbited by that
    object.

    For example, suppose you have the following map:

    COM)B
    B)C
    C)D
    D)E
    E)F
    B)G
    G)H
    D)I
    E)J
    J)K
    K)L
    K)YOU
    I)SAN

    Visually, the above map of orbits looks like this:

                              YOU
                             /
            G - H       J - K - L
           /           /
    COM - B - C - D - E - F
                   \
                    I - SAN

    In this example, YOU are in orbit around K, and SAN is in orbit around I. To move from K to I,
    a minimum of 4 orbital transfers are required:

    K to J
    J to E
    E to D
    D to I

    Afterward, the map of orbits looks like this:

            G - H       J - K - L
           /           /
    COM - B - C - D - E - F
                   \
                    I - SAN
                     \
                      YOU

    What is the minimum number of orbital transfers required to move from the object YOU are
    orbiting to the object SAN is orbiting? (Between the objects they are orbiting - not between
    YOU and SAN.)
    */
    class Day06Puzzle2 : IPuzzle
    {
        public void Run()
        {
            Console.WriteLine("--- Day 6: Universal Orbit Map (Part 2) ---");


            BuildOrbitTree(Day06Common.OrbitMap);


            var weAreHere = uniqueBodies["YOU"].Orbits;
            var weGoHere = uniqueBodies["SAN"].Orbits;

            var foundTransfers = 0; //CountAllOrbits();

            var areHere2COM = GetPathToUniversalCOM(weAreHere);
            var goHere2COM = GetPathToUniversalCOM(weGoHere);

            RemoveShared(areHere2COM, goHere2COM);

            foundTransfers = areHere2COM.Count + goHere2COM.Count;

            Console.WriteLine($"Orbit Transfers found: {foundTransfers}");
            if (Day06Common.ExpectedOrbitTransfers >= 0)
                Console.WriteLine("    " + (Day06Common.ExpectedOrbitTransfers == foundTransfers ? "CORRECT" : "You done it wrong!"));
            Console.WriteLine();
        }



        readonly Dictionary<string, OrbitPair> uniqueBodies = new Dictionary<string, OrbitPair>();

        void BuildOrbitTree(IEnumerable<string> orbitMapData)
        {
            uniqueBodies.Clear();

            var orbitMap = orbitMapData
                .Select(m => m.Split(')'))
                .Select(p => new { com = p[0], satalite = p[1] })
                .ToList();

            var coms = orbitMap.Select(m => m.com);
            var sats = orbitMap.Select(m => m.satalite);
            var uniqueObjects = coms.Union(sats).Distinct();
            foreach (var com in uniqueObjects)
                uniqueBodies.Add(com, new OrbitPair(com));

            foreach (var orbit in orbitMap)
            {
                var comObj = uniqueBodies[orbit.com];
                var orbitObj = uniqueBodies[orbit.satalite];
                orbitObj.Orbits = comObj;
            }

        }

        List<OrbitPair> GetPathToUniversalCOM(OrbitPair obj)
        {
            var path = new List<OrbitPair>();
            do
            {
                path.Add(obj);
                obj = obj.Orbits;
            } while (obj != null);
            return path;
        }

        private void RemoveShared(List<OrbitPair> areHere2COM, List<OrbitPair> goHere2COM)
        {
            while (true)
            {
                var last1 = areHere2COM.LastOrDefault();
                var last2 = goHere2COM.LastOrDefault();

                if (last1 != last2)
                    return;

                areHere2COM.Remove(last1);
                goHere2COM.Remove(last2);
            }
        }


        class OrbitPair
        {
            public OrbitPair(string com)
            {
                COM = com;
            }

            public string COM { get; private set; }

            public OrbitPair Orbits { get; set; }
        }
    }
}
