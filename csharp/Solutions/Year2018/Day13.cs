namespace AdventOfCode.Solutions.Year2018
{
    class Day13 : ASolution
    {
        Dictionary<char, (int dX, int dY)> charToDirection = new Dictionary<char, (int, int)>
        {
            {'^', (0, -1) },
            {'<', (-1, 0) },
            {'>', (1, 0) },
            {'v', (0, 1) },
        };
        Dictionary<(int dX, int dY), char> directionToChar = new Dictionary<(int, int), char>
        {
            {(0, -1), '^' },
            {(-1, 0), '<' },
            {(1, 0), '>' },
            {(0, 1), 'v' },
        };

        public Day13() : base(13, 2018, "")
        {

        }

        protected override object SolvePartOne()
        {
            var input = Input.SplitByNewline();
            int numRows = input.Length;
            int numCols = input[0].Length;
            char[,] grid = new char[numRows, numCols];
            Dictionary<(int, int), Cart> carts = new Dictionary<(int, int), Cart>();
            Dictionary<(int, int), char> tracks = new Dictionary<(int, int), char>();

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    char c = input[y][x];

                    if (c == '\0')
                        continue;
                    if ("^<>v".Contains(c))
                    {
                        carts.Add((x, y), new Cart(x, y, charToDirection[c].dX, charToDirection[c].dY));
                        c = c == '^' || c == 'v' ? '|' : '-';
                    }
                    if (@"/\+".Contains(c))
                    {
                        tracks.Add((x, y), c);
                    }

                    grid[y, x] = c;
                }
            }
            WriteConsole(grid.GetLength(0), grid.GetLength(1), 0, 0, (y, x) =>
            {
                if (carts.ContainsKey((x, y)))
                {
                    return (ConsoleColor.Red, directionToChar[(carts[(x, y)].DX, carts[(x, y)].DY)]);
                }
                else
                {
                    return (ConsoleColor.White, grid[y, x]);
                }
            });

            Dictionary<(int, int), Cart> newCarts = new Dictionary<(int, int), Cart>();
            Dictionary<(int, int), Cart> cartsLeftToIterate = new Dictionary<(int, int), Cart>(carts);
            bool firstCrash = true;
            int cX = 0, cY = 0;
            int gen = 0;
            while (carts.Count() != 1)
            {
                newCarts.Clear();
                cartsLeftToIterate = new Dictionary<(int, int), Cart>(carts);
                foreach (var cart in carts)
                {
                    cartsLeftToIterate.Remove((cart.Key.Item1, cart.Key.Item2));
                    if (!cart.Value.Alive)
                        continue;
                    cart.Value.Tick();
                    if (tracks.ContainsKey((cart.Value.X, cart.Value.Y)))
                    {
                        switch (tracks[(cart.Value.X, cart.Value.Y)])
                        {
                            case char c when c == '\\' || c == '/':
                                cart.Value.Reflect(c);
                                break;
                            case '+':
                                cart.Value.Turn();
                                break;
                        }
                    }
                    if (cartsLeftToIterate.ContainsKey((cart.Value.X, cart.Value.Y)) || newCarts.ContainsKey((cart.Value.X, cart.Value.Y)))
                    {
                        if (firstCrash)
                        {
                            cX = cart.Value.X;
                            cY = cart.Value.Y;
                            firstCrash = false;
                        }
                        // true if : > - <
                        if (!newCarts.Remove((cart.Value.X, cart.Value.Y)))
                            carts[(cart.Value.X, cart.Value.Y)].Alive = false;

                    }
                    else
                    {
                        newCarts.Add((cart.Value.X, cart.Value.Y), cart.Value);
                    }
                }
                WriteConsole(grid.GetLength(0), grid.GetLength(1), 0, 0, (y, x) =>
                {
                    if (newCarts.ContainsKey((x, y)))
                    {
                        return (ConsoleColor.Red, directionToChar[(newCarts[(x, y)].DX, newCarts[(x, y)].DY)]);
                    }
                    else
                    {
                        return (ConsoleColor.White, grid[y, x]);
                    }
                });

                gen++;
                // Thread.Sleep(1);
                carts = new Dictionary<(int, int), Cart>(newCarts.OrderBy(kvp => kvp.Key.Item2).ThenBy(kvp => kvp.Key.Item1).ToDictionary(kvp => (kvp.Key.Item1, kvp.Key.Item2), kvp => kvp.Value));
            }

            Console.Clear();
            // Console.WriteLine($"{cX},{cY}");
            // Console.WriteLine($"{carts.Values.First().X},{carts.Values.First().Y}");
            return $"{carts.Values.First().X},{carts.Values.First().Y}";
        }


        protected override object SolvePartTwo()
        {
            return null;
        }

        public class Cart
        {
            private int mem = 0;
            public Cart(int x, int y, int dX, int dY)
            {
                X = x;
                Y = y;
                DX = dX;
                DY = dY;
            }

            public bool Alive { get; set; } = true;
            public int X { get; private set; }
            public int Y { get; private set; }
            public int DX { get; private set; }
            public int DY { get; private set; }

            public void Reflect(char corner)
            {
                int temp;
                switch (corner)
                {
                    case '/':
                        temp = DX;
                        DX = -DY;
                        DY = -temp;
                        break;
                    case '\\':
                        temp = DX;
                        DX = DY;
                        DY = temp;
                        break;
                }

                //return this;
            }

            public void Turn()
            {
                int tempDX;
                switch (mem)
                {
                    // Turn Left
                    case 0:
                        tempDX = DX;
                        DX = DY;
                        DY = -tempDX;
                        break;
                    // Turn Right
                    case 2:
                        tempDX = DX;
                        DX = -DY;
                        DY = tempDX;
                        break;
                    // Continue
                    default:
                        break;
                }
                mem = (mem + 1) % 3;

                //return this;
            }

            public void Tick()
            {
                X += DX;
                Y += DY;

                //return this;
            }
        }

        static string[] tinput =
@"
/->-\        
|   |  /----\
| /-+--+-\  |
| | |  | v  |
\-+-/  \-+--/
  \------/   ".SplitByNewline();

        static string[] t2input =
@"
/>-<\  
|   |  
| /<+-\
| | | v
\>+</ |
  |   ^
  \<->/
".SplitByNewline();
    }
}
