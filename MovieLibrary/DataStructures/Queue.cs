public class Queue<T>
{
    private readonly System.Collections.Generic.Queue<T> queue = new();
    
    // Add to the queue
    public void Enqueue(T item) => queue.Enqueue(item);

    // Remove from the queue
    public T Dequeue() => queue.Count > 0 ? queue.Dequeue() : default;

    // Check if queue is empty
    public bool isEmpty() => queue.Count == 0;

    public int Count() => queue.Count;
}