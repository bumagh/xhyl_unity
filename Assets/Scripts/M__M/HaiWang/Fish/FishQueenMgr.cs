using M__M.HaiWang.FishSpawn;
using System;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class FishQueenMgr : MonoBehaviour
	{
		private static FishQueenMgr s_instance;

		[SerializeField]
		private FishQueen m_prefab;

		[SerializeField]
		private FishSpawnConfig m_spawnConfig;

		public static FishQueenMgr Get()
		{
			return s_instance;
		}

		private void Awake()
		{
			s_instance = this;
		}

		private void Start()
		{
		}

		public FishQueen GetFishQueen()
		{
			FishQueen fishQueen = UnityEngine.Object.Instantiate(m_prefab);
			fishQueen.transform.SetParent(base.transform);
			fishQueen.transform.localScale = Vector3.one;
			fishQueen.gameObject.SetActive(value: true);
			fishQueen.onFinish = null;
			FishQueen fishQueen2 = fishQueen;
			fishQueen2.onFinish = (Action<FishQueen>)Delegate.Combine(fishQueen2.onFinish, (Action<FishQueen>)delegate(FishQueen queen)
			{
				UnityEngine.Object.DestroyImmediate(queen.gameObject);
			});
			return fishQueen;
		}

		public float GetSpeedByFishType(FishType fishType)
		{
			if (m_spawnConfig == null)
			{
				return FishSpawnConfig.staticDefaultFishSpeed;
			}
			return m_spawnConfig.GetConstSpeed(fishType);
		}
	}
}
