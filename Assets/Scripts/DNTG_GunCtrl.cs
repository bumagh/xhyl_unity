using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DNTG_GunCtrl : MonoBehaviour
{
	[HideInInspector]
	public int indexGun;

	[HideInInspector]
	public Transform tfGun;

	[HideInInspector]
	public Image imgGun;

	private Text txtGunValue;

	private GameObject effShoot;

	private GameObject effGun;

	private DNTG_GameInfo gameInfo;

	private Vector3 vecGun;

	private List<Image> shooEff;

	private List<GameObject> gunEff;

	private Color flipZreo = new Color(1f, 1f, 1f, 0f);

	private void Awake()
	{
		gameInfo = DNTG_GameInfo.getInstance();
		GetAndInitCompenent();
		flipZreo = new Color(1f, 1f, 1f, 0f);
	}

	private void GetAndInitCompenent()
	{
		tfGun = base.transform.Find("TfGun");
		vecGun = Camera.main.WorldToScreenPoint(tfGun.position);
		imgGun = tfGun.Find("ImgGun").GetComponent<Image>();
		imgGun.transform.localEulerAngles = Vector3.zero;
		txtGunValue = base.transform.Find("TxtGunValue").GetComponent<Text>();
		effShoot = tfGun.Find("ShootEff").gameObject;
		shooEff = new List<Image>();
		gunEff = new List<GameObject>();
		for (int i = 0; i < effShoot.transform.childCount; i++)
		{
			shooEff.Add(effShoot.transform.GetChild(i).GetComponent<Image>());
		}
		effGun = tfGun.Find("ImgGun").gameObject;
		for (int j = 0; j < effGun.transform.childCount; j++)
		{
			gunEff.Add(effGun.transform.GetChild(j).gameObject);
		}
	}

	public void CaculateGunDir()
	{
		vecGun = Camera.main.WorldToScreenPoint(tfGun.position);
		Vector3 mousePosition = UnityEngine.Input.mousePosition;
		float f = (mousePosition.x - vecGun.x) / (mousePosition.y - vecGun.y);
		float num = Mathf.Atan(f);
		float num2 = num * 180f / 3.14159f;
		if (mousePosition.y < vecGun.y)
		{
			num2 = ((!(mousePosition.x < vecGun.x)) ? (num2 + 180f) : (num2 - 180f));
		}
		tfGun.localEulerAngles = Vector3.back * num2;
	}

	public void GunShot(bool bFire)
	{
		imgGun.transform.DOLocalMoveY(-5f, 0.05f).OnComplete(delegate
		{
			if (bFire)
			{
				LunchBullet();
			}
			effShoot.SetActive(value: true);
			Shot();
		});
		imgGun.transform.DOLocalMoveY(10f, 0.05f).SetDelay(0.05f);
	}

	public void SetShot(int index)
	{
		indexGun = index;
		for (int i = 0; i < gunEff.Count; i++)
		{
			if (i != indexGun && gunEff[i].activeInHierarchy)
			{
				gunEff[i].SetActive(value: false);
			}
		}
		gunEff[indexGun].SetActive(value: true);
	}

	private void Shot()
	{
		for (int i = 0; i < shooEff.Count; i++)
		{
			shooEff[i].color = flipZreo;
		}
		shooEff[indexGun].DOFade(1f, 0.1f).OnComplete(delegate
		{
			shooEff[indexGun].DOFade(0f, 0.1f);
		});
	}

	private void LunchBullet()
	{
		int seatIndex = gameInfo.User.SeatIndex;
		int nPower = (gameInfo.User.ScoreCount > gameInfo.User.GunValue) ? gameInfo.User.GunValue : gameInfo.User.ScoreCount;
		DNTG_BulletPoolMngr singleton = DNTG_BulletPoolMngr.GetSingleton();
		int nPlayer = seatIndex;
		Vector3 localEulerAngles = tfGun.localEulerAngles;
		singleton.LanchBullet(nPlayer, localEulerAngles.z, nPower, IsSuperShoot(seatIndex - 1));
	}

	private bool IsSuperShoot(int index)
	{
		return DNTG_GameInfo.getInstance().IsSuperShoot[index];
	}
}
