
using System;
using System.Reflection;

namespace AdventOfCode.CSharp.Year2024;

public class Day09(ITestOutputHelper output)
{
    private const int DAY = 9;



    private (IList<int> input, long? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadText(DAY, inputName)
                               .Select(c => Convert.ToInt32(c) - 48)
                               .ToList();

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt64();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var blocks = ConvertDiskMapToBlocks(input).ToArray();
        ShiftBlocksLeft(blocks);
        var value = CalculateChecksum(blocks);

        output.WriteLine($"Answer: {value}");

        if (value >= 6307275793643) output.WriteLine("   Too High");

        Assert.Equal(expected, value);
    }



    const int EMPTY_BLOCK = -1;

    private IEnumerable<int> ConvertDiskMapToBlocks(IList<int> diskMap)
    {
        var isFile = true;
        var fileId = 0;
        foreach (var blockSize in diskMap)
        {
            for (int i = 0; i < blockSize; i++)
            {
                yield return isFile
                    ? fileId
                    : EMPTY_BLOCK;
            }

            if (isFile)
            {
                fileId++;
            }
            isFile = !isFile;
        }
    }

    private void ShiftBlocksLeft(IList<int> blocks)
    {
        var freeIdx = 0;
        var fileIdx = blocks.Count - 1;

        while (freeIdx < fileIdx)
        {
            freeIdx = GetNextFreeBlock(blocks, freeIdx);
            fileIdx = GetNextFileBlock(blocks, fileIdx);

            if (freeIdx < fileIdx)
            {
                blocks[freeIdx] = blocks[fileIdx];
                blocks[fileIdx] = EMPTY_BLOCK;

                freeIdx++;
                fileIdx--;
            }
        }
    }

    private long CalculateChecksum(IList<int> blocks)
    {
        var result = blocks.Select((id, index) => (id, index))
                           .Where(pair => pair.id != EMPTY_BLOCK)
                           .Sum(pair => (long)pair.id * pair.index);
        return result;
    }

    int GetNextFreeBlock(IList<int> blocks, int index)
    {
        while (index < blocks.Count)
        {
            if (blocks[index] == EMPTY_BLOCK)
            {
                return index;
            }
            index++;
        }
        return -1;
    }
    int GetNextFileBlock(IList<int> blocks, int index)
    {
        while (0 <= index)
        {
            if (blocks[index] != EMPTY_BLOCK)
            {
                return index;
            }
            index--;
        }
        return -1;
    }

    private void ShowDiskBlocks(IList<int> blocks)
    {
        var aa = blocks.Select(v => v != EMPTY_BLOCK ? v.ToString() : ".");
        var line = string.Join("", aa);
        output.WriteLine(line);
    }

}
