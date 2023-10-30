using BCBM_UICommon;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BCBM_UI : MonoBehaviour
{
	public static BCBM_PersonInfo mUserInfo;

	public static BCBM_UI publicUI;

	private bool AudioButtonBool;

	private bool HelpButtonBool;

	[Header("声音组建链表")]
	public List<AudioSource> AudioSourceLiat = new List<AudioSource>();

	private bool SettingBool;

	[Header("提示界面")]
	public GameObject PromptScene;

	public Transform setBtnArr;

	public Transform setPanel;

	public Transform setPanelOldPos;

	public Transform setPanelTagPos;

	public bool isMoveOver = true;

	private void Awake()
	{
		publicUI = this;
		Screen.sleepTimeout = -1;
	}

	public void OnEnable1()
	{
		if (setPanel != null)
		{
			setPanel.localPosition = setPanelOldPos.localPosition;
		}
		if (setBtnArr != null)
		{
			setBtnArr.localScale = Vector3.one;
		}
		isMoveOver = true;
		SettingBool = false;
	}

	public void SettingButton()
	{
		if (isMoveOver)
		{
			isMoveOver = false;
			SettingBool = !SettingBool;
			if (SettingBool)
			{
				Transform target = setPanel;
				Vector3 localPosition = setPanelTagPos.localPosition;
				target.DOLocalMoveY(localPosition.y, 0.25f).OnComplete(delegate
				{
					isMoveOver = true;
				});
				setBtnArr.localScale = new Vector3(1f, -1f, 1f);
			}
			else
			{
				Transform target2 = setPanel;
				Vector3 localPosition2 = setPanelOldPos.localPosition;
				target2.DOLocalMoveY(localPosition2.y, 0.5f).OnComplete(delegate
				{
					isMoveOver = true;
				});
				setBtnArr.localScale = Vector3.one;
			}
		}
	}

	public void OnClickSafeBox()
	{
		if (ZH2_GVars.OpenCheckSafeBoxPwdPanel != null)
		{
			ZH2_GVars.OpenCheckSafeBoxPwdPanel(ZH2_GVars.GameType_DJ.bcbm_desk);
		}
	}

	public void OnClickToUp()
	{
		if (ZH2_GVars.OpenPlyBoxPanel != null)
		{
			ZH2_GVars.OpenPlyBoxPanel(ZH2_GVars.GameType_DJ.bcbm_desk);
		}
	}

	public void AudioButtonMethon()
	{
		AudioButtonBool = !AudioButtonBool;
		if (AudioButtonBool)
		{
			foreach (AudioSource item in AudioSourceLiat)
			{
				item.GetComponent<AudioSource>().mute = true;
			}
		}
		else
		{
			foreach (AudioSource item2 in AudioSourceLiat)
			{
				item2.GetComponent<AudioSource>().mute = false;
			}
		}
	}

	public void OnClickExitGame()
	{
		if (BCBM_BetScene.publicBetScene != null && BCBM_BetScene.publicBetScene.CounterText.text == string.Empty)
		{
			UnityEngine.Debug.LogError("=====开奖中====");
			All_GameMiniTipPanel.publicMiniTip.ShowTip("请在倒计时退出");
		}
		else
		{
			BCBM_GameTipManager.GetSingleton().ShowTip(EGameTipType.IsExitGame, string.Empty);
		}
	}

	public void dropOutButton()
	{
		UnityEngine.Debug.LogError("=======5");
		BCBM_NetMngr.GetSingleton().MyCreateSocket.SendQuitGame();
		ZH2_GVars.isStartedFromGame = true;
		GameObject gameObject = GameObject.Find("netMngr");
		GameObject gameObject2 = GameObject.Find("GameMngr");
		if (gameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		else
		{
			UnityEngine.Debug.LogError("====netMngr===为空");
		}
		if (gameObject2 != null)
		{
			UnityEngine.Object.Destroy(gameObject2);
		}
		else
		{
			UnityEngine.Debug.LogError("====gameMngr===为空");
		}
		SceneManager.LoadScene("MainScene");
	}

	private void Clear()
	{
	}

	public void KaiScene(GameObject Scene)
	{
		Scene.SetActive(value: true);
	}

	public void HelpBtn(GameObject Scene)
	{
		HelpButtonBool = !HelpButtonBool;
		if (HelpButtonBool)
		{
			Scene.SetActive(value: true);
		}
		else
		{
			Scene.SetActive(value: false);
		}
	}

	public void GuanScene(GameObject Scene)
	{
		Scene.SetActive(value: false);
	}

	public void PromptMethon(string game)
	{
		PromptScene.transform.GetChild(2).GetComponent<Text>().text = game;
		PromptScene.SetActive(value: true);
	}

	private void OnApplicationQuit()
	{
	}
}
