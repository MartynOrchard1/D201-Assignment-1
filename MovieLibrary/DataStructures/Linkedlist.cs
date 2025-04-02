public class Node<T> // Node class for the Linkedlist
{
    public T Data;
    public Node<T> Next;

    public Node (T Data)
    {
        Data = data;
        Next = null;
    }

    public class LinkedList<T> 
    {
        private Node<T> head;

        public void Add(T data) // Add a new item to the list
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
        public List<T> ToList() // Conver linked list to normal list
        {
            List<T> list = new();
            Node<T> current = head;
            while (current != null)
            {
                list.Add(current.Data);
                current = current.Next;
            }
            return list;
        }
    }
}