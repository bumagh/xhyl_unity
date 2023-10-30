using UnityEngine;
using UnityEngine.UI;

public class STTF_GunCtrl : MonoBehaviour
{
	[HideInInspector]
	public int indexGun;

	[HideInInspector]
	public Transform tfGun;

	[HideInInspector]
	public Image imgGun;

	private Animator aniGun;

	private Text txtGunValue;

	private GameObject effShoot;

	private STTF_GameInfo gameInfo;

	private Vector3 vecGun;

	private void Start()
	{
		gameInfo = STTF_GameInfo.getInstance();
		GetAndInitCompenent();
	}

	private void GetAndInitCompenent()
	{
		tfGun = base.transform.Find("TfGun");
		vecGun = Camera.main.WorldToScreenPoint(tfGun.position);
		imgGun = tfGun.Find("ImgGun").GetComponent<Image>();
		aniGun = imgGun.transform.GetComponent<Animator>();
		indexGun = 0;
		imgGun.transform.localEulerAngles = Vector3.zero;
		txtGunValue = base.transform.Find("TxtGunValue").GetComponent<Text>();
		effShoot = tfGun.Find("ShootEff").gameObject;
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
		aniGun.Play("GunShoot");
		if (bFire)
		{
			LunchBullet();
		}
		effShoot.SetActive(value: true);
	}

	private void LunchBullet()
	{
		int nPower = (gameInfo.User.ScoreCount > gameInfo.User.GunValue) ? gameInfo.User.GunValue : gameInfo.User.ScoreCount;
		STTF_BulletPoolMngr singleton = STTF_BulletPoolMngr.GetSingleton();
		int seatIndex = gameInfo.User.SeatIndex;
		Vector3 localEulerAngles = tfGun.localEulerAngles;
		singleton.LanchBullet(seatIndex, localEulerAngles.z, nPower);
	}
}
