import getopt

class PuzzleRunner:
    titles: list[str] = []
    maxTitleLength: int = 0
    titleBorder: str = ''
    separatorLine: str = ''.ljust(60, '-')
    runAll: bool = False
    indexes: list[int] = []


    def __init__(self, titles: list[str], argv: list[str]) -> None:
        self.titles = titles
        self.maxTitleLength = max(map(len, self.titles))
        self.titleBorder = '+-' + ''.ljust(self.maxTitleLength, '-') + '-+'

        options, args = getopt.getopt(argv, 'a') #'bea')
        self.indexes = [*map(int, args)]
        for name, value in options:
            # if name == '-b':
            #     runBenchmarks = True
            # if name == '-e':
            #     runExamples = True
            if name == '-a':
                self.runAll = True

    
    def write_header(self) -> None:
        print(self.titleBorder)
        for line in self.titles:
            print('| ' + line.ljust(self.maxTitleLength) + ' |')
        print(self.titleBorder)
        print('')


    def get_puzzles(self, puzzleDict: dict) -> list:
        """Get the list of puzzle functions to execute.

        Args:
            puzzleDict (dict): The dictionary of day/puzzles.

        Returns:
            list: The list of puzzle functions to execute.
        """
        if self.runAll:
            return list(puzzleDict.values())

        if (len(self.indexes) > 0):
            puzzleList = []
            for i in self.indexes:
                puzzleList.append(puzzleDict[i])
            return puzzleList

        v = [*puzzleDict.values()][-1]
        puzzleList = [v]
        return puzzleList


    def run(self, puzzleDict: dict) -> None:
        self.write_header()

        puzzleToRun = self.get_puzzles(puzzleDict)
        for puzzle in puzzleToRun:
            puzzle()
            print('')
            print(self.separatorLine)
            print('')