import utils


class KeyValuePair:
    key = ""
    value = ""

requiredFields = ["byr","iyr","eyr","hgt","hcl","ecl","pid"] # Ignoring "cid"


def get_passort_key_values(lines: list[str]) -> list[KeyValuePair]:
    keyValues = list[KeyValuePair]()

    for l in lines:
        parts = l.split()
        for p in parts:
            aaa = p.split(":")
            
            item = KeyValuePair()
            item.key = aaa[0]
            item.value = aaa[1]

            keyValues.append(item)

    return keyValues

def validate_passort(keyValues: list[KeyValuePair]) -> bool:
    for f in requiredFields:
        notFound = True

        for kv in keyValues:
            if kv.key == f:
                notFound = False
                break
        
        if notFound == True:
            return False

    return True


def count_valid_passorts(input: list[str]) -> int:
    passportCount = 0

    ppStart = 0
    ppEnd = 0

    while ppStart < len(input):
        try:
            ppEnd = input.index("", ppStart)
        except ValueError:
            ppEnd = len(input)

        lines = input[ppStart:ppEnd]    

        keyValues = get_passort_key_values(lines)

        if validate_passort(keyValues):
            passportCount += 1

        ppStart = ppEnd + 1

    return passportCount


def run(title: str, input: list[str], correctResult: int):
    result = count_valid_passorts(input)
    utils.validate_result(title, result, correctResult)
    print()


if __name__ == "__main__":
    utils.show_title(4, 1, "Passport Processing")

    run("Test Case 1",
        utils.read_input_as_list2("day04-example1"),
        2)

    run("problem",
        utils.read_input_as_list2("day04"),
        202)