import getopt
from typing import Callable, Dict, List
from puzzleBase import PuzzleBase

class PuzzleRunner:
    _titles: List[str] = []
    _maxTitleLength: int = 0
    _titleBorder: str = ''
    _separatorLine: str = ''.ljust(60, '-')
    _runAll: bool = False
    _indexes: List[int] = []


    def __init__(self, titles: List[str], argv: List[str]) -> None:
        self._titles = titles
        self._maxTitleLength = max(map(len, self._titles))
        self._titleBorder = '+-' + ''.ljust(self._maxTitleLength, '-') + '-+'

        options, args = getopt.getopt(argv, 'a') #'bea')
        
        for name, value in options:
            # if name == '-b':
            #     runBenchmarks = True
            # if name == '-e':
            #     runExamples = True
            if name == '-a':
                self._runAll = True

        self._indexes = [*map(int, args)]

    
    def __write_header(self) -> None:
        print(self._titleBorder)
        for line in self._titles:
            print('| ' + line.ljust(self._maxTitleLength) + ' |')
        print(self._titleBorder)
        print('')

    
    def __write_section_seperator(self) -> None:
        print('')
        print(self._separatorLine)
        print('')


    def __get_puzzles(self, puzzleDict: Dict[int, Callable[[], None]]) -> List[Callable[[], None]]:
        if self._runAll:
            return [*puzzleDict.values()]

        if (len(self._indexes) > 0):
            return [puzzleDict[i] for i in self._indexes]

        lastPuzzle = [*puzzleDict.values()][-1]
        return [lastPuzzle]


    def run(self, puzzleDict: Dict[int, Callable[[], None]]) -> None:
        self.__write_header()

        puzzleToRun = self.__get_puzzles(puzzleDict)
        for puzzle in puzzleToRun:
            puzzle()
            self.__write_section_seperator()


##########################################################################################

class PuzzleRunner2:
    _titles: list[str] = []
    _maxTitleLength: int = 0
    _titleBorder: str = ''
    _separatorLine: str = ''.ljust(60, '-')
    _runAll: bool = False
    _runExamples: bool = False
    _runBenchmarks: bool = False
    _indexes: list[int] = []


    def __init__(self, titles: List[str], argv: List[str]) -> None:
        self._titles = titles
        self._maxTitleLength = max(map(len, self._titles))
        self._titleBorder = '+-' + ''.ljust(self._maxTitleLength, '-') + '-+'

        options, args = getopt.getopt(argv, 'bea')
        self._indexes = [*map(int, args)]
        for name, value in options:
            if name == '-b':
                self._runBenchmarks = True
            if name == '-e':
                self._runExamples = True
            if name == '-a':
                self._runAll = True

    
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