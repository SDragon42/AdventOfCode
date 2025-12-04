import unittest
from typing import Dict, List

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

import inputHelper
import src.day15 as puzzle


_DAY = 15

class Day15(unittest.TestCase):
    def get_test_data(self, name:str, part:int) -> tuple[List[str], int, int]:
        input = inputHelper.load_file(2020, _DAY, name).splitlines()

        lastTurn = int(inputHelper.load_file(2020, _DAY, name+f'-lastturn{part}'))

        expectedAnswer = inputHelper.load_file(2020, _DAY, f"{name}-answer{part}")
        if expectedAnswer is not None:
            expectedAnswer = int(expectedAnswer)

        return input, lastTurn, expectedAnswer


    def test_part1_example1(self):
        input, lastTurn, expected = self.get_test_data("example1", 1)
        answer = puzzle.run_part(input, lastTurn)
        self.assertEqual(answer, expected)

    def test_part1(self):
        input, lastTurn, expected = self.get_test_data("input", 1)
        answer = puzzle.run_part(input, lastTurn)
        self.assertEqual(answer, expected)


    def test_part2_example2(self):
        input, lastTurn, expected = self.get_test_data("example1", 2)
        answer = puzzle.run_part(input, lastTurn)
        self.assertEqual(answer, expected)

    def test_part2(self):
        input, lastTurn, expected = self.get_test_data("input", 2)
        answer = puzzle.run_part(input, lastTurn)
        self.assertEqual(answer, expected)
