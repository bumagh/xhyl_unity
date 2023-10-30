using UnityEngine;

public class JSYS_LL_All_ShanDian : MonoBehaviour
{
	private Vector3 Ini_Scale;

	private Vector3 Ini_Position;

	private Quaternion Ini_Rotation;

	private bool isFirstTime;

	private void Awake()
	{
		isFirstTime = true;
		Ini_Scale = base.transform.localScale;
		Ini_Position = base.transform.localPosition;
		Ini_Rotation = base.transform.localRotation;
	}

	private void OnDisable()
	{
		base.transform.localPosition = Ini_Position;
		base.transform.localRotation = Ini_Rotation;
		base.transform.localScale = Ini_Scale;
	}

	private void Start()
	{
		isFirstTime = false;
	}

	private void OnEnable()
	{
		if (!isFirstTime)
		{
			OnFly();
		}
		iTween.ScaleFrom(base.gameObject, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeInOutSine));
		iTween.RotateTo(base.gameObject, iTween.Hash("rotation", new Vector3(0f, 0f, 20f), "time", 0.5, "delay", 1, "easetype", iTween.EaseType.easeInOutSine));
		iTween.RotateTo(base.gameObject, iTween.Hash("rotation", new Vector3(0f, 0f, -20f), "time", 0.5, "delay", 1.5, "easetype", iTween.EaseType.easeInOutSine));
		iTween.RotateTo(base.gameObject, iTween.Hash("rotation", new Vector3(-49f, 19f, -17f), "time", 1, "delay", 2, "easetype", iTween.EaseType.easeInOutSine));
		iTween.MoveTo(base.gameObject, iTween.Hash("position", new Vector3(-1f, 3.56f, 5.7f), "time", 1, "delay", 2, "easetype", iTween.EaseType.easeInOutSine));
		iTween.ScaleTo(base.gameObject, iTween.Hash("scale", Vector3.one, "time", 1, "delay", 2, "easetype", iTween.EaseType.easeInOutSine));
	}

	private void Update()
	{
	}

	public void OnFly()
	{
		JSYS_LL_MusicMngr.GetSingleton().PlayEffectSound(JSYS_LL_MusicMngr.MUSIC_EFFECT_MUSIC.Effect_ALL_LIGHTING_X2_FLY);
	}

	public void OnFlyEnd()
	{
		JSYS_LL_MusicMngr.GetSingleton().PlayEffectSound(JSYS_LL_MusicMngr.MUSIC_EFFECT_MUSIC.Effect_ALL_LIGHTING_X2_FLYEND);
	}
}
