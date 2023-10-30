using System.Collections.Generic;

namespace XJ.Unity3D.GUI
{
	public class ViewContainer
	{
		private class ElementItem
		{
			public object element;

			public bool visible;

			public bool interactive;
		}

		private List<ElementItem> m_items = new List<ElementItem>();

		public static ViewContainer New()
		{
			return new ViewContainer();
		}

		public ViewContainer Use(params object[] elements)
		{
			foreach (object element in elements)
			{
				ElementItem elementItem = new ElementItem();
				elementItem.element = element;
				elementItem.visible = true;
			}
			return this;
		}

		public void OnGUI()
		{
			foreach (ElementItem item in m_items)
			{
			}
		}
	}
}
