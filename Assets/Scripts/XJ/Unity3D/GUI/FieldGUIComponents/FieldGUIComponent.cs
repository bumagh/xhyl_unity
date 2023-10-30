using System;
using System.Reflection;

namespace XJ.Unity3D.GUI.FieldGUIComponents
{
	public abstract class FieldGUIComponent
	{
		public object data
		{
			get;
			private set;
		}

		public FieldInfo fieldInfo
		{
			get;
			private set;
		}

		public GUIAttribute guiAttribute
		{
			get;
			private set;
		}

		public FieldGUIComponent(object data, FieldInfo fieldInfo)
			: this(data, fieldInfo, Attribute.GetCustomAttribute(fieldInfo, typeof(GUIAttribute)) as GUIAttribute)
		{
		}

		public FieldGUIComponent(object data, FieldInfo fieldInfo, GUIAttribute guiAttribute)
		{
			this.data = data;
			this.fieldInfo = fieldInfo;
			this.guiAttribute = guiAttribute;
		}

		public abstract void Show();

		public abstract void Save();

		public abstract void Load();

		public string ToTitleCase(string text)
		{
			return char.ToUpper(text[0]) + text.Substring(1);
		}
	}
}
