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

#-------------------------------------------------------------------------------

class ConwayCube:

    cubeSpace: dict[str, str] = {}

    def __init__(self, inputList: list[str]):
        z = 0
        y = len(inputList) // 2
        xStart = (len(inputList[0])) // 2 * -1

        for line in inputList:
            x = xStart
            for c in line:
                addr = Location3D(x, y, z)
                self.cubeSpace[str(addr)] = c
                x += 1
            y -= 1
        pass

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
        
            
    def get_cube(self, addr: Location3D) -> str:
        sAddr = str(addr)
        if sAddr in self.cubeSpace:
            return self.cubeSpace[sAddr]
        return "."
    
    def show_cube_space(self): #, z: int):
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
                    line += self.get_cube(addr)
                    # if addr in self.cubeSpace:
                    #     line += self.cubeSpace[addr]
                    # else:
                    #     line += "."
                print(line)
            print("")
    
    def rule_active_cube(self, addr: Location3D, cubes: dict[str, str]):
        """
        If a cube is active and exactly 2 or 3 of its neighbors are also active, the cube remains active. 
        Otherwise, the cube becomes inactive.
        """
        pass

    def rule_inactive_cube(self, addr: Location3D, cubes: dict[str, str]):
        """
        If a cube is inactive but exactly 3 of its neighbors are active, the cube becomes active.
        Otherwise, the cube remains inactive.
        """
        pass

    def apply_rules(self):
        oldCubeSpace = self.cubeSpace.copy()

        zList = self.get_z_range()
        yList = self.get_y_range()
        xList = self.get_x_range()

        for z in zList:
            for y in yList:
                for x in xList:
                    addr = Location3D(x, y, z)

                    pass
        pass

    RULES = {
        ".": rule_inactive_cube,
        "#": rule_active_cube
    }

#-------------------------------------------------------------------------------


def run_part1(title: str, inputList: list[str], correctResult: int):
    cc = ConwayCube(inputList)
    cc.show_cube_space()

    # cycle = 1
    # while cycle <= 3:
    #     cc.apply_rules()

    #     cycle += 1

    result = 0
    utils.validate_result(title, result, correctResult)


def run_part2(title: str, inputList: list[str], correctResult: int):
    result = 0
    utils.validate_result(title, result, correctResult)


if __name__ == "__main__":
    day = 17
    print(f"---- Day {day}: Conway Cubes ----")

    run_part1("Test Case 1",
        utils.read_input_as_list(day, "example1"),
        0)
    # run_part1("problem",
    #     utils.read_input_as_list(day, "input"),
    #     0)

    # print("---- part 2 ----")

    # run_part2("Test Case 1",
    #     utils.read_input_as_list(day, "example1"),
    #     0)
    # run_part2("problem",
    #     utils.read_input_as_list(day, "input"),
    #     0)