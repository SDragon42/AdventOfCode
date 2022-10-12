import unittest
from typing import Dict, List

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

import inputHelper
import src.day23 as puzzle


_DAY = 23

class Day23(unittest.TestCase):
    def get_test_data(self, name:str, part:int) -> tuple[List[int], int, int]:
        text = inputHelper.load_file(_DAY, name)
        input = [int(c) for c in text]

        numMoves = int(inputHelper.load_file(_DAY, f"{name}-moves{part}"))
        
        expectedAnswer = inputHelper.load_file(_DAY, f"{name}-answer{part}")
        if expectedAnswer is not None:
            expectedAnswer = int(expectedAnswer)

        return input, numMoves, expectedAnswer


    def test_part1_example1(self):
        input, numMoves, expected = self.get_test_data("example1", 1)
        answer = puzzle.run_part1(input, numMoves)
        self.assertEqual(answer, expected)

    def test_part1_example2(self):
        input, numMoves, expected = self.get_test_data("example2", 1)
        answer = puzzle.run_part1(input, numMoves)
        self.assertEqual(answer, expected)

    def test_part1(self):
        input, numMoves, expected = self.get_test_data("input", 1)
        answer = puzzle.run_part1(input, numMoves)
        self.assertEqual(answer, expected)


    def test_part2_example1(self):
        input, numMoves, expected = self.get_test_data("example1", 2)
        answer = puzzle.run_part2(input, numMoves)
        self.assertEqual(answer, expected)

    def test_part2(self):
        input, numMoves, expected = self.get_test_data("input", 2)
        answer = puzzle.run_part2(input, numMoves)
        self.assertEqual(answer, expected)
