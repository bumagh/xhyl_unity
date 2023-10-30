using System.Reflection;
using UnityEngine;

namespace XJ.Unity3D.GUI.FieldGUIComponents
{
	public class UnsupportedGUI : FieldGUIComponent
	{
		public UnsupportedGUI(object data, FieldInfo fieldInfo, GUIAttribute guiAttribute)
			: base(data, fieldInfo, guiAttribute)
		{
		}

		public override void Show()
		{
			UnityEngine.GUILayout.BeginHorizontal();
			UnityEngine.GUILayout.Label("Unsupported Field : " + ToTitleCase(base.fieldInfo.Name));
			UnityEngine.GUILayout.EndHorizontal();
		}

		public override void Save()
		{
		}

		public override void Load()
		{
		}
	}
}
