import unittest
from typing import Dict, List

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

import inputHelper
import src.day17 as puzzle


_DAY = 17

class Day17(unittest.TestCase):
    def get_test_data(self, name:str, part:int) -> tuple[List[str], int, int]:
        input = inputHelper.load_file(2020, _DAY, name).splitlines()
        
        numDimensions = int(inputHelper.load_file(2020, _DAY, f"dimensions{part}"))

        expectedAnswer = inputHelper.load_file(2020, _DAY, f"{name}-answer{part}")
        if expectedAnswer is not None:
            expectedAnswer = int(expectedAnswer)

        return input, numDimensions, expectedAnswer


    def test_part1_example1(self):
        input, numDimensions, expected = self.get_test_data("example1", 1)
        answer = puzzle.run_part(input, numDimensions)
        self.assertEqual(answer, expected)

    def test_part1(self):
        input, numDimensions, expected = self.get_test_data("input", 1)
        answer = puzzle.run_part(input, numDimensions)
        self.assertEqual(answer, expected)


    def test_part2_example1(self):
        input, numDimensions, expected = self.get_test_data("example1", 2)
        answer = puzzle.run_part(input, numDimensions)
        self.assertEqual(answer, expected)

    def test_part2(self):
        input, numDimensions, expected = self.get_test_data("input", 2)
        answer = puzzle.run_part(input, numDimensions)
        self.assertEqual(answer, expected)
