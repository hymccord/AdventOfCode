namespace AdventOfCode.Solutions.Year2015;

class Day07 : ASolution
{
    private Expression[] _expressions;

    public Day07() : base(7, 2015, "Some Assembly Required", false)
    {
    }

    protected override void Preprocess()
    {
        _expressions = Input.SplitByNewline().Select(Decoder.Decode).ToArray();
    }

    protected override object SolvePartOne()
    {
        Dictionary<string, int> wireValues = new Dictionary<string, int>();
        while (true)
        {
            bool ranOne = false;
            List<Expression> ran = new List<Expression>();
            for (int i = 0; i < _expressions.Length; i++)
            {
                Expression ex = _expressions[i];
                if (ex.ReadyToEval())
                {
                    ran.Add(ex);
                    ranOne = true;
                    int value = ex.Eval();
                    wireValues.Add(ex.Output, value);
                }
            }

            if (!ranOne)
            {
                break;
            }

            _expressions = _expressions.Except(ran).ToArray();

            foreach (var wire in _expressions)
            {
                wire.UpdateExpressions(wireValues);
            }
        }
        return wireValues["a"];
    }

    protected override object SolvePartTwo()
    {
        int b = int.Parse(Part1);

        Dictionary<string, int> wireValues = new() { { "b", b } };
        foreach (var wire in _expressions)
        {
            wire.UpdateExpressions(wireValues);
        }
        _expressions = _expressions.Where(e => !(e is UnaryOpExpression v && v.Output == "b")).ToArray();

        while (true)
        {
            bool ranOne = false;
            List<Expression> ran = new List<Expression>();
            for (int i = 0; i < _expressions.Length; i++)
            {
                Expression ex = _expressions[i];
                if (ex.ReadyToEval())
                {
                    ran.Add(ex);
                    ranOne = true;
                    int value = ex.Eval();
                    wireValues.Add(ex.Output, value);
                }
            }

            if (!ranOne)
            {
                break;
            }

            _expressions = _expressions.Except(ran).ToArray();

            foreach (var wire in _expressions)
            {
                wire.UpdateExpressions(wireValues);
            }
        }
        return wireValues["a"];
    }

    protected override string LoadDebugInput()
    {
        return @"123 -> x
456 -> y
x AND y -> d
x OR y -> e
x LSHIFT 2 -> f
y RSHIFT 2 -> g
NOT x -> h
NOT y -> i";
    }

    abstract class Expression
    {
        public Expression() { }

        public Expression(string output)
        {
            Output = output;
        }

        public object Tag { get; set; }
        public string Output { get; private set; }

        public override string ToString()
        {
            return $"{Tag}";
        }

        public abstract bool ReadyToEval();
        public abstract int Eval();

        public virtual void UpdateExpressions(Dictionary<string, int> wires) { }
    }

    class ValueExpression : Expression
    {
        public ValueExpression(int value)
        {
            Tag = value;
        }

        public override bool ReadyToEval() => true;
        public override int Eval() => (int)Tag;
    }
    class VariableExpression : Expression
    {
        public VariableExpression(string var)
        {
            Tag = var;
        }

        public override bool ReadyToEval() => false;
        public override int Eval() => throw new NotSupportedException();
    }

    class UnaryOpExpression : Expression
    {
        public UnaryOpExpression(string value, string output, string op = default)
            : base(output)
        {
            if (int.TryParse(value, out int result))
            {
                Value = new ValueExpression(result);
            }
            else
            {
                Value = new VariableExpression(value);
            }
            Operation = GetUnaryOp(op);
        }
        public UnaryOp Operation { get; set; }
        public Expression Value { get; set; }

        public override bool ReadyToEval() => Value is ValueExpression;

        public override int Eval()
        {
            return Operation switch
            {
                UnaryOp.Eq => Value.Eval(),
                UnaryOp.Not => ~(ushort)Value.Eval() & ushort.MaxValue,
                _ => throw new NotImplementedException(),
            };
        }

