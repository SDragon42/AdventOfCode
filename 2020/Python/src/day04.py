import re
from typing import List


class KeyValuePair:
    key: str
    value: str

    def __init__(self, key: str, value: str) -> None:
        self.key = key
        self.value = value




_eyeColors = ["amb","blu","brn","gry","grn","hzl","oth"]

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
    return value in _eyeColors


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


def get_passort_key_values(lines: List[str]) -> List[KeyValuePair]:
    keyValues: List[KeyValuePair] = []

    for l in lines:
        parts = l.split()
        for p in parts:
            aaa = p.split(":")
            item = KeyValuePair(aaa[0], aaa[1])
            keyValues.append(item)

    return keyValues


def has_required_fields(keyValues: List[KeyValuePair]) -> bool:
    for key in passportFieldDict:
        notFound = True

        for kv in keyValues:
            if kv.key == key:
                notFound = False
                break
        
        if notFound == True:
            return False

    return True


def validate_passort_fields(keyValues: List[KeyValuePair]) -> bool:
    try:
        result = True
        for kv in keyValues:
            if kv.key in passportFieldDict:
                result &= passportFieldDict[kv.key](kv.value)
        return result
    except:
        return False


def count_valid_passorts(input: List[str], checkValues: bool) -> int:
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