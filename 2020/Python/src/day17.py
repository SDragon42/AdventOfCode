from typing import List, Dict

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    input: List[str]
    expectedAnswer: int
    numDimensions: int

    def __init__(self, name: str, part: int) -> None:
        day = 17
        self.input = inputHelper.load_file(day, name).splitlines()

        self.numDimensions = int(inputHelper.load_file(day, f'dimensions{part}'))

        answer = inputHelper.load_file(day, f"{name}-answer{part}")
        self.expectedAnswer = int(answer) if answer is not None else None



class SpatialAddress:
    coordinates: List[int] = []

    def __init__(self, coordinates: List[int]):
        self.coordinates = coordinates

    def __str__(self) -> str:
        s = [str(value) for value in self.coordinates]
        return ",".join(s)

    def shift(self, offset: 'SpatialAddress'):
        i = 0
        while i < len(self.coordinates):
            self.coordinates[i] += offset.coordinates[i]
            i += 1

    def create_shifted(self, offset: 'SpatialAddress'):
        newLocation = SpatialAddress(self.coordinates.copy())
        newLocation.shift(offset)
        return newLocation



class ConwayCube:

    ACTIVE = "#"
    INACTIVE = "."

    SHIFTS: List[SpatialAddress] = []

    cubeSpace: Dict[str, str] = {}
    numDimensions: int = 0

    def __init__(self, inputList: List[str], numDimensions: int):
        self.numDimensions = numDimensions
        y = len(inputList) // 2
        xStart = (len(inputList[0])) // 2 * -1

        self.cubeSpace.clear()
        for line in inputList:
            x = xStart
            for c in line:
                baseCoordinate = [x, y]
                baseCoordinate.extend([0] * (numDimensions - 2))
                addr = SpatialAddress(baseCoordinate)
                self.cubeSpace[str(addr)] = c
                x += 1
            y -= 1

        self.build_address_offsets([0] * numDimensions)

    def inc_index(self, i: int, arr: List[int], maxIdx: int) -> bool:
        if arr[i] < maxIdx:
            arr[i] += 1
            return True

        arr[i] = 0

        if i == len(arr) - 1:
            return False

        return self.inc_index(i + 1, arr, maxIdx)

    def build_address_offsets(self, indexList: List[int]):
        self.SHIFTS.clear()
        offsetRange = [i for i in range(3)]
        maxIdx = len(offsetRange) - 1

        while True:
            addr = [(offsetRange[i] - 1) for i in indexList]

            isOrigin = True
            for i in addr:
                if i != 0:
                    isOrigin = False
                    break

            if not isOrigin:
                self.SHIFTS.append(SpatialAddress(addr))

            if not self.inc_index(0, indexList, maxIdx):
                break

    def create_location3d(self, coordinate: str) -> SpatialAddress:
        s = [int(value) for value in coordinate.split(",")]
        return SpatialAddress(s)

    def make_int_list(self, low: int, high: int) -> List[int]:
        result: List[int] = []
        while low <= high:
            result.append(low)
            low += 1
        return result

    def get_lowest(self, value1: int, value2: int) -> int:
        if value1 < value2:
            return value1
        return value2

    def get_highest(self, value1: int, value2: int) -> int:
        if value1 > value2:
            return value1
        return value2

    def get_range(self, dimIdx: int) -> List[int]:
        low = 0
        high = 0
        for key in self.cubeSpace:
            addr = self.create_location3d(key)
            low = self.get_lowest(low, addr.coordinates[dimIdx])
            high = self.get_highest(high, addr.coordinates[dimIdx])
        return self.make_int_list(low, high)


    def get_cube(self, addr: SpatialAddress, cubes: Dict[str, str]) -> str:
        sAddr = str(addr)
        if sAddr in cubes:
            return cubes[sAddr]
        return self.INACTIVE

    def set_cube(self, addr: SpatialAddress, newState: str):
        sAddr = str(addr)
        if newState == self.ACTIVE:
            self.cubeSpace[sAddr] = newState
            return
        self.cubeSpace.pop(sAddr, None)


    def inflate_range(self, indexes: List[int]):
        indexes.insert(0, indexes[0] - 1)
        indexes.append(indexes[len(indexes) - 1] + 1)


    def rule_active_cube(self, addr: SpatialAddress, cubes: Dict[str, str]):
        """
        If a cube is active and exactly 2 or 3 of its neighbors are also active, the cube remains active.
        Otherwise, the cube becomes inactive.
        """
        numActive = 0
        for offset in self.SHIFTS:
            tmpAddr = addr.create_shifted(offset)
            state = self.get_cube(tmpAddr, cubes)
            if state == self.ACTIVE:
                numActive += 1

        newState = self.INACTIVE
        if numActive == 2 or numActive == 3:
            newState = self.ACTIVE
        self.set_cube(addr, newState)


    def rule_inactive_cube(self, addr: SpatialAddress, cubes: Dict[str, str]):
        """
        If a cube is inactive but exactly 3 of its neighbors are active, the cube becomes active.
        Otherwise, the cube remains inactive.
        """
        numActive = 0
        for offset in self.SHIFTS:
            tmpAddr = addr.create_shifted(offset)
            state = self.get_cube(tmpAddr, cubes)
            if state == self.ACTIVE:
                numActive += 1

        newState = self.INACTIVE
        if numActive == 3:
            newState = self.ACTIVE
        self.set_cube(addr, newState)


    def apply_rule_at(self, address: List[int], rangeList: List[int], otherRanges: List[List[int]], cubes: Dict[str, str]):
        for i in rangeList:
            newAddr = address + [i]
            if len(otherRanges) > 0:
                self.apply_rule_at(newAddr, otherRanges[0], otherRanges[1:], cubes)
            else:
                addr = SpatialAddress(newAddr)
                state = self.get_cube(addr, cubes)

                if state == self.ACTIVE:
                    self.rule_active_cube(addr, cubes)
                elif state == self.INACTIVE:
                    self.rule_inactive_cube(addr, cubes)



    def apply_rules(self):
        oldCubeSpace = self.cubeSpace.copy()
        allRanges: List[List[int]] = []
        i = 0
        while i < self.numDimensions:
            temp = self.get_range(i)
            self.inflate_range(temp)
            allRanges.append(temp)
            i += 1

        self.apply_rule_at([], allRanges[0], allRanges[1:], oldCubeSpace)


    def get_num_active_cubes(self) -> int:
        result = 0
        for key in self.cubeSpace:
            if self.cubeSpace[key] == self.ACTIVE:
                result += 1
        return result



class Puzzle(PuzzleBase):

    def run_part(self, data: InputData) -> str:
        cc = ConwayCube(data.input, data.numDimensions)

        cycle = 1
        while cycle <= 6:
            cc.apply_rules()
            cycle += 1

        result = cc.get_num_active_cubes()
        return helper.validate_result('How many cubes are left in the active state after the sixth cycle?', result, data.expectedAnswer)


    def solve(self):
        print("Day 17: Conway Cubes")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part(InputData('example1', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part(InputData('input', 1)))

        print("")

        self.run_example(lambda: "P2 Ex1) " + self.run_part(InputData('example1', 2)))
        self.run_problem(lambda: "Part 2) " + self.run_part(InputData('input', 2)))