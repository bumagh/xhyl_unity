using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;

namespace M__M.HaiWang.Player.Gun
{
	public class GunUIMgr : BaseUIForm
	{
		public GunSettings settings;

		private static GunUIMgr instance;

		[SerializeField]
		public List<GunUIController> GunUIList;

		public GameObject point1;

		public GameObject point2;

		private bool rotate;

		private int[] idMap = new int[4]
		{
			3,
			4,
			1,
			2
		};

		public static GunUIMgr Get()
		{
			return instance;
		}

		private void Awake()
		{
			instance = this;
			ResetAllGunsUI();
			uiType.uiFormType = UIFormTypes.Normal;
		}

		public void SetRotate(bool value)
		{
			UnityEngine.Debug.Log("SetRotate:" + value);
			rotate = value;
			for (int i = 0; i < 4; i++)
			{
				GunUIController gunUIController = GunUIList[i];
				GunConfig config = settings.GetConfig(GetMapedId(i + 1));
				gunUIController.SetConfig(config);
				gunUIController.DoRest();
			}
		}

		private int GetMapedId(int id)
		{
			if (rotate)
			{
				return idMap[id - 1];
			}
			return id;
		}

		public GunUIController GetGunByID(int id)
		{
			foreach (GunUIController gunUI in GunUIList)
			{
				if (gunUI.GetID() == id)
				{
					return gunUI;
				}
			}
			return null;
		}

		public GunUIController GetNativeGun()
		{
			foreach (GunUIController gunUI in GunUIList)
			{
				if (gunUI.HasUser() && gunUI.IsNative())
				{
					return gunUI;
				}
			}
			return null;
		}

		private void ResetAllGunsUI()
		{
			UnityEngine.Debug.Log("ResetAllGunsUI");
			for (int i = 0; i < 4; i++)
			{
				GunUIController gunUIController = GunUIList[i];
				GunConfig config = settings.GetConfig(i + 1);
				gunUIController.SetConfig(config);
				gunUIController.DoRest();
			}
		}

		public void ClearGunsUI()
		{
			foreach (GunUIController gunUI in GunUIList)
			{
				gunUI.txtName.text = null;
				gunUI.txtPower.text = null;
				gunUI.txtScore.text = null;
			}
		}

		public IEnumerator pointMove()
		{
			if (point1.activeSelf)
			{
				yield return new WaitForSeconds(3f);
				point1.gameObject.SetActive(value: false);
			}
			if (point2.activeSelf)
			{
				yield return new WaitForSeconds(3f);
				point2.gameObject.SetActive(value: false);
			}
		}
	}
}
