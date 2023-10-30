using M__M.HaiWang.FishPathEditor.Core;
using System;
using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.Tests
{
	public class FK3_FPE_test_1 : MonoBehaviour
	{
		private void Start()
		{
			StartCoroutine(IE_Test_SpawnerBase_Int());
		}

		private IEnumerator IE_Test_SpawnerBase_Int()
		{
			UnityEngine.Debug.Log("IE_Test_SpawnerBase_Int");
			FK3_SpawnerBase<int> spawner = null;
			spawner = Create_SpawnerBase_Int(1, play: true, log: true, 0f, 1.5f);
			yield return new WaitWhile(() => spawner.IsPlaying);
			yield return new WaitForSeconds(0.1f);
			spawner = Create_SpawnerBase_Int(2, play: true, log: true, 3.7f, 1.5f);
			yield return new WaitWhile(() => spawner.IsPlaying);
			yield return new WaitForSeconds(0.1f);
			spawner = Create_SpawnerBase_Int(3, play: true, log: true, 5.6f, 1.5f);
		}

		private FK3_SpawnerBase<int> Create_SpawnerBase_Int(int testId, bool play = true, bool log = true, float playStartTime = 0f, float startDelay = 0f, int count = 5, float interval = 1f, float finishDelay = 0.5f)
		{
			FK3_SpawnerBase<int> spanwer = new FK3_SpawnerBase<int>();
			FK3_SpawnerData spawnerData = new FK3_SpawnerData
			{
				startDelay = startDelay,
				count = count,
				interval = interval,
				finishDelay = finishDelay
			};
			FK3_SingleGenerator<int> fK3_SingleGenerator = new FK3_SingleGenerator<int>();
			fK3_SingleGenerator.value = 100;
			FK3_IGenerator<int> generator = fK3_SingleGenerator;
			Action<FK3_ProcessData<int>> processAction = delegate(FK3_ProcessData<int> data)
			{
				FK3_SpawnerBase<int>.RuntimeData runtimeData2 = spanwer.Debug_GetRuntimeData();
				if (log)
				{
					UnityEngine.Debug.Log($"spawner[{testId}] process> [value:{data.value},[index:{data.index},count{runtimeData2.count},processCount:{runtimeData2.processCount}],[virtualTimer:{runtimeData2.virtualTimer},elpased:{runtimeData2.Elapsed},realTimer:{runtimeData2.realTimer}]]");
				}
			};
			spanwer.Setup(spawnerData, generator, this, processAction);
			FK3_SpawnerBase<int> fK3_SpawnerBase = spanwer;
			fK3_SpawnerBase.OnFinish = (Action<FK3_SpawnerBase<int>>)Delegate.Combine(fK3_SpawnerBase.OnFinish, (Action<FK3_SpawnerBase<int>>)delegate(FK3_SpawnerBase<int> _spawner)
			{
				FK3_SpawnerBase<int>.RuntimeData runtimeData = spanwer.Debug_GetRuntimeData();
				if (log)
				{
					UnityEngine.Debug.Log(string.Format("spawner[{0}] finished> [playBeginTime:{7},[elpased:{1},realTimer:{2}],[totalDuration:{3},virutalTimer:{4}],[count:{5},processCount:{6}]]", testId, _spawner.Elapsed, runtimeData.realTimer, spawnerData.totalDuration, runtimeData.virtualTimer, runtimeData.count, runtimeData.processCount, runtimeData.BeginTime));
				}
			});
			if (log)
			{
				UnityEngine.Debug.Log($"spawnerData[id:{testId}]> [startDelay:{spawnerData.startDelay},count:{spawnerData.count},totalDuration:{spawnerData.totalDuration}]");
			}
			if (log)
			{
				UnityEngine.Debug.Log($"spawner[{testId}] play at {playStartTime}");
			}
			if (play)
			{
				spanwer.Play(playStartTime);
			}
			return spanwer;
		}
	}
}
