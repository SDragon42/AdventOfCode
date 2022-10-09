from typing import List, Dict



class WaypointPosition:
    x: int
    y: int

    def __init__(self, x: int, y:int):
        self.x = x
        self.y = y



class Position:
    x: int
    y: int
    heading: str
    waypoint: WaypointPosition

    def __init__(self, x: int, y:int, heading: str):
        self.x = x
        self.y = y
        self.heading = heading
        self.waypoint = WaypointPosition(0,0)



cardinals = ["N", "E", "S", "W"]


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


def calc_manhattan_distance(x: int, y: int) -> int:
    return abs(x) + abs(y)


def get_distance_from_start(input: List[str], actions: Dict) -> int:
    currentPos = Position(0, 0, "E")
    currentPos.waypoint = WaypointPosition(10, 1)

    path: List[Position] = []
    path.append(currentPos)

    for line in input:
        code = line[0]
        num = int(line[1:])
        
        if (actions.get(code) == None):
            code = currentPos.heading
        currentPos = actions[code](currentPos, num)
        
        path.append(currentPos)

    result = calc_manhattan_distance(currentPos.x, currentPos.y)
    return result
