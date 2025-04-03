// 우선순위 큐 구현 (A* 알고리즘 성능 향상을 위해 사용)

using System.Collections.Generic;
using System.Linq;

public class PriorityQueue<T>
{
    private List<KeyValuePair<T, float>> elements = new List<KeyValuePair<T, float>>();

    public int Count
    {
        get { return elements.Count; }
    }

    public void Enqueue(T item, float priority)
    {
        elements.Add(new KeyValuePair<T, float>(item, priority));
        elements.Sort((a, b) => a.Value.CompareTo(b.Value)); // 낮은 우선순위가 먼저
    }

    public T Dequeue()
    {
        if (elements.Count == 0)
        {
            throw new System.InvalidOperationException("PriorityQueue is empty");
        }
        T item = elements[0].Key;
        elements.RemoveAt(0);
        return item;
    }

    public bool Contains(T item)
    {
        return elements.Any(element => EqualityComparer<T>.Default.Equals(element.Key, item));
    }

    public void UpdatePriority(T item, float newPriority)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            if (EqualityComparer<T>.Default.Equals(elements[i].Key, item))
            {
                elements[i] = new KeyValuePair<T, float>(item, newPriority);
                elements.Sort((a, b) => a.Value.CompareTo(b.Value));
                return;
            }
        }
    }
}