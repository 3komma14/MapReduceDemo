namespace MapReduce
{
    public class WordItem
    {
        public string Key { get; set; }
        public int Value { get; set; }

        public WordItem(string key, int value)
        {
            Key = key;
            Value = value;
        }
    }
}