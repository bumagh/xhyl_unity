using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LKB_Cell : MonoBehaviour
{
	private LKB_ShaderAni shaderAni;

	private LKB_CellAnimator cAnim;

	[SerializeField]
	private LKB_Border border;

	public UnityEvent onCelebrateEnd;

	private bool bPreInit;

	private Dictionary<string, bool> eventRecord = new Dictionary<string, bool>();

	public static float CelebrateDuration = 3f;

	public static float FlashDuration = 0.8f;

	public LKB_CellType CurType
	{
		get;
		set;
	}

	public LKB_CellState CurState
	{
		get;
		set;
	}

	private void Awake()
	{
		PreInit();
	}

	private void Start()
	{
		border.Show(isShow: false);
	}

	private void PreInit()
	{
		if (!bPreInit)
		{
			cAnim = GetComponent<LKB_CellAnimator>();
		}
	}

	public void PlayState(LKB_CellState cSF_CellState = LKB_CellState.normal, LKB_CellType type = LKB_CellType.None)
	{
		CurType = ((type == LKB_CellType.None) ? CurType : type);
		if (CurType == LKB_CellType.None)
		{
			cAnim.Play((int)CurType * 5);
			return;
		}
		switch (cSF_CellState)
		{
		case LKB_CellState.normal:
			cAnim.Play((int)(CurType - 1) * 5);
			CurState = LKB_CellState.normal;
			break;
		case LKB_CellState.gray:
			cAnim.Play((int)(CurType - 1) * 5 + 1);
			CurState = LKB_CellState.gray;
			break;
		case LKB_CellState.flash:
			cAnim.Play((int)(CurType - 1) * 5 + 2);
			CurState = LKB_CellState.flash;
			break;
		case LKB_CellState.celebrate:
			cAnim.Play((int)(CurType - 1) * 5 + 3);
			CurState = LKB_CellState.celebrate;
			break;
		case LKB_CellState.bounce:
			cAnim.Play((int)(CurType - 1) * 5 + 4);
			CurState = LKB_CellState.bounce;
			break;
		}
	}

	public void OnAniEvent(string aniEvent)
	{
		if (onCelebrateEnd != null)
		{
			onCelebrateEnd.Invoke();
		}
	}

	public void ResetCell()
	{
		if (!bPreInit)
		{
			PreInit();
		}
		cAnim.AnimScroll.gameObject.SetActive(value: false);
		border.Show(isShow: false);
		PlayState();
	}

	private void OnAnimatorEvent(string eventName)
	{
		eventRecord[eventName] = true;
	}

	private IEnumerator _waitForEventHappen(string eventName, float timeOut)
	{
		float timer = 0f;
		bool happened = false;
		if (eventRecord.TryGetValue(eventName, out happened) && happened)
		{
			UnityEngine.Debug.LogError(eventName + " is already happened");
		}
		while (timer < timeOut)
		{
			timer += Time.deltaTime;
			if (eventRecord.TryGetValue(eventName, out happened) && happened)
			{
				break;
			}
			yield return null;
		}
	}

	public void RolllingStart()
	{
		eventRecord.Clear();
		PlayState(LKB_CellState.bounce, CurType);
	}

	public IEnumerator RolllingEnd(LKB_CellType endType)
	{
		PlayState(LKB_CellState.normal, endType);
		yield return StartCoroutine(cAnim.AnimScroll.AniStop(endType));
		base.transform.DOBlendableLocalMoveBy(Vector3.down * 5f, 0.1f).OnComplete(delegate
		{
			transform.DOBlendableLocalMoveBy(Vector3.up * 5f, 0.1f);
			LKB_SoundManager.Instance.PlayStopAndio();
		});
		yield return StartCoroutine(_waitForEventHappen("Bounce:End", 0.5f));
		PlayState();
	}

	public void ShowBorder(string state = "green")
	{
		border.Show(isShow: true);
		border.PlaySate(state);
	}

	public void HideBorder()
	{
		border.Show(isShow: false);
	}
}
