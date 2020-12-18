import utils

OPS = ["+","*"]

def find_last(text: str, toFind: str) -> int:
    result = -1
    i = 0
    while i >= 0:
        i = text.find(toFind, result + 1)
        if i > result:
            result = i
    return result

def evaluate(equation: str) -> int:

    while True:
        endP = equation.find(")")
        if endP < 0:
            break
        startP = find_last(equation[:endP], "(")

        innerEQ = equation[startP+1:endP]
        subEQ = equation[startP:endP+1]
        result = evaluate(innerEQ)
        equation = equation.replace(subEQ, str(result))
        pass

    # ----------------
    equationParts = equation.split()
    value = 0
    op = ""
    for p in equationParts:
        if p in OPS:
            op = p
            continue

        currValue = int(p)
        if op == "":
            value = currValue
            continue

        if op == "+":
            value += currValue
            op = ""
            continue

        if op == "*":
            value *= currValue
            op = ""
            continue

    result = value
    return result

def run_part1(title: str, input: list[str], correctResult: int):
    result = 0
    for line in input:
        result += evaluate(line)
        # print(f"{total:-6} = {line}")

    utils.validate_result(title, result, correctResult)


def run_part2(title: str, input: list[str], correctResult: int):
    result = 0
    utils.validate_result(title, result, correctResult)


if __name__ == "__main__":
    day = 18
    print(f"---- Day {day}: Operation Order ----")

    run_part1("Test Case 1",
        utils.read_input_as_list(day, "example1"),
        26457)
    run_part1("problem",
        utils.read_input_as_list(day, "input"),
        75592527415659)

    # print("---- part 2 ----")

    # run_part2("Test Case 1",
    #     utils.read_input_as_list(day, "example1"),
    #     0)
    # run_part2("problem",
    #     utils.read_input_as_list(day, "input"),
    #     0)