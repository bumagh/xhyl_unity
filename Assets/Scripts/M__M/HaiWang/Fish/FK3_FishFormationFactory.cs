using PathSystem;
using System;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class FK3_FishFormationFactory : MonoBehaviour, FK3_IAgentFactory<FK3_FishType>
	{
		public Func<FK3_FishType, int, FK3_NavPathAgent> createFun;

		public Action<FK3_NavPathAgent, FK3_FishType> destoryAction;

		private static FK3_FishFormationFactory s_instance;

		public static FK3_FishFormationFactory Get()
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

		public FK3_NavPathAgent Create(FK3_FishType type, object fishID)
		{
			if (createFun == null)
			{
				return null;
			}
			return createFun(type, (int)fishID);
		}

		public void Destroy(FK3_NavPathAgent obj, FK3_FishType type)
		{
			if (destoryAction != null)
			{
				destoryAction(obj, type);
			}
		}
	}
}
