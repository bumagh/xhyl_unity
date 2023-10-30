using M__M.HaiWang.Fish;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace M__M.HaiWang.Tests.SpawnFishTest
{
	public class Scene_SpawnFish_Test_1_Main : MonoBehaviour
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
			FishType type = FishType.Gurnard_迦魶鱼;
			int pathId = 1;
			int startFishID = 1000;
			FishQueen fishQueen = FishQueenMgr.Get().GetFishQueen();
			FishQueenData fishQueenData = new FishQueenData();
			FP_Sequence fP_Sequence = new FP_Sequence();
			FG_SingleType<FishType> fG_SingleType = new FG_SingleType<FishType>();
			fG_SingleType.type = type;
			fG_SingleType.count = 1;
			fishQueenData.delay = 0f;
			fishQueenData.generator = fG_SingleType;
			fishQueenData.speed = 2f;
			fP_Sequence.pathId = pathId;
			fishQueen.Setup(fishQueenData, fP_Sequence, 1, startFishID);
			fishQueen.Play();
			yield break;
		}
	}
}
