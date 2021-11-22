import sys
import math
from typing import Any, List

sys.path.append('../../Python.Common')
import helper
import inputHelper

# part 2 assistence from:
# https://nopaste.ml/#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

TileGrid = List[List[str]]

class TileData:
    id: int
    tile: TileGrid
    tileSize: int
    tileSides: List[str]
    allPossibleTileSides: List[str]

    def __init__(self, id: int, tileData: List[str]) -> None:
        self.id = id
        self.tile = []
        for line in tileData:
            tmp = [c for c in line]
            self.tile.append(tmp)
        self.tileSize = len(self.tile)
        self.tileSides = []
        self.allPossibleTileSides = []
        self.set_tile_sides()

    def __repr__(self) -> str:
        return f"Tile {self.id}:\n" + "\n".join(["".join(x) for x in self.tile])

    def remove_edges(self) -> None:
        self.tile = [line[1:self.tileSize - 1] for line in self.tile[1:self.tileSize - 1]]
        self.tileSize = len(self.tile)
        self.set_tile_sides()

    def set_tile_sides(self) -> None:
        self.tileSides = [
            self.get_edge_top(),
            self.get_edge_right(),
            self.get_edge_bottom(),
            self.get_edge_left()
        ]
        self.allPossibleTileSides = self.tileSides + [x[::-1] for x in self.tileSides]

    def get_edge_top(self) -> str:
        return "".join(self.tile[0])

    def get_edge_bottom(self) -> str:
        return "".join(self.tile[self.tileSize - 1])

    def get_edge_left(self) -> str:
        return self.get_vertical_line(0)

    def get_edge_right(self) -> str:
        return self.get_vertical_line(self.tileSize - 1)

    def get_vertical_line(self, index: int) -> str:
        y = self.tileSize - 1
        newLine = ""
        while y >= 0:
            newLine += self.tile[y][index]
            y -= 1
        return newLine

    def rotate_tile(self):
        newTile: TileGrid = []
        x = 0
        while x < self.tileSize:
            newLine = self.get_vertical_line(x)
            tmp = [c for c in newLine]
            newTile.append(tmp)
            x += 1
        self.tile = newTile
        self.set_tile_sides()

    def flip_tile(self):
        y = 0
        while y < self.tileSize:
            self.tile[y].reverse()
            y += 1
        self.set_tile_sides()

    def get_rotate_flip_actions(self) -> List[Any]:
        actionList = ([self.rotate_tile] * 3) + [self.flip_tile] + ([self.rotate_tile] * 4)
        return actionList

#-------------------------------------------------------------------------------

def create_tile(data: str) -> TileData:
    lines = data.split("\n")
    tmp = lines[0]
    id = int(tmp[5:tmp.index(":")])
    tile = TileData(id, lines[1:])
    return tile


def create_all_tiles(inputData: str) -> List[TileData]:
    puzzleData = inputData.split("\n\n")
    tileList = [create_tile(p) for p in puzzleData]
    return tileList

def find_matching_tiles(tileList: List[TileData], edge: str) -> List[TileData]:
    return [
        tile
        for tile in tileList
        if edge in tile.allPossibleTileSides]


def does_edge_match(tileList: List[TileData], edge: str) -> bool:
    for t in tileList:
        if edge in t.allPossibleTileSides:
            return True
    return False


def get_open_side_tiles(tileList: List[TileData], numOpenSides: int) -> List[TileData]:
    openTiles: List[TileData] = []
    for tile in tileList:
        otherTiles = tileList.copy()
        otherTiles.remove(tile)

        num = 0
        for edge in tile.tileSides:
            if not does_edge_match(otherTiles, edge):
                num += 1
                continue
        if num == numOpenSides:
            openTiles.append(tile)
    return openTiles


def run_part1(title: str, inputData: str, correctResult: int):
    tileList = create_all_tiles(inputData)
    cornerTiles = get_open_side_tiles(tileList, 2)
    result = 1
    for corner in cornerTiles:
        result *= corner.id
        helper.dprint(f"tile id: {corner.id}")
    helper.validate_result(title, result, correctResult)


