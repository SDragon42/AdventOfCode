import utils
import re


class KeyValuePair:
    key = ""
    value = ""

eyeColors = ["amb","blu","brn","gry","grn","hzl","oth"]


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


passportFieldDict = {
    "byr": validate_birth_year,
    "iyr": validate_issue_year,
    "eyr": validate_expiration_year,
    "hgt": validate_height,
    "hcl": validate_hair_color,
    "ecl": validate_eye_color,
    "pid": validate_passport_id,
}


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


def validate_passort(keyValues: list[KeyValuePair]) -> bool:
    if has_required_fields(keyValues) == False:
        return False

    try:
        result = True
        for kv in keyValues:
            if kv.key in passportFieldDict:
                result &= passportFieldDict[kv.key](kv.value)
        return result
    except:
        return False


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
    utils.show_title(4, 2, "Passport Processing")

    run("invalid passports",
        utils.read_input_as_list("day04-invalid-passports"),
        0)
    
    run("valid passports",
        utils.read_input_as_list("day04-valid-passports"),
        4)

    run("problem",
        utils.read_input_as_list("day04"),
        137)