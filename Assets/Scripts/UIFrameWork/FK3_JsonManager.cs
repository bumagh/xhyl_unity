using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrameWork
{
	public class FK3_JsonManager
	{
		[Serializable]
		public class FK3_KeyValueInfo
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
			FK3_KeyValueInfo fK3_KeyValueInfo = JsonUtility.FromJson<FK3_KeyValueInfo>(textAsset.text);
			for (int i = 0; i < fK3_KeyValueInfo.keyValueNodes.Count; i++)
			{
				dicValues.Add(fK3_KeyValueInfo.keyValueNodes[i].key, fK3_KeyValueInfo.keyValueNodes[i].value);
			}
			return dicValues;
		}
	}
}
