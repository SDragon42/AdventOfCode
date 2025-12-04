import unittest
from typing import List

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

import inputHelper
from helper import string_to_int_list
from src.intcode_computer import IntCode
import src.day02 as puzzle


_DAY = 2

class Day02(unittest.TestCase):
    """
    Day 2: 1202 Program Alarm
    https://adventofcode.com/2019/day/2
    """

    def _get_test_data(self, name:str, part:int) -> tuple[List[int], int]:
        text = inputHelper.load_file(2019, _DAY, name)
        input = string_to_int_list(text)

        expectedAnswer = inputHelper.load_file(2019, _DAY, f'{name}-answer{part}')
        if expectedAnswer is not None:
            expectedAnswer = int(expectedAnswer)

        return input, expectedAnswer


    def part1(self, inputName:str, valueAt1:int = -1, valueAt2:int = -1) -> None:
        input, expected = self._get_test_data(inputName, 1)
        value = puzzle.run_code(input, valueAt1, valueAt2)
        self.assertEqual(value, expected)

    def test_part1_example_1(self):
        self.part1('example1')

    def test_part1(self):
        self.part1('input', 12, 2)


    def test_part2(self):
        input, expected = self._get_test_data('input', 2)
        value = puzzle.find_noun_verb(input, 19690720)
        self.assertEqual(value, expected)
