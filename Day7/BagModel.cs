using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day7.Models
{
    public class BagModel
    {
        public List<BagModel> innerBags;
        public string color;
        public int count;

        public BagModel()
        {
            this.innerBags = new List<BagModel>();
            this.color = string.Empty;
            this.count = 0;
        }

        public BagModel(string color) : this()
        {
            this.color = color;
        }

        public void AddBags(BagModel bag, int count)
        {
            bag.count = count;
            this.innerBags.Add(bag);
        }

        public int CountBagsInside()
        {
            var count = 0;

            if (innerBags.Any())
            {
                foreach (var bag in innerBags)
                {
                    count += (bag.CountBagsInside() + 1) * bag.count;
                }
            }

            return count;
        }
    }
}