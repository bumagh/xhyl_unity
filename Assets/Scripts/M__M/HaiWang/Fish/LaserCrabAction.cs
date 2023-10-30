using DG.Tweening;
using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.Effect;
using M__M.HaiWang.GameDefine;
using M__M.HaiWang.Misc;
using M__M.HaiWang.Player.Gun;
using M__M.HaiWang.Scenario;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class LaserCrabAction : MonoBehaviour
	{
		protected int _killerSeatID;

		protected Vector3 _killerGunPos;

		protected int _killerBulletPower;

		protected HashSet<FishType> _validFishTypes;

		protected bool _hitFishMsgReturned;

		protected int _bombCount;

		protected Task _actionTask;

		protected Task _setCountTask;

		protected Task _setLaserFireTask;

		private Action<FishBehaviour, FishDeadInfo> _fishDeadAction;

		private GunBehaviour _laserGunBehaviour;

		private static LaserCrabAction instance;

		private Effect_Logo effect_logoLaserGun;

		private Effect_Logo effect_logo_LaserCrabScore;

		private bool isFiring;

		private float _waitBigFishOrBossShowTime = 9f;

		private bool _hasBigFish;

		private bool _hasBoss;

		private int _totalScore;

		private Stopwatch stopwatch = new Stopwatch();

		protected bool _isOwner => _killerSeatID == HW2_GVars.game.curSeatId;

		public float waitBigFishOrBossShowTime
		{
			get
			{
				return _waitBigFishOrBossShowTime;
			}
			set
			{
				_waitBigFishOrBossShowTime = value;
			}
		}

		public bool hasBigFish
		{
			get
			{
				return _hasBigFish;
			}
			set
			{
				_hasBigFish = value;
			}
		}

		public bool hasBoss
		{
			get
			{
				return _hasBoss;
			}
			set
			{
				_hasBoss = value;
			}
		}

		public int totalScore
		{
			get
			{
				return _totalScore;
			}
			set
			{
				_totalScore = value;
			}
		}

		public static LaserCrabAction Get()
		{
			return instance;
		}

		private void Awake()
		{
			instance = this;
		}

		public void Play(FishBehaviour laserCrab, Action<FishBehaviour, FishDeadInfo> fishDeadAction, int seatID, int bulletPower)
		{
			if (_fishDeadAction == null && _actionTask == null && _setCountTask == null && _setLaserFireTask == null)
			{
				totalScore = 0;
				hasBigFish = false;
				hasBoss = false;
				waitBigFishOrBossShowTime = 0f;
				_hitFishMsgReturned = false;
				_killerSeatID = seatID;
				GunBehaviour gunById = fiSimpleSingletonBehaviour<GunMgr>.Get().GetGunById(seatID);
				_killerGunPos = gunById.transform.position;
				_killerBulletPower = bulletPower;
				_fishDeadAction = fishDeadAction;
				_laserGunBehaviour = fiSimpleSingletonBehaviour<GunMgr>.Get().GetGunById(_killerSeatID);
				_actionTask = new Task(IE_DoAction());
				laserCrab.Die();
			}
		}

		public void OnActionMsgReturn(bool canContinue, List<DeadFishData> deadFishs)
		{
			UnityEngine.Debug.Log($"LaserCrabAction.OnActionMsgReturn> continue:{canContinue}, deadFishs:{deadFishs.Count}");
			_hitFishMsgReturned = true;
			stopwatch.Reset();
			stopwatch.Start();
			foreach (DeadFishData deadFish in deadFishs)
			{
				FishDeadInfo fishDeadInfo = new FishDeadInfo();
				fishDeadInfo.fishId = deadFish.fish.id;
				fishDeadInfo.fishType = (int)deadFish.fish.type;
				fishDeadInfo.killerSeatId = _killerSeatID;
				fishDeadInfo.addScore = deadFish.score;
				fishDeadInfo.bulletPower = deadFish.bulletPower;
				fishDeadInfo.deadWay = 3;
				fishDeadInfo.fishRate = deadFish.fishRate;
				if (_fishDeadAction != null)
				{
					_fishDeadAction(deadFish.fish, fishDeadInfo);
				}
			}
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
				_setLaserFireTask = new Task(SetLaserFire());
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
			if (effect_logoLaserGun != null)
			{
				effect_logoLaserGun.Over();
				effect_logoLaserGun = null;
			}
			if (effect_logo_LaserCrabScore != null)
			{
				effect_logo_LaserCrabScore.Over();
				effect_logo_LaserCrabScore = null;
			}
			if (_laserGunBehaviour != null)
			{
				_laserGunBehaviour.KillEnterEMGunSequence();
				_laserGunBehaviour.Event_LaserCrabFire = null;
				_laserGunBehaviour.Event_StopLaserCrab = null;
			}
			_fishDeadAction = null;
			isFiring = false;
		}

		private IEnumerator IE_DoAction()
		{
			UnityEngine.Debug.Log(HW2_LogHelper.Lime($"电磁蟹 begin"));
			PlayClip();
			_laserGunBehaviour.EnterEMGun();
			_laserGunBehaviour.Event_StopLaserCrab = Stop;
			yield return new WaitForSeconds(3f);
			effect_logoLaserGun = Effect_LogoMgr.Get().SpawnLaserGunLogo(_killerGunPos, _killerSeatID, _killerBulletPower);
			_laserGunBehaviour.SetStorageCapacity();
			HW2_Singleton<SoundMgr>.Get().PlayClip("电磁炮聚能音效", loop: true);
			yield return new WaitForSeconds(6f);
			_laserGunBehaviour.Event_LaserCrabFire = FireNow;
			_laserGunBehaviour.emCanFire = true;
			_setCountTask = new Task(_laserGunBehaviour.SetEMBackCount());
			yield return new WaitForSeconds(10f);
			isFiring = true;
			_setLaserFireTask = new Task(SetLaserFire());
		}

		private IEnumerator SetLaserFire()
		{
			HW2_Singleton<SoundMgr>.Get().StopClip(HW2_Singleton<SoundMgr>.Get().GetClip("电磁炮聚能音效"));
			HW2_Singleton<SoundMgr>.Get().PlayClip("电磁炮发射时的音效");
			HW2_Singleton<SoundMgr>.Get().SetVolume(HW2_Singleton<SoundMgr>.Get().GetClip("电磁炮发射时的音效"), 1f);
			_laserGunBehaviour.emCanFire = false;
			_laserGunBehaviour.SetLaserFire();
			yield return new WaitForSeconds(0.3f);
			fiSimpleSingletonBehaviour<StoryScenarioMgr>.Get().ShakeScreen(0);
			if (_isOwner)
			{
				Rect rect = new Rect(-6.4f, -3.6f, 12.8f, 7.2f);
				Transform transEMBarrel_Attach = _laserGunBehaviour.m_transEMBarrel_Attach;
				Vector3 position = transEMBarrel_Attach.Find("dot0").position;
				Vector3 position2 = transEMBarrel_Attach.Find("dot1").position;
				Vector3 position3 = transEMBarrel_Attach.Find("dot2").position;
				Vector3 position4 = transEMBarrel_Attach.Find("dot3").position;
				Vector3 position5 = transEMBarrel_Attach.Find("dot4").position;
				List<FishBehaviour> inLaserFishList = FishMgr.Get().GetInLaserFishList(position, 1f, position2, position3, position4, position5, rect, (FishBehaviour _fish) => _fish.IsLive() && _fish.Hitable && _fish.type <= FishType.Boss_Lantern_暗夜炬兽);
				List<FishData4Hit> list = (from _fish in inLaserFishList
					select new FishData4Hit(_fish)).ToList();
				if (list.Count == 0)
				{
					list.Add(new FishData4Hit(0, 1));
				}
				string text = (from _fish in list
					select $"{_fish.fishId}#{_fish.fishType}").JoinStrings("-");
				object[] args = new object[2]
				{
					12,
					text
				};
				HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/gunHitFishInAction", args);
			}
			yield return new WaitForSeconds(2f);
			_laserGunBehaviour.OutEMGun();
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
				yield return new WaitForSeconds(9f);
			}
			else if (hasBoss)
			{
				yield return new WaitForSeconds(waitBigFishOrBossShowTime);
			}
			GunBehaviour gun = fiSimpleSingletonBehaviour<GunMgr>.Get().GetNativeGun();
			int num = totalScore / gun.GetPower();
			Vector3 position6 = effect_logoLaserGun.transform.position;
			ShortcutExtensions.DOMoveY(endValue: position6.y + ((_killerSeatID == 1 || _killerSeatID == 2) ? 0.8f : (-0.8f)), target: effect_logoLaserGun.transform, duration: 0.5f);
			effect_logo_LaserCrabScore = Effect_LogoMgr.Get().SpawnLogoScore(_killerGunPos, _killerSeatID, totalScore, Effect_LogoMgr.LogoScoreTypes.ScoreLaserCrab);
			yield return new WaitForSeconds(3f);
			effect_logo_LaserCrabScore.DoFade();
			effect_logoLaserGun.DoFade();
			yield return new WaitForSeconds(1f);
			effect_logo_LaserCrabScore.Over();
			effect_logoLaserGun.Over();
			effect_logo_LaserCrabScore = null;
			effect_logoLaserGun = null;
			_laserGunBehaviour.Event_LaserCrabFire = null;
			_laserGunBehaviour.Event_StopLaserCrab = null;
			_fishDeadAction = null;
			_actionTask = null;
			_setCountTask = null;
			_setLaserFireTask = null;
			isFiring = false;
			stopwatch.Stop();
			UnityEngine.Debug.Log(HW2_LogHelper.Orange($"电磁炮收到死鱼通知后动画效果耗时：{stopwatch.Elapsed.TotalSeconds}"));
		}

		public void PalyResultClip(int temp)
		{
			if (temp < 20)
			{
				HW2_Singleton<SoundMgr>.Get().PlayClip("结果1");
			}
			else if (temp >= 20 && temp < 90)
			{
				HW2_Singleton<SoundMgr>.Get().PlayClip("结果3");
			}
			else if (temp >= 90 && temp < 120)
			{
				HW2_Singleton<SoundMgr>.Get().PlayClip("结果2");
			}
			else if (temp >= 120)
			{
				HW2_Singleton<SoundMgr>.Get().PlayClip("结果4");
			}
		}

		private void PlayClip()
		{
			int num = UnityEngine.Random.Range(0, 3);
			UnityEngine.Debug.Log("捕获电磁蟹index" + num);
			switch (num)
			{
			case 0:
				HW2_Singleton<SoundMgr>.Get().PlayClip("捕获电磁蟹获得电磁炮语音1");
				break;
			case 1:
				HW2_Singleton<SoundMgr>.Get().PlayClip("捕获电磁蟹获得电磁炮语音2");
				break;
			case 2:
				HW2_Singleton<SoundMgr>.Get().PlayClip("捕获电磁蟹获得电磁炮语音3");
				break;
			default:
				UnityEngine.Debug.Log("error 电磁蟹 Clip index");
				break;
			}
		}
	}
}
