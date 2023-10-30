using HW3L;
using System;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class FK3_FishQueenMgr : MonoBehaviour
	{
		private static FK3_FishQueenMgr s_instance;

		[SerializeField]
		private FK3_FishQueen m_prefab;

		[SerializeField]
		private FK3_FishSpawnConfig m_spawnConfig;

		public static FK3_FishQueenMgr Get()
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

		public FK3_FishQueen GetFishQueen()
		{
			FK3_FishQueen fK3_FishQueen = UnityEngine.Object.Instantiate(m_prefab);
			fK3_FishQueen.transform.SetParent(base.transform);
			fK3_FishQueen.transform.localScale = Vector3.one;
			fK3_FishQueen.gameObject.SetActive(value: true);
			fK3_FishQueen.onFinish = null;
			FK3_FishQueen fK3_FishQueen2 = fK3_FishQueen;
			fK3_FishQueen2.onFinish = (Action<FK3_FishQueen>)Delegate.Combine(fK3_FishQueen2.onFinish, (Action<FK3_FishQueen>)delegate(FK3_FishQueen queen)
			{
				UnityEngine.Object.DestroyImmediate(queen.gameObject);
			});
			return fK3_FishQueen;
		}

		public float GetSpeedByFishType(FK3_FishType fishType)
		{
			if (m_spawnConfig == null)
			{
				return FK3_FishSpawnConfig.staticDefaultFishSpeed;
			}
			return m_spawnConfig.GetConstSpeed(fishType);
		}
	}
}
