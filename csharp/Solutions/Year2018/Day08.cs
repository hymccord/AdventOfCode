
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2018
{
    class Day08 : ASolution
    {
        public Day08() : base(08, 2018, "")
        {

        }

        protected override object SolvePartOne()
        {

            //var nums = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2".Split().Select(int.Parse).ToArray();
            var nums = Input.Trim().Split().Select(int.Parse).ToArray();

            int index = 0;
            var head = Node.Parse(nums, ref index);

            return head.SimpleLicense();
        }

        protected override object SolvePartTwo()
        {
            //var nums = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2".Split().Select(int.Parse).ToArray();
            var nums = Input.Trim().Split().Select(int.Parse).ToArray();

            int index = 0;
            var head = Node.Parse(nums, ref index);

            return head.MetaLicense();
        }

        private void ParseNode(int[] nums)
        {
            //if (nums.Length == 0)
            //return null;

            Node n = new Node();
            int children = nums[0];
            int metaData = nums[1];

            for (int i = 0; i < children; i++)
            {
                //n.Children.Add(ParseNode(nums.Skip(2).ToArray()));
            }
            n.Metadata.AddRange(nums.Take(metaData));
        }

        private class Node
        {
            public int NumChildren { get; set; }
            public int NumMetadata { get; set; }
            public List<Node> Children { get; set; } = new List<Node>();
            public List<int> Metadata { get; set; } = new List<int>();

            public static Node Parse(int[] nums, ref int index)
            {
                var n = new Node();
                if (nums.Length == 0)
                    return n;

                n.NumChildren = nums[index++];
                n.NumMetadata = nums[index++];

                for (int i = 0; i < n.NumChildren; i++)
                {
                    n.Children.Add(Parse(nums, ref index));
                }
                for (int i = 0; i < n.NumMetadata; i++)
                {
                    n.Metadata.Add(nums[index++]);
                }

                return n;

            }

            internal int SimpleLicense()
            {
                return Metadata.Sum() + Children.Sum(n => n.SimpleLicense());
            }

            internal int MetaLicense()
            {
                if (NumChildren == 0)
                    return Metadata.Sum();
                else
                {
                    int sum = 0;
                    foreach (var index in Metadata)
                    {
                        int i = index - 1;
                        if (i < Children.Count)
                        {
                            sum += Children[i].MetaLicense();
                        }
                    }
                    return sum;
                }
            }
        }
    }
}
