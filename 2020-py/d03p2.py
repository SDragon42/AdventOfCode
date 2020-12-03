import utils


def count_trees(input: list[str], slopeX: int, slopeY: int) -> int:
    x = 0
    y = 0
    numTrees = 0
    inputWidth = len(input[0])

    while y < len(input) - 1:
        x += slopeX
        y += slopeY

        if x >= inputWidth:
            x -= inputWidth

        val = input[y][x]
        if val == '#':
            numTrees += 1

    return numTrees


def run(title: str, input: list[str], slopes: list[str], correctResult: int):
    result = 1
    for sl in slopes:
        result *= count_trees(input, int(sl[0]), int(sl[1]))
    utils.validate_result(title, result, correctResult)
    print()


if __name__ == "__main__":
    utils.show_title_extended(3, 2, "Toboggan Trajectory")

    slopes = ["11","31","51","71","12"]

    run("Test Case 1", [
        "..##.......",
        "#...#...#..",
        ".#....#..#.",
        "..#.#...#.#",
        ".#...##..#.",
        "..#.##.....",
        ".#.#.#....#",
        ".#........#",
        "#.##...#...",
        "#...##....#",
        ".#..#...#.#"],
        slopes,
        336)

    run("problem",
        utils.read_input_as_list(3),
        slopes,
        2224913600)