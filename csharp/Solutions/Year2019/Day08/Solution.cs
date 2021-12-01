namespace AdventOfCode.Solutions.Year2019
{

    class Day08 : ASolution
    {

        private const int Rows = 6;
        private const int Cols = 25;

        int _numLayers;
        int _layerLen;
        byte[] _data;
        byte[,] _image;

        string test = "0222112222120000";
        public Day08() : base(8, 2019, "")
        {

        }

        protected override object SolvePartOne()
        {
            _data = Input.SplitByNewline().First().ToCharArray().Select(f => (byte)(f - '0')).ToArray();
            _numLayers = _data.Length / (Rows * Cols);
            _layerLen = _data.Length / _numLayers;

            byte[] layer = new byte[0];
            int minIndex = 0;
            int numZeros = int.MaxValue;
            for (int i = 0; i < _numLayers; i++)
            {
                var b = _data[(i * _layerLen)..(i * _layerLen + _layerLen)];
                var temp = b.Count(b => b == 0);
                if (temp < numZeros)
                {
                    numZeros = temp;
                    minIndex = i;
                    layer = b;
                }
            }

            int numOnes = layer.Count(b => b == 1);
            int numTwos = layer.Count(b => b == 2);

            return (numOnes * numTwos).ToString();
        }

        protected override object SolvePartTwo()
        {
            _image = new byte[Rows, Cols];
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    _image[row, col] = 0;
                    for (int layer = 0; layer < _numLayers; layer++)
                    {
                        byte curPixel = _data[layer * _layerLen + row * Cols + col];
                        if (curPixel == 2)
                            continue;

                        _image[row, col] = curPixel;
                        break;
                    }
                }
            }

            WriteConsole(Rows, Cols, 15, 15, (row, col) =>
            {
                return _image[row, col] switch
                {
                    0 => (ConsoleColor.Gray, ' '),
                    1 => (ConsoleColor.White, '#'),
                    _ => throw new Exception(),
                };
            });
            return null;
        }
    }
}
