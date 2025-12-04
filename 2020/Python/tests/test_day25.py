import unittest
from typing import Dict, List

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

import inputHelper
import src.day25 as puzzle


_DAY = 25

class Day25(unittest.TestCase):
    def get_test_data(self, name:str, part:int) -> tuple[List[int], int]:
        lines = inputHelper.load_file(2020, _DAY, name).splitlines()
        input = [int(l) for l in lines]

        expectedAnswer = inputHelper.load_file(2020, _DAY, f"{name}-answer{part}")
        if expectedAnswer is not None:
            expectedAnswer = int(expectedAnswer)

        return input, expectedAnswer


    def test_part1_example1(self):
        input, expected = self.get_test_data("example1", 1)
        answer = puzzle.run_part1(input)
        self.assertEqual(answer, expected)

    def test_part1(self):
        input, expected = self.get_test_data("input", 1)
        answer = puzzle.run_part1(input)
        self.assertEqual(answer, expected)