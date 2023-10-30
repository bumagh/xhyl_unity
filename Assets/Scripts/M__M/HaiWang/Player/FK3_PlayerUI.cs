using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Player
{
	public class FK3_PlayerUI : MonoBehaviour
	{
		[SerializeField]
		private Button m_btnChangeGunPower;

		[SerializeField]
		private Button m_btnChangeGunMode;

		[SerializeField]
		private Text m_textGunPower;

		[SerializeField]
		private Text m_playerScore;

		[SerializeField]
		private Text m_playerName;

		public bool isHost;

		public int score;

		public int power;

		public int id;

		private FK3_PlayerController m_player;

		public static List<FK3_PlayerUI> playerUIs = new List<FK3_PlayerUI>();

		public event Action<FK3_PlayerUI> UIEvent_PlayerChangeGunPower;

		public event Action<FK3_PlayerUI> UIEvent_PlayerChangeGunMode;

		public event Action<FK3_PlayerUI> UIEvent_AutoShoot;

		private void Awake()
		{
			playerUIs.Add(this);
		}

		private void Start()
		{
			m_player = FK3_PlayerMgr.Get().GetPlayer(id);
		}

		private void Update()
		{
		}

		public void OnBtnClick_ChangeGunPower()
		{
			UnityEngine.Debug.Log("OnBtnClick_ChangeGunPower");
			if (this.UIEvent_PlayerChangeGunPower != null)
			{
				this.UIEvent_PlayerChangeGunPower(this);
			}
		}

		public void OnBtnClick_ChangeGunMode()
		{
			UnityEngine.Debug.Log("OnBtnClick_ChangeGunMode");
			if (this.UIEvent_PlayerChangeGunMode != null)
			{
				this.UIEvent_PlayerChangeGunMode(this);
			}
		}

		public void OnBtnClick_AutoShoot()
		{
			UnityEngine.Debug.Log("OnBtnClick_AutoShoot");
			if (this.UIEvent_AutoShoot != null)
			{
				this.UIEvent_AutoShoot(this);
			}
		}

		public FK3_PlayerController GetPlayer()
		{
			return m_player;
		}

		public static FK3_PlayerUI GetPlayerUIById(int id)
		{
			for (int i = 0; i < playerUIs.Count; i++)
			{
				if (playerUIs[i].id == id)
				{
					return playerUIs[i];
				}
			}
			return null;
		}

		public void SetNative(bool isHost)
		{
			base.transform.Find("btn_ChangeGunPower").gameObject.SetActive(isHost);
			base.transform.Find("btn_SwitchGunMode").gameObject.SetActive(isHost);
			if (isHost)
			{
				Transform transform = base.transform.parent.Find("btnAuto");
				transform.GetComponent<Button>().onClick.RemoveAllListeners();
				transform.GetComponent<Button>().onClick.AddListener(OnBtnClick_AutoShoot);
			}
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
