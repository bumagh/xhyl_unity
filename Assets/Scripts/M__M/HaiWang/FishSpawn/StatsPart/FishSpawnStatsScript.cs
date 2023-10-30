using FullInspector;
using M__M.HaiWang.Fish;
using M__M.HaiWang.FishPathEditor.UseFullInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.FishSpawn.StatsPart
{
	public class FishSpawnStatsScript : BaseBehavior<FullSerializerSerializer>
	{
		private FormationStats stats = new FormationStats();

		private bool isRunning => Application.isPlaying;

		protected override void Awake()
		{
			base.Awake();
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		[InspectorButton]
		private void Run_FormationSpawnFishStats()
		{
			StartCoroutine(IE_PlayFormationForStats());
		}

		private void Run_PathSpawnFishStats()
		{
		}

		private IEnumerator IE_PlayFormationForStats()
		{
			List<int> list = new List<int>();
			list.Add(1);
			list.Add(2);
			list.Add(3);
			list.Add(4);
			list.Add(5);
			list.Add(6);
			list.Add(7);
			list.Add(8);
			List<int> list2 = list;
			foreach (int item in list2)
			{
				fiSimpleSingletonBehaviour<FormationMgr<FishType>>.Get().PlayFormationById(item);
			}
			yield break;
		}

		private IEnumerator IE_PlayPathForStats()
		{
			new List<FishType>();
			yield break;
		}
	}
}
