from typing import List

import helper
import inputHelper

from puzzleBase import PuzzleBase



class InputData:
    input: List[str]
    expectedAnswer: int

    def __init__(self, name: str, part: int) -> None:
        day = 13
        self.input = inputHelper.load_input_file(day, name)
        
        lines = inputHelper.load_answer_file(day, part, name)
        self.expectedAnswer = int(lines[0]) if lines is not None else None



class BusDepatureInfo:
    busId: int
    departureTime: int

    def __init__(self, busId: int, departureTime: int):
        self.busId = busId
        self.departureTime = departureTime



class Puzzle(PuzzleBase):

    def get_next_time(self, currentTime: int, busId: int) -> int:
        while currentTime % busId != 0:
            currentTime += 1
        return currentTime


    def get_soonest_bus(self, earliestTime: int, buses: List[str]) -> BusDepatureInfo:
        nextDepartures: List[BusDepatureInfo] = []

        for x in buses:
            if x == "x":
                continue
            busId = int(x)
            time = self.get_next_time(earliestTime, busId)
            nextDepartures.append(BusDepatureInfo(busId, time))

        result = BusDepatureInfo(-1, 0)
        for z in nextDepartures:
            if z.departureTime < result.departureTime or result.busId < 0:
                result = z

        return result


    def get_next_matching_time(self, time: int, busId: int, increment: int, offset: int) -> int:
        time += increment
        while (time + offset) % busId != 0:
            time += increment
        return time


    def run_part1(self, data: InputData) -> str:
        earliestTime = int(data.input[0])
        buses = data.input[1].split(",")
        bus = self.get_soonest_bus(earliestTime, buses)
        result = (bus.departureTime - earliestTime) * bus.busId
        return helper.validate_result("What is the ID of the earliest bus you can take to the airport multiplied by the number of minutes you'll need to wait for that bus?", result, data.expectedAnswer)


    def run_part2(self, data: InputData) -> str:
        buses = data.input[1].split(",")
        increment = 0
        lcmValue = 1
        time = 0
        i = 0
        while i < len(buses):
            if buses[i] != "x":
                busId = int(buses[i])
                if increment == 0:
                    increment = 1
                time = self.get_next_matching_time(time, busId, increment, i)
                lcmValue = helper.get_lcm(lcmValue, busId)
                increment = lcmValue
            i += 1
        return helper.validate_result("What is the earliest timestamp such that all of the listed bus IDs depart at offsets matching their positions in the list?", time, data.expectedAnswer)


    def solve(self):
        print("Day 13: Shuttle Search")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part1(InputData('example1', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part1(InputData('input', 1)))

        print("")

        self.run_example(lambda: "P2 Ex1) " + self.run_part2(InputData('example1', 2)))
        self.run_example(lambda: "P2 Ex2) " + self.run_part2(InputData('example2', 2)))
        self.run_example(lambda: "P2 Ex3) " + self.run_part2(InputData('example3', 2)))
        self.run_example(lambda: "P2 Ex4) " + self.run_part2(InputData('example4', 2)))
        self.run_example(lambda: "P2 Ex5) " + self.run_part2(InputData('example5', 2)))
        self.run_example(lambda: "P2 Ex6) " + self.run_part2(InputData('example6', 2)))
        self.run_problem(lambda: "Part 2) " + self.run_part2(InputData('input', 2)))