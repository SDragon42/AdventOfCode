import collections
from typing import List, Dict, Tuple



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



def parse_move_line(line: str) -> List[str]:
    moveList: List[str] = []
    i = 0
    while i < len(line):
        toCopy = 1
        if line[i] == 'n' or line[i] == 's':
            toCopy = 2
        moveList.append(line[i:i+toCopy])
        i += toCopy
    return moveList


def get_addr(moveList: List[str]) -> SpatialAddress:
    addr = SpatialAddress([0, 0])
    for move in moveList:
        offset = DIRECTION_OFFSETS[move]
        addr.shift(offset)
    return addr


def count_black_tiles(values: List[bool]) -> int:
    return sum([1 for v in values if v == BLACK])



def build_tile_floor(input: List[str]) -> TileDict:
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
    return value1 if value1 < value2 else value2


def get_highest(value1: int, value2: int) -> int:
    return value1 if value1 > value2 else value2


def get_range(tileFloor: TileDict, dimIdx: int) -> Tuple[int, int]:
    low = 0
    high = 0
    for key in tileFloor:
        addr = address_str_to_spatial_address(key)
        low = get_lowest(low, addr.coordinates[dimIdx])
        high = get_highest(high, addr.coordinates[dimIdx])
    return (low, high)


def get_search_range(tileFloor: TileDict, dimension: int) -> List[int]:
    r = get_range(tileFloor, dimension)
    r = (r[0] - 1, r[1] + 1)
    dist = abs(r[1] - r[0]) + 1
    offset = r[0]
    l = [x + offset for x in range(dist)]
    return l


def build_address_list(tileFloor: TileDict) -> List[str]:
    xRange = get_search_range(tileFloor, 0)
    yRange = get_search_range(tileFloor, 1)

    addressKeys: List[str] = []
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
    for dirOffset in DIRECTION_OFFSETS.values():
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


def run_part1(input: List[int]) -> int:
    tileFloor = build_tile_floor(input)
    result = count_black_tiles([x for x in tileFloor.values()])
    return result


def run_part2(input: List[int]) -> int:
    tileFloor = build_tile_floor(input)
    day = 0
    result = count_black_tiles([x for x in tileFloor.values()])
    # helper.dprint(f"Initial: {result} black tiles")
    while day < 100:
        day += 1
        update_floor_tiles(tileFloor)
        result = count_black_tiles([x for x in tileFloor.values()])
        # helper.dprint(f"Day {day}: {result}")

    return result
