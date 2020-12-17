import utils

class Location3D:
    x: int
    y: int
    z: int

    def __init__(self, x: int, y: int, z: int):
        self.x = x
        self.y = y
        self.z = z

    def __str__(self) -> str:
        return f"{self.x},{self.y},{self.z}"

    def shift(self, offset: 'Location3D'):
        self.x += offset.x
        self.y += offset.y
        self.z += offset.z

    def create_shifted(self, offset: 'Location3D'):
        newLocation = Location3D(self.x, self.y, self.z)
        newLocation.shift(offset)
        return newLocation

#-------------------------------------------------------------------------------

class ConwayCube:

    cubeSpace: dict[str, str] = {}

    def __init__(self, inputList: list[str]):
        z = 0
        y = len(inputList) // 2
        xStart = (len(inputList[0])) // 2 * -1

        self.cubeSpace.clear()
        for line in inputList:
            x = xStart
            for c in line:
                addr = Location3D(x, y, z)
                self.cubeSpace[str(addr)] = c
                x += 1
            y -= 1

        # Build SHIFTS
        self.SHIFTS.clear()
        for x in range(3):
            for y in range(3):
                for z in range(3):
                    if x == 1 and y == 1 and z == 1:
                        continue
                    loc = Location3D(x - 1, y - 1, z - 1)
                    self.SHIFTS.append(loc)


    def create_location3d(self, xyz: str) -> Location3D:
        p = xyz.split(",")
        return Location3D(int(p[0]), int(p[1]), int(p[2]))

    # def get_z_keys(self, z: int) -> list[Location3D]:
    #     result: list[Location3D] = []
    #     for key in self.cubeSpace:
    #         loc = self.create_location3d(key)
    #         if loc.z == z:
    #             result.append(loc)
    #     return result

    def make_int_list(self, low: int, high: int) -> list[int]:
        result: list[int] = []
        while low <= high:
            result.append(low)
            low += 1
        return result

    def get_lowest(self, value1: int, value2: int) -> int:
        if value1 < value2:
            return value1
        return value2

    def get_highest(self, value1: int, value2: int) -> int:
        if value1 > value2:
            return value1
        return value2

    def get_z_range(self) -> list[int]:
        low = 0
        high = 0
        for key in self.cubeSpace:
            addr = self.create_location3d(key)
            low = self.get_lowest(low, addr.z)
            high = self.get_highest(high, addr.z)
        return self.make_int_list(low, high)

    def get_y_range(self) -> list[int]:
        low = 0
        high = 0
        for key in self.cubeSpace:
            addr = self.create_location3d(key)
            low = self.get_lowest(low, addr.y)
            high = self.get_highest(high, addr.y)
        return self.make_int_list(low, high)

    def get_x_range(self) -> list[int]:
        low = 0
        high = 0
        for key in self.cubeSpace:
            addr = self.create_location3d(key)
            low = self.get_lowest(low, addr.x)
            high = self.get_highest(high, addr.x)
        return self.make_int_list(low, high)


    # def get_cube(self, addr: Location3D) -> str:
    #     sAddr = str(addr)
    #     if sAddr in self.cubeSpace:
    #         return self.cubeSpace[sAddr]
    #     return "."

    def get_cube(self, addr: Location3D, cubes: dict[str, str]) -> str:
        sAddr = str(addr)
        if sAddr in cubes:
            return cubes[sAddr]
        return self.INACTIVE

    def set_cube(self, addr: Location3D, newState: str):
        sAddr = str(addr)
        if newState == self.ACTIVE:
            self.cubeSpace[sAddr] = newState
            return

        # if sAddr in self.cubeSpace:
        self.cubeSpace.pop(sAddr, None)


    def inflate_range(self, indexes: list[int]):
        indexes.insert(0, indexes[0] - 1)
        indexes.append(indexes[len(indexes) - 1] + 1)


    def show_cube_space(self, title: str): #, z: int):
        print(title)
        print()
        zList = self.get_z_range()
        yList = self.get_y_range()
        xList = self.get_x_range()

        yList.reverse()

        for z in zList:
            print(f"z={z}")
            for y in yList:
                line = ""
                for x in xList:
                    addr = Location3D(x, y, z)
                    line += self.get_cube(addr, self.cubeSpace)
                    # if addr in self.cubeSpace:
                    #     line += self.cubeSpace[addr]
                    # else:
                    #     line += self.INACTIVE
                print(line)
            print()
        print()


    def rule_active_cube(self, addr: Location3D, cubes: dict[str, str]):
        """
        If a cube is active and exactly 2 or 3 of its neighbors are also active, the cube remains active.
        Otherwise, the cube becomes inactive.
        """
        numActive = 0
        for offset in self.SHIFTS:
            tmpAddr = addr.create_shifted(offset)
            state = self.get_cube(tmpAddr, cubes)
            if state == self.ACTIVE:
                numActive += 1

        newState = self.INACTIVE
        if numActive == 2 or numActive == 3:
            newState = self.ACTIVE
        self.set_cube(addr, newState)


    def rule_inactive_cube(self, addr: Location3D, cubes: dict[str, str]):
        """
        If a cube is inactive but exactly 3 of its neighbors are active, the cube becomes active.
        Otherwise, the cube remains inactive.
        """
        numActive = 0
        for offset in self.SHIFTS:
            tmpAddr = addr.create_shifted(offset)
            state = self.get_cube(tmpAddr, cubes)
            if state == self.ACTIVE:
                numActive += 1

        newState = self.INACTIVE
        if numActive == 3:
            newState = self.ACTIVE
        self.set_cube(addr, newState)


    def apply_rules(self):
        oldCubeSpace = self.cubeSpace.copy()

        zList = self.get_z_range()
        yList = self.get_y_range()
        xList = self.get_x_range()

        self.inflate_range(zList)
        self.inflate_range(yList)
        self.inflate_range(xList)

        for z in zList:
            for y in yList:
                for x in xList:
                    addr = Location3D(x, y, z)
                    state = self.get_cube(addr, oldCubeSpace)
                    self.RULES[state](self, addr, oldCubeSpace)


    def get_num_active_cubes(self) -> int:
        result = 0
        for key in self.cubeSpace:
            if self.cubeSpace[key] == self.ACTIVE:
                result += 1
        return result

    ACTIVE = "#"
    INACTIVE = "."

    RULES = {
        INACTIVE: rule_inactive_cube,
        ACTIVE: rule_active_cube,
    }

    SHIFTS: list[Location3D] = []

#-------------------------------------------------------------------------------


def run_part1(title: str, inputList: list[str], numCycles: int, correctResult: int):
    cc = ConwayCube(inputList)
    # cc.show_cube_space("Before any cycles:")

    cycle = 1
    while cycle <= numCycles:
        cc.apply_rules()
        # cc.show_cube_space(f"After {cycle} cycle:")
        cycle += 1

    result = cc.get_num_active_cubes()
    utils.validate_result(title, result, correctResult)


def run_part2(title: str, inputList: list[str], correctResult: int):
    result = 0
    utils.validate_result(title, result, correctResult)


if __name__ == "__main__":
    day = 17
    print(f"---- Day {day}: Conway Cubes ----")

    run_part1("Test Case 1",
        utils.read_input_as_list(day, "example1"),
        6,
        112)
    run_part1("problem",
        utils.read_input_as_list(day, "input"),
        6,
        362)

    # print("---- part 2 ----")

    # run_part2("Test Case 1",
    #     utils.read_input_as_list(day, "example1"),
    #     0)
    # run_part2("problem",
    #     utils.read_input_as_list(day, "input"),
    #     0)