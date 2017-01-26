using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CircularDoublyLinkedListNode<T> {
    
    public CircularDoublyLinkedListNode<T> next;
    public CircularDoublyLinkedListNode<T> prev;

    private T data;

    public CircularDoublyLinkedListNode(T data) {
        this.data = data;
    }

    public CircularDoublyLinkedListNode() {
        
    }

    public T Data {
        get { return data; }
        set { data = value; }
    }

    //public bool Equals(T data) {
    //    return this.data.Equals(data);
    //}
}
