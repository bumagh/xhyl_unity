using M__M.HaiWang.Bullet;
using PathologicalGames;
using UnityEngine;

namespace M__M.HaiWang.Effect
{
	public class FK3_Effect_ExplodeMgr : MonoBehaviour
	{
		private static FK3_Effect_ExplodeMgr s_intance;

		private FK3_SpawnPool m_explodePool;

		public static FK3_Effect_ExplodeMgr Get()
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
			m_explodePool = FK3_PoolManager.Pools["Explode"];
		}

		public void SpawnExplode(Vector3 pos, FK3_BulletController fK3_Bullet)
		{
			Transform prefab = m_explodePool.prefabs[(!(fK3_Bullet.name == "GatlingBullet3")) ? "FK3_GatlingBullet_Boom" : "FK3_GatlingBullet_Boom3"];
			Transform transform = m_explodePool.transform;
			Transform transform2 = m_explodePool.Spawn(prefab, transform);
			FK3_GatlingBullet_Boom component = transform2.GetComponent<FK3_GatlingBullet_Boom>();
			component.Reset_EventHandler();
			component.Event_Over_Handler += delegate(FK3_GatlingBullet_Boom _explode)
			{
				m_explodePool.Despawn(_explode.transform);
			};
			component.Play(pos, fK3_Bullet.seatId);
		}
	}
}
