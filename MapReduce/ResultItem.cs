namespace MapReduce
{
    public class ResultItem
    {
        public string Word { get; set; }
        public int Value { get; set; }

        public ResultItem(string word, int value)
        {
            Word = word;
            Value = value;
        }
    }
}