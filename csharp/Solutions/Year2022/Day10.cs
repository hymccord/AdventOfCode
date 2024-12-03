namespace AdventOfCode.Solutions.Year2022;

#nullable enable

internal class Day10 : ASolution
{
    private CPU2022 _cpu = default!;

    public Day10() : base(10, 2022, "Cathode-Ray Tube", false)
    {

    }

    protected override void Preprocess()
    {
        _cpu = CPU2022.Parse(InputByNewLine);
    }

    protected override object SolvePartOne()
    {
        List<int> signals = new List<int>();

        _cpu.During += (s, e) =>
        {
            if ((_cpu.Cycle - 20) % 40 == 0)
            {
                signals.Add(_cpu.Registers['X'] * _cpu.Cycle);
            }
        };
        _cpu.Run();

        return signals.Sum();
    }

    protected override object SolvePartTwo()
    {
        List<char> signals = new();

        _cpu.Reset();
        _cpu.During += (s, e) =>
        {
            int pixel = _cpu.Cycle % 40 - 1;
            if (pixel >= (_cpu.Registers['X'] - 1) && pixel <= (_cpu.Registers['X'] + 1))
            {
                signals.Add('█');
            }
            else
            {
                signals.Add(' ');
            }
        };
        _cpu.Run();

        return "\n" + signals.Chunk(40).Select(x => new string(x)).JoinAsStrings("\n");
    }

    protected override string LoadDebugInput()
    {
        return """
        addx 15
        addx -11
        addx 6
        addx -3
        addx 5
        addx -1
        addx -8
        addx 13
        addx 4
        noop
        addx -1 //20
        addx 5
        addx -1
        addx 5
        addx -1
        addx 5
        addx -1
        addx 5
        addx -1
        addx -35
        addx 1
        addx 24
        addx -19
        addx 1
        addx 16
        addx -11
        noop
        noop
        addx 21
        addx -15
        noop
        noop
        addx -3
        addx 9
        addx 1
        addx -3
        addx 8
        addx 1
        addx 5
        noop
        noop
        noop
        noop
        noop
        addx -36
        noop
        addx 1
        addx 7
        noop
        noop
        noop
        addx 2
        addx 6
        noop
        noop
        noop
        noop
        noop
        addx 1
        noop
        noop
        addx 7
        addx 1
        noop
        addx -13
        addx 13
        addx 7
        noop
        addx 1
        addx -33
        noop
        noop
        noop
        addx 2
        noop
        noop
        noop
        addx 8
        noop
        addx -1
        addx 2
        addx 1
        noop
        addx 17
        addx -9
        addx 1
        addx 1
        addx -3
        addx 11
        noop
        noop
        addx 1
        noop
        addx 1
        noop
        noop
        addx -13
        addx -19
        addx 1
        addx 3
        addx 26
        addx -30
        addx 12
        addx -1
        addx 3
        addx 1
        noop
        noop
        noop
        addx -9
        addx 18
        addx 1
        addx 2
        noop
        noop
        addx 9
        noop
        noop
        noop
        addx -1
        addx 2
        addx -37
        addx 1
        addx 3
        noop
        addx 15
        addx -21
        addx 22
        addx -6
        addx 1
        noop
        addx 2
        addx 1
        noop
        addx -10
        noop
        noop
        addx 20
        addx 1
        addx 2
        addx 2
        addx -6
        addx -11
        noop
        noop
        noop
        """;
    }
}

class CPU2022
{
    private Queue<(int, Instruction)> _pipelines = new();
    private Dictionary<char, int> _registers = new Dictionary<char, int>();
    private int _ip = 0;
    private int _numPipes = 1;
    private int _cycle = 1;

    private Instruction[] _instructions;
#pragma warning disable IDE0052, CS0414 // Remove unread private members
    private Instruction? _current;

    public event EventHandler? Start;
    public event EventHandler? During;
    public event EventHandler? After;
#pragma warning restore IDE0052, CS0414 // Remove unread private members

    public IReadOnlyDictionary<char, int> Registers => _registers;

    public int Cycle => _cycle;

    private CPU2022(IEnumerable<string> program, int pipelines = 1)
    {
        _instructions = ParseInstructions(program);
        Reset();
    }

    public static CPU2022 Parse(string program)
    {
        var cpu = new CPU2022(program.SplitByNewline());
        return cpu;
    }

    public static CPU2022 Parse(IEnumerable<string> program)
    {
        var cpu = new CPU2022(program);
        return cpu;
    }

    public void Reset()
    {
        _current = null;
        _registers.Clear();
        _registers['X'] = 1;
        _pipelines.Clear();
        _cycle = 1;
        _ip = 0;

        Start = null;
        During = null;
        After = null;
    }

    public void Run()
    {
        while (_ip < _instructions.Length)
        {
            if (_pipelines.Count < _numPipes)
            {
                _pipelines.Enqueue((_cycle - 1, _instructions[_ip++]));
            }
            // Start

            // During
            During?.Invoke(this, EventArgs.Empty);

            // After
            for (int i = 0; i < _pipelines.Count; i++)
            {
                (int startCycle, Instruction inst) = _pipelines.Dequeue();
                if ((startCycle + inst.Cycles) == _cycle)
                {
                    inst.Execute(this);
                }
                else
                {
                    _pipelines.Enqueue((startCycle, inst));
                }
            }

            _cycle++;
        }
    }

    private static Instruction[] ParseInstructions(IEnumerable<string> strInstructions)
    {
        List<Instruction> instructions = new();
        foreach (var instruction in strInstructions)
        {
            instructions.Add(instruction[0..4] switch
            {
                "noop" => new Noop(),
                "addx" => new AddX(instruction),
                _ => throw new NotImplementedException(),
            });
        }

        return instructions.ToArray();
    }

    private class Noop : Instruction
    {
        public override int Cycles => 1;
    }

    private class AddX : Instruction
    {
        private int _value;

        public override int Cycles => 2;

        public AddX(string instruction)
        {
            _value = int.Parse(instruction.Split(' ')[1]);
        }

        public override void Execute(CPU2022 cpu)
        {
            cpu._registers['X'] += _value;
        }
    }

    private abstract class Instruction
    {
        public abstract int Cycles { get; }

        public virtual void Execute(CPU2022 cpu)
        { }
    }
}
