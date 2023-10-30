using M__M.HaiWang.Player.Gun;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Player
{
	public class FK3_PlayerController : MonoBehaviour
	{
		public bool isNativePlayer;

		public int id;

		public bool allowShoot = true;

		[SerializeField]
		private float m_shootInterval = 0.3f;

		public bool uiBlock;

		private bool m_doNext;

		private float m_shootTimer;

		public static Action<FK3_PlayerController> OnStartAction;

		public static List<FK3_PlayerController> players = new List<FK3_PlayerController>();

		private FK3_GunController m_gun;

		private void Awake()
		{
			players.Add(this);
			m_gun = GetComponent<FK3_GunController>();
		}

		private void Start()
		{
			OnStartAction(this);
		}

		private void Update()
		{
			if (isNativePlayer && FK3_HostInput.Get() != null)
			{
				if (FK3_HostInput.Get().IsTouching())
				{
					DoNext();
				}
				m_shootTimer += Time.deltaTime;
			}
		}

		private void DoNext()
		{
			m_doNext = false;
			m_gun.RotateByInput();
			if (m_shootTimer > m_shootInterval)
			{
				m_gun.Shoot();
				m_shootTimer = 0f;
			}
		}

		public FK3_GunController GetGun()
		{
			return m_gun;
		}

		public void Show()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
