from typing import List


def check_password(entry: str) -> int:
    entryParts = entry.split()

    range = entryParts[0].split('-')
    min = int(range[0])
    max = int(range[1])

    letter = entryParts[1].split(":")[0]

    password = entryParts[2]

    count = 0
    for x in password:
        if (x == letter):
            count += 1

    if (min <= count <= max):
        return 1
    return 0


def sum_check_password(input:List[str]) -> int:
    result = sum([check_password(entry) for entry in input])
    return result


def check_password2(entry: str) -> int:
    entryParts = entry.split()
    
    positions = entryParts[0].split('-')
    letter = entryParts[1].split(":")[0]
    password = entryParts[2]

    matches = 0
    for x in positions:
        check = password[int(x) - 1]
        if check == letter:
            matches += 1
    
    if matches == 1:
        return 1
    return 0


def sum_check_password2(input:List[str]) -> int:
    result = sum([check_password2(entry) for entry in input])
    return result
