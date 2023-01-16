using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static bool IsEqual<T>(this List<T> first, List<T> second)
    {
        if (first == null && second == null) {
            return true;
        }
 
        if (first == null || second == null || first.Count != second.Count) {
            return false;
        }
 
        for (var i = 0; i < first.Count; i++)
        {
            var comparer = EqualityComparer<T>.Default;
            if (!comparer.Equals(first[i], second[i])) {
                return false;
            }
        }
 
        return true;
    }
}
