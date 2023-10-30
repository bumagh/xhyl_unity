using M__M.HaiWang.Effect;
using M__M.HaiWang.Fish;
using M__M.HaiWang.GameDefine;
using M__M.HaiWang.Player.Gun;
using System;
using System.Collections;
using UnityEngine;

public class FK3_WindCrabAction : MonoBehaviour
{
	protected FK3_FishBehaviour _bombCrab;

	protected bool _canContinue;

	protected bool _hitFishMsgReturned;

	private Action _cleanAction;

	protected int _killerSeatID;

	private Action<FK3_FishBehaviour, FK3_FishDeadInfo> _fishDeadAction;

	protected Vector3 _killerGunPos;

	private FK3_Effect_Logo _effect_LogobombCrab;

	protected FK3_Task _actionTask;

	public int totalScore
	{
		get;
		set;
	}

	public float waitBigFishShowTime
	{
		get;
		set;
	}

	private void Update()
	{
		if (_bombCrab != null)
		{
			_bombCrab.transform.localEulerAngles -= new Vector3(0f, 0f, 5f);
		}
	}

	public void DoReset()
	{
		_canContinue = false;
		_bombCrab = null;
		if (_cleanAction != null)
		{
			_cleanAction();
		}
	}

	public void Play(FK3_FishBehaviour bombCrab, Action<FK3_FishBehaviour, FK3_FishDeadInfo> fishDeadAction, int seatID, int bulletPower)
	{
		if (!(_bombCrab != null))
		{
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("捕获连环炸弹蟹获得连环炸弹时的语音3");
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("电磁炮聚能音效", loop: true);
			_bombCrab = bombCrab;
			_killerSeatID = seatID;
			_fishDeadAction = fishDeadAction;
			_canContinue = true;
			totalScore = 0;
			waitBigFishShowTime = 0f;
			FK3_GunBehaviour gunById = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(seatID);
			_killerGunPos = gunById.transform.position;
			_effect_LogobombCrab = FK3_Effect_LogoMgr.Get().SpawnBombCrabLogo(bombCrab.GetPosition(), _killerSeatID, bulletPower);
			_hitFishMsgReturned = false;
			bombCrab.Dying();
			_actionTask = new FK3_Task(IE_DoAction());
		}
	}

	private IEnumerator IE_DoAction()
	{
		UnityEngine.Debug.LogError("暴风蟹开始!");
		yield return null;
	}
}
