using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Solutions.Year2019 {

    class Day14 : ASolution {

        Dictionary<string, List<ChemicalInput>> _outputToInputs = new Dictionary<string, List<ChemicalInput>>();
        Dictionary<string, long> _outputToCount = new Dictionary<string, long>();

        public Day14() : base(14, 2019, "") {
                
        }

        protected override object SolvePartOne() {
            BuildReactionDict();

            this.DebugOutput = false;
            var oreNeeded = ProcessChemical("", new ChemicalInput("FUEL", 1), new Dictionary<string, long>());

            return oreNeeded.ToString(); 
        }

        protected override object SolvePartTwo() {
            this.DebugOutput = false;

            var minFuel = ProcessChemical("", new ChemicalInput("FUEL", 1), new Dictionary<string, long>());

            long fuel = 1;
            long target = (long)1E12;

            while (true)
            {
                var ore = OreForFuel(fuel + 1);
                if (ore > target)
                {
                    return fuel.ToString();
                }
                else
                {
                    fuel = Math.Max(fuel + 1, (fuel + 1) * target / ore);
                }
            }
        }

        private void BuildReactionDict()
        {
            foreach (var line in Input.SplitByNewline())
            {
                var arr = line.Split("=>");
                var inputs = arr[0].Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => 
                    {
                        var chem = s.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                        return new ChemicalInput(chem[1], long.Parse(chem[0]));
                    })
                    .ToList();
                var outputs = arr[1].Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                _outputToCount[outputs[1]] = long.Parse(outputs[0]);

                _outputToInputs[outputs[1]] = inputs;
            }
        }

        private long OreForFuel(long fuel)
        {
            var ore = ProcessChemical("", new ChemicalInput("FUEL", fuel), new Dictionary<string, long>());
            Console.WriteLine($"{fuel:N0} fuel needs {ore:N0} ore.");
            return ore;
        }

        private long ProcessChemical(string space, ChemicalInput reactionOutput, Dictionary<string, long> surplus)
        {
            long reactionOutputsNeeded = reactionOutput.Count;

            if (surplus.TryGetValue(reactionOutput.Chemical, out var surplusValue))
            {
                long used = Math.Min(surplusValue, reactionOutputsNeeded);
                surplus[reactionOutput.Chemical] -= used;
                WriteOutput($"{space}Used {used} spares leaving {surplus[reactionOutput.Chemical]}");
                reactionOutputsNeeded -= used;

                if (reactionOutputsNeeded == 0)
                    return 0;
            }

            WriteOutput($"{space}Still need: {reactionOutputsNeeded} of {reactionOutput.Chemical}");

            List<ChemicalInput> reactionInputs = _outputToInputs[reactionOutput.Chemical];

            long outputsPerReaction = _outputToCount[reactionOutput.Chemical];
            long reactionCount = (long)Math.Ceiling(reactionOutputsNeeded / (double)outputsPerReaction);

            WriteOutput($"{space}{reactionCount} reactions will produce: {outputsPerReaction * reactionCount}");

            long spare = reactionCount * outputsPerReaction - reactionOutputsNeeded;
            WriteOutput($"{space}Leaving {spare} spare");
            surplus[reactionOutput.Chemical] = spare;

            long i = 0;
            foreach (ChemicalInput input in reactionInputs)
            {
                if (input.Chemical == "ORE")
                {
                    WriteOutput($"{space}Using {reactionCount * _outputToInputs[reactionOutput.Chemical][0].Count} ore");
                    return reactionCount * _outputToInputs[reactionOutput.Chemical][0].Count;
                }
                else
                {
                    WriteOutput($"{space}Need: {input.Count * reactionCount} of {input.Chemical}");
                    i += ProcessChemical($"{space}\t", new ChemicalInput(input.Chemical, input.Count * reactionCount), surplus);
                }
            }

            return i;
        }

        [DebuggerDisplay("{Count} {Chemical}")]
        class ChemicalInput
        {
            public long Count { get; }
            public string Chemical { get; }

            public ChemicalInput(string chemical, long count)
            {
                Chemical = chemical;
                Count = count;
            }
        }

        string test = @"10 ORE => 10 A
1 ORE => 1 B
7 A, 1 B => 1 C
7 A, 1 C => 1 D
7 A, 1 D => 1 E
7 A, 1 E => 1 FUEL";
        string test2 = @"9 ORE => 2 A
8 ORE => 3 B
7 ORE => 5 C
3 A, 4 B => 1 AB
5 B, 7 C => 1 BC
4 C, 1 A => 1 CA
2 AB, 3 BC, 4 CA => 1 FUEL";
        string test3 = @"157 ORE => 5 NZVS
165 ORE => 6 DCFZ
44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL
12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ
179 ORE => 7 PSHF
177 ORE => 5 HKGWZ
7 DCFZ, 7 PSHF => 2 XJWVT
165 ORE => 2 GPVTF
3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT";
        string test4 = @"2 VPVL, 7 FWMGM, 2 CXFTF, 11 MNCFX => 1 STKFG
17 NVRVD, 3 JNWZP => 8 VPVL
53 STKFG, 6 MNCFX, 46 VJHF, 81 HVMC, 68 CXFTF, 25 GNMV => 1 FUEL
22 VJHF, 37 MNCFX => 5 FWMGM
139 ORE => 4 NVRVD
144 ORE => 7 JNWZP
5 MNCFX, 7 RFSQX, 2 FWMGM, 2 VPVL, 19 CXFTF => 3 HVMC
5 VJHF, 7 MNCFX, 9 VPVL, 37 CXFTF => 6 GNMV
145 ORE => 6 MNCFX
1 NVRVD => 8 CXFTF
1 VJHF, 6 MNCFX => 4 RFSQX
176 ORE => 6 VJHF";
        string test5 = @"171 ORE => 8 CNZTR
7 ZLQW, 3 BMBT, 9 XCVML, 26 XMNCP, 1 WPTQ, 2 MZWV, 1 RJRHP => 4 PLWSL
114 ORE => 4 BHXH
14 VRPVC => 6 BMBT
6 BHXH, 18 KTJDG, 12 WPTQ, 7 PLWSL, 31 FHTLT, 37 ZDVW => 1 FUEL
6 WPTQ, 2 BMBT, 8 ZLQW, 18 KTJDG, 1 XMNCP, 6 MZWV, 1 RJRHP => 6 FHTLT
15 XDBXC, 2 LTCX, 1 VRPVC => 6 ZLQW
13 WPTQ, 10 LTCX, 3 RJRHP, 14 XMNCP, 2 MZWV, 1 ZLQW => 1 ZDVW
5 BMBT => 4 WPTQ
189 ORE => 9 KTJDG
1 MZWV, 17 XDBXC, 3 XCVML => 2 XMNCP
12 VRPVC, 27 CNZTR => 2 XDBXC
15 KTJDG, 12 BHXH => 5 XCVML
3 BHXH, 2 VRPVC => 7 MZWV
121 ORE => 7 VRPVC
7 XCVML => 6 RJRHP
5 BHXH, 4 VRPVC => 5 LTCX";
    }
}
