using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FK3_GraphLogger : MonoBehaviour
{
	private class Line
	{
		public bool isNumber;

		public SortedDictionary<int, string> strings;

		public SortedDictionary<int, float> points;

		private int minTime;

		private int maxTime;

		private float minPoint;

		private float maxPoint;

		private float xSize;

		private float ySize;

		private List<int> keys = new List<int>();

		public Line(int t, float x)
		{
			isNumber = true;
			minPoint = x;
			maxPoint = x;
			minTime = t;
			maxTime = t;
			xSize = graphLogger.size.x;
			ySize = graphLogger.size.y;
			points = new SortedDictionary<int, float>();
			AddFloat(t, x);
		}

		public Line(int t, string s)
		{
			isNumber = false;
			strings = new SortedDictionary<int, string>();
			AddString(t, s);
		}

		public void AddFloat(int t, float x)
		{
			if (points.ContainsKey(t))
			{
				points.Remove(t);
			}
			else
			{
				keys.Add(t);
			}
			points.Add(t, x);
			if (keys.Count > graphLogger.maxCount)
			{
				minTime = keys[keys.Count - graphLogger.maxCount];
				float num = points[keys[keys.Count - graphLogger.maxCount]];
				if (minPoint == num)
				{
					minPoint = maxPoint;
					for (int i = keys.Count - graphLogger.maxCount + 1; i < keys.Count; i++)
					{
						if (points[keys[i]] < minPoint)
						{
							minPoint = points[keys[i]];
						}
					}
					if (minPoint != maxPoint)
					{
						ySize = graphLogger.size.y / (maxPoint - minPoint);
					}
				}
				else if (maxPoint == num)
				{
					maxPoint = minPoint;
					for (int j = keys.Count - graphLogger.maxCount + 1; j < keys.Count; j++)
					{
						if (points[keys[j]] > maxPoint)
						{
							maxPoint = points[keys[j]];
						}
					}
					if (minPoint != maxPoint)
					{
						ySize = graphLogger.size.y / (maxPoint - minPoint);
					}
				}
			}
			maxTime = t;
			if (maxTime != minTime)
			{
				xSize = graphLogger.size.x / (float)(maxTime - minTime);
			}
			if (x < minPoint)
			{
				minPoint = x;
				if (minPoint != maxPoint)
				{
					ySize = graphLogger.size.y / (maxPoint - minPoint);
				}
			}
			else if (x > maxPoint)
			{
				maxPoint = x;
				if (minPoint != maxPoint)
				{
					ySize = graphLogger.size.y / (maxPoint - minPoint);
				}
			}
		}

		public void AddString(int t, string s)
		{
			strings.Add(t, s);
			keys.Add(t);
		}

		public float GetX(int i)
		{
			if (keys.Count > graphLogger.maxCount)
			{
				i += keys.Count - graphLogger.maxCount;
			}
			return ((float)(keys[i] - minTime) * xSize + graphLogger.offset.x) * graphLogger.xScale;
		}

		public float GetY(int i)
		{
			if (keys.Count > graphLogger.maxCount)
			{
				i += keys.Count - graphLogger.maxCount;
			}
			return ((points[keys[i]] - minPoint) * ySize + graphLogger.offset.y) * graphLogger.yScale;
		}
	}

	public enum Format
	{
		Column,
		Table
	}

	public static FK3_GraphLogger graphLogger;

	public bool appendLogFile;

	public Format format = Format.Table;

	public bool graphOn = true;

	public int maxCount = 6000;

	public Color[] colors = new Color[6]
	{
		Color.cyan,
		Color.green,
		Color.magenta,
		Color.red,
		Color.yellow,
		Color.blue
	};

	public Vector2 size = new Vector2(300f, 150f);

	public Vector2 offset = new Vector2(10f, 10f);

	private Material mat;

	private Dictionary<string, Line> lines = new Dictionary<string, Line>();

	private float xScale;

	private float yScale;

	private void Awake()
	{
		if (graphLogger == null)
		{
			graphLogger = this;
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
		xScale = 1f / (float)Screen.width;
		yScale = 1f / (float)Screen.height;
		mat = new Material("Shader \"Lines/Colored Blended\" {SubShader { Pass {     Blend SrcAlpha OneMinusSrcAlpha     ZWrite Off Cull Off Fog { Mode Off }     BindChannels {      Bind \"vertex\", vertex Bind \"color\", color }} } }");
		mat.hideFlags = HideFlags.HideAndDontSave;
		mat.shader.hideFlags = HideFlags.HideAndDontSave;
	}

	public static void AddPoint(int point)
	{
		AddPoint((float)point, "default");
	}

	public static void AddPoint(int point, string name)
	{
		AddPoint((float)point, name);
	}

	public static void AddPoint(float point)
	{
		AddPoint(point, "default");
	}

	public static void AddPoint(float point, string name)
	{
		int frameCount = Time.frameCount;
		if (!graphLogger.lines.ContainsKey(name))
		{
			graphLogger.lines.Add(name, new Line(frameCount, point));
		}
		else
		{
			graphLogger.lines[name].AddFloat(frameCount, point);
		}
	}

	public static void AddPoint(string point)
	{
		AddPoint(point, "default");
	}

	public static void AddPoint(string point, string name)
	{
		int frameCount = Time.frameCount;
		if (!graphLogger.lines.ContainsKey(name))
		{
			graphLogger.lines.Add(name, new Line(frameCount, point));
		}
		else
		{
			graphLogger.lines[name].AddString(frameCount, point);
		}
	}

	public static void Clear()
	{
		graphLogger.lines.Clear();
	}

	private void OnApplicationQuit()
	{
		Application.CancelQuit();
		UnityEngine.Object.DestroyImmediate(mat);
		SaveLog();
		Application.Quit();
	}

	private void OnApplicationPause(bool paused)
	{
		if (paused)
		{
			SaveLog();
		}
	}

	private void OnApplicationFocus(bool paused)
	{
		if (paused)
		{
			SaveLog();
		}
	}

	public static void SaveLog()
	{
		if (graphLogger.lines.Count != 0)
		{
			try
			{
				using (StreamWriter file = new StreamWriter(Application.persistentDataPath + "/log.csv", graphLogger.appendLogFile))
				{
					if (graphLogger.format == Format.Column)
					{
						graphLogger.writeColumn(file);
					}
					else if (graphLogger.format == Format.Table)
					{
						graphLogger.writeTable(file);
					}
					UnityEngine.Debug.Log("Log file saved: " + Application.persistentDataPath + "/log.csv");
				}
			}
			catch
			{
				UnityEngine.Debug.LogError("Could not save to path: " + Application.persistentDataPath + "/log.csv");
			}
		}
	}

	private void writeColumn(StreamWriter file)
	{
		foreach (KeyValuePair<string, Line> line in lines)
		{
			file.WriteLine("Frame," + line.Key);
			Line value = line.Value;
			if (value.isNumber)
			{
				foreach (KeyValuePair<int, float> point in value.points)
				{
					file.WriteLine(point.Key + "," + point.Value);
				}
			}
			else
			{
				foreach (KeyValuePair<int, string> @string in value.strings)
				{
					file.WriteLine(@string.Key + "," + @string.Value);
				}
			}
			file.WriteLine();
		}
	}

	private void writeTable(StreamWriter file)
	{
		SortedDictionary<int, string> sortedDictionary = new SortedDictionary<int, string>();
		foreach (KeyValuePair<string, Line> line in lines)
		{
			Line value = line.Value;
			if (value.isNumber)
			{
				foreach (int key in value.points.Keys)
				{
					if (!sortedDictionary.ContainsKey(key))
					{
						sortedDictionary.Add(key, string.Empty);
					}
				}
			}
			else
			{
				foreach (int key2 in value.strings.Keys)
				{
					if (!sortedDictionary.ContainsKey(key2))
					{
						sortedDictionary.Add(key2, string.Empty);
					}
				}
			}
		}
		List<int> list = new List<int>(sortedDictionary.Keys);
		foreach (KeyValuePair<string, Line> line2 in lines)
		{
			foreach (int item in list)
			{
				SortedDictionary<int, string> sortedDictionary2 = new SortedDictionary<int, string>();
				int num = 0;
				(sortedDictionary2 = sortedDictionary)[num = item] = sortedDictionary2[num] + ",";
			}
			Line value2 = line2.Value;
			if (value2.isNumber)
			{
				foreach (KeyValuePair<int, float> point in value2.points)
				{
					if (sortedDictionary.ContainsKey(point.Key))
					{
						SortedDictionary<int, string> sortedDictionary3 = new SortedDictionary<int, string>();
						int num2 = 0;
						(sortedDictionary3 = sortedDictionary)[num2 = point.Key] = sortedDictionary3[num2] + point.Value.ToString();
					}
				}
			}
			else
			{
				foreach (KeyValuePair<int, string> @string in value2.strings)
				{
					if (sortedDictionary.ContainsKey(@string.Key))
					{
						SortedDictionary<int, string> sortedDictionary4 = new SortedDictionary<int, string>();
						int num3 = 0;
						(sortedDictionary4 = sortedDictionary)[num3 = @string.Key] = sortedDictionary4[num3] + @string.Value.ToString();
					}
				}
			}
		}
		file.Write("Frame");
		foreach (KeyValuePair<string, Line> line3 in lines)
		{
			file.Write("," + line3.Key);
		}
		file.WriteLine();
		foreach (KeyValuePair<int, string> item2 in sortedDictionary)
		{
			file.WriteLine(item2.Key + item2.Value);
		}
		file.WriteLine();
	}

	private void OnPostRender()
	{
		if (graphOn)
		{
			GL.PushMatrix();
			mat.SetPass(0);
			GL.LoadOrtho();
			GL.Begin(1);
			int num = 0;
			foreach (KeyValuePair<string, Line> line in lines)
			{
				Line value = line.Value;
				if (!value.isNumber)
				{
					break;
				}
				float x = value.GetX(0);
				float y = value.GetY(0);
				GL.Color(colors[num % 6]);
				for (int i = 1; i < value.points.Count && i < maxCount; i++)
				{
					float x2 = value.GetX(i);
					float y2 = value.GetY(i);
					GL.Vertex3(x, y, 0f);
					GL.Vertex3(x2, y2, 0f);
					x = x2;
					y = y2;
				}
				num++;
			}
			GL.End();
			GL.PopMatrix();
		}
	}
}
