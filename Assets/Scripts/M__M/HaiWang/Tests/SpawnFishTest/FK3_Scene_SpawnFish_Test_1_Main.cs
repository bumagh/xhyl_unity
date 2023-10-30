using M__M.HaiWang.Fish;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace M__M.HaiWang.Tests.SpawnFishTest
{
	public class FK3_Scene_SpawnFish_Test_1_Main : MonoBehaviour
	{
		private void Start()
		{
			CheckDependency();
			StartCoroutine(IE_Test());
		}

		private void CheckDependency()
		{
			UnityEngine.Debug.Log($"{GetType()} CheckDependency");
			Assert.raiseExceptions = true;
		}

		private IEnumerator IE_Test()
		{
			FK3_FishType type = FK3_FishType.Gurnard_迦魶鱼;
			int pathId = 1;
			int startFishID = 1000;
			FK3_FishQueen fishQueen = FK3_FishQueenMgr.Get().GetFishQueen();
			FK3_FishQueenData fK3_FishQueenData = new FK3_FishQueenData();
			FK3_FP_Sequence fK3_FP_Sequence = new FK3_FP_Sequence();
			FK3_FG_SingleType<FK3_FishType> fK3_FG_SingleType = new FK3_FG_SingleType<FK3_FishType>();
			fK3_FG_SingleType.type = type;
			fK3_FG_SingleType.count = 1;
			fK3_FishQueenData.delay = 0f;
			fK3_FishQueenData.generator = fK3_FG_SingleType;
			fK3_FishQueenData.speed = 2f;
			fK3_FP_Sequence.pathId = pathId;
			fishQueen.Setup(fK3_FishQueenData, fK3_FP_Sequence, 1, startFishID);
			fishQueen.Play();
			yield break;
		}
	}
}
