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


    def run_day5(self, inputName:str, inputValue:int) -> None:
        input, expected = self._get_test_data(inputName, 1)
        value = puzzle.run_code(input, inputValue)
        self.assertEqual(value, expected)


    def test_part1_example_1(self):
        self.run_day5('example1', 69)

    def test_part1_example_2(self):
        self.run_day5('example2', 1)

    def test_part1(self):
        self.run_day5('input', 1)


    def test_part2_examples_3(self):
        data = [
            ('3,9,8,9,10,9,4,9,99,-1,8', 8, 1),
            ('3,9,7,9,10,9,4,9,99,-1,8', 8, 0),
            ('3,3,1108,-1,8,3,4,3,99', 8, 1),
            ('3,3,1107,-1,8,3,4,3,99', 8, 0),

            ('3,9,8,9,10,9,4,9,99,-1,8', 7, 0),
            ('3,9,7,9,10,9,4,9,99,-1,8', 7, 1),
            ('3,3,1108,-1,8,3,4,3,99', 7, 0),
            ('3,3,1107,-1,8,3,4,3,99', 7, 1),
        ]
        for inputData, inputValue, expected in data:
            input = string_to_int_list(inputData)
            value = puzzle.run_code(input, inputValue)
            self.assertEqual(value, expected)

    def test_part2_example_4(self):
        data = [
            ('3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9', 0, 0),
            ('3,3,1105,-1,9,1101,0,0,12,4,12,99,1', 0, 0),

            ('3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9', 42, 1),
            ('3,3,1105,-1,9,1101,0,0,12,4,12,99,1', 42, 1),
        ]
        for inputData, inputValue, expected in data:
            input = string_to_int_list(inputData)
            value = puzzle.run_code(input, inputValue)
            self.assertEqual(value, expected)

    def test_part2(self):
        self.run_day5('input', 5)