import unittest
from typing import List

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

import inputHelper
import src.day06 as puzzle


_DAY = 6

class Day06(unittest.TestCase):
    """
    Day 6: Universal Orbit Map
    https://adventofcode.com/2019/day/6
    """

    def get_test_data(self, name:str, part:int) -> tuple[List[str], int]:
        input = inputHelper.load_file(2019, _DAY, name).splitlines()

        expectedAnswer = inputHelper.load_file(2019, _DAY, f'{name}-answer{part}')
        if expectedAnswer is not None:
            expectedAnswer = int(expectedAnswer)

        return input, expectedAnswer


    def run_part1(self, inputName:str) -> None:
        input, expected = self.get_test_data(inputName, 1)
        result = puzzle.part1(input)
        self.assertEqual(result, expected)

    def test_part1_example_1(self):
        self.run_part1('example1')

    def test_part1(self):
        self.run_part1('input')


    def run_part2(self, inputName:str) -> None:
        input, expected = self.get_test_data(inputName, 2)
        result = puzzle.part2(input)
        self.assertEqual(result, expected)

    def test_part2_example_2(self):
        self.run_part2('example2')
        
    def test_part2(self):
        self.run_part2('input')