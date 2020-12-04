import utils


class KeyValuePair:
    key = ""
    value = ""

requiredFields = ["byr","iyr","eyr","hgt","hcl","ecl","pid"] # Ignoring "cid"
eyeColors = ["amb","blu","brn","gry","grn","hzl","oth"]


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
    for f in requiredFields:
        notFound = True

        for kv in keyValues:
            if kv.key == f:
                notFound = False
                break
        
        if notFound == True:
            return False

    return True

def validate_passort(keyValues: list[KeyValuePair]) -> bool:
    if has_required_fields(keyValues) == False:
        return False

    try:
        for kv in keyValues:
            if kv.key == "byr":
                year = int(kv.value)
                if year < 1920 or year > 2002:
                    return False

            elif kv.key == "iyr":
                year = int(kv.value)
                if year < 2010 or year > 2020:
                    return False

            elif kv.key == "eyr":
                year = int(kv.value)
                if year < 2020 or year > 2030:
                    return False

            elif kv.key == "hgt":
                try:
                    i = kv.value.index("cm")
                    height = int(kv.value[:i])
                    if height < 150 or height > 193:
                        return False
                except ValueError:
                    try:
                        i = kv.value.index("in")
                        height = int(kv.value[:i])
                        if height < 59 or height > 76:
                            return False
                    except ValueError:
                        return False

            elif kv.key == "hcl":
                if kv.value[0] != "#":
                    return False
                hexCode = kv.value[1:]
                if len(hexCode) != 6:
                    return False
                for c in hexCode:
                    if "a" <= c <= "f" or "0" <= c <= "9":
                        continue
                    else:
                        return False

            elif kv.key == "ecl":
                if kv.value not in eyeColors:
                    return False

            elif kv.key == "pid":
                if len(kv.value) != 9:
                    return False
                id = int(kv.value)

        #  "byr","iyr","eyr","hgt","hcl","ecl","pid"
        return True
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