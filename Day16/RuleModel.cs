using System.Collections.Generic;

namespace AdventOfCode.Day16.Models
{
    public class RuleModel
    {
        public string ruleName;
        public int min1;
        public int max1;
        public int min2;
        public int max2;
        public List<int> positionsValidated;

        public RuleModel(string ruleName = "", int min1 = 0, int max1 = 0, int min2 = 0, int max2 = 0)
        {
            this.ruleName = ruleName;
            this.min1 = min1;
            this.max1 = max1;
            this.min2 = min2;
            this.max2 = max2;
            this.positionsValidated = new List<int>();
        }

        public bool CheckIfNumberIsValid(int number)
        {
            return (number >= min1 && number <= max1) || (number >= min2 && number <= max2);
        }
    }
}