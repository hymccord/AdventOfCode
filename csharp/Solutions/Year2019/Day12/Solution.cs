using System.Diagnostics;
using System.Numerics;

namespace AdventOfCode.Solutions.Year2019
{

    class Day12 : ASolution
    {

        string test = @"<x=-1, y=0, z=2>
<x=2, y=-10, z=-7>
<x=4, y=-8, z=8>
<x=3, y=5, z=-1>";
        string test2 = @"<x=-8, y=-10, z=0>
<x=5, y=5, z=10>
<x=2, y=-7, z=3>
<x=9, y=-8, z=-3>";
        private List<Moon> _moons = new List<Moon>();
        public Day12() : base(12, 2019, "")
        {

        }

        protected override object SolvePartOne()
        {
            ParseInput();

            for (int i = 0; i < 1000; i++)
            {
                SimulateOneRound();
            }

            return _moons.Sum(m => m.TotalEnergy).ToString();
        }

        private int currentSim = 0;
        protected override object SolvePartTwo()
        {
            ParseInput();

            var xs = new HashSet<int>();

            var repeatX = 0;
            var repeatY = 0;
            var repeatZ = 0;
            while (repeatX == 0 || repeatY == 0 || repeatZ == 0)
            {
                currentSim++;
                SimulateOneRound();
                if (_moons.All(m => m.IsAtOriginalX) && repeatX == 0)
                {
                    repeatX = currentSim;
                }
                if (_moons.All(m => m.IsAtOriginalY) && repeatY == 0)
                {
                    repeatY = currentSim;
                }
                if (_moons.All(m => m.IsAtOriginalZ) && repeatZ == 0)
                {
                    repeatZ = currentSim;
                }
            }
            return (new long[] { repeatX, repeatY, repeatZ }).LCM().ToString();
        }

        private void SimulateOneRound()
        {
            ApplyGravity();
            ApplyVelocity();
        }

        private void ApplyGravity()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = i + 1; j < 4; j++)
                {
                    var m1 = _moons[i];
                    var m2 = _moons[j];

                    m1.ComputeVelocities(m2);
                }
            }
        }

        private void ApplyVelocity()
        {
            foreach (var moon in _moons)
            {
                moon.Move();
            }
        }

        private void ParseInput()
        {
            _moons.Clear();
            foreach (var line in Input.SplitByNewline())
            {
                var args = line.Split(new char[] { '<', '>', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.Parse(s.Substring(2))).ToArray();
                _moons.Add(new Moon(new Vector3(args[0], args[1], args[2]), new Vector3()));
            }
        }
    }

    [DebuggerDisplay("{ToString(),nq}")]
    class Moon
    {
        public Vector3 Original { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }

        public float PotentialEnergy => Math.Abs(Position.X) + Math.Abs(Position.Y) + Math.Abs(Position.Z);
        public float KinecticEnergy => Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) + Math.Abs(Velocity.Z);

        public bool IsAtOriginalX => Position.X == Original.X && Velocity.X == 0;
        public bool IsAtOriginalY => Position.Y == Original.Y && Velocity.Y == 0;
        public bool IsAtOriginalZ => Position.Z == Original.Z && Velocity.Z == 0;

        public int TotalEnergy => (int)(PotentialEnergy * KinecticEnergy);
        public Moon(Vector3 position, Vector3 velocity)
        {
            Original = position;
            Position = position;
            Velocity = velocity;
        }

        public void ComputeVelocities(Moon other)
        {
            int dx = other.Position.X.CompareTo(Position.X);
            int dy = other.Position.Y.CompareTo(Position.Y);
            int dz = other.Position.Z.CompareTo(Position.Z);
            var dV = new Vector3(dx, dy, dz);
            Velocity += dV;
            other.Velocity += -dV;
        }

        public void Move()
        {
            Position += Velocity;
        }

        public override string ToString()
        {
            return $"{Position}, {Velocity}";
        }
    }
}
