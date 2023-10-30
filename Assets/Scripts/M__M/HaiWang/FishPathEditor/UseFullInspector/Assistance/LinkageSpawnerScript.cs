using FullInspector;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector.Assistance
{
	[ExecuteInEditMode]
	public class LinkageSpawnerScript : BaseBehavior<FullSerializerSerializer>
	{
		public CopySpawnerWorkshop workshop;

		protected override void Awake()
		{
			base.Awake();
			if (workshop == null)
			{
				workshop = new CopySpawnerWorkshop();
			}
			if (workshop.root == null)
			{
				workshop.root = base.transform;
			}
			workshop.Awake();
		}

		private void Start()
		{
		}

		private void AOTSupportHelp()
		{
		}
	}
}
