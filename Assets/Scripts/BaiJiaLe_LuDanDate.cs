using UnityEngine;

public class BaiJiaLe_LuDanDate : MonoBehaviour
{
	public static BaiJiaLe_LuDanDate instance;

	public Sprite[] LuDanSprite1;

	public Sprite[] LuDanSprite2;

	public Sprite[] LuDanSprite3;

	public Sprite[] LuDanSprite4;

	public Sprite[] LuDanSprite5;

	public Sprite[] LuDanSprite6;

	public Texture2D[] Numbers;

	public Material[] ChipMaterial;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			Application.runInBackground = true;
			Screen.sleepTimeout = -1;
			Application.targetFrameRate = 60;
			UnityEngine.Debug.unityLogger.logEnabled = true;
			for (int i = 0; i < base.transform.childCount; i++)
			{
				base.transform.GetChild(i).gameObject.SetActive(value: true);
			}
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
