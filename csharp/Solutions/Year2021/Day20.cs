using System.Diagnostics;
using System.Text;

namespace AdventOfCode.Solutions.Year2021
{
    internal class Day20 : ASolution
    {
        private string _algo;
        private char[,] _image;
        int _step = 0;

        public Day20() : base(20, 2021, "Trench Map", true)
        {
        }

        protected override void Preprocess()
        {
            DebugOutput = false;

            var input = Input.SplitByBlankLine();
            _algo = input[0];
            _image = input[1].SplitByNewline().Select(s => s.ToCharArray()).ToArray().To2D();
        }

        protected override object SolvePartOne()
        {
            while (_step < 2)
            {
                Enhance();
            }

            return _image.Flatten().Count(c => c == '#');
        }


        protected override object SolvePartTwo()
        {
            while (_step < 50)
            {
                Enhance();
            }

            return _image.Flatten().Count(c => c == '#');

        }

        private void Enhance()
        {
            int nextRows = _image.RowLength() + 2;
            int nextCols = _image.ColLength() + 2;

            char[,] stepImage = new char[nextRows, nextCols];
            for (int y = 0; y < nextRows; y++)
            {
                for (int x = 0; x < nextCols; x++)
                {
                    var p = new Point(x - 1, y - 1);
                    stepImage[y, x] = PointTranslate(_image, p);
                }
            }

            _image = new char[nextRows, nextCols];

            _image = (char[,])stepImage.Clone();

            _step++;
        }

        protected override string LoadDebugInput()
        {
            return @"..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#

#..#.
#....
##..#
..#..
..###";
        }

        private char PointTranslate(char[,] image, Point p)
        {
            var rows = image.GetLength(0);
            var cols = image.GetLength(1);

            var sb = new StringBuilder(9);
            foreach (var pixel in Kernel(p))
            {
                if (pixel.IsInBoundsOfArray(rows, cols))
                {
                    sb.Append(image[pixel.Y, pixel.X]);
                }
                else
                {
                    sb.Append(_step % 2 == 0 ? _algo[^1] : _algo[0]);
                }
            }

            return _algo[PixelStringToInt(sb.ToString())];
        }

        private int PixelStringToInt(string pixels)
        {
            pixels = pixels.Replace('.', '0').Replace('#', '1');
            return Convert.ToInt32(pixels, 2);
        }

        [DebuggerStepThrough]
        private IEnumerable<Point> Kernel(Point p)
        {
            yield return p.Offset(-1, -1);
            yield return p.Offset(0, -1);
            yield return p.Offset(1, -1);
            yield return p.Offset(-1, 0);
            yield return p;
            yield return p.Offset(1, 0);
            yield return p.Offset(-1, 1);
            yield return p.Offset(0, 1);
            yield return p.Offset(1, 1);
        }
    }
}
