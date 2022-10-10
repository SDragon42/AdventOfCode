from typing import List



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

    equationParts = equation.split()
    result = 0
    op = ""
    for p in equationParts:
        if p in OPS:
            op = p
            continue

        currValue = int(p)
        if op == "":
            result = currValue
            continue

        if op == "+":
            result += currValue
            op = ""
            continue

        if op == "*":
            result *= currValue
            op = ""
            continue

    return result


def evaluate2(equation: str) -> int:
    while True:
        endP = equation.find(")")
        if endP < 0:
            break
        startP = find_last(equation[:endP], "(")

        innerEQ = equation[startP+1:endP]
        subEQ = equation[startP:endP+1]
        result = evaluate2(innerEQ)
        equation = equation.replace(subEQ, str(result))

    equationParts = equation.split()
    
    i = 0
    while i < len(equationParts):
        if equationParts[i] == "+":
            a = int(equationParts[i - 1])
            b = int(equationParts[i + 1])
            equationParts[i - 1] = str(a + b)
            equationParts.pop(i)
            equationParts.pop(i)
            i = 0
            continue
        i += 1

    result = 0
    op = ""
    for p in equationParts:
        if p in OPS:
            op = p
            continue

        currValue = int(p)
        if op == "":
            result = currValue
            continue

        if op == "*":
            result *= currValue
            op = ""
            continue

    return result


def run_part1(input: List[str]) -> int:
    result = sum([evaluate(line) for line in input])
    return result


def run_part2(input: List[str]) -> int:
    result = sum([evaluate2(line) for line in input])
    return result
