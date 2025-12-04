import unittest
from typing import List

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

import inputHelper
import src.day03 as puzzle


_DAY = 3

class Day03(unittest.TestCase):
    def get_test_data(self, name:str, part:int) -> tuple[List[str], List[str], int]:
        input = inputHelper.load_file(2020, _DAY, name).splitlines()

        slopes = inputHelper.load_file(2020, _DAY, f'slopes{part}').split(',')

        expectedAnswer = inputHelper.load_file(2020, _DAY, f"{name}-answer{part}")
        if expectedAnswer is not None:
            expectedAnswer = int(expectedAnswer)

        return input, slopes, expectedAnswer


    def test_part1_example1(self):
        input, slopes, expected = self.get_test_data("example1", 1)
        answer = puzzle.do_it(input, slopes)
        self.assertEqual(answer, expected)

    def test_part1(self):
        input, slopes, expected = self.get_test_data("input", 1)
        answer = puzzle.do_it(input, slopes)
        self.assertEqual(answer, expected)


    def test_part2_example1(self):
        input, slopes, expected = self.get_test_data("example1", 2)
        answer = puzzle.do_it(input, slopes)
        self.assertEqual(answer, expected)

    def test_part2(self):
        input, slopes, expected = self.get_test_data("input", 2)
        answer = puzzle.do_it(input, slopes)
        self.assertEqual(answer, expected)