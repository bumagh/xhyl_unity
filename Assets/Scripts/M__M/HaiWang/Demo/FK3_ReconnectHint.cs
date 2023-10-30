using M__M.GameHall.Common;
using M__M.HaiWang.UIDefine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Demo
{
	public class FK3_ReconnectHint : FK3_MB_Singleton<FK3_ReconnectHint>
	{
		private GameObject _goContainer;

		[SerializeField]
		private Text _textInfo;

		[SerializeField]
		private GameObject _goUI;

		private Coroutine _coTextInfoAni;

		private float countTime;

		private void Awake()
		{
			if (FK3_MB_Singleton<FK3_ReconnectHint>._instance == null)
			{
				FK3_MB_Singleton<FK3_ReconnectHint>.SetInstance(this);
				PreInit();
			}
			Object.DontDestroyOnLoad(base.gameObject);
			FK3_GVars.dontDestroyOnLoadList.Add(base.gameObject);
			base.gameObject.SetActive(value: false);
		}

		public void PreInit()
		{
			_goContainer = base.gameObject;
		}

		private void OnEnable()
		{
			countTime = 0f;
		}

		private void Update()
		{
			countTime += Time.deltaTime;
			if (countTime > 25f)
			{
				FK3_MB_Singleton<FK3_AppManager>.Get()._timeoutQuit();
			}
		}

		public void Show()
		{
			if (!_goContainer.activeSelf)
			{
				float z = (FK3_GVars.m_curState != FK3_Demo_UI_State.InGame) ? 0f : ((FK3_GVars.lobby.curSeatId > 2) ? 180f : 0f);
				base.transform.localEulerAngles = new Vector3(0f, 0f, z);
				_goContainer.SetActive(value: true);
				_setUIEnable(isEnable: false);
				_coTextInfoAni = StartCoroutine(_textInfoAni());
			}
		}

		public void Hide()
		{
			_goContainer.SetActive(value: false);
			if (_coTextInfoAni != null)
			{
				StopCoroutine(_coTextInfoAni);
			}
			_coTextInfoAni = null;
		}

		private void _setUIEnable(bool isEnable)
		{
			_goUI.SetActive(isEnable);
		}

		public void OnBtnBg_Click()
		{
		}

		private IEnumerator _textInfoAni()
		{
			int count2 = 0;
			yield return new WaitForSeconds(0f);
			_setUIEnable(isEnable: true);
			while (true)
			{
				string prefix = (FK3_GVars.language == "zh") ? "网络断开，正在重连 " : "Reconnecting to the network ";
				string postfix = string.Empty;
				count2++;
				count2 %= 8;
				for (int i = 0; i < count2; i++)
				{
					postfix += ((i == 3) ? " " : "·");
				}
				_textInfo.text = prefix + postfix;
				yield return new WaitForSeconds(0.3f);
			}
		}
	}
}
