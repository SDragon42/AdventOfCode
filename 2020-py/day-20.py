import utils

class TileData:
    id: int = 0
    tile: list[str] = []
    tileSize: int = 0

    def __init__(self, inputData: str) -> None:
        lines = inputData.split("\n")
        tmp = lines[0]

        self.id = int(tmp[5:tmp.index(":")])
        self.tile = lines[1:]
        self.tileSize = len(self.tile)

    def get_edge_top(self) -> str:
        return self.tile[0]

    def get_edge_bottom(self) -> str:
        return self.tile[self.tileSize - 1][::-1]

    def get_edge_left(self) -> str:
        return self.get_vertical_line(0)

    def get_edge_right(self) -> str:
        return self.get_vertical_line(self.tileSize - 1)[::-1]

    def get_all_edges(self) -> list[str]:
        edges = [
            self.get_edge_top(),
            self.get_edge_right(),
            self.get_edge_bottom(),
            self.get_edge_left(),
        ]
        return edges

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

    def flip_tile(self):
        y = 0
        while y < self.tileSize:
            self.tile[y] = self.tile[y][::-1]
            y += 1

    def display_tile(self):
        print(f"Tile {self.id}:")
        print("\n".join(self.tile))

#-------------------------------------------------------------------------------

def create_puzzle_tiles(inputData: str) -> list[TileData]:
    puzzleData = inputData.split("\n\n")
    tiles = [TileData(p) for p in puzzleData]
    return tiles


def does_edge_match(tileList: list[TileData], edge: str) -> bool:
    edgeReversed = edge[::-1]
    for tile in tileList:
        for otherEdge in tile.get_all_edges():
            if otherEdge == edge or otherEdge == edgeReversed:
                return True
    return False


def get_open_side_tiles(tileList: list[TileData], numOpenSides: int) -> list[TileData]:
    openTiles: list[TileData] = []
    for tile in tileList:
        otherTiles = tileList.copy()
        otherTiles.remove(tile)

        num = 0
        for edge in tile.get_all_edges():
            if not does_edge_match(otherTiles, edge):
                num += 1
                continue
        if num == numOpenSides:
            openTiles.append(tile)
    return openTiles


def run_part1(title: str, inputData: str, correctResult: int):
    tileList = create_puzzle_tiles(inputData)
    cornerTiles = get_open_side_tiles(tileList, 2)
    result = 1
    for corner in cornerTiles:
        result *= corner.id
        utils.dprint(f"tile id: {corner.id}")
    utils.validate_result(title, result, correctResult)


def run_part2(title: str, inputData: str, correctResult: int):
    tileList = create_puzzle_tiles(inputData)
    cornerTiles = get_open_side_tiles(tileList, 2)
    sideTiles = get_open_side_tiles(tileList, 1)
    for t in cornerTiles: tileList.remove(t)
    for t in sideTiles: tileList.remove(t)

    result = 1
    for corner in cornerTiles:
        result *= corner.id
        utils.dprint(f"tile id: {corner.id}")
    utils.validate_result(title, result, correctResult)


# utils.showDebug = True
if __name__ == "__main__":
    day = 20
    print(f"---- Day {day}: Jurassic Jigsaw ----")

    # run_part1("Test Case 1",
    #     utils.read_input(day, "example1"),
    #     20899048083289)
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