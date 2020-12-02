import utils


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

    # run("Test Case #", [
    #     "1-3 a: abcde",
    #     "1-3 b: cdefg",
    #     "2-9 c: ccccccccc"],
    #     2)

    run("problem",
        utils.read_input_as_list(2),
        548)