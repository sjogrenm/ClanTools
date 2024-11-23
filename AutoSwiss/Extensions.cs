namespace AutoSwiss;

internal static class Extensions
{
    public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> values)
    {
        foreach (var value in values)
        {
            set.Add(value);
        }
    }

    public static void AddOrUpdate<TK, TV>(this IDictionary<TK, TV> map, TK key, TV value, Func<TV, TV> updateFunc)
        where TK : notnull
    {
        if (!map.TryAdd(key, value))
        {
            map[key] = updateFunc(map[key]);
        }
    }

    public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
    {
        foreach (var value in values)
        {
            action(value);
        }
    }

    public static void RemoveIf<TK, TV>(this IDictionary<TK, TV> map, Func<TK, bool> predicate)
    {
        foreach (var k in map.Keys.Where(predicate).ToList())
        {
            map.Remove(k);
        }
    }

    public static bool NextBool(this Random rng)
    {
        return rng.Next(2) == 0;
    }
}
