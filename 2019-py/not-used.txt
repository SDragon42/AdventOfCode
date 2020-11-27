import sys
import d01p1



def puzzleSelector(day, puzzle):
    val = (day * 100) + puzzle

    switcher = {
        101: d01p1.Run
    }
    
    func = switcher.get(val)
    func()


############################################################
if __name__ == "__main__":

    # scriptFile = F"./day{sys.argv[1]}/puzzle{sys.argv[2]}.py"
    # print(scriptFile)

    # exec(open(scriptFile).read())

    # day01.puzzle01.Run()
    # puzzle01.Run()

    print(f"arg count: {len(sys.argv)}")

    day = int(sys.argv[1])
    puz = int(sys.argv[2])

    puzzleSelector(day, puz)