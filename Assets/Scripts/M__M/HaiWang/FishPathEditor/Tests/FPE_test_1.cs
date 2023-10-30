using M__M.HaiWang.FishPathEditor.Core;
using System;
using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.Tests
{
	public class FPE_test_1 : MonoBehaviour
	{
		private void Start()
		{
			StartCoroutine(IE_Test_SpawnerBase_Int());
		}

		private IEnumerator IE_Test_SpawnerBase_Int()
		{
			UnityEngine.Debug.Log("IE_Test_SpawnerBase_Int");
			SpawnerBase<int> spawner = null;
			spawner = Create_SpawnerBase_Int(1, play: true, log: true, 0f, 1.5f);
			yield return new WaitWhile(() => spawner.IsPlaying);
			yield return new WaitForSeconds(0.1f);
			spawner = Create_SpawnerBase_Int(2, play: true, log: true, 3.7f, 1.5f);
			yield return new WaitWhile(() => spawner.IsPlaying);
			yield return new WaitForSeconds(0.1f);
			spawner = Create_SpawnerBase_Int(3, play: true, log: true, 5.6f, 1.5f);
		}

		private SpawnerBase<int> Create_SpawnerBase_Int(int testId, bool play = true, bool log = true, float playStartTime = 0f, float startDelay = 0f, int count = 5, float interval = 1f, float finishDelay = 0.5f)
		{
			SpawnerBase<int> spanwer = new SpawnerBase<int>();
			SpawnerData spawnerData = new SpawnerData
			{
				startDelay = startDelay,
				count = count,
				interval = interval,
				finishDelay = finishDelay
			};
			SingleGenerator<int> singleGenerator = new SingleGenerator<int>();
			singleGenerator.value = 100;
			IGenerator<int> generator = singleGenerator;
			Action<ProcessData<int>> processAction = delegate(ProcessData<int> data)
			{
				SpawnerBase<int>.RuntimeData runtimeData2 = spanwer.Debug_GetRuntimeData();
				if (log)
				{
					UnityEngine.Debug.Log($"spawner[{testId}] process> [value:{data.value},[index:{data.index},count{runtimeData2.count},processCount:{runtimeData2.processCount}],[virtualTimer:{runtimeData2.virtualTimer},elpased:{runtimeData2.Elapsed},realTimer:{runtimeData2.realTimer}]]");
				}
			};
			spanwer.Setup(spawnerData, generator, this, processAction);
			SpawnerBase<int> spawnerBase = spanwer;
			spawnerBase.OnFinish = (Action<SpawnerBase<int>>)Delegate.Combine(spawnerBase.OnFinish, (Action<SpawnerBase<int>>)delegate(SpawnerBase<int> _spawner)
			{
				SpawnerBase<int>.RuntimeData runtimeData = spanwer.Debug_GetRuntimeData();
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
