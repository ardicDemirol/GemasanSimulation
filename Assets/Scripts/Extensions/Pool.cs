using System;
using System.Collections.Generic;

public readonly struct Pool<T>
{
    private readonly Queue<T> pool;
    private readonly Func<T> objectGenerator;

    public Pool(Func<T> objectGenerator, int initialSize)
    {
        pool = new();
        this.objectGenerator = objectGenerator;
        for (int i = 0; i < initialSize; i++)
        {
            T obj = objectGenerator();
            pool.Enqueue(obj);
        }
    }

    public readonly T GetObjectFromPool()
    {
        if (pool.Count == 0) return objectGenerator();
        else return pool.Dequeue();
    }

    public readonly void ReturnObjectToPool(T obj)
    {
        pool.Enqueue(obj);
    }
}


