import unittest
from typing import List

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

import inputHelper
import src.day09 as puzzle


_DAY = 9

class Day09(unittest.TestCase):
    def get_test_data(self, name:str, part:int) -> tuple[List[int], int, int]:
        lines = inputHelper.load_file(_DAY, name).splitlines()
        input = [int(l) for l in lines]

        windowSize = int(inputHelper.load_file(_DAY, f"{name}-windowsize"))

        expectedAnswer = inputHelper.load_file(_DAY, f"{name}-answer{part}")
        if expectedAnswer is not None:
            expectedAnswer = int(expectedAnswer)

        return input, windowSize, expectedAnswer


    def test_part1_example1(self):
        input, windowSize, expected = self.get_test_data("example1", 1)
        answer = puzzle.scan_input(input, windowSize)
        self.assertEqual(answer, expected)

    def test_part1(self):
        input, windowSize, expected = self.get_test_data("input", 1)
        answer = puzzle.scan_input(input, windowSize)
        self.assertEqual(answer, expected)


    def test_part2_example1(self):
        input, windowSize, expected = self.get_test_data("example1", 2)
        invalidNum = puzzle.scan_input(input, windowSize)
        answer = puzzle.find_weakness(input, invalidNum)
        self.assertEqual(answer, expected)

    def test_part2(self):
        input, windowSize, expected = self.get_test_data("input", 2)
        invalidNum = puzzle.scan_input(input, windowSize)
        answer = puzzle.find_weakness(input, invalidNum)
        self.assertEqual(answer, expected)