using M__M.HaiWang.Player.Gun;
using PathologicalGames;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Bullet
{
	public class BulletMgr : MonoBehaviour
	{
		[SerializeField]
		private HW2_SpawnPool m_bulletPool;

		public bool debugAllowShoot = true;

		public Action<BulletController> Event_BulletOver_Handler;

		private List<BulletController> m_bulletList = new List<BulletController>();

		public RectTransform bulletBorder;

		private static BulletMgr s_instance;

		public static BulletMgr Get()
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

		private void Start()
		{
		}

		private void Update()
		{
		}

		public void Handle_Shoot(GunController gun, Vector3 pos, bool faceDown, float angle, int gunValue, int seatId)
		{
		}

		public void Handle_Shoot2(Vector3 pos, float gunAngle, float angleOffset, int gunValue, int seatId)
		{
			if (debugAllowShoot)
			{
				ShootBullet(pos, gunAngle, angleOffset, gunValue, seatId);
			}
		}

		private BulletController SpawnBullet()
		{
			Transform prefab = m_bulletPool.prefabs["GatlingBullet"];
			Transform transform = m_bulletPool.Spawn(prefab, m_bulletPool.transform);
			return transform.GetComponent<BulletController>();
		}

		private BulletController ShootBullet(Vector3 pos, float gunAngle, float angleOffset, int gunValue, int seatId)
		{
			float num = gunAngle + angleOffset;
			BulletController instBullet = SpawnBullet();
			m_bulletList.Add(instBullet);
			instBullet.gameObject.SetActive(value: true);
			string text = "Bullet";
			try
			{
				if (instBullet.gameObject.tag != text)
				{
					instBullet.gameObject.tag = text;
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("设置tag错误: " + arg);
			}
			instBullet.seatId = seatId;
			instBullet.gunValue = gunValue;
			instBullet.Launch(pos, 90f - num);
			instBullet.Event_BulletOver_Handler = delegate(BulletController _bullet)
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

		public BulletController Shoot(Vector3 pos, float gunAngle, float angleOffset, int gunValue, int seatId, int bulletId)
		{
			BulletController bulletController = ShootBullet(pos, gunAngle, angleOffset, gunValue, seatId);
			bulletController.bulletId = bulletId;
			return bulletController;
		}

		public void DespawnBullet(BulletController _bullet)
		{
			m_bulletPool.Despawn(_bullet.transform);
		}

		public void RemoveAllBullets()
		{
			foreach (BulletController bullet in m_bulletList)
			{
				DespawnBullet(bullet);
			}
			m_bulletList.Clear();
		}
	}
}
