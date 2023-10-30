using M__M.HaiWang.Bullet;
using M__M.HaiWang.Player.Gun;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace M__M.HaiWang.Player
{
	public class PlayerMgr : MonoBehaviour
	{
		private static PlayerMgr s_instance;

		public bool debugAllowShoot = true;

		private List<PlayerController> m_players;

		[SerializeField]
		public RuntimeAnimatorController[] gunAnimatorControllers;

		private int m_nativeId;

		private PlayerController m_nativePlayer;

		private GunController m_nativeGun;

		private PlayerUI m_nativeUI;

		[CompilerGenerated]
		private static Action<PlayerController> f_mgcache;

		public int nativeId
		{
			get
			{
				return m_nativeId;
			}
			set
			{
				m_nativeId = value;
			}
		}

		public static PlayerMgr Get()
		{
			return s_instance;
		}

		private void Awake()
		{
			s_instance = this;
			PlayerController.OnStartAction = AddPlayer;
			m_players = PlayerController.players;
		}

		private void Start()
		{
			UnityEngine.Debug.Log("player count:" + m_players.Count);
		}

		private void Update()
		{
		}

		public static void AddPlayer(PlayerController player)
		{
			if (s_instance != null)
			{
				GunController gun = player.GetGun();
				gun.Event_Shoot_Handler = (GunController.EventHandler_Shoot)Delegate.Combine(gun.Event_Shoot_Handler, new GunController.EventHandler_Shoot(BulletMgr.Get().Handle_Shoot));
			}
		}

		private void _AddPlayer(PlayerController player)
		{
			UnityEngine.Debug.Log("AddPlayer " + player.id);
			m_players.Add(player);
		}

		public static void RemovePlayer(PlayerController player)
		{
			if (s_instance != null)
			{
				s_instance._RemovePlayer(player);
			}
		}

		private void _RemovePlayer(PlayerController player)
		{
			UnityEngine.Debug.Log("RemovePlayer " + player.id);
			m_players.Remove(player);
		}

		public void UpdateInput()
		{
			UnityEngine.Debug.Log("mouse screen pos " + UnityEngine.Input.mousePosition);
			Camera main = Camera.main;
			Vector3 mousePosition = UnityEngine.Input.mousePosition;
			Vector3 position = Camera.main.transform.position;
			Vector3 vector = main.ScreenToWorldPoint(mousePosition + position.z * Vector3.back);
			UnityEngine.Debug.Log("mouse world pos " + vector);
		}

		public PlayerController GetPlayer(int seatId)
		{
			for (int i = 0; i < m_players.Count; i++)
			{
				if (m_players[i].id == seatId)
				{
					return m_players[i];
				}
			}
			return null;
		}

		public PlayerController GetNativePlayer()
		{
			return m_nativePlayer;
		}

		public GunController GetNativeGun()
		{
			return m_nativeGun;
		}

		public PlayerUI GetNativeUI()
		{
			return m_nativeUI;
		}

		public void SetNativePlayer(int id)
		{
			m_nativeId = id;
			m_nativePlayer = GetPlayer(id);
			m_nativeGun = m_nativePlayer.GetGun();
			m_nativeUI = PlayerUI.GetPlayerUIById(id);
			m_nativePlayer.isNativePlayer = true;
			m_nativeGun.isNativePlayer = true;
			m_nativeUI.SetNative(isHost: true);
		}

		public void ShowAll()
		{
			foreach (PlayerController player in m_players)
			{
				player.Show();
			}
		}

		public void HideAll()
		{
			foreach (PlayerController player in m_players)
			{
				player.Hide();
			}
		}
	}
}
