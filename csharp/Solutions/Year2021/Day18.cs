namespace AdventOfCode.Solutions.Year2021
{
    internal class Day18 : ASolution
    {
        BaseNode[] _inputs;
        BaseNode _root;

        public Day18() : base(18, 2021, "Snailfish", false)
        {

        }

        protected override void Preprocess()
        {
            _inputs = InputByNewLine.Select(Parse).ToArray();
        }

        protected override object SolvePartOne()
        {
            var sum = _inputs[0];

            for (int i = 1; i < _inputs.Length; i++)
            {
                sum = Add(sum, _inputs[i]);
                Simplify(sum);
            }


            return sum.Magnitude();
        }

        protected override object SolvePartTwo()
        {
            var l = InputByNewLine.Length;
            int max = int.MinValue;
            for (int i = 0; i < l - 1; i++)
            {
                for (int j = i + 1; j < l; j++)
                {
                    var a = Parse(InputByNewLine[i]);
                    var b = Parse(InputByNewLine[j]);

                    var temp = Simplify(Add(a, b)).Magnitude();
                    if (temp > max)
                    {
                        max = temp;
                    }

                    a = Parse(InputByNewLine[i]);
                    b = Parse(InputByNewLine[j]);

                    temp = Simplify(Add(b, a)).Magnitude();
                    if (temp > max)
                    {
                        max = temp;
                    }
                }
            }

            return max;
        }

        private BaseNode Simplify(BaseNode node)
        {
            bool didSomething;
            do
            {
                didSomething = TryExplode(node);
                if (didSomething)
                    continue;
                didSomething = TrySplit(node);
            } while (didSomething);

            return node;
        }

        private bool TryExplode(BaseNode node)
        {
            if (node is BracketNode b)
            {
                var numBParents = 0;
                var b2 = b;
                while (b.Parent is not null & b.Parent is BracketNode)
                {
                    b = b.Parent as BracketNode;
                    numBParents++;

                    if (numBParents >= 4)
                    {
                        Explode(b2);
                        if ((b2.Parent as BracketNode).Left == b2)
                        {
                            ((BracketNode)b2.Parent).Left = new ValueNode(0) { Parent = b2.Parent };
                        }
                        else if ((b2.Parent as BracketNode).Right == b2)
                        {
                            ((BracketNode)b2.Parent).Right = new ValueNode(0) { Parent = b2.Parent };
                        }
                        return true;
                    }
                }

                return TryExplode(b2.Left) || TryExplode(b2.Right);
            }

            return false;
        }

        private bool TrySplit(BaseNode node)
        {
            if (node is BracketNode b)
            {
                return TrySplit(b.Left) || TrySplit(b.Right);
            }

            if (node is ValueNode v && v.Value >= 10)
            {
                node = Split(v);
                node.Parent = v.Parent;
                if ((v.Parent as BracketNode).Left == v)
                {
                    ((BracketNode)v.Parent).Left = node;
                }
                else if ((v.Parent as BracketNode).Right == v)
                {
                    ((BracketNode)v.Parent).Right = node;
                }
                return true;
            }

            return false;
        }

        private BaseNode Add(BaseNode left, BaseNode right)
        {
            var b = new BracketNode
            {
                Left = left,
                Right = right
            };
            b.Left.Parent = b;
            b.Right.Parent = b;

            return b;
        }

        private void Explode(BracketNode bracketNode)
        {
            BaseNode tip = bracketNode;
            while (tip.Parent is not null)
            {
                tip = tip.Parent;
            }
            var daList = Flatten(tip).ToList();
            var index = daList.IndexOf(bracketNode);

            if (index == 1)
            {
                // left goes away
                (daList[index + 3] as ValueNode).Value += (daList[index + 1] as ValueNode).Value;
            }
            else if (index == daList.Count - 2)
            {
                // right goes away
                (daList[index - 3] as ValueNode).Value += (daList[index - 1] as ValueNode).Value;
            }
            else
            {
                (daList[index + 3] as ValueNode).Value += (daList[index + 1] as ValueNode).Value;
                (daList[index - 3] as ValueNode).Value += (daList[index - 1] as ValueNode).Value;
            }
        }

        private BracketNode Split(ValueNode valueNode)
        {
            var div2 = valueNode.Value / 2;

            var b = new BracketNode { };
            b.Left = new ValueNode(div2) { Parent = b };
            b.Right = new ValueNode(div2 + valueNode.Value % 2) { Parent = b };

            return b;
        }

        private IEnumerable<BaseNode> Flatten(BaseNode root)
        {
            if (root is BracketNode bracketNode)
            {
                foreach (var node in Flatten(bracketNode.Left))
                {
                    yield return node;
                }
            }

            yield return root;

            if (root is BracketNode bracketNode1)
            {
                foreach (var node in Flatten(bracketNode1.Right))
                {
                    yield return node;
                }
            }
        }

        protected override string LoadDebugInput()
        {
            return @"[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]
[[[5,[2,8]],4],[5,[[9,9],0]]]
[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]
[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]
[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]
[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]
[[[[5,4],[7,7]],8],[[8,3],8]]
[[9,3],[[9,9],[6,[4,9]]]]
[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]
[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]";
        }

        private BaseNode Parse(string input)
        {
            Match m;
            m = Regex.Match(input, @"\[(?>\[(?<c>)|[^\[\]]+|\](?<-c>))*(?(c)(?!))\]");
            if (m.Success)
            {
                if (m.Length == input.Length)
                {
                    return Parse(m.Groups[0].Value[1..^1]);
                }
                else if (m.Index == 0)
                {
                    string left = m.Groups[0].Value[1..^1];
                    string right = input[(m.Length + 1)..];
                    var b = new BracketNode
                    {
                        Left = Parse(left),
                        Right = Parse(right)
                    };
                    b.Left.Parent = b;
                    b.Right.Parent = b;

                    return b;
                }
                else
                {
                    string left = input[0..(m.Index - 1)];
                    string right = m.Groups[0].Value[1..^1];
                    var b = new BracketNode
                    {
                        Left = Parse(left),
                        Right = Parse(right)
                    };
                    b.Left.Parent = b;
                    b.Right.Parent = b;

                    return b;
                }
            }

            if (input.Length == 1)
            {
                return new ValueNode(input);
            }

            var a = input.Split(',');

            var bn = new BracketNode
            {
                Left = new ValueNode(a[0]),
                Right = new ValueNode(a[1])
            };
            bn.Left.Parent = bn;
            bn.Right.Parent = bn;

            return bn;
        }

        class BaseNode
        {
            public BaseNode Parent { get; set; }

            public virtual int Magnitude() => 0;
        }
        class BracketNode : BaseNode
        {
            public BaseNode Left { get; set; }
            public BaseNode Right { get; set; }

            public override int Magnitude()
            {
                return 3 * Left.Magnitude() + 2 * Right.Magnitude();
            }

            public override string ToString()
            {
                return $"[{Left},{Right}]";
            }
        }
        class ValueNode : BaseNode
        {
            public int Value { get; set; }
            public ValueNode(string s)
            {
                Value = int.Parse(s);
            }
            public ValueNode(int value)
            {
                Value = value;
            }

            public override int Magnitude() => Value;

            public override string ToString()
            {
                return $"{Value}";
            }
        }
    }
}
