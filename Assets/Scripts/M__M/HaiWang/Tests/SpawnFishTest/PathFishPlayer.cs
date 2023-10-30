using FullInspector;
using M__M.HaiWang.Fish;
using System;
using UnityEngine;

namespace M__M.HaiWang.Tests.SpawnFishTest
{
	public class PathFishPlayer
	{
		[InspectorDisabled]
		public bool isPlaying;

		[InspectorDisabledIf("isPlaying")]
		public int pathId;

		[InspectorDisabledIf("isPlaying")]
		public FishType fishType = FishType.Gurnard_迦魶鱼;

		[InspectorDisabledIf("isPlaying")]
		public bool isTeam;

		[InspectorShowIf("isTeam")]
		[InspectorDisabledIf("isPlaying")]
		public int teamId;

		public Action<PathFishPlayerParam> playAction;

		public Action stopAction;

		[NotSerialized]
		public int fishId;

		[InspectorHideIf("isPlaying")]
		[InspectorButton]
		public void Play()
		{
			UnityEngine.Debug.Log(HW2_LogHelper.Orange("PathFishPlayer.Play> fishType:{0}, pathId:{1}", fishType, pathId));
			if (playAction != null)
			{
				playAction(new PathFishPlayerParam
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
			UnityEngine.Debug.Log(HW2_LogHelper.Orange("PathFishPlayer.Stop> fishType:{0}, pathId:{1}", fishType, pathId));
			if (stopAction != null)
			{
				stopAction();
			}
		}
	}
}
