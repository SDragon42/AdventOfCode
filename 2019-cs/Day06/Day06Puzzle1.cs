using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent_of_Code.Day06
{
    /*
    --- Day 6: Universal Orbit Map ---
    You've landed at the Universal Orbit Map facility on Mercury. Because navigation in space
    often involves transferring between orbits, the orbit maps here are useful for finding
    efficient routes between, for example, you and Santa. You download a map of the local orbits
    (your puzzle input).

    Except for the universal Center of Mass (COM), every object in space is in orbit around
    exactly one other object. An orbit looks roughly like this:

                      \
                       \
                        |
                        |
    AAA--> o            o <--BBB
                        |
                        |
                       /
                      /
    
    In this diagram, the object BBB is in orbit around AAA. The path that BBB takes around AAA
    (drawn with lines) is only partly shown. In the map data, this orbital relationship is
    written AAA)BBB, which means "BBB is in orbit around AAA".

    Before you use your map data to plot a course, you need to make sure it wasn't corrupted
    during the download. To verify maps, the Universal Orbit Map facility uses orbit count
    checksums - the total number of direct orbits (like the one shown above) and indirect orbits.

    Whenever A orbits B and B orbits C, then A indirectly orbits C. This chain can be any number
    of objects long: if A orbits B, B orbits C, and C orbits D, then A indirectly orbits D.

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

    Visually, the above map of orbits looks like this:

            G - H       J - K - L
           /           /
    COM - B - C - D - E - F
                   \
                    I

    In this visual representation, when two objects are connected by a line, the one on the
    right directly orbits the one on the left.

    Here, we can count the total number of orbits as follows:

    D directly orbits C and indirectly orbits B and COM, a total of 3 orbits.
    L directly orbits K and indirectly orbits J, E, D, C, B, and COM, a total of 7 orbits.
    COM orbits nothing.
    The total number of direct and indirect orbits in this example is 42.

    What is the total number of direct and indirect orbits in your map data?
    */
    class Day06Puzzle1 : IPuzzle
    {
        public void Run()
        {
            Console.WriteLine("--- Day 6: Universal Orbit Map ---");


            BuildOrbitTree(Day06Common.OrbitMap);
            var foundOrbits = CountAllOrbits();

            Console.WriteLine($"Orbits found: {foundOrbits}");
            if (Day06Common.ExpectedOrbits >= 0)
                Console.WriteLine("    " + (Day06Common.ExpectedOrbits == foundOrbits ? "CORRECT" : "You done it wrong!"));
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

        private int CountAllOrbits()
        {
            var orbitCount = 0;

            foreach (var key in uniqueBodies.Keys)
                orbitCount += CountChain(uniqueBodies[key]);

            return orbitCount;
        }

        private int CountChain(OrbitPair item)
        {
            if (item.Orbits == null)
                return 0;
            return 1 + CountChain(item.Orbits);
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
