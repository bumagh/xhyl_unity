using M__M.GameHall.Common;
using M__M.HaiWang.Fish;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Demo
{
	public class FK3_SynchronizeHint : FK3_MB_Singleton<FK3_SynchronizeHint>
	{
		private GameObject goContainer;

		[SerializeField]
		private Text textInfo;

		private Coroutine textInfoAniIE;

		private float timeCount = 1f;

		private void Awake()
		{
			if (FK3_MB_Singleton<FK3_SynchronizeHint>._instance == null)
			{
				FK3_MB_Singleton<FK3_SynchronizeHint>.SetInstance(this);
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
				int count = FK3_FishMgr.Get().GetScreenFishList(delegate(FK3_FishBehaviour _fish)
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
				string prefix = ZH2_GVars.ShowTip("正在同步鱼群 ", "Synchronizing the fish ", string.Empty);
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
