using M__M.HaiWang.UIDefine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Demo
{
	public class SaveAndTakeScores : SimpleSingletonBehaviour<SaveAndTakeScores>
	{
		[SerializeField]
		private Image _imgYinDao;

		[SerializeField]
		private Image _imgCheck;

		[SerializeField]
		private Text _txtGold;

		[SerializeField]
		private Text _txtScore;

		[SerializeField]
		private GameObject _scoreError;

		[SerializeField]
		private Button _btnSaveScores;

		[SerializeField]
		private Button _btnTakeScores;

		private float delay = 0.3f;

		private bool _bSave;

		private bool _bTake;

		private float _lastDdownTime;

		private int temp = 50;

		private int _gold;

		private int _score;

		private int _isYinDao;

		private InGameUIContext m_context = new InGameUIContext();

		public Action EventHandler_UI_PanelCoinInOut_OnHide;

		private int tempYinDao;

		public event Action EventHandler_UI_OnBtnCoinInClick;

		public event Action EventHandler_UI_OnBtnCoinOutClick;

		private void Awake()
		{
			SimpleSingletonBehaviour<SaveAndTakeScores>.s_instance = this;
			_btnSaveScores.onClick.AddListener(SaveScores);
			_btnTakeScores.onClick.AddListener(TakeScores);
			base.transform.Find("bg").GetComponent<Button>().onClick.AddListener(CloseGameObj);
		}

		private void CloseGameObj()
		{
			base.gameObject.SetActive(value: false);
		}

		public InGameUIContext GetContext()
		{
			return m_context;
		}

		public void HideInOutPanel()
		{
			if (EventHandler_UI_PanelCoinInOut_OnHide != null)
			{
				EventHandler_UI_PanelCoinInOut_OnHide();
			}
			base.gameObject.SetActive(value: false);
			if (_imgCheck.gameObject.activeSelf)
			{
				_isYinDao = 1;
				PlayerPrefs.SetInt("isYinDao2", _isYinDao);
			}
			_imgYinDao.SetActive(active: false);
		}

		private void OnEnable()
		{
			_scoreError.SetActive(value: false);
		}

		private void OnDisable()
		{
			_bSave = false;
			_bTake = false;
		}

		private void Update()
		{
			if (_bSave && Time.time - _lastDdownTime > delay)
			{
				delay = 0.1f;
				SaveScores();
				_lastDdownTime = Time.time;
			}
			if (_bTake && Time.time - _lastDdownTime > delay)
			{
				delay = 0.1f;
				TakeScores();
				_lastDdownTime = Time.time;
			}
			RefreshScorePanel();
		}

		private void RefreshScorePanel()
		{
			_gold = m_context.curGold;
			_score = m_context.curScore;
			_txtGold.text = m_context.curGold.ToString();
			_txtScore.text = m_context.curScore.ToString();
		}

		public void OnSaveScoreDown()
		{
			if (_score == 0)
			{
				ShowScoreError(0);
				return;
			}
			_bSave = true;
			_lastDdownTime = Time.time;
			if (this.EventHandler_UI_OnBtnCoinOutClick != null)
			{
				this.EventHandler_UI_OnBtnCoinOutClick();
			}
		}

		public void OnSaveScoreUp()
		{
			_bSave = false;
			delay = 0.3f;
		}

		private void ShowScoreError(int errorIndex)
		{
			string[] array = new string[3]
			{
				"分值不足",
				"币值不足",
				"取分不可超过90万"
			};
			_scoreError.GetComponentInChildren<Text>().text = array[errorIndex];
			_scoreError.transform.localPosition = ((errorIndex == 1) ? new Vector3(0f, 135f, 0f) : new Vector3(0f, 60f, 0f));
			if (!_scoreError.activeSelf)
			{
				_scoreError.SetActive(value: true);
				StartCoroutine(DelayCall(2f, delegate
				{
					_scoreError.gameObject.SetActive(value: false);
				}));
			}
		}

		public void SaveScores()
		{
			if (_score == 0)
			{
				ShowScoreError(0);
			}
			else if (this.EventHandler_UI_OnBtnCoinOutClick != null)
			{
				this.EventHandler_UI_OnBtnCoinOutClick();
			}
		}

		public void OnTakeScoresDown()
		{
			_bTake = true;
			_lastDdownTime = Time.time;
			TakeScores();
		}

		public void OnTakeScoresUp()
		{
			_bTake = false;
			delay = 0.3f;
		}

		public void TakeScores()
		{
			if (_gold == 0)
			{
				ShowScoreError(1);
			}
			else if (_score + HW2_GVars.lobby.GetCurDesk().onceExchangeValue * HW2_GVars.lobby.GetCurDesk().exchange > 900000)
			{
				ShowScoreError(2);
			}
			else if (this.EventHandler_UI_OnBtnCoinInClick != null)
			{
				this.EventHandler_UI_OnBtnCoinInClick();
			}
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			if (!hasFocus)
			{
				_bSave = false;
				_bTake = false;
			}
		}

		public void Reset_EventHandler()
		{
			this.EventHandler_UI_OnBtnCoinInClick = null;
			this.EventHandler_UI_OnBtnCoinOutClick = null;
			EventHandler_UI_PanelCoinInOut_OnHide = null;
		}

		private IEnumerator DelayCall(float time, Action call)
		{
			yield return new WaitForSeconds(time);
			call();
		}

		public IEnumerator GunMoveDelay()
		{
			base.gameObject.SetActive(value: false);
			yield return new WaitForSeconds(1.7f);
			base.gameObject.SetActive(value: true);
			tempYinDao = PlayerPrefs.GetInt("isYinDao2");
			if (tempYinDao == 1)
			{
				_imgYinDao.SetActive(active: false);
			}
			else
			{
				_imgYinDao.SetActive();
			}
		}
	}
}
