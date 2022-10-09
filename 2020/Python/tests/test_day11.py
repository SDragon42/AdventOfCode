import unittest
from typing import Dict, List

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

import inputHelper
import src.day11 as puzzle


_DAY = 11

class Day11(unittest.TestCase):
    def get_test_data(self, name:str, part:int) -> tuple[List[str], int]:
        input = inputHelper.load_file(_DAY, name).splitlines()

        expectedAnswer = inputHelper.load_file(_DAY, f"{name}-answer{part}")
        if expectedAnswer is not None:
            expectedAnswer = int(expectedAnswer)

        return input, expectedAnswer


    def get_rules_1(self):
        rules = {
            ".": puzzle.rule_floor,
            "L": puzzle.rule_empty_seat_part1,
            "#": puzzle.rule_occupied_seat_part1,
        }
        return rules


    def test_part1_example1(self):
        input, expected = self.get_test_data("example1", 1)
        rules = self.get_rules_1()
        answer = puzzle.get_number_occupied_seats(input, rules)
        self.assertEqual(answer, expected)

    def test_part1(self):
        input, expected = self.get_test_data("input", 1)
        rules = self.get_rules_1()
        answer = puzzle.get_number_occupied_seats(input, rules)
        self.assertEqual(answer, expected)


    def get_rules_2(self):
        rules = {
            ".": puzzle.rule_floor,
            "L": puzzle.rule_empty_seat_part2,
            "#": puzzle.rule_occupied_seat_part2,
        }
        return rules

    def test_part2_example1(self):
        input, expected = self.get_test_data("example1", 2)
        rules = self.get_rules_2()
        answer = puzzle.get_number_occupied_seats(input, rules)
        self.assertEqual(answer, expected)

    def test_part2(self):
        input, expected = self.get_test_data("input", 2)
        rules = self.get_rules_2()
        answer = puzzle.get_number_occupied_seats(input, rules)
        self.assertEqual(answer, expected)