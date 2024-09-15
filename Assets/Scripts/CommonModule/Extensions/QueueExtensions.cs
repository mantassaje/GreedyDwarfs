using System.Collections.Generic;

public static class QueueExtensions
{
    public static T DequeueOrDefault<T>(this Queue<T> queue)
    {
        if (queue.Count > 0)
        {
            return queue.Dequeue();
        }
        else
        {
            return default;
        }
    }
}