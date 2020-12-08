import utils
import re


class KeyValuePair:
    key = ""
    value = ""


def validate_birth_year(value):
    return 1920 <= int(value) <= 2002


def validate_issue_year(value):
    return 2010 <= int(value) <= 2020


def validate_expiration_year(value):
    return 2020 <= int(value) <= 2030


def validate_height(value):
    isMatch = re.match("^[0-9]+cm$", value)
    if isMatch:
        height = int(isMatch.string[:isMatch.endpos-2])
        return 150 <= height <= 193
    
    isMatch = re.match("^[0-9]+in$", value)
    if isMatch:
        height = int(isMatch.string[:isMatch.endpos-2])
        return 59 <= height <= 76
    return False


def validate_hair_color(value):
    isMatch = re.match("^[#]{1}[0-9a-f]{6}$", value)
    return isMatch is not None


def validate_eye_color(value):
    return value in eyeColors


def validate_passport_id(value):
    isMatch = re.match("^[0-9]{9}$", value)
    return isMatch is not None


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


def has_required_fields(keyValues: list[KeyValuePair]) -> bool:
    for key in passportFieldDict:
        notFound = True

        for kv in keyValues:
            if kv.key == key:
                notFound = False
                break
        
        if notFound == True:
            return False

    return True


def validate_passort_fields(keyValues: list[KeyValuePair]) -> bool:
    try:
        result = True
        for kv in keyValues:
            if kv.key in passportFieldDict:
                result &= passportFieldDict[kv.key](kv.value)
        return result
    except:
        return False


def count_valid_passorts(input: list[str], checkValues: bool) -> int:
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

        isValid = has_required_fields(keyValues)
        if checkValues:
            isValid &= validate_passort_fields(keyValues)

        if isValid:        
            passportCount += 1

        ppStart = ppEnd + 1

    return passportCount


def run(title: str, checkValues: bool, input: list[str], correctResult: int):
    result = count_valid_passorts(input, checkValues)
    utils.validate_result(title, result, correctResult)


eyeColors = ["amb","blu","brn","gry","grn","hzl","oth"]
passportFieldDict = {
    "byr": validate_birth_year,
    "iyr": validate_issue_year,
    "eyr": validate_expiration_year,
    "hgt": validate_height,
    "hcl": validate_hair_color,
    "ecl": validate_eye_color,
    "pid": validate_passport_id,
}

if __name__ == "__main__":
    print("---- Day 4: Passport Processing ----")

    # run("Test Case 1", False,
    #     utils.read_input_as_list("day04-example1"),
    #     2)
    run("problem", False,
        utils.read_input_as_list("day04"),
        202)

    print("---- part 2 ----")

    # run("invalid passports", True,
    #     utils.read_input_as_list("day04-invalid-passports"),
    #     0)
    # run("valid passports", True,
    #     utils.read_input_as_list("day04-valid-passports"),
    #     4)
    run("problem", True,
        utils.read_input_as_list("day04"),
        137)