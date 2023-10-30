using FullInspector;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector.Assistance
{
	[ExecuteInEditMode]
	public class FK3_LinkageSpawnerScript : BaseBehavior<FullSerializerSerializer>
	{
		public FK3_CopySpawnerWorkshop workshop;

		protected override void Awake()
		{
			base.Awake();
			if (workshop == null)
			{
				workshop = new FK3_CopySpawnerWorkshop();
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
