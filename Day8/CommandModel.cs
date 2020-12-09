namespace AdventOfCode.Day8.Models
{
    public class CommandModel
    {
        public string type;
        public int value;
        public bool executed;

        public CommandModel()
        {
            this.executed = false;
            this.type = string.Empty;
            this.value = 0;
        }

        public CommandModel(string type, int value) : this()
        {
            this.type = type;
            this.value = value;
        }
    }
}