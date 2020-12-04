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


def run(title: str, input: list[str], correctResult: int):
    result = count_trees(input, 3, 1)
    utils.validate_result(title, result, correctResult)
    print()


if __name__ == "__main__":
    utils.show_title(3, 1, "Toboggan Trajectory")


    run("Test Case 1", 
        utils.read_input_as_list("day03-example1"),
        7)

    run("problem",
        utils.read_input_as_list("day03"),
        259)