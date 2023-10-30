using UnityEngine;

public class DCDF_MajorGameController : DCDF_MB_Singleton<DCDF_MajorGameController>
{
	private DCDF_RollManager rollManager;

	private GameObject objCaishen;

	private GameObject objCaishendao;

	private void Awake()
	{
		if (DCDF_MB_Singleton<DCDF_MajorGameController>._instance == null)
		{
			DCDF_MB_Singleton<DCDF_MajorGameController>.SetInstance(this);
			PreInit();
		}
	}

	private void PreInit()
	{
		rollManager = base.transform.GetComponent<DCDF_RollManager>();
		objCaishen = GameObject.Find("Caishen");
		objCaishen.SetActive(value: false);
		objCaishendao = GameObject.Find("Caishendao");
		objCaishendao.SetActive(value: false);
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
		objCaishen.SetActive(value: true);
		objCaishendao.SetActive(value: true);
		DCDF_MySqlConnection.curView = "MajorGame";
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
		objCaishen.SetActive(value: false);
		objCaishendao.SetActive(value: false);
	}

	public void InitGame()
	{
		rollManager.InitGame();
	}
}
