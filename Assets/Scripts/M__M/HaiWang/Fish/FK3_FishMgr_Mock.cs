using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class FK3_FishMgr_Mock : MonoBehaviour
	{
		private static FK3_FishMgr_Mock _instance;

		public List<FK3_FishBehaviour> fishList = new List<FK3_FishBehaviour>();

		public Dictionary<int, FK3_FishBehaviour> fishTypeMap = new Dictionary<int, FK3_FishBehaviour>();

		public Dictionary<int, FK3_FishBehaviour> fishIdMap = new Dictionary<int, FK3_FishBehaviour>();

		public static FK3_FishMgr_Mock Get()
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
			foreach (FK3_FishBehaviour fish in fishList)
			{
				fishTypeMap.Add((int)fish.type, fish);
			}
			UnityEngine.Debug.Log(fishTypeMap.Count);
		}
	}
}
