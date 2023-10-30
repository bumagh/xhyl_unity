using System.Collections.Generic;

namespace FullInspector.Samples.ItemDatabase
{
	public class ItemDatabaseSample : BaseScriptableObject<FullSerializerSerializer>
	{
		public Dictionary<string, IItem> Items;
	}
}
