import utils
import collections

# flippedTiles: set['LobbyTile'] = set()

directions: dict[str, tuple[int, int]] = {'e':  (2,  0), 'w':  (-2,  0),
                               'ne': (1,  1), 'nw': (-1,  1),
                               'se': (1, -1), 'sw': (-1, -1)}

allTiles: dict[str, bool] = collections.defaultdict(bool)
# flippedTileKeys: set[str] = set()

# class LobbyTile:
#     isBlack: bool = False
#     # adjacentTiles: dict[str, 'LobbyTile'] = {}

#     def __init__(self) -> None:
#         # global flippedTiles
#         self.isBlack = False
#         # self.adjacentTiles = {}
#         # flippedTiles.add(self)

#     def flip(self) -> None:
#         global flippedTiles
#         self.isBlack = not self.isBlack
#         flippedTiles.add(self)
#         # utils.dprint(f"Black? {self.isBlack}  -  {self}")

#     def get_tile(self, dir: str) -> 'LobbyTile':
#         if dir not in self.adjacentTiles:
#             self.adjacentTiles[dir] = LobbyTile()
#         return self.adjacentTiles[dir]

#-------------------------------------------------------------------------------

def parse_move_line(line: str) -> list[str]:
    moveList = []
    i = 0
    while i < len(line):
        toCopy = 1
        if line[i] == 'n' or line[i] == 's':
            toCopy = 2
        moveList.append(line[i:i+toCopy])
        i += toCopy
    # utils.dprint(f"moves: {moveList}")
    # utils.dprint(line)
    # utils.dprint(moveList)
    # utils.dprint("")
    return moveList


def get_addr_key(x: int, y: int) -> str:
    key = f"{x}|{y}"
    return key


def get_addr(moveList: list[str]) -> tuple[int, int]:
    x = 0
    y = 0
    for move in moveList:
        offset = directions[move]
        x += offset[0]
        y += offset[1]
    return (x,y)


def count_black_tiles() -> int:
    count = 0
    # for key in flippedTileKeys:
    #     if allTiles[key]:
    #         count += 1
    for v in allTiles.values():
        if v:
            count += 1
    # for t in flippedTiles:
    #     if t.isBlack:
    #         count += 1
    return count


def run_part1(title: str, input: list[str], correctResult: int):
    # origin = LobbyTile()
    allTiles.clear()
    for line in input:
        moveList = parse_move_line(line)
        if len(moveList) == 0:
            continue

        addr = get_addr(moveList)
        addrKey = get_addr_key(addr[0], addr[1])

        allTiles[addrKey] = not allTiles[addrKey]
        # tile = origin
        # for move in moveList:
        #     tile = tile.get_tile(move)
        # tile.flip()

    result = count_black_tiles()
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