import unittest
from typing import List

# this adds the 'src' folder to the path. Needed to get the src imports to work in unit tests.
import config

import inputHelper
import src.day01 as puzzle


_DAY = 1

class Day01(unittest.TestCase):
    """
    Day 1: The Tyranny of the Rocket Equation
    https://adventofcode.com/2019/day/1
    """

    def get_test_data(self, name:str, part:int) -> tuple[List[int], int]:
        lines = inputHelper.load_file(_DAY, name).splitlines()
        input = [int(l) for l in lines]

        expectedAnswer = inputHelper.load_file(_DAY, f"{name}-answer{part}")
        if expectedAnswer is not None:
            expectedAnswer = int(expectedAnswer)

        return input, expectedAnswer

    
    def test_calc_fuel_for_mass(self):
        testData = [
            (12, 2),
            (14, 2),
            (1969, 654),
            (100756, 33583),
        
        ]
        for mass, expected in testData:
            fuel = puzzle.calc_fuel_for_mass(mass)
            self.assertEqual(fuel, expected)

    def test_calc_fuel_for_mass_and_fuel(self):
        testData = [
            (12, 2),
            (14, 2),
            (1969, 966),
            (100756, 50346),
        
        ]
        for mass, expected in testData:
            fuel = puzzle.calc_total_fuel_for_mass(mass)
            self.assertEqual(fuel, expected)



    def part1_calc_total_fuel(self, inputName:str) -> None:
        input, expected = self.get_test_data(inputName, 1)
        totalFuel = puzzle.part1(input)
        self.assertEqual(totalFuel, expected)

    def test_part1_example_1(self):
        self.part1_calc_total_fuel('example1')

    def test_part1_example_2(self):
        self.part1_calc_total_fuel('example2')

    def test_part1_example_3(self):
        self.part1_calc_total_fuel('example3')

    def test_part1_example_4(self):
        self.part1_calc_total_fuel('example4')

    def test_part1(self):
        self.part1_calc_total_fuel('input')



    def part2_calc_total_fuel(self, inputName:str) -> None:
        input, expected = self.get_test_data(inputName, 2)
        totalFuel = puzzle.part2(input)
        self.assertEqual(totalFuel, expected)

    def test_part2_example_2(self):
        self.part2_calc_total_fuel('example2')

    def test_part2_example_3(self):
        self.part2_calc_total_fuel('example3')

    def test_part2(self):
        self.part1_calc_total_fuel('input')
