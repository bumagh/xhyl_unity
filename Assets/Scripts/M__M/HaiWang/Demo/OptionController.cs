using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Demo
{
	public class OptionController : SimpleSingletonBehaviour<OptionController>
	{
		[SerializeField]
		private Transform _option;

		public Transform _option2;

		public Transform _option3;

		[SerializeField]
		private Image _imgOption;

		private bool _bIsBack;

		[SerializeField]
		private Button _btnBack;

		[SerializeField]
		private Button _btnOpScore;

		[SerializeField]
		private Button _btnSet;

		[SerializeField]
		private Button _btnRule;

		[SerializeField]
		private Button _btnChat;

		[SerializeField]
		private RectTransform OpScore;

		public Action Event_OnBackToLobby;

		private Tweener tweener;

		private Vector3 oldPos;

		private Vector3 tagPos;

		private SafeAreaFitter safeAreaFitter;

		private GameObject optionItemBG;

		private void Awake()
		{
			SimpleSingletonBehaviour<OptionController>.s_instance = this;
			AddListener();
			safeAreaFitter = base.transform.Find("LeftPanel").GetComponent<SafeAreaFitter>();
			optionItemBG = _option.Find("optionItem").gameObject;
		}

		private void AddListener()
		{
			_btnBack.onClick.AddListener(QuitGameToDesk);
			_btnOpScore.onClick.AddListener(Show_InOutPanel);
			_btnSet.onClick.AddListener(OnBtnSet_Click);
			_btnRule.onClick.AddListener(OnBtnRule_Click);
			_btnChat.onClick.AddListener(OnBtnChat_Click);
			_imgOption.GetComponent<Button>().onClick.AddListener(MoveTotarget);
		}

		private void Start()
		{
			oldPos = _option.localPosition;
			tagPos = _option2.localPosition;
			if (safeAreaFitter.bangSize > 0f)
			{
				optionItemBG.SetActive(value: false);
			}
			else
			{
				optionItemBG.SetActive(value: true);
			}
		}

		private void OnEnable()
		{
			try
			{
				UIIngameManager.GetInstance().CloseUI("Set");
				UIIngameManager.GetInstance().CloseUI("Rule");
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
		}

		public void Init()
		{
			if (_bIsBack)
			{
				MoveTotarget();
			}
		}

		private void Update()
		{
		}

		public void MoveTotarget()
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("左拉右拉按钮音效");
			HW2_Singleton<SoundMgr>.Get().SetVolume("左拉右拉按钮音效", 1f);
			if (!_bIsBack && (tweener == null || !tweener.IsPlaying()))
			{
				optionItemBG.SetActive(value: true);
				tweener = _option.transform.DOLocalMoveX(tagPos.x, 0.45f);
				_bIsBack = true;
				GoToTag(0.42f, _option, _option2.localPosition);
			}
		}

		private void GoToTag(float time, Transform pos, Vector3 tagPos, bool isHide = false)
		{
			StartCoroutine(WaitToTag(time, pos, tagPos, isHide));
		}

		private IEnumerator WaitToTag(float time, Transform pos, Vector3 tagPos, bool isHide)
		{
			yield return new WaitForSeconds(time);
			pos.localPosition = tagPos;
			if (isHide)
			{
				optionItemBG.SetActive(value: false);
			}
		}

		public void QuitGameToDesk()
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("返回界面弹出音效");
			HW2_Singleton<SoundMgr>.Get().SetVolume("返回界面弹出音效", 1f);
			HW2_AlertDialog.Get().ShowDialog("是否退出游戏？", showOkCancel: true, delegate
			{
				if (Event_OnBackToLobby != null)
				{
					Event_OnBackToLobby();
				}
				else
				{
					UnityEngine.Debug.LogError("Event_OnBackToLobby为空");
				}
			});
		}

		public void Reset_EventHandler()
		{
			Event_OnBackToLobby = null;
		}

		public void Show_InOutPanel()
		{
			if (HW2_GVars.lobby.user.gameGold == 0 && HW2_GVars.lobby.curRoomId == 2 && HW2_GVars.lobby.user.gameScore == 0)
			{
				HW2_AlertDialog.Get().ShowDialog("游戏币不足");
			}
			else if (HW2_GVars.lobby.user.expeGold == 0 && HW2_GVars.lobby.curRoomId == 1 && HW2_GVars.lobby.user.gameScore == 0)
			{
				HW2_AlertDialog.Get().ShowDialog("体验币不足");
			}
			else
			{
				UIIngameManager.GetInstance().OpenUI("OpScore");
			}
		}

		public void HideOption()
		{
			if (_bIsBack && (tweener == null || !tweener.IsPlaying()))
			{
				optionItemBG.SetActive(value: true);
				tweener = _option.transform.DOLocalMoveX(oldPos.x, 0.45f);
				_bIsBack = false;
				GoToTag(0.42f, _option, _option3.localPosition, (safeAreaFitter.bangSize > 0f) ? true : false);
			}
		}

		public void OnBtnUpScore_Click()
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("帮助设置");
			HW2_Singleton<SoundMgr>.Get().SetVolume("帮助设置", 1f);
			UIIngameManager.GetInstance().OpenUI("OpScore");
		}

		public void OnBtnSet_Click()
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("帮助设置");
			HW2_Singleton<SoundMgr>.Get().SetVolume("帮助设置", 1f);
			UIIngameManager.GetInstance().OpenUI("Set");
		}

		public void OnBtnRule_Click()
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("帮助设置");
			HW2_Singleton<SoundMgr>.Get().SetVolume("帮助设置", 1f);
			UIIngameManager.GetInstance().OpenUI("Rule");
		}

		public void OnBtnChat_Click()
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("帮助设置");
			HW2_Singleton<SoundMgr>.Get().SetVolume("帮助设置", 1f);
			UnityEngine.Debug.Log(HW2_LogHelper.Magenta("OnBtnChat_Click"));
			SimpleSingletonBehaviour<SendChatController>.Get().ShowChatWindow(-1);
		}
	}
}
