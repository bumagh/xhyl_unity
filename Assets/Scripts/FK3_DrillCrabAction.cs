using DG.Tweening;
using HW3L;
using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.Bullet;
using M__M.HaiWang.Effect;
using M__M.HaiWang.Fish;
using M__M.HaiWang.GameDefine;
using M__M.HaiWang.Player.Gun;
using M__M.HaiWang.Scenario;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FK3_DrillCrabAction : MonoBehaviour
{
	private static FK3_DrillCrabAction instance;

	protected FK3_Task _actionTask;

	protected FK3_Task _setCountTask;

	protected FK3_Task _setLaserFireTask;

	private FK3_GunBehaviour _drillGunBehaviour;

	private FK3_Effect_Logo effect_logoDrillGun;

	private FK3_Effect_Logo effect_logo_DrillCrabScore;

	private Action<FK3_FishBehaviour, FK3_FishDeadInfo> _fishDeadAction;

	private bool isFiring;

	protected bool _hitFishMsgReturned;

	private Stopwatch stopwatch = new Stopwatch();

	protected int _killerSeatID;

	protected int _killerBulletPower;

	protected Vector3 _killerGunPos;

	private bool canMove;

	protected bool _isOwner => _killerSeatID == FK3_GVars.game.curSeatId;

	public int totalScore
	{
		get;
		set;
	}

	public float waitBigFishOrBossShowTime
	{
		get;
		set;
	} = 9f;


	public bool hasBigFish
	{
		get;
		set;
	}

	public bool hasBoss
	{
		get;
		set;
	}

	private void Awake()
	{
		instance = this;
	}

	public static FK3_DrillCrabAction Get()
	{
		return instance;
	}

	public void OnActionMsgReturn(bool canContinue, List<FK3_DeadFishData> deadFishs)
	{
		_hitFishMsgReturned = true;
		stopwatch.Reset();
		stopwatch.Start();
		foreach (FK3_DeadFishData deadFish in deadFishs)
		{
			FK3_FishDeadInfo fK3_FishDeadInfo = new FK3_FishDeadInfo();
			fK3_FishDeadInfo.fishId = deadFish.fish.id;
			fK3_FishDeadInfo.fishType = (int)deadFish.fish.type;
			fK3_FishDeadInfo.killerSeatId = _killerSeatID;
			fK3_FishDeadInfo.addScore = deadFish.score;
			fK3_FishDeadInfo.bulletPower = deadFish.bulletPower;
			fK3_FishDeadInfo.deadWay = 3;
			fK3_FishDeadInfo.fishRate = deadFish.fishRate;
			if (_fishDeadAction != null)
			{
				_fishDeadAction(deadFish.fish, fK3_FishDeadInfo);
			}
		}
	}

	public void Play(FK3_FishBehaviour DrillCrab, Action<FK3_FishBehaviour, FK3_FishDeadInfo> fishDeadAction, int seatID, int bulletPower)
	{
		if (_fishDeadAction == null && _actionTask == null && _setCountTask == null && _setLaserFireTask == null)
		{
			totalScore = 0;
			hasBigFish = false;
			hasBoss = false;
			waitBigFishOrBossShowTime = 0f;
			_hitFishMsgReturned = false;
			_killerSeatID = seatID;
			FK3_GunBehaviour gunById = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(seatID);
			_killerGunPos = gunById.transform.position;
			_killerBulletPower = bulletPower;
			_fishDeadAction = fishDeadAction;
			_drillGunBehaviour = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(_killerSeatID);
			_actionTask = new FK3_Task(IE_DoAction());
			DrillCrab.Die();
		}
	}

	private IEnumerator IE_DoAction()
	{
		UnityEngine.Debug.LogError("钻头蟹开始");
		FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("捕获电磁蟹获得电磁炮语音3");
		_drillGunBehaviour.EnterDRGun();
		_drillGunBehaviour.Event_StopDrillCrab = Stop;
		yield return new WaitForSeconds(3f);
		effect_logoDrillGun = FK3_Effect_LogoMgr.Get().SpawnDrillGunLogo(_killerGunPos, _killerSeatID, _killerBulletPower);
		yield return new WaitForSeconds(1f);
		_drillGunBehaviour.Event_DrillCrabFire = FireNow;
		_drillGunBehaviour.drCanFire = true;
		_setCountTask = new FK3_Task(_drillGunBehaviour.SetDRBackCount());
		yield return new WaitForSeconds(8f);
		_drillGunBehaviour.canAutoDrillFire = true;
		isFiring = true;
		_setLaserFireTask = new FK3_Task(SetDrillFire());
	}

	private IEnumerator Fride()
	{
		canMove = true;
		yield return new WaitForSeconds(4f);
		canMove = false;
	}

	private IEnumerator SetDrillFire()
	{
		FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("捕获电磁蟹获得电磁炮语音3");
		FK3_Singleton<FK3_SoundMgr>.Get().SetVolume("捕获电磁蟹获得电磁炮语音3", 1f);
		_drillGunBehaviour.drCanFire = false;
		_drillGunBehaviour.SetDrillFire();
		_drillGunBehaviour.m_transDRBarrel.SetActive(active: false);
		FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().ShakeScreen(3);
		yield return new WaitForSeconds(0.3f);
		StartCoroutine(Fride());
		Collider rectFride = null;
		while (canMove)
		{
			if (_isOwner)
			{
				Rect rect = FK3_BulletMgr.Get().bulletBorder.rect;
				try
				{
					if (rectFride == null)
					{
						rectFride = FK3_BulletMgr.Get().ShootDRBull.GetComponent<Collider>();
					}
				}
				catch (Exception message)
				{
					UnityEngine.Debug.LogError(message);
				}
				if ((bool)rectFride)
				{
					List<FK3_FishBehaviour> inLaserFishList = FK3_FishMgr.Get().GetInLaserFishList(rectFride, rect, (FK3_FishBehaviour _fish) => _fish.IsLive() && _fish.Hitable && _fish.type <= FK3_FishType.Boss_Lantern_暗夜炬兽);
					List<FK3_FishData4Hit> source = (from _fish in inLaserFishList
						select new FK3_FishData4Hit(_fish)).ToList();
					string text = (from _fish in source
						select $"{_fish.fishId}#{_fish.fishType}").JoinStrings("|");
					object[] args = new object[2]
					{
						13,
						text
					};
					FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/gunHitFishInAction", args);
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
		try
		{
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogError("删除炮弹错误: " + arg);
		}
		yield return new WaitForSeconds(2f);
		_drillGunBehaviour.OutDRGun();
		float lastTime = Time.realtimeSinceStartup;
		float elapsedTime = 0f;
		while (!_hitFishMsgReturned)
		{
			elapsedTime += Time.realtimeSinceStartup - lastTime;
			lastTime = Time.realtimeSinceStartup;
			if (elapsedTime > 4f)
			{
				break;
			}
			yield return null;
		}
		if (hasBigFish)
		{
			yield return new WaitForSeconds(8f);
		}
		else if (hasBoss)
		{
			yield return new WaitForSeconds(waitBigFishOrBossShowTime);
		}
		FK3_GunBehaviour gun = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun();
		int num = totalScore / gun.GetPower();
		Vector3 position = effect_logoDrillGun.transform.position;
		ShortcutExtensions.DOMoveY(endValue: position.y + 1f, target: effect_logoDrillGun.transform, duration: 0.5f);
		effect_logo_DrillCrabScore = FK3_Effect_LogoMgr.Get().SpawnLogoScore(_killerGunPos, _killerSeatID, totalScore, FK3_Effect_LogoMgr.LogoScoreTypes.ScoreLaserCrab);
		effect_logo_DrillCrabScore.GetComponent<Image>().DOFade(1f, 0.1f);
		effect_logoDrillGun.GetComponent<Image>().DOFade(1f, 0.1f);
		yield return new WaitForSeconds(3f);
		effect_logo_DrillCrabScore.GetComponent<Image>().DOFade(0f, 1f);
		effect_logoDrillGun.GetComponent<Image>().DOFade(0f, 1f);
		yield return new WaitForSeconds(1f);
		effect_logo_DrillCrabScore.Over();
		effect_logoDrillGun.Over();
		effect_logo_DrillCrabScore = null;
		effect_logoDrillGun = null;
		_drillGunBehaviour.Event_DrillCrabFire = null;
		_drillGunBehaviour.Event_StopDrillCrab = null;
		_fishDeadAction = null;
		_actionTask = null;
		_setCountTask = null;
		_setLaserFireTask = null;
		isFiring = false;
		stopwatch.Stop();
		UnityEngine.Debug.Log(FK3_LogHelper.Orange($"电磁炮收到死鱼通知后动画效果耗时：{stopwatch.Elapsed.TotalSeconds}"));
	}

	public void FireNow()
	{
		if (!isFiring)
		{
			isFiring = true;
			if (_setCountTask != null)
			{
				_setCountTask.Stop();
				_setCountTask = null;
			}
			_actionTask.Stop();
			_setLaserFireTask = new FK3_Task(SetDrillFire());
		}
	}

	public void Stop()
	{
		if (_actionTask != null)
		{
			_actionTask.Stop();
			_actionTask = null;
		}
		if (_setCountTask != null)
		{
			_setCountTask.Stop();
			_setCountTask = null;
		}
		if (_setLaserFireTask != null)
		{
			_setLaserFireTask.Stop();
			_setLaserFireTask = null;
		}
		if (effect_logoDrillGun != null)
		{
			effect_logoDrillGun.Over();
			effect_logoDrillGun = null;
		}
		if (effect_logo_DrillCrabScore != null)
		{
			effect_logo_DrillCrabScore.Over();
			effect_logo_DrillCrabScore = null;
		}
		if (_drillGunBehaviour != null)
		{
			_drillGunBehaviour.KillEnterEMGunSequence();
			_drillGunBehaviour.Event_DrillCrabFire = null;
			_drillGunBehaviour.Event_StopDrillCrab = null;
		}
		_fishDeadAction = null;
		isFiring = false;
	}
}
