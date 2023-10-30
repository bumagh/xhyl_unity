using UnityEngine;

namespace FullInspector.Samples.ItemDatabase
{
	[AddComponentMenu("Full Inspector Samples/Other/ScriptableObjects")]
	public class ItemDatabaseBehavior : BaseBehavior<FullSerializerSerializer>
	{
		[ShowInInspector]
		[InspectorHidePrimary]
		[InspectorComment(CommentType.Info, "ScriptableObject references work as expected. With Full Inspector 2.4, you can even view them inline! Just click the dropdown.")]
		private int _comment;

		public ItemDatabaseSample ScriptableObject;
	}
}
