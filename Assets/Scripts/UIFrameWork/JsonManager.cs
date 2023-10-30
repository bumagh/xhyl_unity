using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrameWork
{
	public class JsonManager
	{
		[Serializable]
		public class KeyValueInfo
		{
			public List<KeyValueNode> keyValueNodes = new List<KeyValueNode>();
		}

		[Serializable]
		public class KeyValueNode
		{
			public string key = string.Empty;

			public string value = string.Empty;
		}

		private Dictionary<string, string> dicValues = new Dictionary<string, string>();

		public Dictionary<string, string> LoadJsonData(string path)
		{
			dicValues.Clear();
			TextAsset textAsset = Resources.Load(path) as TextAsset;
			KeyValueInfo keyValueInfo = JsonUtility.FromJson<KeyValueInfo>(textAsset.text);
			for (int i = 0; i < keyValueInfo.keyValueNodes.Count; i++)
			{
				dicValues.Add(keyValueInfo.keyValueNodes[i].key, keyValueInfo.keyValueNodes[i].value);
			}
			return dicValues;
		}
	}
}
