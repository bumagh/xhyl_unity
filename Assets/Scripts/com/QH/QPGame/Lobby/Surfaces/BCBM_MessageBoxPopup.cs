using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace com.QH.QPGame.Lobby.Surfaces
{
	public class BCBM_MessageBoxPopup : MonoBehaviour
	{
		public BCBM_MessageBoxCallback2 callback;

		public Text Title_lbl;

		public Text Text_lbl;

		public GameObject Close_btn;

		public GameObject Confirm_btn;

		public GameObject Ok_btn;

		public GameObject Cancel_btn;

		public Transform Front_panel;

		public BCBM_MessageBoxResult m_mResultStyle;

		public bool IsShown()
		{
			return base.gameObject.gameObject.activeSelf;
		}

		public void Show(string title, string message, BCBM_ButtonStyle _ButtonStyle, BCBM_MessageBoxCallback2 _callback = null, float _fWaitTime = 5f)
		{
			base.gameObject.SetActive(value: true);
			Close_btn.SetActive(value: false);
			Ok_btn.SetActive(value: false);
			Confirm_btn.SetActive(value: false);
			Cancel_btn.SetActive(value: false);
			if ((_ButtonStyle & BCBM_ButtonStyle.Confirm) != 0)
			{
				Confirm_btn.SetActive(value: true);
			}
			if ((_ButtonStyle & BCBM_ButtonStyle.Yes) != 0)
			{
				Ok_btn.SetActive(value: true);
			}
			if ((_ButtonStyle & BCBM_ButtonStyle.No) != 0)
			{
				Cancel_btn.SetActive(value: true);
			}
			if ((_ButtonStyle & BCBM_ButtonStyle.OK) != 0)
			{
				Ok_btn.SetActive(value: true);
			}
			if ((_ButtonStyle & BCBM_ButtonStyle.Cancel) != 0)
			{
				Cancel_btn.SetActive(value: true);
			}
			Title_lbl.text = title;
			Text_lbl.text = message;
			callback = _callback;
			if (_fWaitTime > 0f)
			{
				StartCoroutine(WaitTime(_fWaitTime));
			}
		}

		public void Show(string title, string message, BCBM_ButtonStyle _ButtonStyle)
		{
			base.gameObject.SetActive(value: true);
			Close_btn.SetActive(value: false);
			Ok_btn.SetActive(value: false);
			Confirm_btn.SetActive(value: false);
			Cancel_btn.SetActive(value: false);
			if ((_ButtonStyle & BCBM_ButtonStyle.Confirm) != 0)
			{
				Confirm_btn.SetActive(value: true);
			}
			if ((_ButtonStyle & BCBM_ButtonStyle.Yes) != 0)
			{
				Ok_btn.SetActive(value: true);
			}
			if ((_ButtonStyle & BCBM_ButtonStyle.No) != 0)
			{
				Cancel_btn.SetActive(value: true);
			}
			if ((_ButtonStyle & BCBM_ButtonStyle.OK) != 0)
			{
				Ok_btn.SetActive(value: true);
			}
			if ((_ButtonStyle & BCBM_ButtonStyle.Cancel) != 0)
			{
				Cancel_btn.SetActive(value: true);
			}
			Title_lbl.text = title;
			Text_lbl.text = message;
		}

		public void Confirm(string title, string message, BCBM_MessageBoxCallback2 _callback = null, float _fWaitTime = 5f)
		{
			Show(title, message, BCBM_ButtonStyle.Confirm, _callback, _fWaitTime);
		}

		public void Onclick_Confirm()
		{
			if (callback != null)
			{
				callback(BCBM_MessageBoxResult.Confirm);
			}
		}

		public void Onclick_Yes()
		{
			if (callback != null)
			{
				callback(BCBM_MessageBoxResult.Yes);
			}
			base.gameObject.SetActive(value: false);
		}

		public void Onclick_No()
		{
			if (callback != null)
			{
				callback(BCBM_MessageBoxResult.No);
			}
			base.gameObject.SetActive(value: false);
		}

		public void Onclick_Ok()
		{
			if (callback != null)
			{
				callback(BCBM_MessageBoxResult.OK);
			}
			base.gameObject.SetActive(value: false);
		}

		public void Onclick_Cancel()
		{
			if (callback != null)
			{
				callback(BCBM_MessageBoxResult.Cancel);
			}
			base.gameObject.SetActive(value: false);
		}

		public void Onclick_WaitTime()
		{
			if (callback != null)
			{
				callback(BCBM_MessageBoxResult.Timeout);
			}
			base.gameObject.SetActive(value: false);
		}

		private IEnumerator WaitTime(float _time)
		{
			string text = Title_lbl.text;
			while (_time > 0f)
			{
				yield return new WaitForFixedUpdate();
				_time -= Time.deltaTime;
				if (_time < 30f)
				{
					Title_lbl.text = text + $" ({(int)_time})";
				}
			}
			Onclick_WaitTime();
		}
	}
}
