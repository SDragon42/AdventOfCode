namespace AdventOfCode.CSharp.Year2022;

public class Day07_No_Space_Left_On_Device
{
    private const int DAY = 7;

    private readonly ITestOutputHelper output;
    public Day07_No_Space_Left_On_Device(ITestOutputHelper output) => this.output = output;



    private (List<string> input, int? expected) GetTestData(int part, string inputName)
    {
        var input = TestServices.Input.ReadLines(DAY, inputName)
            .ToList();

        var expected = TestServices.Input.ReadLines(DAY, $"{inputName}-answer{part}")
            ?.FirstOrDefault()
            ?.ToInt32();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetSumTotalOfDirectoriesUnder(input, 100000);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = GetSizeOfDirectoryToDelete(input, 70000000, 30000000);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    private int GetSumTotalOfDirectoriesUnder(List<string> input, int maxFolderSize)
    {
        var directoryTree = BuildDirectoryStructure(input);
        directoryTree.CalcFolderSize();

        var allFolders = GetAllFolders(directoryTree);

        var result = allFolders
            .Where(f => f.Size <= maxFolderSize)
            .Sum(f => f.Size);

        return result;

        
    }

    private int GetSizeOfDirectoryToDelete(List<string> input, int totalDiskSpace, int neededDiskSpace)
    {
        var directoryTree = BuildDirectoryStructure(input);
        directoryTree.CalcFolderSize();

        var freeDiskSpace = totalDiskSpace - directoryTree.Size;
        var allFolders = GetAllFolders(directoryTree);

        var result = allFolders
            .Where(f => freeDiskSpace + f.Size > neededDiskSpace)
            .OrderBy(f => f.Size)
            .First();

        return result.Size;
    }


    private FileNode BuildDirectoryStructure(List<string> input)
    {
        var root = FileNode.MakeDirectory(null, "/");
        var current = root;

        foreach (var line in input)
        {
            var partArray = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            switch (partArray[0])
            {
                case "$":
                    switch (partArray[1])
                    {
                        case "cd":
                            switch (partArray[2])
                            {
                                case "/":
                                    current = root;
                                    break;

                                case "..":
                                    if (current.Parent != null)
                                        current = current.Parent;
                                    break;

                                default:
                                    current = current.Children
                                        .Where(n => n.IsDirectory)
                                        .Where(n => n.Name == partArray[2])
                                        .First();
                                    break;
                            }
                            break;

                        case "ls":
                            break;
                    }
                    break;

                case "dir":
                    current.Children.Add(FileNode.MakeDirectory(
                        current,
                        partArray[1])
                        );
                    break;

                default:
                    current.Children.Add(FileNode.MakeFile(
                        current,
                        partArray[1],
                        partArray[0].ToInt32())
                        );
                    break;
            }
        }

        return root;
    }

    IEnumerable<FileNode> GetAllFolders(FileNode node)
    {
        if (!node.IsDirectory)
            yield break;

        yield return node;

        foreach (var child in node.Children)
        {
            var a = GetAllFolders(child);
            foreach (var b in a)
                yield return b;
        }
    }

    private class FileNode
    {
        public static FileNode MakeFile(FileNode? parent, string name, int size)
        {
            return new FileNode()
            {
                Name = name,
                Size = size,
                Parent = parent
            };
        }

        public static FileNode MakeDirectory(FileNode? parent, string name)
        {
            return new FileNode()
            {
                Name = name,
                IsDirectory = true,
                Parent = parent
            };
        }

        private FileNode() { }

        public required string Name { get; init; }
        public bool IsDirectory { get; init; } = false;
        public int Size { get; set; } = 0;

        public FileNode? Parent { get; set; } = null;
        public List<FileNode> Children { get; } = new List<FileNode>();

        public void CalcFolderSize()
        {
            if (!IsDirectory)
                return;
            Children.ForEach(c => c.CalcFolderSize());
            Size = Children.Sum(c => c.Size);
        }
    }

}