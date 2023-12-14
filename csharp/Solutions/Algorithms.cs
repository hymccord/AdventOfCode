using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static AdventOfCode.Solutions.ASolution;

namespace AdventOfCode.Solutions;
internal static class Algorithms
{
    /// <summary>
    /// Shoelace Algorithm implementation
    /// </summary>
    public static double PolygonArea(IList<Point> points)
    {
        // Initialize area
        double area = 0.0;

        // Calculate value of shoelace formula
        int j = points.Count - 1;

        for (int i = 0; i < points.Count; i++)
        {
            area += (points[j].X + points[i].X) * (points[j].Y - points[i].Y);

            // j is previous vertex to i
            j = i;
        }

        // Return absolute value
        return Math.Abs(area / 2.0);
    }
}
