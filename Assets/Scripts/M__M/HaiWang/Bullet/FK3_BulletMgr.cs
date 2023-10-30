using M__M.HaiWang.Player.Gun;
using PathologicalGames;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Bullet
{
	public class FK3_BulletMgr : MonoBehaviour
	{
		[SerializeField]
		private FK3_SpawnPool m_bulletPool;

		public bool debugAllowShoot = true;

		public Action<FK3_BulletController> Event_BulletOver_Handler;

		private List<FK3_BulletController> m_bulletList = new List<FK3_BulletController>();

		public RectTransform bulletBorder;

		private static FK3_BulletMgr s_instance;

		private string tempTag = "Bullet";

		public FK3_BulletController ShootDRBull;

		public static FK3_BulletMgr Get()
		{
			return s_instance;
		}

		private void Awake()
		{
			s_instance = this;
		}

		private void OnEnable()
		{
			Transform transform = base.transform;
			Vector3 position = base.transform.position;
			float x = position.x;
			Vector3 position2 = base.transform.position;
			transform.localPosition = new Vector3(x, position2.y, -650f);
		}

		public void Handle_Shoot(FK3_GunController gun, Vector3 pos, bool faceDown, float angle, int gunValue, int seatId)
		{
		}

		public void Handle_Shoot2(Vector3 pos, float gunAngle, float angleOffset, int gunValue, int seatId)
		{
			if (debugAllowShoot)
			{
				ShootBullet(pos, gunAngle, angleOffset, gunValue, seatId);
			}
		}

		private FK3_BulletController SpawnBullet(int id)
		{
			Transform prefab = m_bulletPool.prefabs["GatlingBullet" + ((FK3_GVars.NowGunSkin[id] != 0) ? 2 : 3)];
			Transform transform = m_bulletPool.Spawn(prefab, m_bulletPool.transform);
			transform.name = "GatlingBullet" + ((FK3_GVars.NowGunSkin[id] != 0) ? 2 : 3);
			transform.localScale = new Vector3(100f, (FK3_GVars.NowGunSkin[id] != 0) ? 100 : 140);
			return transform.GetComponent<FK3_BulletController>();
		}

		private FK3_BulletController SpawnDRBullet()
		{
			Transform prefab = m_bulletPool.prefabs["DRBarrel"];
			Transform transform = m_bulletPool.Spawn(prefab, m_bulletPool.transform);
			return transform.GetComponent<FK3_BulletController>();
		}

		private FK3_BulletController ShootDRBullet(Vector3 pos, float gunAngle, float angleOffset, int gunValue, int seatId)
		{
			float num = gunAngle + angleOffset;
			FK3_BulletController fK3_BulletController = SpawnDRBullet();
			fK3_BulletController.gameObject.SetActive(value: true);
			fK3_BulletController.seatId = seatId;
			fK3_BulletController.gunValue = gunValue;
			fK3_BulletController.Launch(pos, 90f - num);
			return fK3_BulletController;
		}

		private FK3_BulletController ShootBullet(Vector3 pos, float gunAngle, float angleOffset, int gunValue, int seatId)
		{
			float num = gunAngle + angleOffset;
			FK3_BulletController instBullet = SpawnBullet(seatId - 1);
			m_bulletList.Add(instBullet);
			instBullet.gameObject.SetActive(value: true);
			try
			{
				if (instBullet.gameObject.tag != tempTag)
				{
					instBullet.gameObject.tag = tempTag;
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("设置tag错误: " + arg);
			}
			instBullet.seatId = seatId;
			instBullet.gunValue = gunValue;
			instBullet.Launch(pos, 90f - num);
			instBullet.Event_BulletOver_Handler = delegate(FK3_BulletController _bullet)
			{
				m_bulletPool.Despawn(instBullet.transform);
				m_bulletList.Remove(instBullet);
				if (Event_BulletOver_Handler != null)
				{
					Event_BulletOver_Handler(_bullet);
				}
			};
			return instBullet;
		}

		public FK3_BulletController Shoot(Vector3 pos, float gunAngle, float angleOffset, int gunValue, int seatId, int bulletId)
		{
			FK3_BulletController fK3_BulletController = ShootBullet(pos, gunAngle, angleOffset, gunValue, seatId);
			fK3_BulletController.bulletId = bulletId;
			return fK3_BulletController;
		}

		public FK3_BulletController ShootDR(Vector3 pos, float gunAngle, float angleOffset, int gunValue, int seatId, int bulletId)
		{
			ShootDRBull = ShootDRBullet(pos, gunAngle, angleOffset, gunValue, seatId);
			ShootDRBull.bulletId = bulletId;
			try
			{
				ShootDRBull.GetComponent<Animator>().SetTrigger("Shoot");
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			return ShootDRBull;
		}

		public void DespawnBullet(FK3_BulletController _bullet)
		{
			m_bulletPool.Despawn(_bullet.transform);
		}

		public void RemoveAllBullets()
		{
			foreach (FK3_BulletController bullet in m_bulletList)
			{
				DespawnBullet(bullet);
			}
			m_bulletList.Clear();
		}
	}
}
