using M__M.GameHall.Common;
using M__M.HaiWang.Fish;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Demo
{
	public class SynchronizeHint : HW2_MB_Singleton<SynchronizeHint>
	{
		private GameObject goContainer;

		[SerializeField]
		private Text textInfo;

		private Coroutine textInfoAniIE;

		private float timeCount = 1f;

		private void Awake()
		{
			if (HW2_MB_Singleton<SynchronizeHint>._instance == null)
			{
				HW2_MB_Singleton<SynchronizeHint>.SetInstance(this);
				PreInit();
			}
			base.gameObject.SetActive(value: false);
		}

		public void PreInit()
		{
			goContainer = base.gameObject;
		}

		public void Show()
		{
			if (!goContainer.activeSelf)
			{
				timeCount = 1f;
				goContainer.SetActive(value: true);
				if (base.gameObject.activeInHierarchy)
				{
					textInfoAniIE = StartCoroutine(TextInfoAniIE());
				}
			}
		}

		private void Update()
		{
			timeCount -= Time.deltaTime;
			if (timeCount < 0f)
			{
				timeCount = 1f;
				int count = FishMgr.Get().GetScreenFishList(delegate(FishBehaviour _fish)
				{
					bool flag = true;
					return _fish.IsLive() && flag;
				}).Count;
				if (count > 0)
				{
					Hide();
				}
			}
		}

		public void Hide()
		{
			if (textInfoAniIE != null)
			{
				StopCoroutine(textInfoAniIE);
			}
			textInfoAniIE = null;
			goContainer.SetActive(value: false);
		}

		private IEnumerator TextInfoAniIE()
		{
			int count2 = 0;
			while (true)
			{
				string prefix = "正在同步鱼群 ";
				string postfix = string.Empty;
				count2++;
				count2 %= 14;
				for (int i = 0; i < count2; i++)
				{
					postfix += ((i % 2 == 0) ? " " : ".");
				}
				textInfo.text = prefix + postfix;
				yield return new WaitForSeconds(0.3f);
			}
		}
	}
}
