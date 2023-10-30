using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SPA_Cell : MonoBehaviour
{
	private SPA_ShaderAni shaderAni;

	private SPA_CellAnimator cAnim;

	[SerializeField]
	private SPA_Border border;

	public UnityEvent onCelebrateEnd;

	private bool bPreInit;

	private Dictionary<string, bool> eventRecord = new Dictionary<string, bool>();

	public static float CelebrateDuration = 3f;

	public static float FlashDuration = 0.8f;

	public SPA_CellType CurType
	{
		get;
		set;
	}

	public SPA_CellState CurState
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
			cAnim = GetComponent<SPA_CellAnimator>();
		}
	}

	public void PlayState(SPA_CellState cSF_CellState = SPA_CellState.normal, SPA_CellType type = SPA_CellType.None)
	{
		CurType = ((type == SPA_CellType.None) ? CurType : type);
		if (CurType == SPA_CellType.None)
		{
			cAnim.Play((int)CurType * 5);
			return;
		}
		switch (cSF_CellState)
		{
		case SPA_CellState.normal:
			cAnim.Play((int)(CurType - 1) * 5);
			CurState = SPA_CellState.normal;
			break;
		case SPA_CellState.gray:
			cAnim.Play((int)(CurType - 1) * 5 + 1);
			CurState = SPA_CellState.gray;
			break;
		case SPA_CellState.flash:
			cAnim.Play((int)(CurType - 1) * 5 + 2);
			CurState = SPA_CellState.flash;
			break;
		case SPA_CellState.celebrate:
			cAnim.Play((int)(CurType - 1) * 5 + 3);
			CurState = SPA_CellState.celebrate;
			break;
		case SPA_CellState.bounce:
			cAnim.Play((int)(CurType - 1) * 5 + 4);
			CurState = SPA_CellState.bounce;
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
		PlayState(SPA_CellState.bounce, CurType);
	}

	public IEnumerator RolllingEnd(SPA_CellType endType)
	{
		PlayState(SPA_CellState.normal, endType);
		yield return StartCoroutine(cAnim.AnimScroll.AniStop(endType));
		base.transform.DOBlendableLocalMoveBy(Vector3.down * 5f, 0.1f).OnComplete(delegate
		{
			transform.DOBlendableLocalMoveBy(Vector3.up * 5f, 0.1f);
			SPA_SoundManager.Instance.PlayStopAndio();
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
