public class Node<T> 
{
    public T Data { get; set; } 
    public Node<T>? Next { get; set; }

    public Node(T data)
    {
        Data = data; 
        Next = null; 
    }
}

public class LinkedList<T> 
{
    private Node<T>? head; 

    public void Add(T data) 
    {
        Node<T> newNode = new Node<T>(data);
        if (head == null)
        {
            head = newNode;
        }
        else
        {
            Node<T> current = head;
            while (current.Next != null)
            {
                current = current.Next;
            }
            current.Next = newNode;
        }
    }

    public List<T> ToList() 
    {
        List<T> list = new();
        Node<T>? current = head;
        while (current != null)
        {
            list.Add(current.Data);
            current = current.Next;
        }
        return list;
    }
}