namespace AdventOfCode.Solutions.Year2015.Day07
{
    class Day07 : ASolution
    {
        public Day07() : base(7, 2015)
        {
        }

        protected override object SolvePartOne()
        {
            var a = Input.SplitByNewline().Select(Decoder.Decode).ToArray();
            return null;
        }

        protected override object SolvePartTwo()
        {
            return null;
        }

        class Wire
        {
            public Wire Input { get; set; }

            public HashSet<Wire> Output { get; set; }
        }

        class Expression
        {
            public object Tag { get; set; }
        }

        class UnaryOpExpression : Expression
        {
            public UnaryOp Operation { get; set; }

            public Expression Value { get; set; }
        }

        class BinaryOpExpression : Expression
        {
            public Expression Left { get; set; }
            public Expression Right { get; set; }
            public BinaryOp Operation { get; set; }
        }

        static class Decoder
        {
            static Regex OneToOne = new Regex(@"(?<binop>NOT)?.*(?<in1>[a-z0-9]+) -> (<out>[a-z]+)");
            static Regex TwoToOne = new Regex(@"(?<in1>[a-z0-9]+) (?<binop>(AND|OR|LSHIFT|RSHIFT)) (?<in2>[a-z0-9]+) -> (?<out>[a-z]+)");
            //static Regex Shift = new Regex(@"(<in1>[a-z]+) (?<binop>(L|R)SHIFT) (<in2>[0-9]+) -> (<out>[a-z]+)");

            public static Expression Decode(string wire)
            {
                Expression e;
                Match m;
                if ((m = OneToOne.Match(wire)).Success)
                {
                    e = new UnaryOpExpression()
                    {

                    };
                }
                else if ((m = TwoToOne.Match(wire)).Success)
                {
                    e = new BinaryOpExpression()
                    {

                    };
                }
                else
                {
                    throw new Exception();
                }

                return e;
            }
        }

        enum UnaryOp
        {
            Not,
            Eq
        }

        enum BinaryOp
        {
            And,
            Or,
            Xor,
            Lsh,
            Rsh,
        }
    }
}
