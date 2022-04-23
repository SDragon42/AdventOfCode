import unittest
from typing import List

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

import inputHelper
import src.day04 as puzzle


_DAY = 4

class Day04(unittest.TestCase):
    def get_test_data(self, name:str, part:int) -> tuple[List[str], int]:
        input = inputHelper.load_file(_DAY, name).splitlines()

        expectedAnswer = inputHelper.load_file(_DAY, f"{name}-answer{part}")
        if expectedAnswer is not None:
            expectedAnswer = int(expectedAnswer)

        return input, expectedAnswer


    def test_part1_example1(self):
        input, expected = self.get_test_data("example1", 1)
        answer = puzzle.count_valid_passorts(input, False)
        self.assertEqual(answer, expected)

    def test_part1(self):
        input, expected = self.get_test_data("input", 1)
        answer = puzzle.count_valid_passorts(input, False)
        self.assertEqual(answer, expected)


    def test_part2_example_invalid_passports(self):
        input, expected = self.get_test_data("example-invalid-passports", 2)
        answer = puzzle.count_valid_passorts(input, True)
        self.assertEqual(answer, expected)

    def test_part2_example_valid_passports(self):
        input, expected = self.get_test_data("example-valid-passports", 2)
        answer = puzzle.count_valid_passorts(input, True)
        self.assertEqual(answer, expected)

    def test_part2(self):
        input, expected = self.get_test_data("input", 2)
        answer = puzzle.count_valid_passorts(input, True)
        self.assertEqual(answer, expected)