using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Year2020.Day17
{
    class Day17 : ASolution
    {
        private const string Test = @".#.
..#
###";
        private Dictionary<(int x, int y, int z), char> _state = new();

        public Day17()
            : base(17, 2020, "Conway Cubes")
        {

        }
        protected override object SolvePartOne()
        {
            HashSet<(int x, int y, int z)> cubes = new();
            string[] input = Input.SplitByNewline();
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    if (input[y][x] == '#')
                        cubes.Add((x, y, 0));
                }
            }

            for (int i = 0; i < 6; i++)
            {
                Dictionary<(int x, int y, int z), int> activeNeigbors = new();
                foreach ((int pX, int pY, int pZ) cur in cubes)
                {
                    for (int z = -1; z < 2; z++)
                    {
                        for (int y = -1; y < 2; y++)
                        {
                            for (int x = -1; x < 2; x++)
                            {
                                if (x == 0 && y == 0 && z == 0)
                                    continue;

                                var p = (cur.pX + x, cur.pY + y, cur.pZ + z);

                                activeNeigbors[p] = activeNeigbors.Get(p, 0) + 1;
                            }
                        }
                    }
                }

                HashSet<(int x, int y, int z)> next = new();
                foreach ((int pX, int pY, int pZ) cur in cubes)
                {
                    int neighbors = activeNeigbors.Get(cur, 0);
                    if (neighbors == 2 || neighbors == 3)
                        next.Add(cur);

                }

                foreach (var growing in activeNeigbors.Where(kvp => kvp.Value == 3))
                {
                    next.Add(growing.Key);
                }

                cubes = next;
            }

            return cubes.Count;
        }

        protected override object SolvePartTwo()
        {
            HashSet<(int w, int x, int y, int z)> cubes = new();
            string[] input = Input.SplitByNewline();
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    if (input[y][x] == '#')
                        cubes.Add((0, x, y, 0));
                }
            }

            for (int i = 0; i < 6; i++)
            {
                Dictionary<(int w, int x, int y, int z), int> activeNeigbors = new();
                foreach ((int pW, int pX, int pY, int pZ) cur in cubes)
                {
                    for (int z = -1; z < 2; z++)
                    {
                        for (int y = -1; y < 2; y++)
                        {
                            for (int x = -1; x < 2; x++)
                            {
                                for (int w = -1; w < 2; w++)
                                {
                                    if (w == 0 && x == 0 && y == 0 && z == 0)
                                        continue;

                                    var p = (cur.pW + w, cur.pX + x, cur.pY + y, cur.pZ + z);

                                    activeNeigbors[p] = activeNeigbors.Get(p, 0) + 1; 
                                }
                            }
                        }
                    }
                }

                HashSet<(int w, int x, int y, int z)> next = new();
                foreach ((int pW, int pX, int pY, int pZ) cur in cubes)
                {
                    int neighbors = activeNeigbors.Get(cur, 0);
                    if (neighbors == 2 || neighbors == 3)
                        next.Add(cur);

                }

                foreach (var growing in activeNeigbors.Where(kvp => kvp.Value == 3))
                {
                    next.Add(growing.Key);
                }

                cubes = next;
            }

            return cubes.Count;
        }
    }
}
