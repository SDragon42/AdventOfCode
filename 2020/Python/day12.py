from typing import List

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    input: List[str]
    expectedAnswer: int

    def __init__(self, name: str, part: int) -> None:
        day = 12
        self.input = inputHelper.load_file(day, name).splitlines()
        
        answer = inputHelper.load_file(day, f"{name}-answer{part}")
        self.expectedAnswer = int(answer) if answer is not None else None



class WaypointPosition:
    x: int
    y: int

    def __init__(self, x: int, y:int):
        self.x = x
        self.y = y



class Position:
    x: int
    y: int
    heading: str
    waypoint: WaypointPosition

    def __init__(self, x: int, y:int, heading: str):
        self.x = x
        self.y = y
        self.heading = heading
        self.waypoint = WaypointPosition(0,0)



class Puzzle(PuzzleBase):
    cardinals = ["N", "E", "S", "W"]


    def move_north(self, currentPos: Position, value: int) -> Position:
        newPos = Position(currentPos.x, currentPos.y + value, currentPos.heading)
        return newPos

    def move_south(self, currentPos: Position, value: int) -> Position:
        newPos = Position(currentPos.x, currentPos.y - value, currentPos.heading)
        return newPos

    def move_east(self, currentPos: Position, value: int) -> Position:
        newPos = Position(currentPos.x + value, currentPos.y, currentPos.heading)
        return newPos

    def move_west(self, currentPos: Position, value: int) -> Position:
        newPos = Position(currentPos.x - value, currentPos.y, currentPos.heading)
        return newPos

    def get_cardinal(self, heading: str, steps: int) -> str:
        i = self.cardinals.index(heading)
        i += steps
        if i < 0:
            i += len(self.cardinals)
        elif i >= len(self.cardinals):
            i -= len(self.cardinals)
        return self.cardinals[i]

    def turn_left(self, currentPos: Position, value: int) -> Position:
        steps = int((value / 360) * 4) * -1
        newCardinal = self.get_cardinal(currentPos.heading, steps)
        return Position(currentPos.x, currentPos.y, newCardinal)

    def turn_right(self, currentPos: Position, value: int) -> Position:
        steps = int((value / 360) * 4)
        newCardinal = self.get_cardinal(currentPos.heading, steps)
        return Position(currentPos.x, currentPos.y, newCardinal)


    #-------------------------------------------------------------------------------


    def move_waypoint_north(self, currentPos: Position, value: int) -> Position:
        wp = currentPos.waypoint
        currentPos.waypoint = WaypointPosition(wp.x, wp.y + value)
        return currentPos

    def move_waypoint_south(self, currentPos: Position, value: int) -> Position:
        wp = currentPos.waypoint
        currentPos.waypoint = WaypointPosition(wp.x, wp.y - value)
        return currentPos

    def move_waypoint_east(self, currentPos: Position, value: int) -> Position:
        wp = currentPos.waypoint
        currentPos.waypoint = WaypointPosition(wp.x + value, wp.y)
        return currentPos

    def move_waypoint_west(self, currentPos: Position, value: int) -> Position:
        wp = currentPos.waypoint
        currentPos.waypoint = WaypointPosition(wp.x - value, wp.y)
        return currentPos


    def rotate_waypoint_left(self, currentPos: Position, value: int) -> Position:
        wp = currentPos.waypoint
        if value == 270:
            return self.rotate_waypoint_right(currentPos, 90)
        elif value == 180:
            currentPos.waypoint = WaypointPosition(wp.x * -1, wp.y * -1)
        else:
            currentPos.waypoint = WaypointPosition(wp.y * -1, wp.x)

        return currentPos

    def rotate_waypoint_right(self, currentPos: Position, value: int) -> Position:
        wp = currentPos.waypoint
        if value == 270:
            return self.rotate_waypoint_left(currentPos, 90)
        elif value == 180:
            currentPos.waypoint = WaypointPosition(wp.x * -1, wp.y * -1)
        else:
            currentPos.waypoint = WaypointPosition(wp.y, wp.x * -1)

        return currentPos


    def full_steam_ahead(self, currentPos: Position, value: int) -> Position:
        currentPos.x += currentPos.waypoint.x * value
        currentPos.y += currentPos.waypoint.y * value
        return currentPos


    #-------------------------------------------------------------------------------


    def move_ship(self, currentPos: Position, code: str, value: int, path: List[Position]) -> Position:
        if code == "F":
            code = currentPos.heading

        newPos = self.actions[code](self, currentPos, value)
        path.append(newPos)
        return newPos
        

    def calc_manhattan_distance(self, x: int, y: int) -> int:
        return abs(x) + abs(y)


    def run_part1(self, data: InputData) -> str:
        currentPos = Position(0, 0, "E")
        path: List[Position] = []
        path.append(currentPos)

        for line in data.input:
            code = line[0]
            num = int(line[1:])
            currentPos = self.move_ship(currentPos, code, num, path)

        result = self.calc_manhattan_distance(currentPos.x, currentPos.y)
        return helper.validate_result('What is the Manhattan distance between that location and the ship''s starting position?', result, data.expectedAnswer)


    def run_part2(self, data: InputData) -> str:
        currentPos = Position(0, 0, "E")
        currentPos.waypoint = WaypointPosition(10, 1)

        path: List[Position] = []
        path.append(currentPos)

        for line in data.input:
            code = line[0]
            num = int(line[1:])
            currentPos = self.actionsP2[code](self, currentPos, num)
            path.append(currentPos)

        result = self.calc_manhattan_distance(currentPos.x, currentPos.y)
        return helper.validate_result('What is the Manhattan distance between that location and the ship''s starting position?', result, data.expectedAnswer)


    actions = {
        "N": move_north,
        "S": move_south,
        "E": move_east,
        "W": move_west,
        "L": turn_left,
        "R": turn_right
    }

    actionsP2 = {
        "N": move_waypoint_north,
        "S": move_waypoint_south,
        "E": move_waypoint_east,
        "W": move_waypoint_west,
        "L": rotate_waypoint_left,
        "R": rotate_waypoint_right,
        "F": full_steam_ahead
    }


    def solve(self):
        print("Day 12: Rain Risk")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part1(InputData('example1', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part1(InputData('input', 1)))

        print("")

        self.run_example(lambda: "P2 Ex1) " + self.run_part2(InputData('example1', 2)))
        self.run_problem(lambda: "Part 2) " + self.run_part2(InputData('input', 2)))