namespace AdventOfCode.Solutions.Year2019
{

    class Day02 : ASolution
    {

        private int[] test = new int[] { 1, 1, 1, 4, 99, 5, 6, 0, 99 };
        private int[] intInputs;

        public Day02() : base(2, 2019, "")
        { }

        protected override void Preprocess()
        {
            intInputs = Input.Split(',', System.StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            //intInputs = test;
        }

        protected override object SolvePartOne()
        {
            IntCode cpu = IntCode.Create(Input.Trim('\n'));
            cpu.ByteCode[1] = 12;
            cpu.ByteCode[2] = 2;
            cpu.Run();

            return cpu.ByteCode[0].ToString();
        }

        protected override object SolvePartTwo()
        {
            IntCode cpu = IntCode.Create(Input.Trim('\n'));

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    cpu.Reset();
                    cpu.ByteCode[1] = i;
                    cpu.ByteCode[2] = j;
                    cpu.Run();

                    if (cpu.ByteCode[0] == 19690720)
                        return $"100 * {i} + {j} = {100 * i + j}";

                }
            }

            return null;
        }
    }
}
