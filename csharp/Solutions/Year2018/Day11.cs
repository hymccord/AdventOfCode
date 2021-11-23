namespace AdventOfCode.Solutions.Year2018
{
    class Day11 : ASolution
    {
        const int input = 7803;
        const int tinput = 18;

        const int serial = input;

        int[,] sums = new int[301, 301];

        public Day11() : base(11, 2018, "")
        {

        }

        protected override object SolvePartOne()
        {
            for (int y = 1; y <= 300; y++)
            {
                for (int x = 1; x <= 300; x++)
                {
                    int id = x + 10;
                    int p = id * y + serial;
                    p = p * id % 1000 / 100 - 5;
                    // sum = i + D + A - B - C
                    sums[y, x] = p + sums[y - 1, x] + sums[y, x - 1] - sums[y - 1, x - 1];
                }
            }

            int maxP = int.MinValue, X = 0, Y = 0;
            for (int y = 3; y <= 300; y++)
            {
                for (int x = 3; x <= 300; x++)
                {
                    int D = sums[y, x];
                    int A = sums[y - 3, x - 3];
                    int B = sums[y - 3, x];
                    int C = sums[y, x - 3];

                    int sum = D - B - C + A;
                    if (sum > maxP)
                    {
                        maxP = sum;
                        X = x - 2;
                        Y = y - 2;
                    }
                }
            }

            return $"{X},{Y}";
        }

        protected override object SolvePartTwo()
        {
            int maxP = int.MinValue, X = 0, Y = 0, size = 0;
            for (int i = 1; i <= 300; i++)
            {
                for (int y = i; y <= 300; y++)
                {
                    for (int x = i; x <= 300; x++)
                    {
                        int D = sums[y, x];
                        int A = sums[y - i, x - i];
                        int B = sums[y - i, x];
                        int C = sums[y, x - i];

                        int sum = D - B - C + A;
                        if (sum > maxP)
                        {
                            maxP = sum;
                            X = x - i + 1;
                            Y = y - i + 1;
                            size = i;
                        }
                    }
                }
            }
            return $"{X},{Y},{size}";
        }


        // Original Implementation
        /*
        private Dictionary<(int, int, int), int> XYWindow = new Dictionary<(int, int, int), int>();
        protected object SolvePartOne2()
        {
            int serialNumber = input;
            int[,] grid = new int[300, 300];
            for (int rowY = 1; rowY <= 300; rowY++)
            {
                for (int colX = 1; colX <= 300; colX++)
                {
                    int id = colX + 10;
                    int p = id * rowY + serialNumber;
                    p = (p * id) % 1000 / 10 - 5;
                    grid[rowY - 1, colX - 1] = p;
                }
            }
            Console.WriteLine(GetMaxPowa(grid, 3));
        }

        protected object SolvePartTwo2()
        {
            int serialNumber = input;
            int[,] grid = new int[300, 300];
            for (int rowY = 0; rowY < 300; rowY++)
            {
                for (int colX = 0; colX < 300; colX++)
                {
                    int id = colX + 10;
                    int p = id * rowY + serialNumber;
                    p = (p * id) % 1000 / 10 - 5;
                    grid[rowY - 1, colX - 1] = p;
                }
            }

            int maxMaxPower = int.MinValue, x = 0, y = 0, gridSize = 0;
            for (int windowSize = 1; windowSize < 300; windowSize++)
            {
                (int currentPower, int col, int row) = GetMaxPowa(grid, windowSize);
                if (currentPower > maxMaxPower)
                {
                    maxMaxPower = currentPower;
                    x = col;
                    y = row;
                    gridSize = windowSize;
                }
            }
            Console.WriteLine($"{x},{y},{gridSize}");
        }

        private (int power, int x, int y) GetMaxPowa(int[,] grid, int windowSize)
        {
            int maxPower = int.MinValue, x = 0, y = 0;
            for (int rowY = 0; rowY < 300 - (windowSize - 1); rowY++)
            {
                for (int colX = 0; colX < 300 - (windowSize - 1); colX++)
                {
                    int currentPower = 0;
                    if (XYWindow.ContainsKey((colX, rowY, windowSize - 1)))
                    {
                        currentPower = XYWindow[(colX, rowY, windowSize - 1)];
                        for (int row = 0; row < windowSize - 1; row++)
                        {
                            // sum the last column excep the last one row
                            currentPower += grid[rowY + row, colX + windowSize - 1];
                        }
                        for (int col = 0; col < windowSize; col++)
                        {
                            // sum the last row
                            currentPower += grid[rowY + windowSize - 1, colX + col];
                        }
                    }
                    else
                    {
                        // compute this block manually
                        for (int row = 0; row < windowSize; row++)
                        {
                            for (int i = 0; i < windowSize; i++)
                            {
                                currentPower += grid[rowY + row, colX + i];
                            }
                        } 
                    }
                    XYWindow[(colX, rowY, windowSize)] = currentPower;

                    if (currentPower > maxPower)
                    {
                        maxPower = currentPower;
                        x = colX + 1;
                        y = rowY + 1;
                    }
                }
            }

            return (maxPower, x, y);
        }

        */

    }
}
