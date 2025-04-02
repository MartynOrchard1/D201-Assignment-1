public class Queue<T>
{
    private readonly System.Collections.Generic.Queue<T> queue = new();
    
    public void Enqueue(T item) => queue.Enqueue(item);

    public T Dequeue() => queue.Count > 0 ? queue.Dequeue() : default;

    public bool isEmpty() => queue.Count == 0;

    public int Count() => queue.Count;
}