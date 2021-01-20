using System.Collections.Generic;

namespace AdventOfCode.Day16.Models
{
    public class TicketModel
    {
        public List<int> numberList;
        public bool valid;

        public TicketModel(List<int> numberList = null, bool valid = true)
        {
            this.numberList = numberList;
            this.valid = valid;
        }
    }
}