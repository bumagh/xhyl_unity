using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class FishMgr_Mock : MonoBehaviour
	{
		private static FishMgr_Mock _instance;

		public List<FishBehaviour> fishList = new List<FishBehaviour>();

		public Dictionary<int, FishBehaviour> fishTypeMap = new Dictionary<int, FishBehaviour>();

		public Dictionary<int, FishBehaviour> fishIdMap = new Dictionary<int, FishBehaviour>();

		public static FishMgr_Mock Get()
		{
			return _instance;
		}

		private void Awake()
		{
			_instance = this;
			if (base.isActiveAndEnabled)
			{
				Init();
			}
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void Init()
		{
			foreach (FishBehaviour fish in fishList)
			{
				fishTypeMap.Add((int)fish.type, fish);
			}
			UnityEngine.Debug.Log(fishTypeMap.Count);
		}
	}
}
