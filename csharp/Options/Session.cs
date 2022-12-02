using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Options;

partial class Session
{
    private string _token = string.Empty;

    public string Token
    {
        get => _token;
        set
        {
            if (AoCSessionRegex().IsMatch(value))
            {
                _token = value;
            }
            else
            {
                _token = string.Empty;
            }
        }
    }

    [GeneratedRegex("^[a-z0-9]+$")]
    private static partial Regex AoCSessionRegex();
}
