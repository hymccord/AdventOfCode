namespace AdventOfCode.Solutions.Year2020.Day08
{
    class Day08 : ASolution
    {
        public Day08() :
            base(08, 2020, "Handheld Halting")
        { }

        protected override object SolvePartOne()
        {
            var hgc = HandheldGameConsole.Create(Input);
            hgc.Run();
            return hgc.Acc;
        }

        protected override object SolvePartTwo()
        {
            int i = 0;
            while (true)
            {
                var hgc = HandheldGameConsole.Create(Input);
                hgc.FlipInstructin(i);
                if (hgc.Run())
                {
                    i = hgc.Acc;
                    break;
                }
                i++;
            }

            return i;
        }

        private class HandheldGameConsole
        {
            private (string instr, int arg)[] _program;
            private int _rip = 0;
            private int _acc = 0;
            private HashSet<int> _alreadyRun = new();

            public int Rip => _rip;
            public int Acc => _acc;

            HandheldGameConsole(string input)
            {
                _program = input.SplitByNewline().Select(s =>
                {
                    var instr = s.Split(' ');
                    return (instr[0], int.Parse(instr[1]));
                }
                ).ToArray();
            }

            public static HandheldGameConsole Create(string input)
            {
                return new HandheldGameConsole(input);
            }

            public void FlipInstructin(int i)
            {
                var (instr, arg) = _program[i];
                instr = instr switch
                {
                    "jmp" => "nop",
                    "nop" => "jmp",
                    _ => instr,
                };
                _program[i] = (instr, arg);
            }

            public bool Run()
            {
                while (!_alreadyRun.Contains(_rip) && _rip < _program.Length)
                {
                    _alreadyRun.Add(_rip);
                    var (instr, arg) = _program[_rip];

                    switch (instr)
                    {
                        case "jmp":
                            _rip += arg;
                            break;
                        case "acc":
                            _acc += arg;
                            _rip++;
                            break;
                        case "nop":
                        default:
                            _rip++;
                            break;
                    }
                }

                return _rip >= _program.Length;
            }
        }
    }
}
