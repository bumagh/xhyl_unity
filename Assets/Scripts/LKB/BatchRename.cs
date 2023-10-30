using System.IO;
using UnityEngine;

namespace LKB
{
	public class BatchRename : MonoBehaviour
	{
		public string strPath;

		public int count;

		private void Start()
		{
			Rename2(strPath);
		}

		private void Rename1(string path)
		{
			if (!Directory.Exists(path))
			{
				UnityEngine.Debug.Log("目录不存在: " + path);
				return;
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			FileInfo[] files = directoryInfo.GetFiles();
			for (int i = 0; i < files.Length; i++)
			{
				if (files[i].Extension == ".meta")
				{
					files[i].Delete();
					continue;
				}
				files[i].MoveTo(Path.Combine(path, count.ToString() + files[i].Extension));
				count++;
			}
		}

		private void Rename2(string path)
		{
			if (!Directory.Exists(path))
			{
				UnityEngine.Debug.Log("目录不存在: " + path);
				return;
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			FileInfo[] files = directoryInfo.GetFiles();
			for (int i = 0; i < files.Length; i++)
			{
				string name = files[i].Name;
				name = name.Replace("a", string.Empty);
				name = name.Replace("-", string.Empty);
				name = name.Replace(" ", string.Empty);
				name = name.Replace("#", string.Empty);
				name = name.Replace(".png", string.Empty);
				name = name.Substring(0, 2);
				name = name.Replace("j", string.Empty);
				name = (int.Parse(name) + count).ToString();
				files[i].MoveTo(Path.Combine(path, name + files[i].Extension));
			}
		}
	}
}
