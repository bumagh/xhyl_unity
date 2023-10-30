using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JSYS_UI : MonoBehaviour
{
	public static JSYS_LL_PersonInfo mUserInfo;

	public static JSYS_UI publicUI;

	[Header("声音按钮图片链表")]
	public List<Sprite> AudioButtonSprite = new List<Sprite>();

	private bool AudioButtonBool;

	[Header("声音组建链表")]
	public List<AudioSource> AudioSourceLiat = new List<AudioSource>();

	private bool SettingBool;

	[Header("提示界面")]
	public GameObject PromptScene;

	private void Awake()
	{
		publicUI = this;
		Screen.sleepTimeout = -1;
	}

	private void Start()
	{
		ZH2_GVars.closeSafeBox = (Action)Delegate.Combine(ZH2_GVars.closeSafeBox, new Action(CloseSafeBox));
		ZH2_GVars.saveScore = (Action)Delegate.Combine(ZH2_GVars.saveScore, new Action(SaveScore));
	}

	private void CloseSafeBox()
	{
		JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendUserCoinIn();
	}

	private void SaveScore()
	{
		JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendUserCoinOut();
	}

	private void OnDisable()
	{
		ZH2_GVars.closeSafeBox = (Action)Delegate.Remove(ZH2_GVars.closeSafeBox, new Action(CloseSafeBox));
		ZH2_GVars.saveScore = (Action)Delegate.Remove(ZH2_GVars.saveScore, new Action(SaveScore));
	}

	public void OnClickSafeBox()
	{
		if (ZH2_GVars.OpenCheckSafeBoxPwdPanel != null)
		{
			ZH2_GVars.OpenCheckSafeBoxPwdPanel(ZH2_GVars.GameType_DJ.jsys_desk);
		}
	}

	public void OnClickToUp()
	{
		if (ZH2_GVars.OpenPlyBoxPanel != null)
		{
			ZH2_GVars.OpenPlyBoxPanel(ZH2_GVars.GameType_DJ.jsys_desk);
		}
	}

	public void SettingButton(GameObject Obj)
	{
		SettingBool = !SettingBool;
		if (SettingBool)
		{
			Obj.SetActive(value: true);
		}
		else
		{
			Obj.SetActive(value: false);
		}
	}

	public void AudioButtonMethon(Image image)
	{
		AudioButtonBool = !AudioButtonBool;
		int num = JSYS_LL_GameInfo.getInstance().Language * 2;
		if (AudioButtonBool)
		{
			image.sprite = AudioButtonSprite[num];
			foreach (AudioSource item in AudioSourceLiat)
			{
				item.GetComponent<AudioSource>().mute = true;
			}
		}
		else
		{
			image.sprite = AudioButtonSprite[num + 1];
			foreach (AudioSource item2 in AudioSourceLiat)
			{
				item2.GetComponent<AudioSource>().mute = false;
			}
		}
	}

	public void dropOutButton()
	{
		ZH2_GVars.isFirstToDaTing = false;
		ZH2_GVars.isGameToDaTing = true;
		JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveSeat(JSYS_LL_GameInfo.getInstance().UserInfo.TableId, JSYS_LL_GameInfo.getInstance().UserInfo.SeatId);
		JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveDesk(JSYS_LL_GameInfo.getInstance().UserInfo.TableId);
		JSYS_LL_NetMngr.GetSingleton().MyCreateSocket.SendLeaveRoom(JSYS_LL_GameInfo.getInstance().UserInfo.TableId);
		JSYS_LL_GameMsgMngr.GetSingleton().On_UIBackBtn_Press();
		JSYS_LL_GameInfo.getInstance().UserInfo.TableId = -1;
		JSYS_LL_GameInfo.getInstance().UserInfo.SeatId = -1;
		SceneManager.LoadScene("JSYS_UIScene");
	}

	public void KaiScene(GameObject Scene)
	{
		Scene.SetActive(value: true);
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
}
