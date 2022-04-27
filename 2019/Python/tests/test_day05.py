import unittest
from typing import List

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

import inputHelper
from helper import string_to_int_list
from src.intcode_computer import IntCode
import src.day05 as puzzle


_DAY = 5

class Day05(unittest.TestCase):
    """
    Day 5: Sunny with a Chance of Asteroids
    https://adventofcode.com/2019/day/5
    """

    def _get_test_data(self, name:str, part:int) -> tuple[List[int], int]:
        text = inputHelper.load_file(_DAY, name)
        input = string_to_int_list(text)

        expectedAnswer = inputHelper.load_file(_DAY, f'{name}-answer{part}')
        if expectedAnswer is not None:
            expectedAnswer = int(expectedAnswer)

        return input, expectedAnswer


    def part1(self, inputName:str, inputValue:int) -> None:
        input, expected = self._get_test_data(inputName, 1)
        value = puzzle.part1(input, inputValue)
        self.assertEqual(value, expected)


    def test_part1_example_1(self):
        self.part1('example1', 69)

    def test_part1_example_2(self):
        self.part1('example2', 1)

    def test_part1(self):
        self.part1('input', 1)


    # def part2(self, inputName:str, inputValue:int) -> None:
    #     input, expected = self._get_test_data(inputName, 1)
    #     value = puzzle.part1(input, inputValue)
    #     self.assertEqual(value, expected)

    # # def test_part2_example_3a(self):
    # #     self.part2('example3')

    # # def test_part2_example_3b(self):
    # #     self.part2('example3')

    # # def test_part2_example_4a(self):
    # #     self.part2('example4')

    # # def test_part2_example_4b(self):
    # #     self.part2('example4')

    # def test_part2(self):
    #     self.part2('input', 5)