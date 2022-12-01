using System.Text.Json;

namespace AdventOfCode
{
    partial class Config
    {

        string _c;
        int _y;
        int[] _d;

        public string Session
        {
            get => _c;
            set
            {
                if (AoCSessionRegex().IsMatch(value))
                {
                    _c = value;
                }
                else
                {
                    _c = "";
                }
            }
        }
        public int Year
        {
            get => _y;
            set
            {
                if (value < 2015 || value > DateTime.Now.Year)
                {
                    _y = DateTime.Now.Year;
                }
                else
                {
                    _y = value;
                }
            }
        }
        public int[] Days
        {
            get => _d;
            set
            {
                bool allDaysCovered = false;
                _d = value.Where(v =>
                {
                    if (v == 0) allDaysCovered = true;
                    return v > 0 && v < 26;
                }).ToArray();

                if (allDaysCovered)
                {
                    _d = new int[] { 0 };
                }
                else
                {
                    Array.Sort(_d);
                }
            }
        }

        public static Config Get(string path)
        {
            var options = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            Config config;
            if (File.Exists(path))
            {
                config = JsonSerializer.Deserialize<Config>(File.ReadAllText(path), options);
            }
            else
            {
                config = new Config();
                config.setDefaults();
                File.WriteAllText(path, JsonSerializer.Serialize(config, options));
            }
            return config;
        }

        void setDefaults()
        {
            Session = "";
            Year = DateTime.Now.Year;
            Days = DateTime.Now.Month == 12 ? new int[] { DateTime.Now.Day } : new int[] { 0 };
        }

        [GeneratedRegex("^[a-z0-9]+$")]
        private static partial Regex AoCSessionRegex();
    }
}
