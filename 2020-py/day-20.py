import utils
import math

# part 2 assistence from:
# https://nopaste.ml/#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

# def edge_to_num(s: str) -> int:
#     return int(s.replace('#', '1').replace('.', '0'), 2)


# def binary_reverse(n: int) -> int:
#     return int(format(n,'010b')[::-1], 2)


class TileData:
    id: int = 0
    tile: list[str] = []
    tileSize: int = 0

    # tileSides: dict[str, str] = {}
    tileSides: list[str] = []
    allPossibleTileSides: list[str] = []

    def __init__(self, id: int, tileData: list[str]) -> None:
        self.id = id
        self.tile = tileData
        self.tileSize = len(self.tile)
        self.set_tile_sides()

    def __repr__(self) -> str:
        return f"Tile {self.id}:\n" + "\n".join([x for x in self.tile])

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
        return self.tile[0]

    def get_edge_bottom(self) -> str:
        return self.tile[self.tileSize - 1]

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
        newTile: list[str] = []
        x = 0
        while x < self.tileSize:
            newLine = self.get_vertical_line(x)
            newTile.append(newLine)
            x += 1
        self.tile.clear()
        self.tile = newTile
        self.set_tile_sides()

    def flip_tile(self):
        y = 0
        while y < self.tileSize:
            self.tile[y] = self.tile[y][::-1]
            y += 1
        self.set_tile_sides()

#-------------------------------------------------------------------------------

def create_tile(data: str) -> TileData:
    lines = data.split("\n")
    tmp = lines[0]
    id = int(tmp[5:tmp.index(":")])
    tile = TileData(id, lines[1:])
    return tile


def create_all_tiles(inputData: str) -> list[TileData]:
    puzzleData = inputData.split("\n\n")
    tileList = [create_tile(p) for p in puzzleData]
    return tileList

def find_matching_tiles(tileList: list[TileData], edge: str) -> list[TileData]:
    return [
        tile
        for tile in tileList
        if edge in tile.allPossibleTileSides]


def does_edge_match(tileList: list[TileData], edge: str) -> bool:
    for t in tileList:
        if edge in t.allPossibleTileSides:
            return True
    return False


def get_open_side_tiles(tileList: list[TileData], numOpenSides: int) -> list[TileData]:
    openTiles: list[TileData] = []
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
    # if utils.showDebug:
    #     for t in tileList:
    #         print(str(t))
    #         print()
    #         t2 = "\n".join(t.get_tile_date_without_edges())
    #         print(t2)
    #         print()
    #         print()
    cornerTiles = get_open_side_tiles(tileList, 2)
    result = 1
    for corner in cornerTiles:
        result *= corner.id
        utils.dprint(f"tile id: {corner.id}")
    utils.validate_result(title, result, correctResult)


def run_part2(title: str, inputData: str, correctResult: int):
    tileList = create_all_tiles(inputData)

    sideSize = int(math.sqrt(len(tileList)))

    grid: list[list[TileData]] = [[None for x in range(sideSize)] for y in range(sideSize)] # jigsaw grid

    cornerTiles = get_open_side_tiles(tileList, 2)
    # sideTiles = get_open_side_tiles(tileList, 1)
    # for t in cornerTiles: tileList.remove(t)
    # for t in sideTiles: tileList.remove(t)

    t = cornerTiles[0]
    grid[0][0] = t

    # starting corner
    for transformFunc in [t.rotate_tile, t.rotate_tile, t.rotate_tile, t.rotate_tile, t.flip_tile, t.rotate_tile, t.rotate_tile, t.rotate_tile]:
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
        rt = rtList[0]
        for transformFunc in [rt.rotate_tile, rt.rotate_tile, rt.rotate_tile, rt.flip_tile, rt.rotate_tile, rt.rotate_tile, rt.rotate_tile]:
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
            nt = ntList[0]
            for transformFunc in [nt.rotate_tile, nt.rotate_tile, nt.rotate_tile, nt.flip_tile, nt.rotate_tile, nt.rotate_tile, nt.rotate_tile, nt.rotate_tile]:
                if nt.get_edge_bottom() == edge:
                    grid[y][x] = nt
                    break
                transformFunc()

    # remove all edges
    for y in range(sideSize):
        for x in range(sideSize):
            grid[y][x].remove_edges()

    # consolidate image
    imageList: list[str] = []

    for y in range(sideSize - 1, -1, -1): #top down (each tile)
        for i in range(grid[0][0].tileSize):
            line = "".join([grid[y][x].tile[i] for x in range(sideSize)])
            imageList.append(line)

    t = TileData(0, imageList)

    monsterLengh = 20
    spaces = " "*(t.tileSize - monsterLengh)
    monsterimage = f"                  # {spaces}#    ##    ##    ###{spaces} #  #  #  #  #  #   "
    monsterIndexes = [i for i,c in enumerate(monsterimage) if c == '#']

    for transformFunc in [t.rotate_tile, t.rotate_tile, t.rotate_tile, t.rotate_tile, t.flip_tile, t.rotate_tile, t.rotate_tile, t.rotate_tile]:
        image = "".join(t.tile)
        for row in range(t.tileSize - 2):
            for i in range(t.tileSize - monsterLengh):
                start = (row * (t.tileSize)) + i
                if find_monster(image, start, monsterIndexes):
                    tag_monster(t, start, monsterIndexes)
        transformFunc()

    # fullImage.rotate_tile()
    # print(str(fullImage))
    # fullImage = "\n".join(imageList)
    # print(fullImage)

        # image += [''.join([grid[y][x].image_edges_stripped()[i] for x in range(12)]) for i in range(8)]



    result = 1
    for corner in cornerTiles:
        result *= corner.id
        utils.dprint(f"tile id: {corner.id}")
    utils.validate_result(title, result, correctResult)

def find_monster(image: str, idx: int, monsterIndexes: list[int]) -> bool:
    result = not any([image[idx + i] == '.' for i in monsterIndexes])
    return result

def tag_monster(tile: TileData, idx: int, monsterIndexes: list[int]) -> None:
    utils.dprint("monster found")
    for i in monsterIndexes:
        tIdx = idx + i
        r = (tIdx % tile.tileSize)
        row = (tIdx - r) // tile.tileSize
        s = [c for c in tile.tile[row]]

        pass
    pass


utils.showDebug = True
if __name__ == "__main__":
    day = 20
    print(f"---- Day {day}: Jurassic Jigsaw ----")

    run_part1("Test Case 1",
        utils.read_input(day, "example1"),
        20899048083289)
    # run_part1("problem",
    #     utils.read_input(day, "input"),
    #     28057939502729)

    print("---- part 2 ----")

    run_part2("Test Case 1",
        utils.read_input(day, "example1"),
        273)
    # run_part2("problem",
    #     utils.read_input(day, "input"),
    #     0)