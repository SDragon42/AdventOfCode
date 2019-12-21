using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Advent_of_Code.IntCodeComputer;

namespace Advent_of_Code.Day11
{
    /*
    --- Day 11: Space Police ---
    On the way to Jupiter, you're pulled over by the Space Police.

    "Attention, unmarked spacecraft! You are in violation of Space Law! All spacecraft must have 
    a clearly visible registration identifier! You have 24 hours to comply or be sent to Space 
    Jail!"

    Not wanting to be sent to Space Jail, you radio back to the Elves on Earth for help. Although 
    it takes almost three hours for their reply signal to reach you, they send instructions for 
    how to power up the emergency hull painting robot and even provide a small Intcode program 
    (your puzzle input) that will cause it to paint your ship appropriately.

    There's just one problem: you don't have an emergency hull painting robot.

    You'll need to build a new emergency hull painting robot. The robot needs to be able to move 
    around on the grid of square panels on the side of your ship, detect the color of its current 
    panel, and paint its current panel black or white. (All of the panels are currently black.)

    The Intcode program will serve as the brain of the robot. The program uses input instructions 
    to access the robot's camera: provide 0 if the robot is over a black panel or 1 if the robot 
    is over a white panel. Then, the program will output two values:

    First, it will output a value indicating the color to paint the panel the robot is over: 0 
    means to paint the panel black, and 1 means to paint the panel white.
    Second, it will output a value indicating the direction the robot should turn: 0 means it 
    should turn left 90 degrees, and 1 means it should turn right 90 degrees.
    After the robot turns, it should always move forward exactly one panel. The robot starts 
    facing up.

    The robot will continue running for a while like this and halt when it is finished drawing. 
    Do not restart the Intcode computer inside the robot during this process.

    For example, suppose the robot is about to start running. Drawing black panels as ., white 
    panels as #, and the robot pointing the direction it is facing (< ^ > v), the initial state 
    and region near the robot looks like this:

    .....
    .....
    ..^..
    .....
    .....

    The panel under the robot (not visible here because a ^ is shown instead) is also black, and 
    so any input instructions at this point should be provided 0. Suppose the robot eventually 
    outputs 1 (paint white) and then 0 (turn left). After taking these actions and moving forward 
    one panel, the region now looks like this:

    .....
    .....
    .<#..
    .....
    .....

    Input instructions should still be provided 0. Next, the robot might output 0 (paint black) 
    and then 0 (turn left):

    .....
    .....
    ..#..
    .v...
    .....

    After more outputs (1,0, 1,0):

    .....
    .....
    ..^..
    .##..
    .....

    The robot is now back where it started, but because it is now on a white panel, input 
    instructions should be provided 1. After several more outputs (0,1, 1,0, 1,0), the area looks 
    like this:

    .....
    ..<#.
    ...#.
    .##..
    .....

    Before you deploy the robot, you should probably have an estimate of the area it will cover:
    specifically, you need to know the number of panels it paints at least once, regardless of
    color. In the example above, the robot painted 6 panels at least once. (It painted its 
    starting panel twice, but that panel is still only counted once; it also never painted the 
    panel it ended on.)

    Build a new emergency hull painting robot and run the Intcode program on it. How many panels 
    does it paint at least once?
     */
    class Day11Puzzle1 : IPuzzle
    {
        readonly IReadOnlyList<long> puzzleInput;
        public Day11Puzzle1()
        {
            puzzleInput = new long[] { 3, 8, 1005, 8, 301, 1106, 0, 11, 0, 0, 0, 104, 1, 104, 0, 3, 8, 102, -1, 8, 10, 1001, 10, 1, 10, 4, 10, 1008, 8, 0, 10, 4, 10, 1002, 8, 1, 29, 1, 1103, 7, 10, 3, 8, 102, -1, 8, 10, 101, 1, 10, 10, 4, 10, 108, 1, 8, 10, 4, 10, 1002, 8, 1, 54, 2, 103, 3, 10, 2, 1008, 6, 10, 1006, 0, 38, 2, 1108, 7, 10, 3, 8, 102, -1, 8, 10, 1001, 10, 1, 10, 4, 10, 108, 1, 8, 10, 4, 10, 1001, 8, 0, 91, 3, 8, 1002, 8, -1, 10, 1001, 10, 1, 10, 4, 10, 1008, 8, 0, 10, 4, 10, 101, 0, 8, 114, 3, 8, 1002, 8, -1, 10, 101, 1, 10, 10, 4, 10, 1008, 8, 1, 10, 4, 10, 1001, 8, 0, 136, 3, 8, 1002, 8, -1, 10, 1001, 10, 1, 10, 4, 10, 1008, 8, 1, 10, 4, 10, 1002, 8, 1, 158, 1, 1009, 0, 10, 2, 1002, 18, 10, 3, 8, 102, -1, 8, 10, 101, 1, 10, 10, 4, 10, 108, 0, 8, 10, 4, 10, 1002, 8, 1, 187, 2, 1108, 6, 10, 3, 8, 1002, 8, -1, 10, 1001, 10, 1, 10, 4, 10, 108, 0, 8, 10, 4, 10, 1002, 8, 1, 213, 3, 8, 1002, 8, -1, 10, 101, 1, 10, 10, 4, 10, 1008, 8, 1, 10, 4, 10, 1001, 8, 0, 236, 1, 104, 10, 10, 1, 1002, 20, 10, 2, 1008, 9, 10, 3, 8, 102, -1, 8, 10, 101, 1, 10, 10, 4, 10, 108, 0, 8, 10, 4, 10, 101, 0, 8, 269, 1, 102, 15, 10, 1006, 0, 55, 2, 1107, 15, 10, 101, 1, 9, 9, 1007, 9, 979, 10, 1005, 10, 15, 99, 109, 623, 104, 0, 104, 1, 21102, 1, 932700598932, 1, 21102, 318, 1, 0, 1105, 1, 422, 21102, 1, 937150489384, 1, 21102, 329, 1, 0, 1105, 1, 422, 3, 10, 104, 0, 104, 1, 3, 10, 104, 0, 104, 0, 3, 10, 104, 0, 104, 1, 3, 10, 104, 0, 104, 1, 3, 10, 104, 0, 104, 0, 3, 10, 104, 0, 104, 1, 21101, 46325083227, 0, 1, 21102, 376, 1, 0, 1106, 0, 422, 21102, 3263269927, 1, 1, 21101, 387, 0, 0, 1105, 1, 422, 3, 10, 104, 0, 104, 0, 3, 10, 104, 0, 104, 0, 21102, 988225102184, 1, 1, 21101, 410, 0, 0, 1105, 1, 422, 21101, 868410356500, 0, 1, 21102, 1, 421, 0, 1106, 0, 422, 99, 109, 2, 21202, -1, 1, 1, 21102, 1, 40, 2, 21102, 1, 453, 3, 21102, 1, 443, 0, 1105, 1, 486, 109, -2, 2106, 0, 0, 0, 1, 0, 0, 1, 109, 2, 3, 10, 204, -1, 1001, 448, 449, 464, 4, 0, 1001, 448, 1, 448, 108, 4, 448, 10, 1006, 10, 480, 1102, 1, 0, 448, 109, -2, 2106, 0, 0, 0, 109, 4, 1201, -1, 0, 485, 1207, -3, 0, 10, 1006, 10, 503, 21101, 0, 0, -3, 22101, 0, -3, 1, 21201, -2, 0, 2, 21102, 1, 1, 3, 21101, 0, 522, 0, 1105, 1, 527, 109, -4, 2106, 0, 0, 109, 5, 1207, -3, 1, 10, 1006, 10, 550, 2207, -4, -2, 10, 1006, 10, 550, 22102, 1, -4, -4, 1105, 1, 618, 21201, -4, 0, 1, 21201, -3, -1, 2, 21202, -2, 2, 3, 21102, 569, 1, 0, 1106, 0, 527, 22101, 0, 1, -4, 21101, 0, 1, -1, 2207, -4, -2, 10, 1006, 10, 588, 21102, 1, 0, -1, 22202, -2, -1, -2, 2107, 0, -3, 10, 1006, 10, 610, 21201, -1, 0, 1, 21101, 610, 0, 0, 105, 1, 485, 21202, -2, -1, -2, 22201, -4, -2, -4, 109, -5, 2105, 1, 0 };
        }


        readonly Dictionary<Point, HullPanel> hull = new Dictionary<Point, HullPanel>();


        public void Run()
        {
            Console.WriteLine("--- Day 11: Space Police ---");

            var robot = new EHPRobot();
            robot.ScanHull += Robot_ScanHull;
            robot.PaintHull += Robot_PaintHull;

            robot.Init(puzzleInput);

            robot.Start();

            Console.WriteLine($"Number of Panels painted: {hull.Count}");
            if (hull.Count == 1681)
                Console.WriteLine("\tCorrect");
            Console.WriteLine();
        }

        private void Robot_PaintHull(object sender, PaintHullColorEventArgs e)
        {
            SetHullPanel(e.Location, e.HullColor);
        }

        private void Robot_ScanHull(object sender, ScanHullColorEventArgs e)
        {
            var panel = GetHullPanel(e.Location);
            e.HullColor = panel.Color;
        }

        HullPanel GetHullPanel(Point location)
        {
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

    }

}
