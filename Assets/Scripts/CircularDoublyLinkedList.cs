using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CircularDoublyLinkedList<T> {

    public CircularDoublyLinkedListNode<T> first = new CircularDoublyLinkedListNode<T>();
    public CircularDoublyLinkedListNode<T> last = new CircularDoublyLinkedListNode<T>();
    public int size;

    public CircularDoublyLinkedList() {
        first.next = last;
        first.prev = last;
        //last = first;
        size = 0;
        last.next = first;
        last.prev = first;
    }

    /// <summary>
    /// Adds elem to the end of list
    /// </summary>
    /// <param name="elem"></param>
    public void Add(T elem) {
        if (size == 0) {
            first.Data = elem;
        } else if (size == 1) {
            last.Data = elem;
        } else {
            CircularDoublyLinkedListNode<T> newNode = new CircularDoublyLinkedListNode<T>(elem);
            newNode.prev = last;
            newNode.next = first;
            first.prev = newNode;
            last.next = newNode;
            last = newNode;
        }
        size++;
    }

    public CircularDoublyLinkedListNode<T> Find(T data) {
        CircularDoublyLinkedListNode<T> currNode = first;
        if (currNode.Data.Equals(data)) {
            return currNode;
        }
        while (currNode != last) {
            currNode = currNode.next;
            if (currNode.Data.Equals(data)) {
                return currNode;
            }
        }
        return null;
    }
}
