using PathologicalGames;
using UnityEngine;

namespace M__M.HaiWang.Effect
{
	public class Effect_ExplodeMgr : MonoBehaviour
	{
		private static Effect_ExplodeMgr s_intance;

		private HW2_SpawnPool m_explodePool;

		public static Effect_ExplodeMgr Get()
		{
			return s_intance;
		}

		private void Awake()
		{
			s_intance = this;
		}

		private void Start()
		{
			Init();
		}

		private void Init()
		{
			_InitBasic();
		}

		private void _InitBasic()
		{
			m_explodePool = HW2_PoolManager.Pools["Explode"];
		}

		private void Update()
		{
		}

		public void SpawnExplode(Vector3 pos)
		{
			Transform prefab = m_explodePool.prefabs["GatlingBullet_Boom"];
			Transform transform = m_explodePool.transform;
			Transform transform2 = m_explodePool.Spawn(prefab, transform);
			GatlingBullet_Boom component = transform2.GetComponent<GatlingBullet_Boom>();
			component.Reset_EventHandler();
			component.Event_Over_Handler += delegate(GatlingBullet_Boom _explode)
			{
				m_explodePool.Despawn(_explode.transform);
			};
			component.Play(pos);
		}
	}
}
