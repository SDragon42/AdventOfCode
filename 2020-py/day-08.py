import utils


idx: int = 0
accValue: int = 0


def no_operation(value: int):
    global idx
    idx += 1


def accumulator(value: int):
    global idx
    global accValue
    accValue += value
    idx += 1


def jump(value: int):
    global idx
    idx += value


def run_till_loop(input: list[str], callStack: list[int]):
    global idx
    global accValue
    idx = 0
    accValue = 0

    callStack.clear()

    while idx < len(input):
        if idx in callStack:
            break
        callStack.append(idx)
        parts = input[idx].split(" ", 1)
        inst = parts[0]
        val = int(parts[1])
        action = instructionDict[inst]
        action(val)
    callStack.append(idx)


def flip_instruction(instructionIdx: int, input: list[str]) -> list[str]:
    if input[instructionIdx].startswith("nop"):
        input[instructionIdx] = input[instructionIdx].replace("nop", "jmp")
    else:
        input[instructionIdx] = input[instructionIdx].replace("jmp", "nop")
    return input


def run_part1(title: str, input: list[str], correctResult: int):
    global idx
    global accValue
    
    callStack: list[int] = []
    run_till_loop(input, callStack)
    utils.validate_result(title, accValue, correctResult)


def run_part2(title: str, input: list[str], correctResult: int):
    global idx
    global accValue
    
    callStack: list[int] = []
    run_till_loop(input, callStack)
    
    while len(callStack) > 0:
        lastIdx = callStack.pop()
        if input[lastIdx].startswith("acc"):
            continue

        newInput = flip_instruction(lastIdx, input.copy())
        newCallStack: list[int] = []
        run_till_loop(newInput, newCallStack)
        lastIdx2 = newCallStack.pop()
        if lastIdx2 >= len(input):
            break

    utils.validate_result(title, accValue, correctResult)
    

instructionDict = {
    "nop": no_operation,
    "acc": accumulator,
    "jmp": jump,
}


if __name__ == "__main__":
    print("---- Day 8: Handheld Halting ----")

    # run_part1("Test Case 1",
    #     utils.read_input_as_list(8, "example1"),
    #     5)
    run_part1("problem",
        utils.read_input_as_list(8, "input"),
        1489)

    print("---- part 2 ----")

    # run_part2("Test Case 1",
    #     utils.read_input_as_list(8, "example1"),
    #     8)
    run_part2("problem",
        utils.read_input_as_list(8, "input"),
        1539)