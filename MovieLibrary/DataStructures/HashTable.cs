public class HashTable<Tkey, TValue>
{
    private readonly Dictionary<Tkey, TValue> table = new();

    public void Add(Tkey key, TValue value)
    {
        if (table.ContainsKey(key))
        {
            throw new Exception("An item with the same key has already been added/Duplicate key");
            table[key] = value;
        }

        public TValue Get(Tkey key)
        {
            table.TryGetValue(key, out var value);
            return value;
        }

        
    }
}