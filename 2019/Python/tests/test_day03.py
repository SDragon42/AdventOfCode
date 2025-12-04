from turtle import distance
import unittest
from typing import List

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

import inputHelper
from point import Point2D
import src.day03 as puzzle


_DAY = 3

class Day03(unittest.TestCase):
    """
    Day 3: Crossed Wires
    https://adventofcode.com/2019/day/3
    """

    def get_test_data(self, name:str, part:int) -> tuple[List[str], int]:
        input = inputHelper.load_file(2019, _DAY, name).splitlines()

        expectedAnswer = inputHelper.load_file(2019, _DAY, f'{name}-answer{part}')
        if expectedAnswer is not None:
            expectedAnswer = int(expectedAnswer)

        return input, expectedAnswer

    
    
    def test_build_wire_points(self):
        input = 'R8,U5,L5,D3'
        wire = puzzle.build_wire_points(input)
        
        self.assertEqual(str(wire[0]), '0,0')
        self.assertEqual(str(wire[1]), '8,0')
        self.assertEqual(str(wire[2]), '8,5')
        self.assertEqual(str(wire[3]), '3,5')
        self.assertEqual(str(wire[4]), '3,2')

    def test_is_same(self):
        data = [
            (0, 0, 0, True),
            (1, 1, 1, True),
            (-5, -5, -5, True),
            (0, 2, 0, False),
            (1, 1, 2, False),
            (-4, -5, -5, False),
        ]
        for a, b, value, expected in data:
            result = puzzle.is_same(a, b, value)
            self.assertEquals(result, expected)

    def test_is_between(self):
        data = [
            (0, 10, 4, True),
            (5,  8, 7, True),
            (2, -4, -1, True),
            (-1, 1, 0, True),
            (0, 10, 11, False),
            (8, 5, 4, False),
            (-4, 2, -11, False),
            (1, -1, 42, False),
        ]
        for a, b, v, expected in data:
            result = puzzle.is_between(a, b, v)
            self.assertEquals(result, expected)

    def test_is_on_line(self):
        data = [
            (puzzle.LineInfo(puzzle.Point2D(0,0), puzzle.Point2D(10,0)), puzzle.Point2D(5,0), True),
            (puzzle.LineInfo(puzzle.Point2D(42,5), puzzle.Point2D(42,-5)), puzzle.Point2D(42,-1), True),
            (puzzle.LineInfo(puzzle.Point2D(0,0), puzzle.Point2D(10,0)), puzzle.Point2D(12,0), False),
            (puzzle.LineInfo(puzzle.Point2D(0,0), puzzle.Point2D(10,0)), puzzle.Point2D(5,1), False),
            (puzzle.LineInfo(puzzle.Point2D(42,5), puzzle.Point2D(42,-5)), puzzle.Point2D(42,9), False),
            (puzzle.LineInfo(puzzle.Point2D(42,5), puzzle.Point2D(42,-5)), puzzle.Point2D(41,-1), False),
        ]
        for line, point, expected in data:
            result = puzzle.is_on_line(line, point)
            self.assertEquals(result, expected)


    def run_part1(self, inputName:str) -> None:
        input, expected = self.get_test_data(inputName, 1)
        result = puzzle.part1(input)
        self.assertEqual(result, expected)

    def test_part1_example_1(self):
        self.run_part1('example1')

    def test_part1_example_2(self):
        self.run_part1('example2')

    def test_part1_example_3(self):
        self.run_part1('example3')

    def test_part1(self):
        self.run_part1('input')


    def run_part2(self, inputName:str) -> None:
        input, expected = self.get_test_data(inputName, 2)
        result = puzzle.part2(input)
        self.assertEqual(result, expected)

    def test_part2_example_4(self):
        self.run_part2('example4')

    def test_part2_example_5(self):
        self.run_part2('example5')
        
    def test_part2(self):
        self.run_part2('input')

