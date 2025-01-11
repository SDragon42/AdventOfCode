namespace AdventOfCode.CSharp.Year2024;

public class Day09(ITestOutputHelper output)
{
    private const int DAY = 9;



    private (IList<int> input, long? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadText(DAY, inputName)
                               .Select(c => Convert.ToInt32(c) - 48)
                               .ToList();

        var expected = TestServices.Input.ReadText(DAY, $"{inputName}-answer{part}")
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

        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var blocks = ConvertDiskMapToBlocks(input).ToArray();
        ShiftWholeFilesLeft(blocks);
        var value = CalculateChecksum(blocks);

        output.WriteLine($"Answer: {value}");

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

    private long CalculateChecksum(IList<int> blocks)
    {
        var result = blocks.Select((id, index) => (id, index))
                           .Where(pair => pair.id != EMPTY_BLOCK)
                           .Sum(pair => (long)pair.id * pair.index);
        return result;
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

    private void ShiftWholeFilesLeft(IList<int> blocks)
    {
        var toMove = blocks.Select((id, index) => (id, index))
                           .Where(p => p.id != EMPTY_BLOCK)
                           .GroupBy(key => key.id,
                                    val => val.index)
                           .Select(g => (fileInfo: (id: g.Key, start: g.Min()), fileSize: g.Max() - g.Min() + 1))
                           .OrderByDescending(p => p.fileInfo.start)
                           .Select(x => (x.fileInfo.id, x.fileInfo.start, x.fileSize))
                           .ToArray();


        foreach (var (id, start, size) in toMove)
        {
            var (freeStart, freeSize) = GetFreeSpaceBlocks(blocks).FirstOrDefault(fs => fs.start < start && fs.size >= size);
            if (freeSize != 0)
            {
                for (var i = 0; i < size; i++)
                {
                    blocks[freeStart + i] = blocks[start + i];
                    blocks[start + i] = EMPTY_BLOCK;
                }
            }
        }
    }

    private static IOrderedEnumerable<(int start, int size)> GetFreeSpaceBlocks(IList<int> blocks)
    {
        return blocks.Select((id, index) => (id, index))
                     .Where(p => p.id == EMPTY_BLOCK)
                     .Aggregate(new List<List<int>>(),
                                (result, item) =>
                                {
                                    if (result.Count == 0 || blocks[item.index - 1] != item.id)
                                        result.Add(new List<int> { item.index });
                                    else
                                        result.Last().Add(item.index);
                                    return result;
                                })
                     .Select(x => (start: x.Min(), size: x.Count()))
                     .OrderBy(x => x.start);
    }

    //private void ShowDiskBlocks(int x, IList<int> blocks)
    //{
    //    var aa = blocks.Select(v => v != EMPTY_BLOCK ? v.ToString() : ".");
    //    var line = string.Join("", aa);
    //    output.WriteLine($"{x,3}: {line}");
    //}

}
