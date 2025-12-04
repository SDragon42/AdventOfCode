import unittest
from typing import List

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

import inputHelper
import src.day08 as puzzle


_DAY = 8

class Day08(unittest.TestCase):
    """
    Day 8: Space Image Format
    https://adventofcode.com/2019/day/8
    """

    def _get_input_data(self, name:str) -> List[int]:
        input = [int(c) for c in inputHelper.load_file(2019, _DAY, name)]
        return input

    def _get_answer_part1(self, name:str) -> int:
        expectedAnswer = inputHelper.load_file(2019, _DAY, f'{name}-answer1')
        if expectedAnswer is not None:
            expectedAnswer = int(expectedAnswer)

        return expectedAnswer

    def _get_answer_part2(self, name:str) -> str:
        expectedAnswer = inputHelper.load_file(2019, _DAY, f'{name}-answer2')
        return expectedAnswer


    def test_get_image_layer(self):
        input = self._get_input_data('input')

        data = [0,1,2]
        for layerNum in data:
            layer = puzzle.get_image_layer(input, layerNum)
            answer = len(layer)

            self.assertEquals(answer, puzzle.IMAGE_SIZE)


    def test_count_in_layer(self):
        data = [
            ([0,1,2], 2, 1),
            ([0,2,2,1,1,2], 2, 3),
            ([0,2,2,1,1,2], 1, 2),
            ([0,2,2,1,1,2], 0, 1),
        ]
        for imageData, value, expected in data:
            answer = puzzle.count_in_layer(imageData, value)
            self.assertEquals(answer, expected)


    def test_part1(self) -> None:
        input = self._get_input_data('input')
        expected = self._get_answer_part1('input')
        value = puzzle.part1(input)
        self.assertEqual(value, expected)


    def test_part2(self) -> None:
        input = self._get_input_data('input')
        expected = self._get_answer_part2('input')
        value = puzzle.part2(input)
        self.assertEqual(value, expected)
