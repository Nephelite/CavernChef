using System;
using System.Collections.Generic;

public static class ListShuffler
{
    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> x)
    {
        int n = x.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = x[k];
            x[k] = x[n];
            x[n] = value;
        }
    }
}
