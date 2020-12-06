import utils


def count_unique_answers(lines: list[str]) -> int:
    answers = []

    for line in lines:
        for c in line:
            if answers.count(c) == 0:
            # try:
            #     answers.index(c)
            # except ValueError:
                answers.append(c)
    
    return len(answers)



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
        result += count_unique_answers(lines)

        groupStart = groupEnd + 1

    utils.validate_result(title, result, correctResult)
    print()


if __name__ == "__main__":
    utils.show_title(6, 1, "Custom Customs")

    run("Test Case 1",
        utils.read_input_as_list("day06-example1"),
        11)

    run("Problem",
        utils.read_input_as_list("day06"),
        6726)