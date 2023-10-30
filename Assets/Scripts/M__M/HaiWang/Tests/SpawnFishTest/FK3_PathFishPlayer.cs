using FullInspector;
using M__M.HaiWang.Fish;
using System;
using UnityEngine;

namespace M__M.HaiWang.Tests.SpawnFishTest
{
	public class FK3_PathFishPlayer
	{
		[InspectorDisabled]
		public bool isPlaying;

		[InspectorDisabledIf("isPlaying")]
		public int pathId;

		[InspectorDisabledIf("isPlaying")]
		public FK3_FishType fishType = FK3_FishType.Gurnard_迦魶鱼;

		[InspectorDisabledIf("isPlaying")]
		public bool isTeam;

		[InspectorDisabledIf("isPlaying")]
		[InspectorShowIf("isTeam")]
		public int teamId;

		public Action<FK3_PathFishPlayerParam> playAction;

		public Action stopAction;

		[NotSerialized]
		public int fishId;

		[InspectorButton]
		[InspectorHideIf("isPlaying")]
		public void Play()
		{
			UnityEngine.Debug.Log(FK3_LogHelper.Orange("FK3_PathFishPlayer.Play> fishType:{0}, pathId:{1}", fishType, pathId));
			if (playAction != null)
			{
				playAction(new FK3_PathFishPlayerParam
				{
					player = this,
					fishType = fishType,
					fishId = fishId,
					pathId = pathId,
					isTeam = isTeam,
					teamId = teamId
				});
			}
		}

		[InspectorShowIf("isPlaying")]
		[InspectorButton]
		public void Stop()
		{
			UnityEngine.Debug.Log(FK3_LogHelper.Orange("FK3_PathFishPlayer.Stop> fishType:{0}, pathId:{1}", fishType, pathId));
			if (stopAction != null)
			{
				stopAction();
			}
		}
	}
}
