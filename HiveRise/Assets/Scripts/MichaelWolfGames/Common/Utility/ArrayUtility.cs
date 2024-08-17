using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MichaelWolfGames.Utility
{
	public static class ArrayUtility
	{
        public static T[] Add<T>(this T[] arr, T val)
        {
            T[] objArray = new T[arr.Length + 1];
            Array.ConstrainedCopy((Array)arr, 0, (Array)objArray, 0, arr.Length);
            objArray[arr.Length] = val;
            return objArray;
        }

        public static T[] AddRange<T>(this T[] arr, T[] val)
        {
            T[] objArray = new T[arr.Length + val.Length];
            Array.ConstrainedCopy((Array)arr, 0, (Array)objArray, 0, arr.Length);
            Array.ConstrainedCopy((Array)val, 0, (Array)objArray, arr.Length, val.Length);
            return objArray;
        }

        public static T[] Remove<T>(this T[] arr, T val)
        {
            List<T> objList = new List<T>((IEnumerable<T>)arr);
            objList.Remove(val);
            return objList.ToArray();
        }

        public static T[] Remove<T>(this T[] arr, IEnumerable<T> val)
        {
            return ((IEnumerable<T>)arr).Except<T>(val).ToArray<T>();
        }

        public static T[] RemoveAt<T>(this T[] arr, int index)
        {
            T[] objArray = new T[arr.Length - 1];
            int index1 = 0;
            for (int index2 = 0; index2 < arr.Length; ++index2)
            {
                if (index2 != index)
                {
                    objArray[index1] = arr[index2];
                    ++index1;
                }
            }
            return objArray;
        }

        public static T[] RemoveAt<T>(this IList<T> list, IEnumerable<int> indices)
        {
            List<int> intList = new List<int>(indices);
            intList.Sort();
            return list.SortedRemoveAt<T>((IList<int>)intList);
        }

        public static T[] SortedRemoveAt<T>(this IList<T> list, IList<int> sorted_indices)
        {
            int count1 = sorted_indices.Count;
            int count2 = list.Count;
            T[] objArray = new T[count2 - count1];
            int index1 = 0;
            for (int index2 = 0; index2 < count2; ++index2)
            {
                if (index1 < count1 && sorted_indices[index1] == index2)
                {
                    while (index1 < count1 && sorted_indices[index1] == index2)
                        ++index1;
                }
                else
                    objArray[index2 - index1] = list[index2];
            }
            return objArray;
        }
    }
}