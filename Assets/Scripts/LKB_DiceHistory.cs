using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LKB_DiceHistory : MonoBehaviour
{
	public static LKB_DiceHistory _Instance;

	[SerializeField]
	private Sprite[] spiPlates;

	private List<Image> imgPlates;

	private Transform tfPlates;

	private int numPlate;

	private void Awake()
	{
		if (_Instance == null)
		{
			_Instance = this;
		}
	}

	private void Start()
	{
		Init();
	}

	private void Init()
	{
		numPlate = 1;
		imgPlates = new List<Image>();
		tfPlates = base.transform.Find("Parent");
		InitPlates(new LKB_BetDiceType[10]
		{
			LKB_BetDiceType.Big,
			LKB_BetDiceType.Big,
			LKB_BetDiceType.Big,
			LKB_BetDiceType.Middle,
			LKB_BetDiceType.Big,
			LKB_BetDiceType.Big,
			LKB_BetDiceType.Big,
			LKB_BetDiceType.Small,
			LKB_BetDiceType.Big,
			LKB_BetDiceType.Middle
		});
	}

	private void InitPlates(LKB_BetDiceType[] dicetypes)
	{
		for (int i = 0; i < dicetypes.Length; i++)
		{
			Image component = tfPlates.GetChild(i).GetComponent<Image>();
			imgPlates.Add(component);
			if (dicetypes[i] == LKB_BetDiceType.Small)
			{
				imgPlates[i].sprite = spiPlates[0];
			}
			else if (dicetypes[i] == LKB_BetDiceType.Middle)
			{
				imgPlates[i].sprite = spiPlates[1];
			}
			else
			{
				imgPlates[i].sprite = spiPlates[2];
			}
		}
	}

	public void ShowPlates(LKB_BetDiceType dicetype)
	{
		Image img = imgPlates[0];
		imgPlates.Add(img);
		Sequence mySequence = DOTween.Sequence();
		Tweener tweener = img.transform.DOLocalMoveX(-700f, 1f);
		tweener.onComplete = delegate
		{
			img.transform.localPosition += Vector3.right * 1470f;
			img.transform.SetAsLastSibling();
			imgPlates.RemoveAt(0);
			Transform target = tfPlates;
			Vector3 localPosition = tfPlates.transform.localPosition;
			target.DOLocalMoveX(localPosition.x - 70f, 1f);
			mySequence.Join(img.transform.DOLocalMoveX(245 + numPlate * 70, 1f));
			numPlate++;
		};
		switch (dicetype)
		{
		case LKB_BetDiceType.Small:
			img.sprite = spiPlates[0];
			break;
		case LKB_BetDiceType.Middle:
			img.sprite = spiPlates[1];
			break;
		default:
			img.sprite = spiPlates[2];
			break;
		}
	}
}
