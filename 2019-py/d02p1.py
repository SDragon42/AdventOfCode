import math
from inputData import get_input_d02, get_answer_d02p1




if __name__ == "__main__":
    print("Day 2 Puzzle 1")
    print("------------------------------------------------------------")
    print()

    # input = [14] # test 1: required fule = 2
    # input = [1969] # test 2: required fule = 966
    # input = [100756] # test 3: required fule = 50346
    input = get_input_d02() # correct answer = 5057481

    

    totalFuel = 0

    print(f"Total Fuel needed: {totalFuel}")
    if get_answer_d02p1() == totalFuel:
        print("   CORRECT")