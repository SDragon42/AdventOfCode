import utils
import collections
import math

BLACK = True
WHITE = False

class SpatialAddress:
    coordinates: list[int] = []

    def __init__(self, coordinates: list[int]) -> None:
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

#-------------------------------------------------------------------------------


directionOffsets: dict[str, 'SpatialAddress'] = {
    "e": SpatialAddress([2, 0]),
    "w": SpatialAddress([-2, 0]),
    "ne": SpatialAddress([1, 1]),
    "nw": SpatialAddress([-1, 1]),
    "se": SpatialAddress([1, -1]),
    "sw": SpatialAddress([-1, -1]),
}

TileDict = dict[str, bool]


def parse_move_line(line: str) -> list[str]:
    moveList = []
    i = 0
    while i < len(line):
        toCopy = 1
        if line[i] == 'n' or line[i] == 's':
            toCopy = 2
        moveList.append(line[i:i+toCopy])
        i += toCopy
    return moveList


def get_addr(moveList: list[str]) -> SpatialAddress:
    addr = SpatialAddress([0, 0])
    for move in moveList:
        offset = directionOffsets[move]
        addr.shift(offset)
    return addr


def count_black_tiles(values: list[bool]) -> int:
    count = 0
    for v in values:
        if v == BLACK:
            count += 1
    return count



def build_tile_floor(input: list[str]) -> TileDict:
    tileFloor: TileDict = collections.defaultdict(bool)
    for line in input:
        moveList = parse_move_line(line)
        if len(moveList) == 0:
            continue
        addr = get_addr(moveList)
        addrKey = str(addr)
        tileFloor[addrKey] = not tileFloor[addrKey]
    return tileFloor


def address_str_to_spatial_address(addressKey: str) -> SpatialAddress:
    keyList = [int(x) for x in addressKey.split(",")]
    addr = SpatialAddress(keyList)
    return addr


def get_lowest(value1: int, value2: int) -> int:
    if value1 < value2:
        return value1
    return value2


def get_highest(value1: int, value2: int) -> int:
    if value1 > value2:
        return value1
    return value2


def get_range(tileFloor: TileDict, dimIdx: int) -> tuple[int, int]:
    low = 0
    high = 0
    for key in tileFloor:
        addr = address_str_to_spatial_address(key)
        low = get_lowest(low, addr.coordinates[dimIdx])
        high = get_highest(high, addr.coordinates[dimIdx])
    return (low, high)


def get_search_range(tileFloor: TileDict, dimension: int) -> list[int]:
    r = get_range(tileFloor, dimension)
    r = (r[0] - 1, r[1] + 1)
    dist = abs(r[1] - r[0]) + 1
    offset = r[0]
    l = [x + offset for x in range(dist)]
    return l


def build_address_list(tileFloor: TileDict) -> list[str]:
    xRange = get_search_range(tileFloor, 0)
    yRange = get_search_range(tileFloor, 1)

    addressKeys: list[str] = []
    for x in xRange:
        for y in yRange:
            bothOdd = (x % 2 == 1 and y % 2 == 1)
            bothEven = (x % 2 == 0 and y % 2 == 0)
            if bothOdd or bothEven:
                addr = SpatialAddress([x, y])
                addressKeys.append(str(addr))
    return addressKeys


def count_adjacent_black_tiles(addressKey: str, tileFloor: TileDict) -> int:
    addr = address_str_to_spatial_address(addressKey)
    blackTileCount = 0
    for dirOffset in directionOffsets.values():
        adjacentAddr = addr.create_shifted(dirOffset)
        if tileFloor[str(adjacentAddr)] == BLACK:
            blackTileCount += 1
    return blackTileCount


def rule_black_tile(addressKey: str, tileFloor: TileDict) -> bool:
    """Any black tile with zero or more than 2 black tiles immediately adjacent to it is flipped to white."""
    blackTileCount = count_adjacent_black_tiles(addressKey, tileFloor)
    if blackTileCount == 0 or blackTileCount > 2:
        return WHITE
    return BLACK


def rule_white_tile(addressKey: str, tileFloor: TileDict) -> bool:
    """Any white tile with exactly 2 black tiles immediately adjacent to it is flipped to black."""
    blackTileCount = count_adjacent_black_tiles(addressKey, tileFloor)
    if blackTileCount == 2:
        return BLACK
    return WHITE


def update_floor_tiles(tileFloor: TileDict):
    prevFloor = tileFloor.copy()
    addressList = build_address_list(prevFloor)
    for addressKey in addressList:
        isBlack = prevFloor[addressKey]
        if isBlack:
            tileFloor[addressKey] = rule_black_tile(addressKey, prevFloor)
        else:
            tileFloor[addressKey] = rule_white_tile(addressKey, prevFloor)


def run_part1(title: str, input: list[str], correctResult: int):
    tileFloor = build_tile_floor(input)
    result = count_black_tiles(tileFloor.values())
    utils.validate_result(title, result, correctResult)


def run_part2(title: str, input: list[str], correctResult: int):
    tileFloor = build_tile_floor(input)
    day = 0
    result = count_black_tiles(tileFloor.values())
    utils.dprint(f"Initial: {result} black tiles")
    while day < 100:
        day += 1
        update_floor_tiles(tileFloor)
        result = count_black_tiles(tileFloor.values())
        utils.dprint(f"Day {day}: {result}")


    utils.validate_result(title, result, correctResult)


# utils.showDebug = True
if __name__ == "__main__":
    day = 24
    print(f"---- Day {day}: Lobby Layout ----")

    # run_part1("Test Case 0",
    #     ['esew', 'nwwswee'],
    #     2)
    # run_part1("Test Case 1",
    #     utils.read_input_as_list(day, "example1"),
    #     10)
    run_part1("problem",
        utils.read_input_as_list(day, "input"),
        339)

    print("---- part 2 ----")

    # run_part2("Test Case 1",
    #     utils.read_input_as_list(day, "example1"),
    #     2208)
    run_part2("problem",
        utils.read_input_as_list(day, "input"),
        3794)