namespace AdventOfCode.Solutions.Year2021;

internal class Day02 : ASolution
{
    public Day02() : base(02, 2021)
    { }

    protected override object SolvePartOne()
    {
        Point p = new();
        for (int i = 0; i < InputByNewLine.Length; i++)
        {
            var line = InputByNewLine[i].Split(' ');
            var direction = line[0];
            var length = int.Parse(line[1]);

            if (direction == "forward")
            {
                p.X += length;
            }
            else if (direction == "up")
            {
                p.Y -= length;
            }
            else if (direction == "down")
            {
                p.Y += length;
            }
        }

        return p.X * p.Y;
    }

    protected override object SolvePartTwo()
    {
        Point p = new();
        int aim = 0;
        for (int i = 0; i < InputByNewLine.Length; i++)
        {
            var line = InputByNewLine[i].Split(' ');
            var direction = line[0];
            var length = int.Parse(line[1]);

            if (direction == "forward")
            {
                p.X += length;
                p.Y += (aim * length);
            }
            else if (direction == "up")
            {
                aim -= length;
            }
            else if (direction == "down")
            {
                aim += length;
            }
        }

        return p.X * p.Y;
    }

    string test = @"forward 5
down 5
forward 8
up 3
down 8
forward 2";
}
