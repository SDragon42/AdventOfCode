namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/1
/// </summary>
public class Day01
{
    public int CalcFuelForMasses(IEnumerable<int> masses)
    {
        return masses.Sum(CalcFuel);
    }

    public int CalcTotalFuelForMasses(IEnumerable<int> masses)
    {
        return masses.Sum(CalcTotalFuel);
    }


    /// <summary>
    /// Calculates the Fuel needed for the mass.
    /// </summary>
    /// <param name="mass"></param>
    /// <returns></returns>
    int CalcFuel(int mass)
    {
        return (mass / 3) - 2;
    }


    /// <summary>
    /// Calculates the fuel need to lift the mass, including the fuel.
    /// </summary>
    /// <param name="mass"></param>
    /// <returns></returns>
    int CalcTotalFuel(int mass)
    {
        var fuelMass = CalcFuel(mass);
        if (fuelMass <= 0L)
            return 0;
        return fuelMass + CalcTotalFuel(fuelMass);
    }

}
