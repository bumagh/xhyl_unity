using System;

namespace XJ.Unity3D.GUI.FieldGUIComponents
{
	public class GUIAttribute : Attribute
	{
		public string Title
		{
			get;
			set;
		}

		public string TabTitle
		{
			get;
			set;
		}

		public string FoldoutTitle
		{
			get;
			set;
		}

		public float MaxValue
		{
			get;
			set;
		}

		public float MinValue
		{
			get;
			set;
		}

		public string[] EnumNames
		{
			get;
			set;
		}

		public bool IPv4
		{
			get;
			set;
		}

		public bool TabClear
		{
			get;
			set;
		}

		public bool FoldoutClear
		{
			get;
			set;
		}

		public GUIAttribute()
		{
			Title = null;
			TabTitle = null;
			FoldoutTitle = null;
			MaxValue = float.NaN;
			MinValue = float.NaN;
			EnumNames = null;
		}
	}
}
