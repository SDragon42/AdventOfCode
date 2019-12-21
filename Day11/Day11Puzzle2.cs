using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Advent_of_Code.IntCodeComputer;

namespace Advent_of_Code.Day11
{
    /*
    --- Part Two ---
    You're not sure what it's trying to paint, but it's definitely not a registration identifier. 
    The Space Police are getting impatient.

    Checking your external ship cameras again, you notice a white panel marked "emergency hull 
    painting robot starting panel". The rest of the panels are still black, but it looks like the 
    robot was expecting to start on a white panel, not a black one.

    Based on the Space Law Space Brochure that the Space Police attached to one of your windows, 
    a valid registration identifier is always eight capital letters. After starting the robot on 
    a single white panel instead, what registration identifier does it paint on your hull?
     */
    class Day11Puzzle2 : IPuzzle
    {
        readonly IReadOnlyList<long> puzzleInput;
        public Day11Puzzle2()
        {
            puzzleInput = new long[] { 3, 8, 1005, 8, 301, 1106, 0, 11, 0, 0, 0, 104, 1, 104, 0, 3, 8, 102, -1, 8, 10, 1001, 10, 1, 10, 4, 10, 1008, 8, 0, 10, 4, 10, 1002, 8, 1, 29, 1, 1103, 7, 10, 3, 8, 102, -1, 8, 10, 101, 1, 10, 10, 4, 10, 108, 1, 8, 10, 4, 10, 1002, 8, 1, 54, 2, 103, 3, 10, 2, 1008, 6, 10, 1006, 0, 38, 2, 1108, 7, 10, 3, 8, 102, -1, 8, 10, 1001, 10, 1, 10, 4, 10, 108, 1, 8, 10, 4, 10, 1001, 8, 0, 91, 3, 8, 1002, 8, -1, 10, 1001, 10, 1, 10, 4, 10, 1008, 8, 0, 10, 4, 10, 101, 0, 8, 114, 3, 8, 1002, 8, -1, 10, 101, 1, 10, 10, 4, 10, 1008, 8, 1, 10, 4, 10, 1001, 8, 0, 136, 3, 8, 1002, 8, -1, 10, 1001, 10, 1, 10, 4, 10, 1008, 8, 1, 10, 4, 10, 1002, 8, 1, 158, 1, 1009, 0, 10, 2, 1002, 18, 10, 3, 8, 102, -1, 8, 10, 101, 1, 10, 10, 4, 10, 108, 0, 8, 10, 4, 10, 1002, 8, 1, 187, 2, 1108, 6, 10, 3, 8, 1002, 8, -1, 10, 1001, 10, 1, 10, 4, 10, 108, 0, 8, 10, 4, 10, 1002, 8, 1, 213, 3, 8, 1002, 8, -1, 10, 101, 1, 10, 10, 4, 10, 1008, 8, 1, 10, 4, 10, 1001, 8, 0, 236, 1, 104, 10, 10, 1, 1002, 20, 10, 2, 1008, 9, 10, 3, 8, 102, -1, 8, 10, 101, 1, 10, 10, 4, 10, 108, 0, 8, 10, 4, 10, 101, 0, 8, 269, 1, 102, 15, 10, 1006, 0, 55, 2, 1107, 15, 10, 101, 1, 9, 9, 1007, 9, 979, 10, 1005, 10, 15, 99, 109, 623, 104, 0, 104, 1, 21102, 1, 932700598932, 1, 21102, 318, 1, 0, 1105, 1, 422, 21102, 1, 937150489384, 1, 21102, 329, 1, 0, 1105, 1, 422, 3, 10, 104, 0, 104, 1, 3, 10, 104, 0, 104, 0, 3, 10, 104, 0, 104, 1, 3, 10, 104, 0, 104, 1, 3, 10, 104, 0, 104, 0, 3, 10, 104, 0, 104, 1, 21101, 46325083227, 0, 1, 21102, 376, 1, 0, 1106, 0, 422, 21102, 3263269927, 1, 1, 21101, 387, 0, 0, 1105, 1, 422, 3, 10, 104, 0, 104, 0, 3, 10, 104, 0, 104, 0, 21102, 988225102184, 1, 1, 21101, 410, 0, 0, 1105, 1, 422, 21101, 868410356500, 0, 1, 21102, 1, 421, 0, 1106, 0, 422, 99, 109, 2, 21202, -1, 1, 1, 21102, 1, 40, 2, 21102, 1, 453, 3, 21102, 1, 443, 0, 1105, 1, 486, 109, -2, 2106, 0, 0, 0, 1, 0, 0, 1, 109, 2, 3, 10, 204, -1, 1001, 448, 449, 464, 4, 0, 1001, 448, 1, 448, 108, 4, 448, 10, 1006, 10, 480, 1102, 1, 0, 448, 109, -2, 2106, 0, 0, 0, 109, 4, 1201, -1, 0, 485, 1207, -3, 0, 10, 1006, 10, 503, 21101, 0, 0, -3, 22101, 0, -3, 1, 21201, -2, 0, 2, 21102, 1, 1, 3, 21101, 0, 522, 0, 1105, 1, 527, 109, -4, 2106, 0, 0, 109, 5, 1207, -3, 1, 10, 1006, 10, 550, 2207, -4, -2, 10, 1006, 10, 550, 22102, 1, -4, -4, 1105, 1, 618, 21201, -4, 0, 1, 21201, -3, -1, 2, 21202, -2, 2, 3, 21102, 569, 1, 0, 1106, 0, 527, 22101, 0, 1, -4, 21101, 0, 1, -1, 2207, -4, -2, 10, 1006, 10, 588, 21102, 1, 0, -1, 22202, -2, -1, -2, 2107, 0, -3, 10, 1006, 10, 610, 21201, -1, 0, 1, 21101, 610, 0, 0, 105, 1, 485, 21202, -2, -1, -2, 22201, -4, -2, -4, 109, -5, 2105, 1, 0 };
        }



        readonly Dictionary<Point, HullPanel> hull = new Dictionary<Point, HullPanel>();


        public void Run()
        {
            Console.WriteLine("--- Day 11: Space Police (Part two) ---");

            var robot = new EHPRobot();
            robot.ScanHull += Robot_ScanHull;
            robot.PaintHull += Robot_PaintHull;

            robot.Init(puzzleInput);

            robot.Start();

            Helper.DrawPointGrid2D(hull, DrawPanel);

            Console.WriteLine();
        }


        void Robot_PaintHull(object sender, PaintHullColorEventArgs e)
        {
            SetHullPanel(e.Location, e.HullColor);
        }

        void Robot_ScanHull(object sender, ScanHullColorEventArgs e)
        {
            var panel = GetHullPanel(e.Location);
            e.HullColor = panel.Color;
        }


        HullPanel GetHullPanel(Point location)
        {
            if (hull.Count == 0)
                SetHullPanel(location, HullColor.White);

            if (hull.ContainsKey(location))
                return hull[location];
            return new HullPanel(location);

        }

        void SetHullPanel(Point location, HullColor color)
        {
            if (!hull.ContainsKey(location))
                hull.Add(location, new HullPanel(location));
            var panel = hull[location];
            panel.Color = color;
        }

        string DrawPanel(HullPanel panel)
        {
            var color = panel?.Color ?? HullColor.Black;

            switch (color)
            {
                case HullColor.Black: return " ";
                case HullColor.White: return "#";
                default: goto case HullColor.Black;
            }
        }

    }

}
