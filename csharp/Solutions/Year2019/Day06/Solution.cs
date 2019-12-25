using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Solutions.Year2019 {

    class Day06 : ASolution {
        string _test = @"COM)B
B)C
D)E
C)D
E)F
B)G
G)H
J)K
D)I
E)J
K)L";

        string _test2 = @"COM)B
B)C
D)E
C)D
E)F
B)G
G)H
J)K
D)I
E)J
K)L
K)YOU
I)SAN";

        private Dictionary<string, TreeNode<string>> _tree;
        private TreeNode<string> _com;

        public Day06() : base(6, 2019, "") {
            
        }

        protected override string SolvePartOne() {
            _tree = BuildTree(Input);
            _com = _tree["COM"];

            var toProcess = new Queue<TreeNode<string>>(new TreeNode<string>[] { _com });
            int count = 0;
            while (toProcess.Count > 0)
            {
                var s = toProcess.Dequeue();
                foreach (TreeNode<string> item in s)
                {
                    toProcess.Enqueue(item);
                    var cur = item;
                    while (cur.Parent != null)
                    {
                        count++;
                        cur = cur.Parent;
                    }
                }
            }
            return count.ToString(); 
        }

        protected override string SolvePartTwo() {
            var you = _tree["YOU"];
            var san = _tree["SAN"];

            var youParents = new Stack<string>();
            TreeNode<string> temp = you;
            while (temp.Parent != null)
            {
                youParents.Push(temp.Parent.Value);
                temp = temp.Parent;
            }

            var sanParents = new Stack<string>();
            temp = san;
            while (temp.Parent != null)
            {
                sanParents.Push(temp.Parent.Value);
                temp = temp.Parent;
            }

            var hmm = youParents.Intersect(sanParents);

            while (youParents.Peek() == sanParents.Peek())
            {
                youParents.Pop();
                sanParents.Pop();
            }
            return (youParents.Count + sanParents.Count).ToString();
        }

        private Dictionary<string, TreeNode<string>> BuildTree(string input)
        {
            var dict = new Dictionary<string, TreeNode<string>>();
            var lines = input.SplitByNewline().Select(s => s.Split(')'));
            foreach (string[] line in lines)
            {
                var node0 = dict.ContainsKey(line[0]) ? dict[line[0]] : new TreeNode<string>(line[0]);
                var node1 = dict.ContainsKey(line[1]) ? dict[line[1]] : new TreeNode<string>(line[1]);
                node0.AddChild(node1);

                if (!dict.ContainsKey(node0.Value))
                    dict.Add(node0.Value, node0);

                if (!dict.ContainsKey(node1.Value))
                    dict.Add(node1.Value, node1);
            }

            return dict;
        }
    }
}
