using System.Collections.Generic;
using UnityEngine;

namespace FullInspector.Samples.DatabaseEditor
{
	[AddComponentMenu("Full Inspector Samples/Other/Database Behavior")]
	public class DatabaseBehavior : BaseBehavior<FullSerializerSerializer>
	{
		[ShowInInspector]
		[InspectorComment(CommentType.Info, "If you have a huge collection of items, for example, a set of skills, then the [SingleItemListEditor] attribute can be extremely useful. It activates an editor that only shows you one item at a time.")]
		[InspectorHidePrimary]
		private int _comment;

		[InspectorDatabaseEditor]
		public List<Ability> Abilities;
	}
}
