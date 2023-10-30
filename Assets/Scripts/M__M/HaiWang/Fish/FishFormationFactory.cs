using PathSystem;
using System;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class FishFormationFactory : MonoBehaviour, IAgentFactory<FishType>
	{
		public Func<FishType, int, NavPathAgent> createFun;

		public Action<NavPathAgent, FishType> destoryAction;

		private static FishFormationFactory s_instance;

		public static FishFormationFactory Get()
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

		private void Update()
		{
		}

		public NavPathAgent Create(FishType type, object fishID)
		{
			if (createFun == null)
			{
				return null;
			}
			return createFun(type, (int)fishID);
		}

		public void Destroy(NavPathAgent obj, FishType type)
		{
			if (destoryAction != null)
			{
				destoryAction(obj, type);
			}
		}
	}
}
