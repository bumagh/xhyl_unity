using FullInspector;
using M__M.HaiWang.Fish;
using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FK3_FishFormationMgr : FK3_FormationMgr<FK3_FishType>
	{
		protected override void Awake()
		{
			base.Awake();
		}

		[InspectorButton]
		private void Test_炸弹蟹_Formations()
		{
			StopAllCoroutines();
			StartCoroutine(IE_Test_炸弹蟹_Formations());
		}

		private IEnumerator IE_Test_炸弹蟹_Formations()
		{
			Vector3[] posArr = new Vector3[4]
			{
				new Vector3(0f, 3f, 0f),
				new Vector3(3f, 0f, 0f),
				new Vector3(0f, -3f, 0f),
				new Vector3(-3f, 0f, 0f)
			};
			Vector3[] array = posArr;
			foreach (Vector3 pos in array)
			{
				PlayFormation(new FK3_FormationPlayParam<FK3_FishType>
				{
					formationId = 101,
					hasOffset = true,
					offset = pos
				});
				yield return new WaitForSeconds(1f);
			}
		}
	}
}
