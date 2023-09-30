import sys, re
# import config
# import inputHelper

def load_file(day: int, name: str) -> str:
    """Loads the contents of a data file.

    Args:
        day (int): The puzzle date
        name (str): The name of the file (without the .TXT extention)

    Returns:
        str: The file contents
        None: None if the file was not found.
    """
    filename = f'.\\..\\input\\day{day:02d}\\{name}.txt'
    try:
        with open(filename) as dataFile:
            return dataFile.read()
    except OSError:
        return None





def main():
    lines = [re.split('[\\s=;,]+', x) for x in load_file(16, 'example1').splitlines()]

    # lines = [re.split('[\\s=;,]+', x) for x in sys.stdin.read().splitlines()]

    # gets the connecting tunnels for each line:  Dictionary<valve, {set of connected tunnels}>
    G = {x[1]: set(x[10:]) for x in lines}
    
    # gets the flow rate for all values > 0: Dictionary<valve, flow rate>
    F = {x[1]: int(x[5]) for x in lines if int(x[5]) != 0}
    
    # dictionary<valve, bit shifted index> (ie. 1,2,4,8,16,32)
    I = {x: 1<<i for i, x in enumerate(F)}
    
    # init floyd-warshall map:  Dict<valve, Dict<valve, int>>
    T = {x: {y: 1 if y in G[x] else float('+inf') for y in G} for x in G} 

    # calc Floyd-Warshall map
    for k in T:
        for i in T:
            for j in T:
                T[i][j] = min(T[i][j], T[i][k]+T[k][j])


    def visit(v, budget, state, flow, answer):
        answer[state] = max(answer.get(state, 0), flow)
        for u in F:
            newbudget = budget - T[v][u] - 1
            
            cond1 = I[u] & state
            cond2 = newbudget <= 0
            if cond1 or cond2: continue
            # if I[u] & state or newbudget <= 0: continue
            visit(u, newbudget, state | I[u], flow + newbudget * F[u], answer)
        return answer


    total1 = max(visit('AA', 30, 0, 0, {}).values())

    visited2 = visit('AA', 26, 0, 0, {})
    total2 = max(v1+v2 for k1, v1 in visited2.items()
                    for k2, v2 in visited2.items() if not k1 & k2)

    print(total1, total2)



if __name__=="__main__":
    main()