class Point2D:
    """
    Represents a 2 dimentional (X,Y) coordinate.
    """

    def __init__(self, x:int, y:int) -> None:
        self._x = x
        self._y = y

    def __repr__(self) -> str:
        return f'{self.x},{self.y}'

    def __str__(self) -> str:
        return f'{self.x},{self.y}'

    def __eq__(self, __o: object) -> bool:
        if __o == None:
            return False
        return (self.x == __o.x and self.y == __o.y)

    @property
    def x(self):
        return self._x

    @property
    def y(self):
        return self._y



class Point3D(Point2D):
    """
    Represents a 3 dimentional (X,Y,Z) coordinate.
    """

    def __init__(self, x:int, y:int, z:int) -> None:
        super(x, y)
        self._z = z

    def __repr__(self) -> str:
        return f'{self.x},{self.y},{self.z}'

    def __str__(self) -> str:
        return f'{self.x},{self.y},{self.z}'

    def __eq__(self, __o: object) -> bool:
        if __o == None:
            return False
        return (super(self, __o) and self.z == __o.z)

    @property
    def z(self):
        return self._z