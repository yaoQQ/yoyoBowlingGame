using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageQueues<T> {

    private object locker = new object();
    private Queue<T> queue = new Queue<T>();

    public void Enqueue(T t)
    {
        lock (this.locker)
        {
            this.queue.Enqueue(t);
        }
    }

    public T Dequeue()
    {
        lock (this.locker)
        {
            return this.queue.Dequeue();
        }
    }

    public int Count { get { return this.queue.Count; } }
}
