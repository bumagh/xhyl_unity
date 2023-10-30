using UnityEngine;

namespace XJ.Unity3D.GUI
{
	public class BaseGUI
	{
		protected string title;

		protected bool boldTitle = true;

		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				title = value;
			}
		}

		public bool BoldTitle
		{
			get
			{
				return boldTitle;
			}
			set
			{
				boldTitle = value;
			}
		}

		public BaseGUI(string title = null, bool boldTitle = false)
		{
			this.title = title;
			this.boldTitle = boldTitle;
		}

		protected virtual void ShowTilte()
		{
			if (title != null)
			{
				if (boldTitle)
				{
					GUILayout.BoldLabel(title);
				}
				else
				{
					UnityEngine.GUILayout.Label(title);
				}
			}
		}

		public virtual void Display()
		{
			ShowTilte();
		}

		public virtual void Update()
		{
		}
	}
}
