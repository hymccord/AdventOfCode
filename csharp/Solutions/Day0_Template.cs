namespace AdventOfCode.Solutions;

internal class DayTemplate : ASolution
{
    public DayTemplate()
        // CHANGE THESE vvv per day. It's how the solution collector finds days for specific years.
        : base(day: -1, year: 0, title: "", debug: false)
    {
        // You can do processing here on input but if you do, assign it via a Lazy(() => {}) factory
        // b/c input will not be available yet.
    }

    /// <summary>
    /// Virtual method that allows you to do work on the input before part 1 and 2
    /// </summary>
    /// <remarks>
    /// Not required to be implemented.
    /// </remarks>
    protected override void Preprocess()
    {
        
    }

    /// <summary>
    /// Abstract method implementation for part 1
    /// </summary>
    /// <returns>The answer to part 1, or null if not done yet.</returns>
    protected override object SolvePartOne()
    {
        return null;
    }

    /// <summary>
    /// Abstract method implementation for part 1
    /// </summary>
    /// <returns>The answer to part 1, or null if not done yet.</returns>
    protected override object SolvePartTwo()
    {
        return null;
    }

    protected override string LoadDebugInput()
    {
        return """

            """;
    }
}
