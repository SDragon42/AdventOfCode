import unittest
from typing import Dict, List

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

import inputHelper
import src.day12 as puzzle


_DAY = 12

class Day12(unittest.TestCase):
    def get_test_data(self, name:str, part:int) -> tuple[List[int], int]:
        input = inputHelper.load_file(_DAY, name).splitlines()

        expectedAnswer = inputHelper.load_file(_DAY, f"{name}-answer{part}")
        if expectedAnswer is not None:
            expectedAnswer = int(expectedAnswer)

        return input, expectedAnswer


    def get_actions_1(self):
        rules = {
            "N": puzzle.move_north,
            "S": puzzle.move_south,
            "E": puzzle.move_east,
            "W": puzzle.move_west,
            "L": puzzle.turn_left,
            "R": puzzle.turn_right
        }
        return rules


    def test_part1_example1(self):
        input, expected = self.get_test_data("example1", 1)
        actions = self.get_actions_1()
        answer = puzzle.get_distance_from_start(input, actions)
        self.assertEqual(answer, expected)

    def test_part1(self):
        input, expected = self.get_test_data("input", 1)
        actions = self.get_actions_1()
        answer = puzzle.get_distance_from_start(input, actions)
        self.assertEqual(answer, expected)


    def get_actions_2(self):
        rules = {
            "N": puzzle.move_waypoint_north,
            "S": puzzle.move_waypoint_south,
            "E": puzzle.move_waypoint_east,
            "W": puzzle.move_waypoint_west,
            "L": puzzle.rotate_waypoint_left,
            "R": puzzle.rotate_waypoint_right,
            "F": puzzle.full_steam_ahead
        }
        return rules

    def test_part2_example1(self):
        input, expected = self.get_test_data("example1", 2)
        actions = self.get_actions_2()
        answer = puzzle.get_distance_from_start(input, actions)
        self.assertEqual(answer, expected)

    def test_part2(self):
        input, expected = self.get_test_data("input", 2)
        actions = self.get_actions_2()
        answer = puzzle.get_distance_from_start(input, actions)
        self.assertEqual(answer, expected)