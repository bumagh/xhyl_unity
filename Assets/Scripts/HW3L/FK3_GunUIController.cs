using DG.Tweening;
using M__M.HaiWang.Player.Gun;
using UnityEngine;
using UnityEngine.UI;

namespace HW3L
{
	public class FK3_GunUIController : MonoBehaviour
	{
		public Text txtName;

		public Text txtScore;

		public Text txtPower;

		private static FK3_GunUIController instance;

		private FK3_GunConfig _config;

		private FK3_GunPlayerData m_data = new FK3_GunPlayerData();

		public static FK3_GunUIController Get()
		{
			return instance;
		}

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
		}

		public void SetConfig(FK3_GunConfig config)
		{
			_config = config;
		}

		public int GetID()
		{
			return _config.id;
		}

		public void SetName(string name)
		{
			m_data.name = name;
			txtName.text = name.ToString();
		}

		public string GetName()
		{
			return m_data.name;
		}

		public void SetScore(int score)
		{
			m_data.score = score;
			txtScore.text = score.ToString();
		}

		private void SetScoreSize(string scoreStr)
		{
			if (scoreStr.Length < 8)
			{
				txtScore.fontSize = 35;
			}
			else
			{
				txtScore.fontSize = 30;
			}
		}

		public void SetPower(int power)
		{
			DOTween.Kill(txtPower.transform);
			txtPower.transform.localScale = Vector3.one;
			m_data.gunPower = power;
			txtPower.text = power.ToString();
			ShowFontSize(txtPower.text.Length);
			if (m_data.isNative)
			{
				txtPower.transform.DOScale(1.5f, 0.1f).SetLoops(2, LoopType.Yoyo);
			}
		}

		public void ShowFontSize(int length)
		{
			switch (length)
			{
			case 1:
				txtPower.fontSize = 25;
				break;
			case 2:
				txtPower.fontSize = 20;
				break;
			case 3:
				txtPower.fontSize = 18;
				break;
			case 4:
				txtPower.fontSize = 15;
				break;
			}
		}

		public void DoRest()
		{
		}

		private void _AdjustTextOrientation()
		{
			bool flag = _config.id == 3 || _config.id == 4;
			Transform parent = txtName.transform.parent;
			Transform parent2 = txtPower.transform.parent;
			parent.localRotation = Quaternion.Euler(0f, 0f, 0f);
			parent.localScale = new Vector3(1f, 1f, 1f);
			parent2.localRotation = Quaternion.Euler(0f, 0f, 0f);
			parent2.localScale = new Vector3(1f, 1f, 1f);
		}

		public void SetPlayerData(FK3_GunPlayerData data)
		{
			m_data = data;
			txtName.text = m_data.name.ToString();
			txtScore.text = m_data.score.ToString();
			txtPower.text = m_data.gunPower.ToString();
		}

		public void ClearGunUI()
		{
			txtName.text = string.Empty;
			txtScore.text = string.Empty;
			txtPower.text = string.Empty;
		}

		public bool HasUser()
		{
			return m_data != null;
		}

		public bool IsNative()
		{
			return m_data.isNative;
		}
	}
}
