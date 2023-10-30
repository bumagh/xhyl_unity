using FullInspector.Internal;
using System.Collections.Generic;
using UnityEngine;

namespace FullInspector.BackupService
{
	[AddComponentMenu("")]
	public class fiStorageComponent : MonoBehaviour, fiIEditorOnlyTag
	{
		public List<string> SeenBaseBehaviors = new List<string>();

		public List<fiSerializedObject> Objects = new List<fiSerializedObject>();

		public void RemoveInvalidBackups()
		{
			int num = 0;
			while (num < Objects.Count)
			{
				if (Objects[num].Target.Target == null)
				{
					Objects.RemoveAt(num);
				}
				else
				{
					num++;
				}
			}
		}
	}
}
