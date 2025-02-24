using System.Collections.Generic;
using UnityEngine;

namespace CGT.Collections
{
    public static class IListExtensions
    {
        public static void AddRange<T>(this IList<T> toAddTo, IList<T> whatToAdd)
        {
            for (int i = 0; i < whatToAdd.Count; i++)
            {
                toAddTo.Add(whatToAdd[i]);
            }
        }

        public static void RemoveAllIn<T>(this IList<T> toRemoveFrom, IList<T> whatToRemove)
        {
            foreach (T item in whatToRemove)
            {
                toRemoveFrom.Remove(item);
            }
        }

        public static IList<T> ReversedCopy<T>(this IList<T> baseList)
        {
            IList<T> result = new List<T>();

            for (int i = baseList.Count - 1; i >= 0; i--)
            {
                result.Add(baseList[i]);
            }

            return result;
        }

        public static T GetRandom<T>(this IList<T> baseList)
        {
            int index = Random.Range(0, baseList.Count);
            T result = baseList[index];
            return result;
        }
    }
}