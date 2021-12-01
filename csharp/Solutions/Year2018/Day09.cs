namespace AdventOfCode.Solutions.Year2018
{
    class Day09 : ASolution
    {
        //int[] rinput = new int[] { 416, 71975 };
        int[] tinput = { 21, 6111 };

        public Day09() : base(09, 2018, "")
        {

        }

        protected override object SolvePartOne()
        {
            var m = Regex.Match(Input, @"(\d+).*?(\d+)");
            int[] input = new int[] { int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value) };

            List<int> marbles = new List<int>() { 0, 1 };
            Dictionary<int, int> players = new Dictionary<int, int>();
            for (int i = 0; i < input[0]; i++)
            {
                players.Add(i, 0);
            }

            int currentMarbleIndex = 1;
            for (int i = 2; i <= input[1]; i++)
            {
                if (i % 23 == 0)
                {
                    currentMarbleIndex = (currentMarbleIndex + marbles.Count - 7) % marbles.Count;
                    players[i % players.Count] += i + marbles[currentMarbleIndex];
                    marbles.RemoveAt(currentMarbleIndex);
                }
                else
                {
                    int insertAt = currentMarbleIndex + 2;

                    if (insertAt > marbles.Count)
                        insertAt %= marbles.Count;

                    marbles.Insert(insertAt, i);
                    currentMarbleIndex = insertAt;

                }
            }

            return players.Values.Max();
        }

        protected override object SolvePartTwo()
        {
            var m = Regex.Match(Input, @"(\d+).*?(\d+)");
            int[] input = new int[] { int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value) };
            input[1] *= 100;

            Dictionary<int, long> players = new Dictionary<int, long>();
            for (int i = 0; i < input[0]; i++)
            {
                players.Add(i, 0);
            }

            LinkedList<long> marbles = new LinkedList<long>();
            Node node = new Node();
            node.Value = 0;
            node.Next = node;
            node.Previous = node;

            for (int i = 1; i < input[1]; i++)
            {
                if (i % 23 == 0)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        node = node.Previous;
                    }
                    players[i % players.Count] += i + node.Value;
                    node = node.Remove();
                }
                else
                {
                    node = node.Next;
                    node = node.AddAfter(i);
                }
            }

            return players.Values.Max();
        }

        private class Node
        {
            public long Value { get; set; }
            public Node Previous { get; set; }
            public Node Next { get; set; }

            public Node AddAfter(long value)
            {
                Node n = new Node();
                n.Value = value;

                n.Next = Next;
                n.Previous = this;
                Next.Previous = n;
                Next = n;



                return n;
            }

            public Node Remove()
            {
                Previous.Next = Next;
                Next.Previous = Previous;
                return Next;
            }
        }
    }
}
