using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BCBM_Control : MonoBehaviour
{
	public static BCBM_Control publicControl;

	[Header("下注界面动画资源[0向上][1向下]")]
	public List<AnimationClip> BetanimationClips = new List<AnimationClip>();

	public List<Sprite> Recording = new List<Sprite>();

	[Header("记录父物体")]
	public GameObject RecordingObj;

	[Header("记录父物体")]
	public GameObject RecordingObj2;

	[Header("等待提示")]
	public GameObject waitScene;

	[Header("奖池链表")]
	public List<GameObject> Box = new List<GameObject>();

	private int Counter;

	private int Num;

	public int ExceedTime;

	public GameObject ExceedTimeScene;

	public List<Sprite> sprites;

	public BCBM_GetPlayerList playerList;

	private string allScore = "0";

	private float waitTime;

	private bool jiesuan;

	private bool show1;

	public static bool isMoreInfo;

	private bool bAllNoticeHasEnd = true;

	private GameObject objNotice;

	private List<string> listNotice = new List<string>();

	private Text txtNotice;

	private int Yanzinum;

	private int roundnum;

	public List<int> NumList = new List<int>();

	public Transform ContentOldPos;

	public Transform ContentTagPos;

	private void Awake()
	{
		publicControl = this;
		BCBM_NetMngr.GetSingleton().MyCreateSocket.SendResultList(BCBM_GameInfo.getInstance().UserInfo.TableId);
	}

	private void Start()
	{
		if (BCBM_AnimationScene.publicAnimationScene != null)
		{
			BCBM_AnimationScene.publicAnimationScene.HidCar();
		}
		ZH2_GVars.isCanSenEnterGame = true;
		ZH2_GVars.closeSafeBox = (Action)Delegate.Combine(ZH2_GVars.closeSafeBox, new Action(CloseSafeBox));
		ZH2_GVars.saveScore = (Action)Delegate.Combine(ZH2_GVars.saveScore, new Action(SaveScore));
	}

	private void OnDisable()
	{
		ZH2_GVars.closeSafeBox = (Action)Delegate.Remove(ZH2_GVars.closeSafeBox, new Action(CloseSafeBox));
		ZH2_GVars.saveScore = (Action)Delegate.Remove(ZH2_GVars.saveScore, new Action(SaveScore));
	}

	private void CloseSafeBox()
	{
		BCBM_NetMngr.GetSingleton().MyCreateSocket.SendUserCoinIn();
	}

	private void SaveScore()
	{
		BCBM_NetMngr.GetSingleton().MyCreateSocket.SendUserCoinOut();
	}

	private void Update()
	{
		if (!jiesuan)
		{
			return;
		}
		waitTime += Time.deltaTime;
		if (!(waitTime > 3f))
		{
			return;
		}
		if (!show1)
		{
			show1 = true;
		}
		else if (waitTime > 6f)
		{
			if (BCBM_link.moreInfo != -1)
			{
				Box[Counter].transform.GetChild(0).gameObject.SetActive(value: false);
				waitTime = 0f;
				isMoreInfo = true;
				jiesuan = false;
				show1 = false;
				BCBM_link.Time_Dji = 17;
				UnityEngine.Debug.LogError("===jiesuan开始旋转===");
				BCBM_link.publiclink.parameter(BCBM_link.Time_Dji, BCBM_link.animalInfo, BCBM_link.moreInfo, BCBM_link.coloer_ani);
				BCBM_link.name_ani = BCBM_link.moreInfo;
				BCBM_link.animal_ani = BCBM_link.animalInfo;
				BCBM_link.moreInfo = -1;
				BCBM_link.animalInfo = -1;
			}
			else
			{
				show1 = false;
				Box[Counter].transform.GetChild(0).gameObject.SetActive(value: false);
				BCBM_BetScene.publicBetScene.TimeMethon(2);
				jiesuan = false;
				waitTime = 0f;
			}
		}
	}

	public void AddNotice(string noticeMessage)
	{
		if (bAllNoticeHasEnd)
		{
			UpdateNotice(noticeMessage);
			objNotice.SetActive(value: true);
			bAllNoticeHasEnd = false;
		}
		else
		{
			listNotice.Add(noticeMessage);
		}
	}

	private void UpdateNotice(string noticeMessage)
	{
		txtNotice.text = noticeMessage;
		txtNotice.transform.DOScale(1f, 0.02f).OnComplete(delegate
		{
			Vector2 sizeDelta = txtNotice.rectTransform.sizeDelta;
			float num = sizeDelta.x / 2f;
			txtNotice.transform.localPosition = Vector3.right * (num + 300f);
			float duration = (num + 600f) / 200f;
			txtNotice.transform.DOLocalMoveX(0f - num - 300f, duration).OnComplete(NoticeEnd);
		});
	}

	public void NoticeEnd()
	{
		if (listNotice.Count == 0)
		{
			bAllNoticeHasEnd = true;
			objNotice.SetActive(value: false);
		}
		else
		{
			UpdateNotice(listNotice[0]);
			listNotice.RemoveAt(0);
		}
	}

	public void Init()
	{
		jiesuan = false;
		waitTime = 0f;
		roundnum = 0;
	}

	public void BoxRes()
	{
		for (int i = 0; i < Box.Count; i++)
		{
			GameObject gameObject = Box[i].transform.GetChild(1).gameObject;
			if (gameObject.activeInHierarchy)
			{
				gameObject.SetActive(value: false);
			}
		}
	}

	public IEnumerator Open(int animal, int Name)
	{
		BCBM_BetScene.publicBetScene.isStartGame = false;
		yield return new WaitForSeconds(1f);
		BCBM_BetScene.publicBetScene.lastBetNum = (int)BCBM_BetScene.publicBetScene.displaylaue[2];
		Num = Name;
		AddNumList(animal);
		UnityEngine.Debug.LogError("=======" + Name + "=========" + Box[Name].name);
		float Sdu = 0.55f;
		Yanzinum = 0;
		while (true)
		{
			if (Counter == 0)
			{
				roundnum++;
			}
			if (Counter > 0)
			{
				Box[Counter - 1].transform.GetChild(0).gameObject.GetComponent<BCBM_TimeGuanBi>().GuanBi1();
			}
			else
			{
				Box[Box.Count - 1].transform.GetChild(0).gameObject.GetComponent<BCBM_TimeGuanBi>().GuanBi1();
			}
			Box[Counter].transform.GetChild(0).gameObject.SetActive(value: true);
			if (roundnum <= 3)
			{
				if (Sdu > 0.05f)
				{
					Sdu -= 0.05f;
				}
				if (isMoreInfo && roundnum > 1)
				{
					if (Counter < Box.Count - 4)
					{
						if (Counter + 4 == Num)
						{
							Sdu = 0.6f;
							Yanzinum = Counter + 4;
						}
					}
					else if (Counter + 4 - Box.Count == Num)
					{
						Sdu = 0.6f;
						Yanzinum = Counter + 4 - Box.Count;
					}
				}
				else if (Sdu > 0.05f)
				{
					Sdu -= 0.05f;
				}
			}
			else
			{
				if (Counter < Box.Count - 4)
				{
					if (Counter + 4 == Num)
					{
						Sdu = 0.6f;
						Yanzinum = Counter + 4;
					}
				}
				else if (Counter + 4 - Box.Count == Num)
				{
					Sdu = 0.6f;
					Yanzinum = Counter + 4 - Box.Count;
				}
				if (Sdu < 0.2f)
				{
					Sdu += 0.01f;
				}
			}
			if (Counter == Num && Counter == Yanzinum && (roundnum > 3 || (isMoreInfo && roundnum > 1)))
			{
				break;
			}
			Counter = (Counter + 1) % Box.Count;
			BCBM_Audio.publicAudio.BgAudioMethon("开始旋转");
			BCBM_Audio.publicAudio.ButtonAudioMethon();
			yield return new WaitForSeconds(Sdu);
		}
		isMoreInfo = false;
		int counter = Counter;
		UnityEngine.Debug.LogError("======AudioNum: " + counter + "  animal: " + animal);
		Recordingmethon(animal);
		BCBM_AnimationScene.publicAnimationScene.ShowCar(animal);
		BCBM_BetScene.publicBetScene.ShowChipOver(animal);
		BCBM_NetMngr.GetSingleton().MyCreateSocket.SendResultList(BCBM_GameInfo.getInstance().UserInfo.TableId);
		jiesuan = true;
		Box[Counter].transform.GetChild(1).gameObject.SetActive(value: true);
		Box[Counter].transform.GetChild(0).gameObject.SetActive(value: false);
	}

	private void AddNumList(int num)
	{
		NumList.Add(num);
		if (num == 12)
		{
			NumList = new List<int>();
			for (int i = 0; i < num; i++)
			{
				NumList.Add(i);
			}
		}
	}

	public void BetWin()
	{
		for (int i = 0; i < NumList.Count; i++)
		{
			StartCoroutine(BCBM_BetScene.publicBetScene.ButtonAnimatorMethon(NumList[i]));
		}
	}

	public void BetSceneMethon(string name)
	{
	}

	public void ResetRecordingObj()
	{
	}

	public void Recordingmethon(int name)
	{
	}

	public void Recordingmethon2(int[] name)
	{
		RecordingObj.transform.localPosition = ContentOldPos.transform.localPosition;
		BetWin();
		RecordingObj.SetActive(value: true);
		RecordingObj2.SetActive(value: false);
		int childCount = RecordingObj.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			if (i < name.Length)
			{
				RecordingObj.transform.GetChild(i).gameObject.SetActive(value: true);
				RecordingObj.transform.GetChild(i).GetChild(0).gameObject.SetActive(value: false);
				int index = name[i];
				RecordingObj.transform.GetChild(i).GetComponent<Image>().sprite = Recording[index];
			}
			else
			{
				RecordingObj.transform.GetChild(i).gameObject.SetActive(value: false);
			}
		}
		RecordingObj.transform.GetChild(0).GetChild(0).gameObject.SetActive(value: true);
		RecordingObj.transform.DOLocalMove(ContentTagPos.localPosition, 1f);
	}

	public void Recordingmethon(int[] name)
	{
		Recordingmethon2(name);
		RecordingObj2.transform.DOScale(Vector3.one, 1f).OnComplete(delegate
		{
			RecordingObj2.SetActive(value: true);
			RecordingObj.SetActive(value: false);
			int childCount = RecordingObj2.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				if (i < name.Length)
				{
					RecordingObj2.transform.GetChild(i).gameObject.SetActive(value: true);
					RecordingObj2.transform.GetChild(i).GetChild(0).gameObject.SetActive(value: false);
					int index = name[i];
					RecordingObj2.transform.GetChild(i).GetComponent<Image>().sprite = Recording[index];
				}
				else
				{
					RecordingObj2.transform.GetChild(i).gameObject.SetActive(value: false);
				}
			}
			RecordingObj2.transform.GetChild(0).GetChild(0).gameObject.SetActive(value: true);
			RecordingObj.transform.localPosition = ContentOldPos.transform.localPosition;
		});
	}

	public void PlayerList(JsonData jsonData)
	{
		playerList.ShowPlayerList(jsonData);
	}

	public IEnumerator ExceedTimeMethon()
	{
		while (true)
		{
			ExceedTime++;
			if (ExceedTime == 0)
			{
				if (ExceedTimeScene.activeInHierarchy)
				{
					ExceedTimeScene.SetActive(value: false);
				}
			}
			else if (ExceedTime >= 2)
			{
				BCBM_link.publiclink.OpenNewTcpNet();
			}
			else if (ExceedTime >= 5)
			{
				ExceedTimeScene.SetActive(value: true);
			}
			yield return new WaitForSeconds(1f);
		}
	}
}
