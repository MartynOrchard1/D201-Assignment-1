public class HashTable<Tkey, TValue> // Hash table to search movies via ID
{
    private readonly Dictionary<Tkey, TValue> table = new();

    public void Add(Tkey key, TValue value) // Add the key-value pair
    {
        if (table.ContainsKey(key))
        {
            throw new Exception("An item with the same key has already been added/Duplicate key");
            table[key] = value;
        }

        public TValue Get(Tkey key) // Get value via key
        {
            table.TryGetValue(key, out var value);
            return value;
        }

        public bool ContainsKey(Tkey key) => table.ContainsKey(key);
        
        public List<TValue> GetAllValues() => table.Values.ToList();
    }
}