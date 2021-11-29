import collections
from typing import List, Dict, Tuple

import helper
import inputHelper
from puzzleBase import PuzzleBase



class InputData:
    input: List[str]
    expectedAnswer: int

    def __init__(self, name: str, part: int) -> None:
        day = 24
        self.input = inputHelper.load_file(day, name).splitlines()
        self.expectedAnswer = int(inputHelper.load_file(day, f"{name}-answer{part}"))



class SpatialAddress:
    coordinates: List[int]


    def __init__(self, coordinates: List[int]) -> None:
        self.coordinates = coordinates


    def __str__(self) -> str:
        s = [str(value) for value in self.coordinates]
        return ",".join(s)


    def shift(self, offset: 'SpatialAddress') -> None:
        i = 0
        while i < len(self.coordinates):
            self.coordinates[i] += offset.coordinates[i]
            i += 1


    def create_shifted(self, offset: 'SpatialAddress'):
        newLocation = SpatialAddress(self.coordinates.copy())
        newLocation.shift(offset)
        return newLocation



BLACK = True
WHITE = False

DIRECTION_OFFSETS: Dict[str, 'SpatialAddress'] = {
    "e": SpatialAddress([2, 0]),
    "w": SpatialAddress([-2, 0]),
    "ne": SpatialAddress([1, 1]),
    "nw": SpatialAddress([-1, 1]),
    "se": SpatialAddress([1, -1]),
    "sw": SpatialAddress([-1, -1]),
}



TileDict = Dict[str, bool]



class Puzzle(PuzzleBase):

    def parse_move_line(self, line: str) -> List[str]:
        moveList: List[str] = []
        i = 0
        while i < len(line):
            toCopy = 1
            if line[i] == 'n' or line[i] == 's':
                toCopy = 2
            moveList.append(line[i:i+toCopy])
            i += toCopy
        return moveList


    def get_addr(self, moveList: List[str]) -> SpatialAddress:
        addr = SpatialAddress([0, 0])
        for move in moveList:
            offset = DIRECTION_OFFSETS[move]
            addr.shift(offset)
        return addr


    def count_black_tiles(self, values: List[bool]) -> int:
        return sum([1 for v in values if v == BLACK])



    def build_tile_floor(self, input: List[str]) -> TileDict:
        tileFloor: TileDict = collections.defaultdict(bool)
        for line in input:
            moveList = self.parse_move_line(line)
            if len(moveList) == 0:
                continue
            addr = self.get_addr(moveList)
            addrKey = str(addr)
            tileFloor[addrKey] = not tileFloor[addrKey]
        return tileFloor


    def address_str_to_spatial_address(self, addressKey: str) -> SpatialAddress:
        keyList = [int(x) for x in addressKey.split(",")]
        addr = SpatialAddress(keyList)
        return addr


    def get_lowest(self, value1: int, value2: int) -> int:
        return value1 if value1 < value2 else value2


    def get_highest(self, value1: int, value2: int) -> int:
        return value1 if value1 > value2 else value2


    def get_range(self, tileFloor: TileDict, dimIdx: int) -> Tuple[int, int]:
        low = 0
        high = 0
        for key in tileFloor:
            addr = self.address_str_to_spatial_address(key)
            low = self.get_lowest(low, addr.coordinates[dimIdx])
            high = self.get_highest(high, addr.coordinates[dimIdx])
        return (low, high)


    def get_search_range(self, tileFloor: TileDict, dimension: int) -> List[int]:
        r = self.get_range(tileFloor, dimension)
        r = (r[0] - 1, r[1] + 1)
        dist = abs(r[1] - r[0]) + 1
        offset = r[0]
        l = [x + offset for x in range(dist)]
        return l


    def build_address_list(self, tileFloor: TileDict) -> List[str]:
        xRange = self.get_search_range(tileFloor, 0)
        yRange = self.get_search_range(tileFloor, 1)

        addressKeys: List[str] = []
        for x in xRange:
            for y in yRange:
                bothOdd = (x % 2 == 1 and y % 2 == 1)
                bothEven = (x % 2 == 0 and y % 2 == 0)
                if bothOdd or bothEven:
                    addr = SpatialAddress([x, y])
                    addressKeys.append(str(addr))
        return addressKeys


    def count_adjacent_black_tiles(self, addressKey: str, tileFloor: TileDict) -> int:
        addr = self.address_str_to_spatial_address(addressKey)
        blackTileCount = 0
        for dirOffset in DIRECTION_OFFSETS.values():
            adjacentAddr = addr.create_shifted(dirOffset)
            if tileFloor[str(adjacentAddr)] == BLACK:
                blackTileCount += 1
        return blackTileCount


    def rule_black_tile(self, addressKey: str, tileFloor: TileDict) -> bool:
        """Any black tile with zero or more than 2 black tiles immediately adjacent to it is flipped to white."""
        blackTileCount = self.count_adjacent_black_tiles(addressKey, tileFloor)
        if blackTileCount == 0 or blackTileCount > 2:
            return WHITE
        return BLACK


    def rule_white_tile(self, addressKey: str, tileFloor: TileDict) -> bool:
        """Any white tile with exactly 2 black tiles immediately adjacent to it is flipped to black."""
        blackTileCount = self.count_adjacent_black_tiles(addressKey, tileFloor)
        if blackTileCount == 2:
            return BLACK
        return WHITE


    def update_floor_tiles(self, tileFloor: TileDict):
        prevFloor = tileFloor.copy()
        addressList = self.build_address_list(prevFloor)
        for addressKey in addressList:
            isBlack = prevFloor[addressKey]
            if isBlack:
                tileFloor[addressKey] = self.rule_black_tile(addressKey, prevFloor)
            else:
                tileFloor[addressKey] = self.rule_white_tile(addressKey, prevFloor)


    def run_part1(self, data: InputData) -> str:
        tileFloor = self.build_tile_floor(data.input)
        result = self.count_black_tiles([x for x in tileFloor.values()])
        return helper.validate_result("How many tiles are left with the black side up?", result, data.expectedAnswer)


    def run_part2(self, data: InputData) -> str:
        tileFloor = self.build_tile_floor(data.input)
        day = 0
        result = self.count_black_tiles([x for x in tileFloor.values()])
        helper.dprint(f"Initial: {result} black tiles")
        while day < 100:
            day += 1
            self.update_floor_tiles(tileFloor)
            result = self.count_black_tiles([x for x in tileFloor.values()])
            helper.dprint(f"Day {day}: {result}")

        return helper.validate_result("How many tiles will be black after 100 days?", result, data.expectedAnswer)


    def solve(self):
        print("Day 24: Lobby Layout")
        print("")

        self.run_example(lambda: "P1 Ex1) " + self.run_part1(InputData('example1', 1)))
        self.run_example(lambda: "P1 Ex2) " + self.run_part1(InputData('example2', 1)))
        self.run_problem(lambda: "Part 1) " + self.run_part1(InputData('input', 1)))

        print("")

        self.run_example(lambda: "P2 Ex2) " + self.run_part2(InputData('example2', 2)))
        self.run_problem(lambda: "Part 2) " + self.run_part2(InputData('input', 2)))