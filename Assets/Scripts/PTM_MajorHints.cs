using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PTM_MajorHints : MonoBehaviour
{
	[SerializeField]
	private Image imgTopHint;

	[SerializeField]
	private GameObject _goContainer;

	[SerializeField]
	private GameObject objWinHint;

	[SerializeField]
	private Text txtWinHint;

	private Coroutine _coHint;

	private string _lastHintState = string.Empty;

	private PTM_MajorHintSprites sprites;

	private void Awake()
	{
		_goContainer = base.gameObject;
		base.gameObject.SetActive(value: false);
	}

	public IEnumerator PlayNormal(bool hasDice)
	{
		List<Sprite> list = new List<Sprite>
		{
			sprites.please_press_start
		};
		if (hasDice)
		{
			list.Add(sprites.dice);
		}
		list.Add(sprites.please_bet);
		IEnumerator iter = _blink(list);
		while (iter.MoveNext())
		{
			yield return iter.Current;
		}
	}

	public IEnumerator PlayRolling()
	{
		IEnumerator iter = _blink(new List<Sprite>
		{
			sprites.good_luck,
			sprites.all_stop
		});
		while (iter.MoveNext())
		{
			yield return iter.Current;
		}
	}

	public IEnumerator PlayWin(bool hasDice)
	{
		List<Sprite> list = new List<Sprite>
		{
			sprites.key_out,
			null
		};
		if (hasDice)
		{
			list.Add(sprites.dice);
		}
		IEnumerator iter = _blink(list);
		while (iter.MoveNext())
		{
			yield return iter.Current;
		}
	}

	public void PlayGainScore()
	{
		objWinHint.SetActive(value: false);
		imgTopHint.sprite = sprites.key_out;
		imgTopHint.SetNativeSize();
		imgTopHint.enabled = true;
	}

	public void PlayWinTip()
	{
		imgTopHint.enabled = false;
		objWinHint.SetActive(value: true);
	}

	public void Show()
	{
		_goContainer.SetActive(value: true);
		objWinHint.GetComponent<Image>().sprite = sprites.gain_score;
		objWinHint.GetComponent<Image>().SetNativeSize();
	}

	public void Hide()
	{
		_goContainer.SetActive(value: false);
	}

	private IEnumerator _blink(List<Sprite> list)
	{
		float hideDuration = 0.1f;
		float showDuration = 1f;
		bool show = true;
		int index2 = 0;
		while (true)
		{
			Sprite sprite = list[index2];
			if (sprite != null)
			{
				imgTopHint.sprite = sprite;
				imgTopHint.SetNativeSize();
			}
			if (show)
			{
				objWinHint.SetActive(sprite == null);
				imgTopHint.enabled = !(sprite == null);
				yield return new WaitForSeconds(showDuration);
				imgTopHint.enabled = false;
				objWinHint.SetActive(value: false);
				index2++;
				index2 %= list.Count;
			}
			else
			{
				yield return new WaitForSeconds(hideDuration);
			}
			show = !show;
		}
	}

	public void PlayHint(string state)
	{
		UnityEngine.Debug.Log($"_playHint: [{state}]");
		if (!(_lastHintState == state) || !(state != "WinTip"))
		{
			if (_lastHintState == "WinTip")
			{
				objWinHint.SetActive(value: false);
			}
			if (state != "WinTip" && !_goContainer.activeSelf)
			{
				_goContainer.SetActive(value: true);
			}
			if (_coHint != null)
			{
				PTM_MB_Singleton<PTM_GameManager>.GetInstance().StopCoroutine(_coHint);
				UnityEngine.Debug.Log($"stop hint: [{_lastHintState}]");
			}
			_lastHintState = state;
			switch (state)
			{
			case "Normal_Dice":
				_coHint = PTM_MB_Singleton<PTM_GameManager>.GetInstance().StartCoroutine(PlayNormal(hasDice: true));
				break;
			case "Normal":
				_coHint = PTM_MB_Singleton<PTM_GameManager>.GetInstance().StartCoroutine(PlayNormal(hasDice: false));
				break;
			case "Win_Dice":
				_coHint = PTM_MB_Singleton<PTM_GameManager>.GetInstance().StartCoroutine(PlayWin(hasDice: true));
				break;
			case "Win":
				_coHint = PTM_MB_Singleton<PTM_GameManager>.GetInstance().StartCoroutine(PlayWin(hasDice: false));
				break;
			case "Rolling":
				_coHint = PTM_MB_Singleton<PTM_GameManager>.GetInstance().StartCoroutine(PlayRolling());
				break;
			case "GainScore":
				PlayGainScore();
				break;
			case "WinTip":
				PlayWinTip();
				break;
			}
		}
	}

	public void SetWinValue(int value)
	{
		txtWinHint.text = value.ToString();
	}
}
