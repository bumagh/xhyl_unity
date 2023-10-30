using LL_UICommon;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DT_GameTipManager : MonoBehaviour
{
	public Button mEnsureBtnCol;

	public Text mTipContent;

	public Text mTime;

	private float overTime;

	private bool isOver = true;

	private Coroutine SendRoomCor;

	private void Awake()
	{
		mEnsureBtnCol.onClick.AddListener(GameOver);
	}

	public void SetTip(string msg, int time, bool isOver)
	{
		UnityEngine.Debug.LogError("收到提示: " + msg);
		if (SceneManager.GetActiveScene().name == "LL_Game")
		{
			if (isOver)
			{
				LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.OutGame, msg);
			}
			else
			{
				LL_GameTipManager.GetSingleton().ShowTip(EGameTipType.OutGame2, msg);
			}
		}
		base.gameObject.SetActive(value: true);
		base.transform.localScale = Vector3.one;
		this.isOver = isOver;
		overTime = 0f;
		mTipContent.text = msg;
		if (SendRoomCor != null)
		{
			StopCoroutine(SendRoomCor);
		}
		SendRoomCor = StartCoroutine(WaitOver(time));
	}

	private IEnumerator WaitOver(int time)
	{
		int tempTime = time;
		for (int i = 0; i < time; i++)
		{
			mTime.text = string.Format("({0})", tempTime.ToString("00"));
			yield return new WaitForSeconds(1f);
			tempTime--;
			if (tempTime <= 0)
			{
				tempTime = 0;
			}
		}
		GameOver();
	}

	private void GameOver()
	{
		if (isOver)
		{
			Application.Quit();
			if (base.gameObject != null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		else if (base.gameObject != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
