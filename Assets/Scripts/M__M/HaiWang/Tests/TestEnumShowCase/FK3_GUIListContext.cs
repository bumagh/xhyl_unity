using System;

namespace M__M.HaiWang.Tests.TestEnumShowCase
{
	public class FK3_GUIListContext
	{
		public string[] itemNames;

		public Action<int> onItemClick;

		public Action onSetItems;

		public void SetItems(string[] itemNames)
		{
			this.itemNames = itemNames;
			if (onSetItems != null)
			{
				onSetItems();
			}
		}
	}
}
