using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Demo
{
	public class FK3_OptionController : FK3_SimpleSingletonBehaviour<FK3_OptionController>
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

		private FK3_SafeAreaFitter safeAreaFitter;

		private GameObject optionItemBG;

		private void Awake()
		{
			FK3_SimpleSingletonBehaviour<FK3_OptionController>.s_instance = this;
			AddListener();
			safeAreaFitter = base.transform.Find("LeftPanel").GetComponent<FK3_SafeAreaFitter>();
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
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("左拉右拉按钮音效");
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("左拉右拉按钮音效", 1f);
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
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("返回界面弹出音效");
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("返回界面弹出音效", 1f);
			FK3_AlertDialog.Get().ShowDialog(ZH2_GVars.ShowTip("是否退出游戏？", "Whether to quit the game", string.Empty), showOkCancel: true, delegate
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
			FK3_UIIngameManager.GetInstance().OpenUI("OpScore");
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
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("帮助设置");
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("帮助设置", 1f);
			FK3_UIIngameManager.GetInstance().OpenUI("OpScore");
		}

		public void OnBtnSet_Click()
		{
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("帮助设置");
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("帮助设置", 1f);
			FK3_UIIngameManager.GetInstance().OpenUI("Set");
		}

		public void OnBtnRule_Click()
		{
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("帮助设置");
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("帮助设置", 1f);
			FK3_UIIngameManager.GetInstance().OpenUI("Rule");
		}

		public void OnBtnChat_Click()
		{
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("帮助设置");
			FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("帮助设置", 1f);
			UnityEngine.Debug.Log(FK3_LogHelper.Magenta("OnBtnChat_Click"));
			FK3_SimpleSingletonBehaviour<FK3_SendChatController>.Get().ShowChatWindow(-1);
		}
	}
}
