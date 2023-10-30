using M__M.HaiWang.Player.Gun;
using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;

namespace HW3L
{
	public class FK3_GunUIMgr : FK3_BaseUIForm
	{
		public FK3_GunSettings settings;

		private static FK3_GunUIMgr instance;

		[SerializeField]
		public List<FK3_GunUIController> GunUIList;

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

		public static FK3_GunUIMgr Get()
		{
			return instance;
		}

		private void Awake()
		{
			instance = this;
			ResetAllGunsUI();
			uiType.uiFormType = FK3_UIFormTypes.Normal;
		}

		public void SetRotate(bool value)
		{
			UnityEngine.Debug.Log("SetRotate:" + value);
			rotate = value;
			for (int i = 0; i < 4; i++)
			{
				FK3_GunUIController fK3_GunUIController = GunUIList[i];
				FK3_GunConfig config = settings.GetConfig(GetMapedId(i + 1));
				fK3_GunUIController.SetConfig(config);
				fK3_GunUIController.DoRest();
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

		public FK3_GunUIController GetGunByID(int id)
		{
			foreach (FK3_GunUIController gunUI in GunUIList)
			{
				if (gunUI.GetID() == id)
				{
					return gunUI;
				}
			}
			return null;
		}

		public FK3_GunUIController GetNativeGun()
		{
			foreach (FK3_GunUIController gunUI in GunUIList)
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
			for (int i = 0; i < 4; i++)
			{
				FK3_GunUIController fK3_GunUIController = GunUIList[i];
				FK3_GunConfig config = settings.GetConfig(i + 1);
				fK3_GunUIController.SetConfig(config);
				fK3_GunUIController.DoRest();
			}
		}

		public void ClearGunsUI()
		{
			foreach (FK3_GunUIController gunUI in GunUIList)
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
