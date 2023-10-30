using System;
using UnityEngine;

namespace XJ.Unity3D.GUI
{
	public class FlexibleWindow
	{
		private float lastShowTime;

		public string Title
		{
			get;
			set;
		}

		public float MinWidth
		{
			get;
			set;
		}

		public float MinHeight
		{
			get;
			set;
		}

		public float MaxWidth
		{
			get;
			set;
		}

		public float MaxHeight
		{
			get;
			set;
		}

		public bool EnableDrag
		{
			get;
			set;
		}

		public bool IsVisible
		{
			get;
			set;
		}

		public int WindowID
		{
			get;
			private set;
		}

		public Rect WindowRect
		{
			get;
			private set;
		}

		public FlexibleWindow(string title = null, float x = 0f, float y = 0f, float minWidth = 0f, float minHeight = 0f, float maxWidth = float.MaxValue, float maxHeight = float.MaxValue, bool enableDrag = true, bool isVisible = false)
		{
			Title = title;
			MinWidth = minWidth;
			MinHeight = minHeight;
			MaxWidth = maxWidth;
			MaxHeight = maxHeight;
			EnableDrag = enableDrag;
			IsVisible = isVisible;
			WindowID = Guid.NewGuid().GetHashCode();
			WindowRect = new Rect(x, y, 0f, 0f);
		}

		public Rect Show(UnityEngine.GUI.WindowFunction windowFunction)
		{
			if (!IsVisible)
			{
				Vector2 position = WindowRect.position;
				float x = position.x;
				Vector2 position2 = WindowRect.position;
				return new Rect(x, position2.y, 0f, 0f);
			}
			if (EnableDrag)
			{
				windowFunction = (UnityEngine.GUI.WindowFunction)Delegate.Combine(windowFunction, (UnityEngine.GUI.WindowFunction)delegate
				{
					UnityEngine.GUI.DragWindow();
				});
			}
			if (lastShowTime != Time.timeSinceLevelLoad)
			{
				WindowRect = new Rect(WindowRect.x, WindowRect.y, 0f, 0f);
				lastShowTime = Time.timeSinceLevelLoad;
			}
			WindowRect = UnityEngine.GUILayout.Window(WindowID, WindowRect, windowFunction, Title, UnityEngine.GUILayout.MinWidth(MinWidth), UnityEngine.GUILayout.MinHeight(MinHeight), UnityEngine.GUILayout.MaxWidth(MaxWidth), UnityEngine.GUILayout.MaxHeight(MaxHeight));
			return WindowRect;
		}
	}
}
