import utils

RuleChains = list[list[str]]

def build_rule_dict(input: list[str]) -> dict[str, RuleChains]:
    rules: dict[str, RuleChains] = {}

    for line in input:
        i = line.index(":")
        key = line[:i].strip()
        line = line[i+1:]

        orParts = line.split("|")
        orChain: RuleChains = []
        for orLine in orParts:
            parts = orLine.split()
            andChain: list[str] = []
            for p in parts:
                val = p.strip().replace('"', '')
                andChain.append(val)
            orChain.append(andChain)
        rules[key] = orChain

    return rules


def process_image(imageData: str, rules: dict[str, RuleChains], charIdx: int, ruleKey: str) -> bool:

    chain: RuleChains = rules[ruleKey]


    return False


def run_part1(title: str, input: list[str], correctResult: int):
    end = input.index("")
    rules = build_rule_dict(input[:end])
    # images = input[end+1:]
    # result = 0
    # for imageData in images:
    #     if process_image(imageData, rules, 0, "0"):
    #         result += 1
    utils.validate_result(title, result, correctResult)


def run_part2(title: str, input: list[str], correctResult: int):
    result = 0
    utils.validate_result(title, result, correctResult)


if __name__ == "__main__":
    day = 19
    print(f"---- Day {day}: Monster Messages ----")

    run_part1("Test Case 1",
        utils.read_input_as_list(day, "example1"),
        2)
    # run_part1("problem",
    #     utils.read_input_as_list(day, "input"),
    #     0)

    # print("---- part 2 ----")

    # run_part2("Test Case 1",
    #     utils.read_input_as_list(day, "example1"),
    #     0)
    # run_part2("problem",
    #     utils.read_input_as_list(day, "input"),
    #     0)