import utils


def count_unique_unanimous_answers(lines: list[str]) -> int:
    answers = {}
    pCount = 0

    for line in lines:
        pCount += 1
        for c in line:
            if c in answers:
                answers[c] = answers[c] + 1
            else:
                answers[c] = 1

    result = 0
    for a in answers:
        if answers[a] == pCount:
            result += 1
    
    return result



def run(title: str, input: list[str], correctResult: int):
    result = 0

    groupStart = 0
    groupEnd = 0
    while groupStart < len(input):
        try:
            groupEnd = input.index("", groupStart)
        except:
            groupEnd = len(input)

        lines = input[groupStart:groupEnd]
        result += count_unique_unanimous_answers(lines)

        groupStart = groupEnd + 1

    utils.validate_result(title, result, correctResult)
    print()


if __name__ == "__main__":
    utils.show_title(6, 2, "Custom Customs")

    run("Test Case 1",
        utils.read_input_as_list("day06-example1"),
        6)

    run("Problem",
        utils.read_input_as_list("day06"),
        3316)