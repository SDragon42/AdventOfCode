import getopt
from typing import Callable, Dict, List
from puzzleBase import PuzzleBase

class PuzzleRunner:
    _titles: list[str]
    _maxTitleLength: int
    _titleBorder: str
    _separatorLine: str = ''.ljust(60, '-')
    _runAll: bool = False
    _runExamples: bool = False
    _runBenchmarks: bool = False
    _indexes: list[int]


    def __init__(self, titles: List[str], argv: List[str]) -> None:
        self._titles = titles
        self._maxTitleLength = max(map(len, self._titles))
        self._titleBorder = '+-' + ''.ljust(self._maxTitleLength, '-') + '-+'

        options, args = getopt.getopt(argv, 'bea')

        self._indexes = []
        for a in args:
            self.__process_puzzle_nums(a)

        for name, value in options:
            if name == '-b':
                self._runBenchmarks = True
            if name == '-e':
                self._runExamples = True
            if name == '-a':
                self._runAll = True

    def __process_puzzle_nums(self, arg: str) -> None:
        argParts = arg.split(',')
        if len(argParts) == 1:
            try:
                self._indexes.append(int(arg))
            except:
                print(f'Bad argument: {arg}')
        else:
            for part in argParts:
                self.__process_puzzle_nums(part)


    
    def __write_header(self) -> None:
        print(self._titleBorder)
        for line in self._titles:
            print('| ' + line.ljust(self._maxTitleLength) + ' |')
        print(self._titleBorder)
        print('')

    
    def __write_section_seperator(self):
        print('')
        print(self._separatorLine)
        print('')


    def __get_puzzles(self, puzzleDict: Dict[int, PuzzleBase]) -> List[PuzzleBase]:
        if self._runAll:
            return [*puzzleDict.values()]

        if (len(self._indexes) > 0):
            return [puzzleDict[i] for i in self._indexes]

        lastPuzzle = [*puzzleDict.values()][-1]
        return [lastPuzzle]


    def run(self, puzzleDict: Dict[int, PuzzleBase]) -> None:
        self.__write_header()

        puzzleToRun = self.__get_puzzles(puzzleDict)
        for puzzle in puzzleToRun:
            p = puzzle()
            p.set_options(self._runBenchmarks, self._runExamples)
            p.solve()
            self.__write_section_seperator()