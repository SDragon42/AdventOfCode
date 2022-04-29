import unittest
from typing import List

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

import inputHelper
from helper import string_to_int_list
from src.intcode_computer import IntCode
import src.day07 as puzzle


_DAY = 7

class Day07(unittest.TestCase):
    """
    Day 7: Amplification Circuit
    https://adventofcode.com/2019/day/7
    """

    def _get_test_data(self, name:str, part:int) -> tuple[List[int], List[int], int]:
        text = inputHelper.load_file(_DAY, name).splitlines()
        input = string_to_int_list(text[0])
        phase = None
        if len(text) > 1:
            phase = string_to_int_list(text[1])

        expectedAnswer = inputHelper.load_file(_DAY, f'{name}-answer{part}')
        if expectedAnswer is not None:
            expectedAnswer = int(expectedAnswer)

        return input, phase, expectedAnswer


    def run_part1(self, inputName:str) -> None:
        input, phase, expected = self._get_test_data(inputName, 1)
        value = puzzle.part1(input, phase)
        self.assertEqual(value, expected)

    def test_part1_example_1(self): self.run_part1('example1')
    def test_part1_example_2(self): self.run_part1('example2')
    def test_part1_example_3(self): self.run_part1('example3')
    def test_part1(self): self.run_part1('input')


    def run_part2(self, inputName:str) -> None:
        input, phase, expected = self._get_test_data(inputName, 2)
        value = puzzle.part2(input, phase)
        self.assertEqual(value, expected)

    def test_part2_example_4(self): self.run_part2('example4')
    def test_part2_example_5(self): self.run_part2('example5')
    def test_part2(self): self.run_part2('input')