def run_part2(title: str, inputData: str, correctResult: int):
    tileList = create_all_tiles(inputData)

    sideSize = int(math.sqrt(len(tileList)))

    grid: List[List[Any]] = [[None for x in range(sideSize)] for y in range(sideSize)]

    cornerTiles = get_open_side_tiles(tileList, 2)

    t = cornerTiles[0]
    grid[0][0] = t

    # starting corner
    for transformFunc in t.get_rotate_flip_actions():
        bottomMatches = find_matching_tiles(tileList, t.get_edge_bottom())
        leftMatches = find_matching_tiles(tileList, t.get_edge_left())
        if len(bottomMatches) == 1 and len(leftMatches) == 1:
            break
        transformFunc()

    # bottom row
    for x in range(1, sideSize):
        edge = grid[0][x-1].get_edge_right()
        rtList = find_matching_tiles(tileList, edge)
        rtList.remove(grid[0][x-1])
        assert(len(rtList) == 1)
        rt = rtList[0]
        for transformFunc in rt.get_rotate_flip_actions():
            if rt.get_edge_left() == edge:
                grid[0][x] = rt
                break
            transformFunc()

    # each column
    for x in range(sideSize):
        for y in range(1, sideSize):
            edge = grid[y-1][x].get_edge_top()
            ntList = find_matching_tiles(tileList, edge)
            ntList.remove(grid[y-1][x])
            assert(len(ntList) == 1)
            nt = ntList[0]
            for transformFunc in nt.get_rotate_flip_actions():
                if nt.get_edge_bottom() == edge:
                    grid[y][x] = nt
                    break
                transformFunc()

    # remove all edges
    for y in range(sideSize):
        for x in range(sideSize):
            grid[y][x].remove_edges()

    # consolidate image
    imageList: List[str] = []

    for y in range(sideSize - 1, -1, -1): #top down (each tile)
        for i in range(grid[0][0].tileSize):
            lineParts = ["".join(grid[y][x].tile[i]) for x in range(sideSize)]

            line = "".join(lineParts)
            imageList.append(line)

    t = TileData(0, imageList)

    monsterLengh = 20
    spaces = " "*(t.tileSize - monsterLengh)
    monsterimage = f"                  # {spaces}#    ##    ##    ###{spaces} #  #  #  #  #  #   "
    monsterIndexes = [i for i,c in enumerate(monsterimage) if c == '#']

    monstersFound = False
    for transformFunc in t.get_rotate_flip_actions():
        image = "".join(["".join(x) for x in t.tile])
        for row in range(t.tileSize - 2):
            for i in range(t.tileSize - monsterLengh):
                start = (row * (t.tileSize)) + i
                if find_monster(image, start, monsterIndexes):
                    monstersFound = True
                    tag_monster(t, start, monsterIndexes)
        if monstersFound:
            break
        transformFunc()

    image = "\n".join(["".join(x) for x in t.tile])
    helper.dprint("--- IMAGE ---")
    helper.dprint(image)
    helper.dprint("")

    result = 0

    for row in range(t.tileSize):
        for x in range(t.tileSize):
            if t.tile[row][x] == "#":
                result += 1

    helper.validate_result(title, result, correctResult)

def find_monster(image: str, idx: int, monsterIndexes: List[int]) -> bool:
    result = not any([image[idx + i] == '.' for i in monsterIndexes])
    return result

def tag_monster(tile: TileData, idx: int, monsterIndexes: List[int]) -> None:
    helper.dprint("monster found")
    for i in monsterIndexes:
        tIdx = idx + i
        r = (tIdx % tile.tileSize)
        row = (tIdx - r) // tile.tileSize
        xidx = tIdx - (row * tile.tileSize)
        tile.tile[row][xidx] = "O"


def solve():
    day = 20
    print(f"Day {day}: Jurassic Jigsaw")
    print("")

    # run_part1("Test Case 1",
    #     inputHelper.read_input(day, "example1"),
    #     20899048083289)
    run_part1("Part 1)",
        inputHelper.read_input(day, "input"),
        28057939502729)

    print("")

    # run_part2("Test Case 1",
    #     inputHelper.read_input(day, "example1"),
    #     273)
    run_part2("Part 2)",
        inputHelper.read_input(day, "input"),
        2489)


if __name__ == "__main__":
    solve()