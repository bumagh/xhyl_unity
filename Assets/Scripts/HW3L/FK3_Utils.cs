using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HW3L
{
	public class FK3_Utils
	{
		public static string GetLevelName(int level, string lang = "zh")
		{
			string[] array = new string[16]
			{
				"新手",
				"学徒",
				"新秀",
				"新贵",
				"高手",
				"达人",
				"精英",
				"专家",
				"大师",
				"宗师",
				"盟主",
				"传奇",
				"赌王",
				"赌圣",
				"赌神",
				"至尊"
			};
			string[] array2 = new string[16]
			{
				"Novice",
				"Trainee",
				"Talent",
				"Upstart",
				"Ace",
				"Artist",
				"Elite",
				"Expert",
				"Master",
				"Godfather",
				"Chief",
				"Legend",
				"King",
				"Saint",
				"God",
				"Extreme"
			};
			if (level == 0)
			{
				if (lang == "en")
				{
					return array2[0];
				}
				return array[0];
			}
			if (lang == "zh")
			{
				return array[level - 1];
			}
			return array2[level - 1];
		}

		public static void TrySetActive(GameObject go, bool active)
		{
			if (go != null)
			{
				go.SetActive(active);
			}
		}

		public static void TrySetText(Text text, string str)
		{
			if (text != null)
			{
				text.text = str;
			}
		}

		public static int[,] GetRandomMatrix3x5()
		{
			int[,] array = new int[3, 5];
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					array[i, j] = UnityEngine.Random.Range(1, 10);
				}
			}
			return array;
		}

		public static string PrintIntArray(int[] array, string separator = ", ")
		{
			string str = "[";
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				str += ((i == num - 1) ? array[i].ToString() : (array[i] + separator));
			}
			return str + "]";
		}

		public static string PrintInt2DArray(int[,] array, bool multiLine = false)
		{
			string str = multiLine ? "[\n" : "[";
			int length = array.Length;
			for (int i = 0; i <= array.GetUpperBound(0); i++)
			{
				string str2 = multiLine ? "  [" : "[";
				for (int j = 0; j <= array.GetUpperBound(1); j++)
				{
					str2 += ((j == array.GetUpperBound(1)) ? array[i, j].ToString() : (array[i, j] + ", "));
				}
				str2 += "]";
				str += str2;
				str += ((i == array.GetUpperBound(0)) ? string.Empty : ",");
				str += (multiLine ? "\n" : string.Empty);
			}
			return str + "]";
		}

		public static string PrintInt2DJaggedArray(int[][] array, bool multiLine = false)
		{
			string str = multiLine ? "[\n" : "[";
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				string str2 = multiLine ? "  [" : "[";
				int[] array2 = array[i];
				int num2 = array2.Length;
				for (int j = 0; j < num2; j++)
				{
					str2 += ((j == num2 - 1) ? array2[j].ToString() : (array2[j] + ", "));
				}
				str2 += "]";
				str += str2;
				str += ((i == num - 1) ? string.Empty : ",");
				str += (multiLine ? "\n" : string.Empty);
			}
			return str + "]";
		}

		public static int[][] ConvertInt2DArrayToJagged(int[,] inArray)
		{
			int length = inArray.GetLength(0);
			int length2 = inArray.GetLength(1);
			int[][] array = new int[length][];
			for (int i = 0; i < length; i++)
			{
				int[] array2 = new int[length2];
				for (int j = 0; j < length2; j++)
				{
					array2[j] = inArray[i, j];
				}
				array[i] = array2;
			}
			return array;
		}

		public static int[,] ConvertIntJaggedTo2DArray(int[][] inArray, int dimension0Size, int dimension1Size)
		{
			int[,] array = new int[dimension0Size, dimension1Size];
			for (int i = 0; i < dimension0Size; i++)
			{
				for (int j = 0; j < dimension1Size; j++)
				{
					array[i, j] = inArray[i][j];
				}
			}
			return array;
		}

		public static IEnumerator DelayCall(float delay, Action action)
		{
			if (delay > 0f)
			{
				yield return new WaitForSeconds(delay);
			}
			action();
		}

		public static IEnumerator WaitCall(Func<bool> judge, Action action)
		{
			while (!judge())
			{
				yield return null;
			}
			action();
		}

		public static T[] ConvertObjectToArray<T>(object obj)
		{
			object[] array = obj as object[];
			int num = array.Length;
			T[] array2 = new T[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = (T)array[i];
			}
			return array2;
		}

		public static T[][] ConvertObjectToJaggedArray<T>(object obj)
		{
			object[] array = obj as object[];
			int num = array.Length;
			T[][] array2 = new T[num][];
			for (int i = 0; i < num; i++)
			{
				object[] array3 = array[i] as object[];
				int num2 = array3.Length;
				array2[i] = new T[num2];
				for (int j = 0; j < num2; j++)
				{
					array2[i][j] = (T)array3[j];
				}
			}
			return array2;
		}

		public static T[,] ConvertObjectTo2DArray<T>(object obj, int innerSize)
		{
			object[] array = obj as object[];
			int num = array.Length;
			T[,] array2 = new T[num, innerSize];
			for (int i = 0; i < num; i++)
			{
				object[] array3 = array[i] as object[];
				int num2 = array3.Length;
				if (num2 != innerSize)
				{
					UnityEngine.Debug.LogError("innerSize is not correct!");
				}
				for (int j = 0; j < innerSize; j++)
				{
					array2[i, j] = (T)array3[j];
				}
			}
			return array2;
		}

		public static IEnumerator WaitFrameAction(int frame, Action action)
		{
			yield return frame;
			action();
		}

		public static IEnumerator WaitSecondsAction(float seconds, Action action)
		{
			yield return new WaitForSeconds(seconds);
			action();
		}

		public static IEnumerator WaitSecondsAction(float seconds, Action<int> action, int param1, FK3_XWait xwait = null)
		{
			if (xwait == null)
			{
				yield return new WaitForSeconds(seconds);
			}
			else
			{
				yield return xwait.WaitForSeconds(seconds);
			}
			action(param1);
		}
	}
}
