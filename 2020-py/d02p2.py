import utils


def check_password(entry: str) -> int:
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



def run(title: str, input: [], correctResult: int):
    result = 0
    for entry in input:
        result += check_password(entry)

    print(f"{title}  --  Result: {result}")
    if correctResult == result:
        print("   CORRECT")
    else:
        print("   WRONG")
    print()


if __name__ == "__main__":
    utils.show_title(2, 1)

    # run("Test Case 1", [
    #     "1-3 a: abcde",
    #     "1-3 b: cdefg",
    #     "2-9 c: ccccccccc"],
    #     1)

    run("problem",
        utils.read_input_as_list(2),
        502)