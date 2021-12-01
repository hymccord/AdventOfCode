namespace AdventOfCode.Solutions.Year2020.Day04
{
    class Day04 : ASolution
    {
        private List<Dictionary<string, string>> _passports;

        public Day04() :
            base(4, 2020, "Passport Processing")
        { }

        protected override void Preprocess()
        {
            // Split by empty line
            var passportLines = Input.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.None)
                .Select(s => s.SplitByNewline().JoinAsStrings(" "));

            // Build into kvp
            _passports = passportLines.Select(p =>
            {
                var matches = Regex.Matches(p, @"(\w+):(\S+)");
                var kvp = matches.ToDictionary(m => m.Groups[1].Value, m => m.Groups[2].Value);
                kvp.Remove("cid");
                return kvp;
            }).ToList();
        }

        protected override object SolvePartOne()
        {
            // Valid passports have 7 required fields, not cid
            return _passports.Count(p => p.Keys.Count == 7).ToString();
        }

        protected override object SolvePartTwo()
        {
            int count = 0;

            foreach (var passport in _passports)
            {
                if (Rules.All(rule => passport.ContainsKey(rule.Key) && rule.Validate(passport[rule.Key])))
                {
                    count++;
                }
            }
            return count.ToString();
        }

        private List<ValidationRule> Rules = new List<ValidationRule>
        {
            new Byr(),
            new Iyr(),
            new Eyr(),
            new Hgt(),
            new Hcl(),
            new Ecl(),
            new Pid(),
        };

        abstract class ValidationRule
        {
            public abstract string Key { get; }
            public abstract bool Validate(string value);
        }

        abstract class IntValidationRule : ValidationRule
        {
            public sealed override bool Validate(string value) => Validate(int.Parse(value));

            public abstract bool Validate(int value);
        }

        abstract class RegexMatchRule : ValidationRule
        {
            public abstract string Pattern { get; }

            public sealed override bool Validate(string value)
            {
                return Regex.IsMatch(value, Pattern);
            }
        }

        class Byr : IntValidationRule
        {
            public override string Key => "byr";

            public override bool Validate(int value)
            {
                return 1920 <= value && value <= 2002;
            }
        }

        class Iyr : IntValidationRule
        {
            public override string Key => "iyr";

            public override bool Validate(int value)
            {
                return 2010 <= value && value <= 2020;
            }
        }

        class Eyr : IntValidationRule
        {
            public override string Key => "eyr";

            public override bool Validate(int value)
            {
                return 2020 <= value && value <= 2030;
            }
        }

        class Hgt : ValidationRule
        {
            public override string Key => "hgt";

            public override bool Validate(string value)
            {
                var m = Regex.Match(value, @"(\d+)(in|cm)");
                if (!m.Success) return false;

                int hgt = int.Parse(m.Groups[1].Value);
                if (m.Groups[2].Value == "in")
                    return 59 <= hgt && hgt <= 76;
                else
                    return 150 <= hgt && hgt <= 193;
            }
        }

        class Hcl : RegexMatchRule
        {
            public override string Key => "hcl";

            public override string Pattern => @"#[a-z0-f]{6}$";
        }

        class Ecl : RegexMatchRule
        {
            public override string Key => "ecl";

            public override string Pattern => @"^(amb|blu|brn|gry|grn|hzl|oth)$";
        }

        class Pid : RegexMatchRule
        {
            public override string Key => "pid";

            public override string Pattern => @"^\d{9}$";
        }
    }
}
