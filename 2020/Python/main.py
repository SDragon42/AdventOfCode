import sys
from typing import Dict

from puzzleRunner import PuzzleRunner2
from puzzleBase import PuzzleBase

import day01, day02, day03, day04, day05, day06, day07, day08, day09, day10, \
       day11, day12, day13, day14, day15, day16, day17, day18, day19, day20, \
       day21, day22, day23, day24, day25
import day22fast

puzzleDict: Dict[int, PuzzleBase] = {
    1: day01.Puzzle,
    2: day02.Puzzle,
    3: day03.Puzzle,
    4: day04.Puzzle,
    5: day05.Puzzle,
    6: day06.Puzzle,
    7: day07.Puzzle,
    8: day08.Puzzle,
    9: day09.Puzzle,
    10: day10.Puzzle,
    # 11: day11.Puzzle,
    # 12: day12.Puzzle,
    # 13: day13.Puzzle,
    # 14: day14.Puzzle,
    # 15: day15.Puzzle,
    # 16: day16.Puzzle,
    # 17: day17.Puzzle,
    # 18: day18.Puzzle,
    # 19: day19.Puzzle,
    # 20: day20.Puzzle,
    # 21: day21.Puzzle,
    # 22: day22.Puzzle,
    # 23: day23.Puzzle,
    # 24: day24.Puzzle,
    # 25: day25.Puzzle,
}


if __name__ == "__main__":
    titles = [
        'Advent of Code 2020',
        'https://adventofcode.com/2020',
        'By: SDragon'
        ]
    runner = PuzzleRunner2(titles, sys.argv[1:])
    runner.run(puzzleDict)