using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Options;

class Session
{
    private string _token = string.Empty;

    public string Token
    {
        get => _token;
        set
        {
            if (Regex.IsMatch(value, @"^[a-z0-9]+$"))
            {
                _token = value;
            }
            else
            {
                _token = string.Empty;
            }
        }
    }
}
