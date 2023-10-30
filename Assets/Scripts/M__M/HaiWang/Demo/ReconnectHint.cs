using M__M.GameHall.Common;
using M__M.HaiWang.UIDefine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Demo
{
	public class ReconnectHint : HW2_MB_Singleton<ReconnectHint>
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
			if (HW2_MB_Singleton<ReconnectHint>._instance == null)
			{
				HW2_MB_Singleton<ReconnectHint>.SetInstance(this);
				PreInit();
			}
			Object.DontDestroyOnLoad(base.gameObject);
			HW2_GVars.dontDestroyOnLoadList.Add(base.gameObject);
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
				HW2_MB_Singleton<HW2_AppManager>.Get()._timeoutQuit();
			}
		}

		public void Show()
		{
			if (!_goContainer.activeSelf)
			{
				UnityEngine.Debug.Log(HW2_LogHelper.Magenta("ReconnectHint Show"));
				float z = (HW2_GVars.m_curState != Demo_UI_State.InGame) ? 0f : ((HW2_GVars.lobby.curSeatId > 2) ? 180f : 0f);
				base.transform.localEulerAngles = new Vector3(0f, 0f, z);
				_goContainer.SetActive(value: true);
				_setUIEnable(isEnable: false);
				_coTextInfoAni = StartCoroutine(_textInfoAni());
			}
		}

		public void Hide()
		{
			UnityEngine.Debug.Log(HW2_LogHelper.Magenta("ReconnectHint Hide"));
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
				string prefix = (HW2_GVars.language == "zh") ? "网络断开，正在重连 " : "Reconnecting to the network ";
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
