from typing import List, Dict

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    input: List[str]
    expectedAnswer: int

    def __init__(self, name: str, part: int) -> None:
        day = 11
        self.input = inputHelper.load_input_file(day, name)
        
        lines = inputHelper.load_answer_file(day, part, name)
        self.expectedAnswer = int(lines[0]) if lines is not None else None



class Offsets:
    x: int
    y: int

    def __init__(self, xOffset, yOffset):
        self.x = xOffset
        self.y = yOffset



Table = List[List[str]]



class Puzzle(PuzzleBase):

    directionOffsets: List[Offsets] = [
        Offsets(-1, -1),
        Offsets(0, -1),
        Offsets(1, -1),
        Offsets(1, 0),
        Offsets(1, 1),
        Offsets(0, 1),
        Offsets(-1, 1),
        Offsets(-1, 0),
    ]

    # helper functions
    #--------------------------------------------------------------------------------
    def clone_table(self, seats: Table) -> Table:
        cloned = [y.copy() for y in seats]
        return cloned


    def input_list_to_table(self, input: List[str]) -> Table:
        seats = [list(y) for y in input] 
        return seats


    def show_seat_map(self, seats: Table, title: str):
        helper.dprint(title)
        for y in seats:
            helper.dprint("".join(y))
        helper.dprint("")


    def count_occupied_seats(self, seats: Table) -> int:
        total = 0
        for y in seats:
            for x in y:
                if x == "#":
                    total += 1
        return total


    def is_inbounds(self, x: int, y: int, seats: Table) -> bool:
        if x < 0 or y < 0:
            return False
        if y >= len(seats):
            return False
        if x >= len(seats[y]):
            return False
        return True

    def get_seat_in_path(self, x: int, y: int, seats: Table, offset: Offsets) -> str:
        x += offset.x
        y += offset.y
        if not self.is_inbounds(x, y, seats):
            return ""
        return seats[y][x]
        

    def get_first_seat_in_path(self, x: int, y: int, seats: Table, offset: Offsets) -> str:
        found = self.get_seat_in_path(x, y, seats, offset)
        if found == "":
            return "."
        if found == ".":
            return self.get_first_seat_in_path(x + offset.x, y + offset.y, seats, offset)
        return found


    # common rules
    #--------------------------------------------------------------------------------
    def rule_floor(self, x: int, y: int, seats: Table) -> str:
        return seats[y][x]


    # rules for Part 1
    #--------------------------------------------------------------------------------
    def rule_empty_seat_part1(self, x: int, y: int, seats: Table) -> str:
        for offset in self.directionOffsets:
            if self.get_seat_in_path(x, y, seats, offset) == "#":
                return seats[y][x]
        return "#"


    def rule_occupied_seat_part1(self, x: int, y: int, seats: Table) -> str:
        numOccupied = 0
        for offset in self.directionOffsets:
            if self.get_seat_in_path(x, y, seats, offset) == "#":
                numOccupied += 1
        if numOccupied >= 4:
            return "L"
        return seats[y][x]


    # rules for Part 2
    #--------------------------------------------------------------------------------
    def rule_empty_seat_part2(self, x: int, y: int, seats: Table) -> str:
        for offset in self.directionOffsets:
            if self.get_first_seat_in_path(x, y, seats, offset) == "#":
                return seats[y][x]
        return "#"


    def rule_occupied_seat_part2(self, x: int, y: int, seats: Table) -> str:
        numOccupied = 0
        for offset in self.directionOffsets:
            if self.get_first_seat_in_path(x, y, seats, offset) == "#":
                numOccupied += 1
        if numOccupied >= 5:
            return "L"
        return seats[y][x]


    # Common
    #--------------------------------------------------------------------------------
    def apply_rules_to_seats(self, seats: Table, rules: Dict) -> int:
        numChanges = 0
        y = 0

        oldSeats = self.clone_table(seats)
        while y < len(oldSeats):
            x = 0
            while x < len(oldSeats[y]):
                currentSeat = oldSeats[y][x]
                newSeat = currentSeat
                if currentSeat in rules:
                    newSeat = rules[currentSeat](x, y, oldSeats)

                if newSeat != currentSeat:
                    seats[y][x] = newSeat
                    numChanges += 1
                
                x += 1
            y += 1

        return numChanges



    def run_part(self, data: InputData, rules: Dict) -> str:
        seats = self.input_list_to_table(data.input)

        numChanges = -1
        numPasses = 0

        while(numChanges != 0):
            numPasses += 1
            numChanges = self.apply_rules_to_seats(seats, rules)

        result = self.count_occupied_seats(seats)
        return helper.validate_result('How many seats end up occupied?', result, data.expectedAnswer)


    def solve(self):
        print("Day 11: Seating System")
        print("")

        rules_part1 = {
            ".": self.rule_floor,
            "L": self.rule_empty_seat_part1,
            "#": self.rule_occupied_seat_part1,
        }

        self.run_example(lambda: "P1 Ex1) " + self.run_part(InputData('example1', 1), rules_part1))
        self.run_problem(lambda: "Part 1) " + self.run_part(InputData('input', 1), rules_part1))

        print("")

        rules_part2 = {
            ".": self.rule_floor,
            "L": self.rule_empty_seat_part2,
            "#": self.rule_occupied_seat_part2,
        }

        self.run_example(lambda: "P2 Ex1) " + self.run_part(InputData('example1', 2), rules_part2))
        self.run_problem(lambda: "Part 2) " + self.run_part(InputData('input', 2), rules_part2))