using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FullInspector
{
	public static class fiListUtility
	{
		private static T GetDefault<T>()
		{
			if (typeof(T) == typeof(string))
			{
				return (T)(object)string.Empty;
			}
			return default(T);
		}

		public static void Add(ref IList list, Type elementType)
		{
			if (list.GetType().IsArray)
			{
				Array array = Array.CreateInstance(elementType, list.Count + 1);
				for (int i = 0; i < list.Count; i++)
				{
					array.SetValue(list[i], i);
				}
				list = array;
			}
			else
			{
				UnityEngine.Debug.Log("Adding " + elementType + " to " + list);
				list.Add(InspectedType.Get(elementType).CreateInstance());
			}
		}

		public static void RemoveAt(ref IList list, Type elementType, int index)
		{
			if (list.GetType().IsArray)
			{
				Array array = Array.CreateInstance(elementType, list.Count - 1);
				int num = 0;
				for (int i = 0; i < list.Count - 1; i++)
				{
					if (i != index)
					{
						array.SetValue(list[i], num++);
					}
				}
				list = array;
			}
			else
			{
				list.RemoveAt(index);
			}
		}

		public static void Add<T>(ref IList list)
		{
			if (list.GetType().IsArray)
			{
				T[] array = (T[])list;
				Array.Resize(ref array, array.Length + 1);
				list = array;
			}
			else
			{
				list.Add(GetDefault<T>());
			}
		}

		public static void InsertAt<T>(ref IList list, int index)
		{
			if (list.GetType().IsArray)
			{
				List<T> list2 = new List<T>((IList<T>)list);
				list2.Insert(index, GetDefault<T>());
				list = list2.ToArray();
			}
			else
			{
				list.Insert(index, GetDefault<T>());
			}
		}

		public static void RemoveAt<T>(ref IList list, int index)
		{
			if (list.GetType().IsArray)
			{
				List<T> list2 = new List<T>((IList<T>)list);
				list2.RemoveAt(index);
				list = list2.ToArray();
			}
			else
			{
				list.RemoveAt(index);
			}
		}
	}
}
