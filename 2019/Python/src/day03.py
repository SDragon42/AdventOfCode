import sys
from typing import List, Tuple

from point import Point2D

class LineInfo:
    def __init__(self, p1:Point2D, p2:Point2D) -> None:
        self._p1 = p1
        self._p2 = p2
        self._a = self.p2.y - self.p1.y
        self._b = self.p2.x - self.p1.x
        self._c = (self.a * self.p1.x) + (self.b * self.p1.y)

    def __repr__(self) -> str:
        return f'LineInfo: {self.p1}  --  {self.p2}'

    def __str__(self) -> str:
        return f'{self.p1}  --  {self.p2}'

    @property
    def p1(self):
        return self._p1
    @property
    def p2(self):
        return self._p2
    @property
    def a(self):
        return self._a
    @property
    def b(self):
        return self._b
    @property
    def c(self):
        return self._c



def build_wire_points(data:str, origin:Point2D = Point2D(0,0)) -> List[Point2D]:
    """
    Creates a wire (List of Point2D objects) using the data string and an origin point.
    """
    x = origin.x
    y = origin.y
    wire = [origin]
    for item in data.split(','):
        dir = item[0]
        dist = int(item[1:])

        if dir == 'R':
            x += dist
        elif dir == 'L':
            x -= dist
        elif dir == 'U':
            y += dist
        elif dir == 'D':
            y -= dist

        wire.append(Point2D(x,y))

    return wire

def build_wires(input:List[str]) -> Tuple[Point2D, List[Point2D], List[Point2D]]:
    """
    Create the wire point lists from input.
    """
    origin = Point2D(0,0)
    wire1 = build_wire_points(input[0], origin)
    wire2 = build_wire_points(input[1], origin)
    return origin, wire1, wire2


def part1(input:List[str]) -> int:
    """
    Return the manhatten distance of the nearest intersection point.
    """
    origin, wire1, wire2 = build_wires(input)

    intersections = find_all_intersection(wire1, wire2)
    intersections = remove_origin(origin, intersections)
    intersectionPoints = [p for p,_ in intersections]
    nearest = find_nearest_intersection(origin, intersectionPoints)
    distance = calculate_manhatten_distance(origin, nearest)
    return distance

def part2(input:List[str]) -> int:
    """
    
    """
    origin, wire1, wire2 = build_wires(input)

    intersections = find_all_intersection(wire1, wire2)
    intersections = remove_origin(origin, intersections)
    distance = find_shortest_intersection_distance(intersections)
    return distance

def create_line(wire:List[Point2D], endIdx:int) -> LineInfo:
    line1 = LineInfo(wire[endIdx - 1], wire[endIdx])
    return line1

def find_nearest_intersection(origin:Point2D, intersections:List[Point2D]) -> Point2D:
    nearestPoint = None
    nearestDistance = sys.maxsize

    for currentPoint in intersections:
        currentDistance = calculate_manhatten_distance(origin, currentPoint)
        if currentDistance < nearestDistance:
            nearestPoint = currentPoint
            nearestDistance = currentDistance
    if nearestDistance > 0:
        return nearestPoint
    return None

def find_shortest_intersection_distance(intersections:List[Tuple[Point2D, int]]) -> int:
    result = min([distance for _,distance in intersections])
    return result
    

def find_all_intersection(wire1:List[Point2D], wire2:List[Point2D]) -> List[Tuple[Point2D, int]]:
    wire1Indexes = range(1, len(wire1))
    wire2Indexes = range(1, len(wire2))

    found:List[Tuple[Point2D, int]] = []

    wire1Distance = 0
    for wire1Idx in wire1Indexes:
        line1 = create_line(wire1, wire1Idx)
        wire2Distance = 0
        for wire2Idx in wire2Indexes:
            line2 = create_line(wire2, wire2Idx)
            intersectPoint = find_intersection(line1, line2)
            if intersectPoint != None:
                totalWireDistance = (
                    wire1Distance + wire2Distance +
                    calculate_manhatten_distance(line1.p1, intersectPoint) +
                    calculate_manhatten_distance(line2.p1, intersectPoint)
                )
                found.append((intersectPoint, totalWireDistance))
            wire2Distance += calculate_manhatten_distance(line2.p1, line2.p2)
        wire1Distance += calculate_manhatten_distance(line1.p1, line1.p2)

    return found

def remove_origin(origin:Point2D, intersections: List[Tuple[Point2D, int]]) -> List[Tuple[Point2D, int]]:
    newIntersections = list(filter(
        lambda x: (x[0] != origin),
        intersections
    ))
    return newIntersections

def find_intersection(line1:LineInfo, line2:LineInfo) -> Point2D:
    delta = (line1.a * line2.b) - (line2.a * line1.b)
    if delta == 0:
        return None
    
    newX = ((line2.b * line1.c) - (line1.b * line2.c)) // delta
    newY = ((line1.a * line2.c) - (line2.a * line1.c)) // delta

    intersectPoint = Point2D(newX, newY)

    if not is_on_line(line1, intersectPoint):
        return None
    if not is_on_line(line2, intersectPoint):
        return None
    return intersectPoint

def is_on_line(line:LineInfo, interset:Point2D) -> bool:
    if is_same(line.p1.x, line.p2.x, interset.x):
        return is_between(line.p1.y, line.p2.y, interset.y)
    if is_same(line.p1.y, line.p2.y, interset.y):
        return is_between(line.p1.x, line.p2.x, interset.x)
    return False

def is_same(a:int, b:int, value:int) -> bool:
    result = (a == b) and (b == value)
    return result

def is_between(a:int, b:int, value:int) -> bool:
    if a <= b:
        result = (a <= value) and (value <= b)
    else:
        result = (a >= value) and (value >= b)
    return result

def calculate_manhatten_distance(origin:Point2D, target:Point2D) -> int:
    distance = abs(origin.x - target.x) + abs(origin.y - target.y)
    return distance
