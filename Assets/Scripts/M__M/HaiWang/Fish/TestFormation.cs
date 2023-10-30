using PathSystem;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class TestFormation : MonoBehaviour, IAgentFactory<FishType>
	{
		private FishFormation _fishFormation;

		private ObjectPool<NavPathAgent> _fish1Pool;

		private ObjectPool<NavPathAgent> _fish2Pool;

		private int index;

		public NavPathAgent Create(FishType type, object fishID)
		{
			switch (type)
			{
			case FishType.Gurnard_迦魶鱼:
				return _fish1Pool.Get();
			case FishType.Clown_小丑鱼:
				return _fish2Pool.Get();
			default:
				return null;
			}
		}

		public void Destroy(NavPathAgent agent, FishType type)
		{
			switch (type)
			{
			case FishType.Gurnard_迦魶鱼:
				_fish1Pool.Release(agent);
				break;
			case FishType.Clown_小丑鱼:
				_fish2Pool.Release(agent);
				break;
			}
		}

		private void Awake()
		{
			_fishFormation = GetComponentInChildren<FishFormation>();
			_fish1Pool = new ObjectPool<NavPathAgent>(delegate
			{
				NavPathAgent navPathAgent = UnityEngine.Object.Instantiate(Resources.Load<NavPathAgent>("Test/Fish1"));
				navPathAgent.name = string.Empty + index++;
				return navPathAgent;
			}, delegate(NavPathAgent obj)
			{
				UnityEngine.Object.Destroy(obj);
			}, delegate(NavPathAgent agent)
			{
				agent.gameObject.SetActive(value: true);
			}, delegate(NavPathAgent agent)
			{
				agent.gameObject.SetActive(value: false);
			});
			_fish1Pool.Prepare(20);
			_fish2Pool = new ObjectPool<NavPathAgent>(() => UnityEngine.Object.Instantiate(Resources.Load<NavPathAgent>("Test/Fish2")), delegate(NavPathAgent obj)
			{
				UnityEngine.Object.Destroy(obj);
			}, delegate(NavPathAgent agent)
			{
				agent.gameObject.SetActive(value: true);
			}, delegate(NavPathAgent agent)
			{
				agent.gameObject.SetActive(value: false);
			});
			_fish2Pool.Prepare(20);
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(1))
			{
				if (_fishFormation.state != FormationState.Playing)
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
