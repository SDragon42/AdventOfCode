import utils
import collections


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


def count_black_tiles(tileFloor: TileDict) -> int:
    count = 0
    for v in tileFloor.values():
        if v:
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


def run_part1(title: str, input: list[str], correctResult: int):
    tileFloor = build_tile_floor(input)
    result = count_black_tiles(tileFloor)
    utils.validate_result(title, result, correctResult)


def run_part2(title: str, input: list[str], correctResult: int):
    result = 0
    utils.validate_result(title, result, correctResult)


utils.showDebug = True
if __name__ == "__main__":
    day = 24
    print(f"---- Day {day}: Lobby Layout ----")

    run_part1("Test Case 0",
        ['esew', 'nwwswee'],
        2)
    run_part1("Test Case 1",
        utils.read_input_as_list(day, "example1"),
        10)
    run_part1("problem",
        utils.read_input_as_list(day, "input"),
        339)

    # print("---- part 2 ----")

    # run_part2("Test Case 1",
    #     utils.read_input_as_list(day, "example1"),
    #     0)
    # run_part2("problem",
    #     utils.read_input_as_list(day, "input"),
    #     0)