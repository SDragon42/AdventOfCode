namespace AdventOfCode.CSharp.Year2019;

/// <summary>
/// https://adventofcode.com/2019/day/4
/// </summary>
public class Day04
{

    delegate bool RuleMethod(int[] digits);
    readonly List<RuleMethod> Rules = new List<RuleMethod>();


    public int RunPart1(List<int> input)
    {
        var passwordRangeMin = input[0];
        var passwordRangeMax = input[1];

        Rules.Clear();
        Rules.Add(Rule_IsSixDigits);
        Rules.Add(Rule_TwoAdjacentDigitsAreTheSame);
        Rules.Add(Rule_LeftToRightDigitValueNeverDecreases);

        var numValidPasswords = 0;
        for (var password = passwordRangeMin; password <= passwordRangeMax; password++)
        {
            if (IsPasswordValid(password))
                numValidPasswords++;
        }

        return numValidPasswords;
    }

    public int RunPart2(List<int> input)
    {
        var passwordRangeMin = input[0];
        var passwordRangeMax = input[1];

        Rules.Clear();
        Rules.Add(Rule_IsSixDigits);
        Rules.Add(Rule_OnlyTwoAdjacentDigitsAreTheSame);
        Rules.Add(Rule_LeftToRightDigitValueNeverDecreases);

        var numValidPasswords = 0;
        for (var password = passwordRangeMin; password <= passwordRangeMax; password++)
        {
            if (IsPasswordValid(password))
                numValidPasswords++;
        }

        return numValidPasswords;
    }


    bool IsPasswordValid(int password)
    {
        var passwordDigits = GetDigits(password);
        var isValid = Rules.All(r => r.Invoke(passwordDigits));
        return isValid;
    }

    bool Rule_IsSixDigits(int[] passwordDigits)
    {
        const int requiredNumDigits = 6;
        var numDigits = passwordDigits.Length;
        return (numDigits == requiredNumDigits);
    }

    bool Rule_TwoAdjacentDigitsAreTheSame(int[] passwordDigits)
    {
        var hasDouble = false;
        for (var i = 1; i < passwordDigits.Length; i++)
            hasDouble |= (passwordDigits[i] == passwordDigits[i - 1]);
        return hasDouble;
    }

    bool Rule_OnlyTwoAdjacentDigitsAreTheSame(int[] passwordDigits)
    {
        var last = passwordDigits[0];
        var repeatCount = 0;
        foreach (var digit in passwordDigits.Skip(1))
        {
            if (digit == last)
            {
                repeatCount++;
                continue;
            }
            else
            {
                if (repeatCount == 1) // double only
                    break;
                repeatCount = 0;
            }

            last = digit;
        }

        var hasDouble = (repeatCount == 1);
        return hasDouble;
    }

    bool Rule_LeftToRightDigitValueNeverDecreases(int[] passwordDigits)
    {
        for (var i = 1; i < passwordDigits.Length; i++)
        {
            var current = passwordDigits[i];
            var last = passwordDigits[i - 1];
            if (current < last)
                return false;
        }
        return true;
    }

    /// <summary>Splits the passed value into an array of digits
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    int[] GetDigits(int value)
    {
        return value.ToString()
            .Select(c => (int)char.GetNumericValue(c))
            .ToArray();
    }

}
