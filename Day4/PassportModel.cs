using System;

namespace AdventOfCode.Day4.Models
{
    public class PassportModel
    {
        private string passportString { get; set; }
        public int byr { get; set; }
        public int iyr { get; set; }
        public int eyr { get; set; }
        public string hgt { get; set; }
        public string hcl { get; set; }
        public string ecl { get; set; }
        public string pid { get; set; }
        public string cid { get; set; }

        public PassportModel()
        {
            this.passportString = string.Empty;
            this.byr = 0;
            this.iyr = 0;
            this.eyr = 0;
            this.hgt = string.Empty;
            this.hcl = string.Empty;
            this.ecl = string.Empty;
            this.pid = string.Empty;
            this.cid = string.Empty;
        }

        public PassportModel(string passportString) : this()
        {
            this.passportString = passportString;
            var properties = passportString.Split(' ');
            foreach (var property in properties)
            {
                int x = 0;
                switch (property.Split(':')[0])
                {
                    case "byr":
                        if (Int32.TryParse(property.Split(':')[1], out x))
                        {
                            this.byr = x;
                        }
                        break;
                    case "iyr":
                        if (Int32.TryParse(property.Split(':')[1], out x))
                        {
                            this.iyr = x;
                        }
                        break;
                    case "eyr":
                        if (Int32.TryParse(property.Split(':')[1], out x))
                        {
                            this.eyr = x;
                        }
                        break;
                    case "hgt":
                        this.hgt = property.Split(':')[1];
                        break;
                    case "hcl":
                        this.hcl = property.Split(':')[1];
                        break;
                    case "ecl":
                        this.ecl = property.Split(':')[1];
                        break;
                    case "pid":
                        this.pid = property.Split(':')[1];
                        break;
                    case "cid":
                        this.cid = property.Split(':')[1];
                        break;
                }
            }
        }

        public override string ToString()
        {
            return this.passportString;
        }
    }
}