using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BCBM_AnimationScene : MonoBehaviour
{
	public static BCBM_AnimationScene publicAnimationScene;

	public Image carIma;

	public Image carLogoIma;

	public Text betText;

	public List<Sprite> carSpr_List = new List<Sprite>();

	public List<Sprite> carLoGoSpr_List = new List<Sprite>();

	public Transform Car;

	public Transform CarOldPos;

	public Transform CarTagPosStart;

	public Transform CarTagPosEnd;

	private int index;

	private int[] beiLv = new int[8]
	{
		5,
		5,
		5,
		5,
		10,
		15,
		25,
		40
	};

	private void Awake()
	{
		publicAnimationScene = this;
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
		{
			if (index >= carLoGoSpr_List.Count)
			{
				index = 0;
			}
			ShowCar(index);
			index++;
		}
	}

	private void OnEnable()
	{
		HidCar();
	}

	public void ShowCar(int CarNum)
	{
		beiLv = new int[8]
		{
			5,
			5,
			5,
			5,
			10,
			15,
			25,
			40
		};
		HidCar();
		carIma.sprite = carSpr_List[CarNum];
		Car.Find("Fir0").gameObject.SetActive(value: true);
		Car.Find("Fir1").gameObject.SetActive(value: true);
		Car.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
		Car.DOLocalMove(CarTagPosStart.localPosition, 0.25f).OnComplete(delegate
		{
			Car.DOLocalMove(CarTagPosEnd.localPosition, 0.25f).OnComplete(delegate
			{
				BCBM_Audio.publicAudio.AudiioMethon(CarNum);
				carLogoIma.transform.parent.gameObject.SetActive(value: true);
				carLogoIma.sprite = carLoGoSpr_List[CarNum];
				BCBM_BetScene.publicBetScene.ShowTips(3);
				betText.text = "X" + beiLv[CarNum];
			});
		});
	}

	public void HidCar()
	{
		Car.localPosition = CarOldPos.localPosition;
		Car.localScale = new Vector3(0f, 1f, 1f);
		carLogoIma.transform.parent.gameObject.SetActive(value: false);
	}
}
