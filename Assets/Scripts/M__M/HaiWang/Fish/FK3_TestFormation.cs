using PathSystem;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class FK3_TestFormation : MonoBehaviour, FK3_IAgentFactory<FK3_FishType>
	{
		private FK3_FishFormation _fishFormation;

		private FK3_ObjectPool<FK3_NavPathAgent> _fish1Pool;

		private FK3_ObjectPool<FK3_NavPathAgent> _fish2Pool;

		private int index;

		public FK3_NavPathAgent Create(FK3_FishType type, object fishID)
		{
			switch (type)
			{
			case FK3_FishType.Gurnard_迦魶鱼:
				return _fish1Pool.Get();
			case FK3_FishType.Clown_小丑鱼:
				return _fish2Pool.Get();
			default:
				return null;
			}
		}

		public void Destroy(FK3_NavPathAgent agent, FK3_FishType type)
		{
			switch (type)
			{
			case FK3_FishType.Gurnard_迦魶鱼:
				_fish1Pool.Release(agent);
				break;
			case FK3_FishType.Clown_小丑鱼:
				_fish2Pool.Release(agent);
				break;
			}
		}

		private void Awake()
		{
			_fishFormation = GetComponentInChildren<FK3_FishFormation>();
			_fish1Pool = new FK3_ObjectPool<FK3_NavPathAgent>(delegate
			{
				FK3_NavPathAgent fK3_NavPathAgent = UnityEngine.Object.Instantiate(Resources.Load<FK3_NavPathAgent>("Test/Fish1"));
				fK3_NavPathAgent.name = string.Empty + index++;
				return fK3_NavPathAgent;
			}, delegate(FK3_NavPathAgent obj)
			{
				UnityEngine.Object.Destroy(obj);
			}, delegate(FK3_NavPathAgent agent)
			{
				agent.gameObject.SetActive(value: true);
			}, delegate(FK3_NavPathAgent agent)
			{
				agent.gameObject.SetActive(value: false);
			});
			_fish1Pool.Prepare(20);
			_fish2Pool = new FK3_ObjectPool<FK3_NavPathAgent>(() => UnityEngine.Object.Instantiate(Resources.Load<FK3_NavPathAgent>("Test/Fish2")), delegate(FK3_NavPathAgent obj)
			{
				UnityEngine.Object.Destroy(obj);
			}, delegate(FK3_NavPathAgent agent)
			{
				agent.gameObject.SetActive(value: true);
			}, delegate(FK3_NavPathAgent agent)
			{
				agent.gameObject.SetActive(value: false);
			});
			_fish2Pool.Prepare(20);
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(1))
			{
				if (_fishFormation.state != FK3_FormationState.Playing)
				{
					_fishFormation.Play();
				}
				else
				{
					_fishFormation.Stop();
				}
			}
		}
	}
}
