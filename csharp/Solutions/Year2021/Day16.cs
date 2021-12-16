namespace AdventOfCode.Solutions.Year2021
{
    internal class Day16 : ASolution
    {
        string _inputAsBinary;
        Packet _packet;

        public Day16() : base(16, 2021, "Packet Decoder", false)
        { }

        protected override void Preprocess()
        {
            _inputAsBinary = string.Join(null, Input.ToCharArray().Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
        }

        protected override object SolvePartOne()
        {
            var reader = new BITSReader(_inputAsBinary.AsMemory());
            var decoder = Decoder.Create(reader);

            decoder.Decode(out _packet);

            return _packet.Traverse().Sum(p => p.Version);
        }

        protected override object SolvePartTwo()
        {
            return _packet.GetValue();
        }

        protected override string LoadDebugInput()
        {
            return "CE00C43D881120";
        }

    }

    public abstract class Packet
    {
        public int Version { get; protected set; }
        public abstract int TypeId { get; }

        public abstract long GetValue();
    }

    public static class PacketExtensions
    {
        public static IEnumerable<Packet> Traverse(this Packet p)
        {
            var stack = new Stack<Packet>(new[] { p });
            while (stack.Count > 0)
            {
                var next = stack.Pop();
                yield return next;
                if (next is OperatorPacket op)
                {
                    foreach (var packet in op.SubPackets)
                    {
                        stack.Push(packet);
                    }
                }
            }
        }
    }

    class LiteralPacket : Packet
    {
        public override int TypeId => 4;

        public long Value { get; private set; }

        public override long GetValue() => Value;

        internal static Packet Decode(BITSReader reader, int version)
        {
            var packet = new LiteralPacket();
            packet.Version = version;

            string result = "";
            string bits;
            do
            {
                bits = reader.ReadString(5);
                result += bits[1..];
            } while (bits[0] != '0');

            packet.Value = Convert.ToInt64(result, 2);

            return packet;
        }
    }

    abstract class OperatorPacket : Packet
    {
        private static readonly Dictionary<int, Func<OperatorPacket>> s_operatorPackets = new()
        {
            { 0, () => new SumPacket() },
            { 1, () => new ProductPacket() },
            { 2, () => new MinimumPacket() },
            { 3, () => new MaximumPacket() },
            { 5, () => new GreaterThanPacket() },
            { 6, () => new LessThanPacket() },
            { 7, () => new EqualToPacket() },
        };

        public IReadOnlyCollection<Packet> SubPackets { get; private set; }

        internal static Packet Decode(BITSReader reader, int version, int typeId)
        {
            OperatorPacket packet = s_operatorPackets[typeId]();
            packet.Version = version;

            int lengthTypeId = reader.ReadBit();

            var decoder = Decoder.Create(reader);
            var subPackets = new List<Packet>();
            if (lengthTypeId == 0)
            {
                // total length of sub-packets
                int length = reader.ReadInt(15);
                int stopPosition = reader.Position + length;
                while (stopPosition != reader.Position)
                {
                    decoder.Decode(out Packet sub);
                    subPackets.Add(sub);
                }
            }
            else
            {
                int numPackets = reader.ReadInt(11);
                for (int i = 0; i < numPackets; i++)
                {
                    decoder.Decode(out Packet sub);
                    subPackets.Add(sub);
                }
            }

            packet.SubPackets = subPackets;

            return packet;
        }
    }

    class SumPacket : OperatorPacket
    {
        public override int TypeId => 0;
        public override long GetValue() => SubPackets.Sum(p => p.GetValue());
    }

    class ProductPacket : OperatorPacket
    {
        public override int TypeId => 1;
        public override long GetValue() => SubPackets.Aggregate(1L, (seed, p) => seed * p.GetValue());
    }

    class MinimumPacket : OperatorPacket
    {
        public override int TypeId => 2;
        public override long GetValue() => SubPackets.Min(p => p.GetValue());
    }

    class MaximumPacket : OperatorPacket
    {
        public override int TypeId => 3;
        public override long GetValue() => SubPackets.Max(p => p.GetValue());
    }

    class GreaterThanPacket : OperatorPacket
    {
        public override int TypeId => 5;
        public override long GetValue()
        {
            var subPackets = SubPackets.ToArray();
            return subPackets[0].GetValue() > subPackets[1].GetValue() ? 1 : 0;
        }
    }
    class LessThanPacket : OperatorPacket
    {
        public override int TypeId => 6;
        public override long GetValue()
        {
            var subPackets = SubPackets.ToArray();
            return subPackets[0].GetValue() < subPackets[1].GetValue() ? 1 : 0;
        }
    }
    class EqualToPacket : OperatorPacket
    {
        public override int TypeId => 7;
        public override long GetValue()
        {
            var subPackets = SubPackets.ToArray();
            return subPackets[0].GetValue() == subPackets[1].GetValue() ? 1 : 0;
        }
    }


    class Decoder
    {
        private readonly BITSReader _reader;

        public Decoder(BITSReader reader)
        {
            _reader = reader;
        }

        public static Decoder Create(BITSReader reader) => new(reader);

        public void Decode(out Packet packet)
        {
            packet = default;

            int version = _reader.ReadInt(3);
            int typeid = _reader.ReadInt(3);

            packet = typeid switch
            {
                4 => LiteralPacket.Decode(_reader, version),
                _ => OperatorPacket.Decode(_reader, version, typeid),
            };
        }
    }

    class BITSReader
    {
        private readonly ReadOnlyMemory<char> _data;
        private int _currentPosition;

        public int Position
        {
            get => _currentPosition;
            set => _currentPosition = value;
        }

        public BITSReader(ReadOnlyMemory<char> data)
        {
            _data = data;
            _currentPosition = 0;
        }

        public int ReadBit() => _data.Span[_currentPosition++] - '0';

        public int ReadInt(int length)
        {
            if (length <= 0)
            {
                return 0;
            }

            var result = Convert.ToInt32(_data.Span.Slice(_currentPosition, length).ToString(), 2);
            _currentPosition += length;

            return result;
        }

        public string ReadString(int length)
        {
            var result = _data.Span.Slice(_currentPosition, length);
            _currentPosition += length;

            return result.ToString();
        }
    }
}
