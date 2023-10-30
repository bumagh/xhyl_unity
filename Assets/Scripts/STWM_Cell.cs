using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class STWM_Cell : MonoBehaviour
{
	private STWM_ShaderAni shaderAni;

	private STWM_CellAnimator cAnim;

	private STWM_CellType curType;

	[SerializeField]
	private STWM_Border border;

	public UnityEvent onCelebrateEnd;

	private bool bPreInit;

	private Dictionary<string, bool> eventRecord = new Dictionary<string, bool>();

	public static float CelebrateDuration = 3f;

	public static float FlashDuration = 0.8f;

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
			cAnim = GetComponent<STWM_CellAnimator>();
		}
	}

	public void PlayState(string state, STWM_CellType type = STWM_CellType.None)
	{
		curType = ((type == STWM_CellType.None) ? curType : type);
		if (curType == STWM_CellType.None)
		{
			cAnim.Play((int)curType * 5);
		}
		else
		{
			if (state == null)
			{
				return;
			}
			if (!(state == "normal"))
			{
				if (!(state == "gray"))
				{
					if (!(state == "flash"))
					{
						if (!(state == "celebrate"))
						{
							if (state == "bounce")
							{
								cAnim.Play((int)(curType - 1) * 5 + 4);
							}
						}
						else
						{
							cAnim.Play((int)(curType - 1) * 5 + 3);
						}
					}
					else
					{
						cAnim.Play((int)(curType - 1) * 5 + 2);
					}
				}
				else
				{
					cAnim.Play((int)(curType - 1) * 5 + 1);
				}
			}
			else
			{
				cAnim.Play((int)(curType - 1) * 5);
			}
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
		cAnim.animScroll.gameObject.SetActive(value: false);
		border.Show(isShow: false);
		PlayState("normal");
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
		PlayState("bounce", curType);
	}

	public IEnumerator RolllingEnd(STWM_CellType endType)
	{
		yield return StartCoroutine(cAnim.animScroll.AniStop(endType));
		PlayState("normal", endType);
		base.transform.DOBlendableLocalMoveBy(Vector3.down * 5f, 0.1f).OnComplete(delegate
		{
			transform.DOBlendableLocalMoveBy(Vector3.up * 5f, 0.1f);
		});
		yield return StartCoroutine(_waitForEventHappen("Bounce:End", 0.5f));
		PlayState("normal");
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