        public override void UpdateExpressions(Dictionary<string, int> wires)
        {
            if (Value is VariableExpression && wires.ContainsKey((string)Value.Tag))
            {
                Value = new ValueExpression(wires[(string)Value.Tag]);
            }
        }

        private UnaryOp GetUnaryOp(string op)
        {
            return op switch
            {
                "NOT" => UnaryOp.Not,
                "" or null => UnaryOp.Eq,
                _ => throw new NotImplementedException(),
            };
        }

        public override string ToString()
        {
            return Operation switch
            {
                UnaryOp.Not => $"NOT {Value} -> {Output}",
                UnaryOp.Eq => $"{Value} -> {Output}",
                _ => throw new NotImplementedException(),
            };
        }
    }

    class BinaryOpExpression : Expression
    {
        public BinaryOpExpression(string left, string right, string binop, string output)
            : base(output)
        {
            Left = int.TryParse(left, out int result)
                ? new ValueExpression(result)
                : new VariableExpression(left);
            Right = int.TryParse(right, out result)
                ? new ValueExpression(result)
                : new VariableExpression(right);
            Operation = GetBinaryOp(binop);
        }

        public Expression Left { get; set; }
        public Expression Right { get; set; }
        public BinaryOp Operation { get; set; }

        public override bool ReadyToEval() => Left is ValueExpression && Right is ValueExpression;

        public override int Eval()
        {
            return Operation switch
            {
                BinaryOp.And => Left.Eval() & Right.Eval(),
                BinaryOp.Or => Left.Eval() | Right.Eval(),
                BinaryOp.Lsh => Left.Eval() << Right.Eval(),
                BinaryOp.Rsh => Left.Eval() >> Right.Eval(),
                _ => throw new NotImplementedException(),
            };
        }

        public override void UpdateExpressions(Dictionary<string, int> wires)
        {
            if (Left is VariableExpression && wires.ContainsKey((string)Left.Tag))
            {
                Left = new ValueExpression(wires[(string)Left.Tag]);
            }

            if (Right is VariableExpression v && wires.ContainsKey((string)Right.Tag))
            {
                Right = new ValueExpression(wires[(string)Right.Tag]);
            }
        }

        private BinaryOp GetBinaryOp(string binop)
        {
            switch (binop)
            {
                case "AND":
                    return BinaryOp.And;
                case "OR":
                    return BinaryOp.Or;
                case "LSHIFT":
                    return BinaryOp.Lsh;
                case "RSHIFT":
                    return BinaryOp.Rsh;
                default:
                    throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            return $"{Left} {Operation} {Right} -> {Output}";
        }
    }

    static class Decoder
    {
        static Regex Wire = new(@"(?<left>.*?) -> (?<right>.*)");
        static Regex OneToOne = new Regex(@"^(?<binop>NOT)?\s?(?<in1>[a-z0-9]+)$");
        static Regex TwoToOne = new Regex(@"^(?<in1>[a-z0-9]+) (?<binop>(AND|OR|LSHIFT|RSHIFT)) (?<in2>[a-z0-9]+)$");
        //static Regex Shift = new Regex(@"(<in1>[a-z]+) (?<binop>(L|R)SHIFT) (<in2>[0-9]+) -> (<out>[a-z]+)");

        public static Expression Decode(string wire)
        {
            Expression e;
            Match m;
            if ((m = Wire.Match(wire)).Success)
            {
                string output = m.Groups["right"].Value;
                string left = m.Groups["left"].Value;
                if ((m = OneToOne.Match(left)).Success)
                {
                    return new UnaryOpExpression(m.Groups["in1"].Value, output, m.Groups["binop"]?.Value);
                }
                else if ((m = TwoToOne.Match(left)).Success)
                {
                    return new BinaryOpExpression(m.Groups["in1"].Value, m.Groups["in2"].Value, m.Groups["binop"].Value, output);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
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
        Lsh,
        Rsh,
    }
}
