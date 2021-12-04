namespace AdventOfCode.Solutions.Year2021
{
    internal class Day04 : ASolution
    {
        int[] balls;
        BingoBoard[] bingoBoards;

        public Day04() : base(04, 2021, "Giant Squid", false)
        {

        }

        protected override void Preprocess()
        {
            var input = Input.SplitByBlankLine();

            balls = input[0].ToIntArray(',');
            bingoBoards = input[1..].Select(BingoBoard.Parse).ToArray();
        }

        protected override object SolvePartOne()
        {
            foreach (var ball in balls)
            {
                for (int i = 0; i < bingoBoards.Length; i++)
                {
                    if (bingoBoards[i].Mark(ball))
                    {
                        return bingoBoards[i].SumUnmarked() * ball;
                    }
                }
            }

            throw new NotFiniteNumberException();
        }

        protected override object SolvePartTwo()
        {
            BingoBoard lastWinner = null;
            foreach (var ball in balls)
            {
                HashSet<BingoBoard> remove = new HashSet<BingoBoard>();
                for (int i = 0; i < bingoBoards.Length; i++)
                {
                    if (bingoBoards[i].Mark(ball))
                    {
                        remove.Add(bingoBoards[i]);
                        lastWinner = bingoBoards[i];
                    }
                }
                bingoBoards = bingoBoards.Except(remove).ToArray();

                if (bingoBoards.Length == 0)
                {
                    return lastWinner.SumUnmarked() * ball;
                }
            }

            throw new NotImplementedException();
        }

        protected override string LoadDebugInput()
        {
            return @"7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19

 3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6

14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7";
        }

        private class BingoBoard
        {
            private Box[,] boardNums;
            private Dictionary<int, Box> boxByBall;

            internal static BingoBoard Parse(string input)
            {
                string[] rows = input.SplitByNewline();

                var board = new BingoBoard
                {
                    // I have a lot of extension methods for int[,] (SliceRow, SliceColumn, Flatten)
                    // That the best format for me
                    // string -> string[] -> int[][] -> int[,]
                    boardNums = rows.Select(s => s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(c => new Box(int.Parse(c), false)).ToArray()).ToArray().To2D()
                };
                board.boxByBall = board.boardNums.Flatten().ToDictionary(b => b.Value);

                return board;
            }

            public bool Mark(int num)
            {
                if (!boxByBall.ContainsKey(num))
                {
                    return false;
                }

                boxByBall[num].Marked = true;

                return HasBingo();
            }

            internal int SumUnmarked()
            {
                return boardNums.Flatten().Where(t => !t.Marked).Sum(t => t.Value);
            }

            private bool HasBingo()
            {
                bool rowBingo = Enumerable.Range(0, 5).Any(i => boardNums.SliceRow(i).All(t => t.Marked));
                bool colBingo = Enumerable.Range(0, 5).Any(i => boardNums.SliceColumn(i).All(t => t.Marked));

                return rowBingo || colBingo;
            }

            private class Box
            {
                public Box(int value, bool marked)
                {
                    Value = value;
                    Marked = marked;
                }

                public int Value { get; set; }
                public bool Marked { get; set; }
            }
        }
    }
}
