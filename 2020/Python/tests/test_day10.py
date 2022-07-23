import unittest
from typing import List

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

import inputHelper
import src.day10 as puzzle


_DAY = 10

class Day10(unittest.TestCase):
    def get_test_data(self, name:str, part:int) -> tuple[List[int], int]:
        lines = inputHelper.load_file(_DAY, name).splitlines()
        input = [int(l) for l in lines]

        expectedAnswer = inputHelper.load_file(_DAY, f"{name}-answer{part}")
        if expectedAnswer is not None:
            expectedAnswer = int(expectedAnswer)

        return input, expectedAnswer


    def test_part1_example1(self):
        input, expected = self.get_test_data("example1", 1)
        answer = puzzle.run_part1(input)
        self.assertEqual(answer, expected)

    def test_part1_example2(self):
        input, expected = self.get_test_data("example2", 1)
        answer = puzzle.run_part1(input)
        self.assertEqual(answer, expected)

    def test_part1(self):
        input, expected = self.get_test_data("input", 1)
        answer = puzzle.run_part1(input)
        self.assertEqual(answer, expected)


    def test_part2_example1(self):
        input, expected = self.get_test_data("example1", 2)
        answer = puzzle.run_part2(input)
        self.assertEqual(answer, expected)

    def test_part2_example2(self):
        input, expected = self.get_test_data("example2", 2)
        answer = puzzle.run_part2(input)
        self.assertEqual(answer, expected)

    def test_part2(self):
        input, expected = self.get_test_data("input", 2)
        answer = puzzle.run_part2(input)
        self.assertEqual(answer, expected)