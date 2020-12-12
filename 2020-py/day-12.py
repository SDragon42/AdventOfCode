import utils


class WaypointPosition:
    x = 0
    y = 0

    def __init__(self, x: int, y:int):
        self.x = x
        self.y = y


class Position:
    x = 0
    y = 0
    heading = ""
    waypoint: WaypointPosition = WaypointPosition(0,0)

    def __init__(self, x: int, y:int, heading: str):
        self.x = x
        self.y = y
        self.heading = heading


#-------------------------------------------------------------------------------


def move_north(currentPos: Position, value: int) -> Position:
    newPos = Position(currentPos.x, currentPos.y + value, currentPos.heading)
    return newPos

def move_south(currentPos: Position, value: int) -> Position:
    newPos = Position(currentPos.x, currentPos.y - value, currentPos.heading)
    return newPos

def move_east(currentPos: Position, value: int) -> Position:
    newPos = Position(currentPos.x + value, currentPos.y, currentPos.heading)
    return newPos

def move_west(currentPos: Position, value: int) -> Position:
    newPos = Position(currentPos.x - value, currentPos.y, currentPos.heading)
    return newPos

def get_cardinal(heading: str, steps: int) -> str:
    i = cardinals.index(heading)
    i += steps
    if i < 0:
        i += len(cardinals)
    elif i >= len(cardinals):
        i -= len(cardinals)
    return cardinals[i]

def turn_left(currentPos: Position, value: int) -> Position:
    steps = int((value / 360) * 4) * -1
    newCardinal = get_cardinal(currentPos.heading, steps)
    return Position(currentPos.x, currentPos.y, newCardinal)

def turn_right(currentPos: Position, value: int) -> Position:
    steps = int((value / 360) * 4)
    newCardinal = get_cardinal(currentPos.heading, steps)
    return Position(currentPos.x, currentPos.y, newCardinal)


#-------------------------------------------------------------------------------


def move_waypoint_north(currentPos: Position, value: int) -> Position:
    wp = currentPos.waypoint
    currentPos.waypoint = WaypointPosition(wp.x, wp.y + value)
    return currentPos

def move_waypoint_south(currentPos: Position, value: int) -> Position:
    wp = currentPos.waypoint
    currentPos.waypoint = WaypointPosition(wp.x, wp.y - value)
    return currentPos

def move_waypoint_east(currentPos: Position, value: int) -> Position:
    wp = currentPos.waypoint
    currentPos.waypoint = WaypointPosition(wp.x + value, wp.y)
    return currentPos

def move_waypoint_west(currentPos: Position, value: int) -> Position:
    wp = currentPos.waypoint
    currentPos.waypoint = WaypointPosition(wp.x - value, wp.y)
    return currentPos


def rotate_waypoint_left(currentPos: Position, value: int) -> Position:
    wp = currentPos.waypoint
    if value == 270:
        return rotate_waypoint_right(currentPos, 90)
    elif value == 180:
        currentPos.waypoint = WaypointPosition(wp.x * -1, wp.y * -1)
    else:
        currentPos.waypoint = WaypointPosition(wp.y * -1, wp.x)

    return currentPos

def rotate_waypoint_right(currentPos: Position, value: int) -> Position:
    wp = currentPos.waypoint
    if value == 270:
        return rotate_waypoint_left(currentPos, 90)
    elif value == 180:
        currentPos.waypoint = WaypointPosition(wp.x * -1, wp.y * -1)
    else:
        currentPos.waypoint = WaypointPosition(wp.y, wp.x * -1)

    return currentPos


def full_steam_ahead(currentPos: Position, value: int) -> Position:
    currentPos.x += currentPos.waypoint.x * value
    currentPos.y += currentPos.waypoint.y * value
    return currentPos


#-------------------------------------------------------------------------------


def move_ship(currentPos: Position, code: str, value: int, path: list[Position]) -> Position:

    if code == "F":
        code = currentPos.heading

    if code in actions:
        newPos = actions[code](currentPos, value)
        path.append(newPos)

    return newPos


def calc_manhattan_distance(x: int, y: int) -> int:
    return abs(x) + abs(y)


def run_part1(title: str, input: list[str], correctResult: int):
    currentPos = Position(0, 0, "E")
    path: list[Position] = []
    path.append(currentPos)

    for data in input:
        code = data[0]
        num = int(data[1:])
        currentPos = move_ship(currentPos, code, num, path)

    result = calc_manhattan_distance(currentPos.x, currentPos.y)
    utils.validate_result(title, result, correctResult)


def run_part2(title: str, input: list[str], correctResult: int):
    currentPos = Position(0, 0, "E")
    currentPos.waypoint = WaypointPosition(10, 1)

    path: list[Position] = []
    path.append(currentPos)

    for data in input:
        code = data[0]
        num = int(data[1:])
        currentPos = actionsP2[code](currentPos, num)
        path.append(currentPos)

    result = calc_manhattan_distance(currentPos.x, currentPos.y)
    utils.validate_result(title, result, correctResult)


cardinals = ["N", "E", "S", "W"]

actions = {
    "N": move_north,
    "S": move_south,
    "E": move_east,
    "W": move_west,
    "L": turn_left,
    "R": turn_right
}

actionsP2 = {
    "N": move_waypoint_north,
    "S": move_waypoint_south,
    "E": move_waypoint_east,
    "W": move_waypoint_west,
    "L": rotate_waypoint_left,
    "R": rotate_waypoint_right,
    "F": full_steam_ahead
}

if __name__ == "__main__":
    day = 12
    print(f"---- Day {day}: Rain Risk ----")

    # run_part1("Test Case 1",
    #     utils.read_input_as_list(day, "example1"),
    #     25)
    run_part1("problem",
        utils.read_input_as_list(day, "input"),
        1441)

    print("---- part 2 ----")

    # run_part2("Test Case 1",
    #     utils.read_input_as_list(day, "example1"),
    #     286)
    run_part2("problem",
        utils.read_input_as_list(day, "input"),
        61616)