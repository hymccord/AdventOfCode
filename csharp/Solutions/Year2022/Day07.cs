using System.Diagnostics;

using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

namespace AdventOfCode.Solutions.Year2022;

internal class Day07 : ASolution
{
    private DeviceDirectory _root;

    public Day07() : base(07, 2022, "", false)
    { }

    protected override void Preprocess()
    {
        _root = new DeviceDirectory() { Name = "/" };
        Stack<DeviceDirectory> dir = new Stack<DeviceDirectory>();
        dir.Push(_root);

        foreach (var line in InputByNewLine.Skip(1))
        {
            if (line.StartsWith("$ cd "))
            {
                var name = line.Substring(5);
                if (name == "..")
                {
                    var oldDir = dir.Pop();
                    dir.Peek().Size += oldDir.Size;
                }
                else
                {
                    dir.Push(dir.Peek().Directories.Single(x => x.Name == name));
                }
            }
            else
            {
                var thisDir = dir.Peek();
                string[] ls = line.Split(' ');

                if (ls[0] == "dir")
                {
                    var dirName = ls[1];
                    thisDir.Directories.Add(new DeviceDirectory { Name = dirName });
                } 
                else
                {
                    thisDir.Size += int.Parse(ls[0]);
                }
            }
        }
        // No last "cd .." to increment root
        dir.Pop();
        _root.Size += dir.Peek().Size;
    }

    protected override object SolvePartOne()
    {
        List<DeviceDirectory> dirList = new List<DeviceDirectory>();
        dirList.Add(_root);

        return dirList
            .Traverse(c => c.Directories)
            .Where(d => d.Size <= 100_000)
            .Sum(d => d.Size);
    }

    protected override object SolvePartTwo()
    {
        var freeSpace = 70000000 - _root.Size;
        var neededSpace = 30000000 - freeSpace;

        return new[] { _root }.Traverse(x => x.Directories)
            .OrderBy(d => d.Size)
            .First(d => d.Size >= neededSpace)
            .Size;
    }

    protected override string LoadDebugInput()
    {
        return """
        $ cd /
        $ ls
        dir a
        14848514 b.txt
        8504156 c.dat
        dir d
        $ cd a
        $ ls
        dir e
        29116 f
        2557 g
        62596 h.lst
        $ cd e
        $ ls
        584 i
        $ cd ..
        $ cd ..
        $ cd d
        $ ls
        4060174 j
        8033020 d.log
        5626152 d.ext
        7214296 k
        """;
    }

    [DebuggerDisplay("{Name}")]
    class DeviceDirectory
    {
        public string Name { get; set; }
        public List<DeviceDirectory> Directories { get; set; } = new();
        public int Size { get; set; }
    }
}
