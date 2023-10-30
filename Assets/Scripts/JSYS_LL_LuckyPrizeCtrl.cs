using UnityEngine;

public class JSYS_LL_LuckyPrizeCtrl : MonoBehaviour
{
	private JSYS_LL_LuckySpin _luckySpin;

	private Quaternion _iniRotation;

	private Vector3 _iniPos;

	private void Start()
	{
		_luckySpin = GetComponent<JSYS_LL_LuckySpin>();
		_iniPos = base.transform.localPosition;
		_iniRotation = base.transform.localRotation;
	}

	private void Update()
	{
	}

	public void SpinLuckyPrize(int nTaihao, JSYS_LL_LuckySpin.LuckyType luckyTyp, float fTaiHao_Time, float fLuckyTyp_Time)
	{
		Reset();
		_luckySpin.SpinTo(nTaihao, luckyTyp, fTaiHao_Time, fLuckyTyp_Time);
	}

	public void FlipClosePlatform(float fDelay, float time)
	{
		iTween.RotateTo(base.gameObject, iTween.Hash("rotation", Vector3.right * 180f, "delay", fDelay, "time", time, "easetype", iTween.EaseType.easeInSine));
		JSYS_LL_MusicMngr.GetSingleton().PlaySceneSound(JSYS_LL_MusicMngr.MUSIC_SCENE_MUSIC.SCENE_FLIP_PLATE);
	}

	public void ResetPrize()
	{
		_luckySpin.ResetOffset();
	}

	public void Reset()
	{
		_luckySpin.Reset();
		iTween.Stop(base.gameObject);
		base.transform.localPosition = _iniPos;
		base.transform.localRotation = _iniRotation;
	}
}
