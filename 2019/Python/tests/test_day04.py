import unittest
from typing import List

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

import inputHelper
import src.day04 as puzzle


_DAY = 4

class Day04(unittest.TestCase):
    """
    Day 4: Secure Container
    https://adventofcode.com/2019/day/4
    """

    def get_test_data(self, name:str, part:int) -> tuple[str, int]:
        input = inputHelper.load_file(2019, _DAY, name)

        expectedAnswer = inputHelper.load_file(2019, _DAY, f'{name}-answer{part}')
        if expectedAnswer is not None:
            expectedAnswer = int(expectedAnswer)

        return input, expectedAnswer

    
    def test_rule_is_six_digits(self):
        data = [
            ([1,2,3,4,5,6], True),
            ([1,2,3,4,5], False),
            ([1,2,3,4,5,6,7], False),
            ([], False),
        ]
        for digits, expected in data:
            result = puzzle.rule_is_six_digits(digits)
            self.assertEquals(result, expected)

    def test_rule_two_adjacent_digits_are_same(self):
        data = [
            ([1,2,2,4,5,6], True),
            ([1,2,3,3,5,6], True),
            ([1,2,2,2,5,6], True),
            ([1,2,3,4,5,6], False),
        ]
        for digits, expected in data:
            result = puzzle.rule_two_adjacent_digits_are_same(digits)
            self.assertEquals(result, expected)

    def test_rule_only_two_adjacent_digits_are_same(self):
        data = [
            ([1,2,2,4,5,6], True),
            ([1,2,3,3,5,6], True),
            ([1,2,2,2,5,6], False),
            ([1,2,3,4,5,6], False),
            ([1,1,1,2,3,3], True),
        ]
        for digits, expected in data:
            result = puzzle.rule_only_two_adjacent_digits_are_same(digits)
            self.assertEquals(result, expected)

    def test_rule_digits_never_decrease(self):
        data = [
            ([1,2,2,4,5,6], True),
            ([1,2,3,3,5,6], True),
            ([1,2,2,2,5,1], False),
            ([1,2,3,4,3,6], False),
        ]
        for digits, expected in data:
            result = puzzle.rule_digits_never_decrease(digits)
            self.assertEquals(result, expected)

    def test_part1_example(self):
        input = '122345-122346'
        expected = 2
        result = puzzle.part1(input)
        self.assertEqual(result, expected)

    def test_part1(self):
        input, expected = self.get_test_data('input', 1)
        result = puzzle.part1(input)
        self.assertEqual(result, expected)

    def test_part2(self):
        input, expected = self.get_test_data('input', 2)
        result = puzzle.part2(input)
        self.assertEqual(result, expected)
