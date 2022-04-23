from typing import List

def count_trees(input: List[str], slopeX: int, slopeY: int) -> int:
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

def do_it(input:List[str], slopes:List[str]) -> int:
    result = 1
    for sl in slopes:
        result *= count_trees(input, int(sl[0]), int(sl[1]))
    return result
