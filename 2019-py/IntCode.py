

class IntCode:

    memory = [int]

    opCodes = [
        opAdd
    ]

    def opAdd(self, valueAddr1, valueAddr2, resultAddr):
        result = self.memory[valueAddr1] + self.memory[valueAddr2]
        self.memory[resultAddr] = result

    def opMultiply(self, valueAddr1, valueAddr2, resultAddr):
        result = self.memory[valueAddr1] * self.memory[valueAddr2]
        self.memory[resultAddr] = result