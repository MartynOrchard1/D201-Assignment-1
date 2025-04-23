namespace MovieLibrary.DataStructures;

public class HashTable<TKey, TValue> where TKey : notnull // Hash table to search movies via ID
{
    private readonly Dictionary<TKey, TValue> table = new();

    public void Add(TKey key, TValue value) // Add the key-value pair
    {
        if (table.ContainsKey(key))
        {
            throw new Exception("An item with the same key has already been added/Duplicate key");
        }
        table[key] = value; // Correct placement of this line
    }

    public TValue Get(TKey key) // Get value via key
    {
        table.TryGetValue(key, out var value);
        return value;
    }

    public bool ContainsKey(TKey key) => table.ContainsKey(key);

    public List<TValue> GetAllValues() => table.Values.ToList();
}