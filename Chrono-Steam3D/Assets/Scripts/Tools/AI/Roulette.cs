using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roulette
{
    public T Run<T>(Dictionary<T, int> dic)
    {
        float total = 0;
        foreach (var item in dic)
        {
            total += item.Value;
        }
        float random = Random.Range(0, total);

        foreach (var item in dic)
        {
            random -= item.Value;
            if (random < 0)
            {
                return item.Key;
            }
        }
        return default(T);
    }
    /*run for value
     * public T Run<T>(Dictionary<int, T> dic)
    {
        float total = 0;
        foreach (var item in dic)
        {
            total += item.Key;
        }
        float random = Random.Range(0, total);

        foreach (var item in dic)
        {
            random -= item.Key;
            if (random < 0)
            {
                return item.Value;
            }
        }
        return default(T);
    }*/
}
