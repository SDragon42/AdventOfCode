from typing import Dict, List

class OrbitPair:
    _centerOfMass:str
    orbits:'OrbitPair'

    def __init__(self, centerOfMass:str) -> None:
        self._centerOfMass = centerOfMass
        self.orbits = None

    def __str__(self) -> str:
        orbit = ''
        if self.orbits != None:
            orbit = f' ) {self.orbits.centerOfMass}'
        return self.centerOfMass + orbit

    def __repr__(self) -> str:
        orbitCenterOfMass = 'None'
        if self.orbits != None:
            orbitCenterOfMass = self.orbits.centerOfMass
        return f'OrbitPair: {self.centerOfMass} ) {orbitCenterOfMass}'

    @property
    def centerOfMass(self) -> str:
        return self._centerOfMass




def part1(input:List[str]) -> int:
    uniqueBodies = build_orbit_dict(input)
    numOrbits = count_all_orbits(uniqueBodies)
    return numOrbits

def part2(input:List[str]) -> int:
    uniqueBodies = build_orbit_dict(input)
    
    meToCommonCOM = get_path_to_universal_com(uniqueBodies['YOU'].orbits)
    destToCommonCOM = get_path_to_universal_com(uniqueBodies['SAN'].orbits)
    remove_shared_coms(meToCommonCOM, destToCommonCOM)

    numOrbitChanges = len(meToCommonCOM) + len(destToCommonCOM)

    return numOrbitChanges
        

def build_orbit_dict(input:List[str]) -> Dict[str, OrbitPair]:
    orbitMap = list(map(
        lambda x: x.split(')'),
        input
    ))

    centerOfMasses = map(lambda x: x[0], orbitMap)
    satalites = map(lambda x: x[1], orbitMap)
    uniqueBodies = {
        key: OrbitPair(key) 
        for key in set(centerOfMasses) | set(satalites)
    }

    # link objects
    for orbit in orbitMap:
        comObj = uniqueBodies[orbit[0]]
        orbitObj = uniqueBodies[orbit[1]]
        orbitObj.orbits = comObj

    return uniqueBodies


def count_all_orbits(uniqueBodies: Dict[str, OrbitPair]) -> int:

    def count_chain(item:OrbitPair) -> int:
        if item.orbits == None:
            return 0
        return 1 + count_chain(item.orbits)

    result = sum([count_chain(uniqueBodies[key]) for key in uniqueBodies.keys()])
    return result

def get_path_to_universal_com(obj:OrbitPair) -> List[OrbitPair]:
    if obj == None:
        return []
    return [obj] + get_path_to_universal_com(obj.orbits)

def remove_shared_coms(meToCommonCOM:List[OrbitPair], destToCommonCOM:List[OrbitPair]):
    while True:
        last1 = meToCommonCOM[-1]
        last2 = destToCommonCOM[-1]

        if last1 != last2:
            return

        meToCommonCOM.remove(last1)
        destToCommonCOM.remove(last2)