using DG.Tweening;
using Framework;
using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.Effect;
using M__M.HaiWang.GameDefine;
using M__M.HaiWang.Player.Gun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class LightingFishAction : ObjectBase
	{
		protected int _killerSeatID;

		protected Vector3 _killerGunPos;

		protected List<HashSet<FishType>> _validFishTypesList = new List<HashSet<FishType>>();

		protected FishBehaviour _lightningfish;

		protected List<LightningDeadFish> _linkedFishes;

		private LightningDeadFish _lastDeadFish;

		private LightningDeadFish _curDeadFish;

		protected Task _actionTask;

		protected bool _canContinue;

		protected bool _hitFishMsgReturned;

		protected Queue<LightningDeadFish> _cachedDeadFishs;

		protected NumberSprite _numSprite;

		private Action<FishBehaviour, FishDeadInfo> _fishDeadAction;

		private int __targetFishID;

		private Effect_Logo effect_logo_Lighting;

		private Task _actionEffectLogoShow;

		private bool _shouldOver_Log_Lighting;

		private bool _hasBigFish;

		private int _totalScore;

		private Stopwatch stopwatch = new Stopwatch();

		protected bool _isOwner => _killerSeatID == HW2_GVars.game.curSeatId;

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

		public LightingFishAction()
		{
			_lightningfish = null;
			_linkedFishes = new List<LightningDeadFish>();
			HashSet<FishType> hashSet = new HashSet<FishType>();
			HashSet<FishType> hashSet2 = new HashSet<FishType>();
			HashSet<FishType> hashSet3 = new HashSet<FishType>();
			HashSet<FishType> hashSet4 = new HashSet<FishType>();
			HashSet<FishType> hashSet5 = new HashSet<FishType>();
			_cachedDeadFishs = new Queue<LightningDeadFish>();
			for (int i = 1; i <= 19; i++)
			{
				hashSet2.Add((FishType)i);
				if (i <= 12)
				{
					hashSet.Add((FishType)i);
				}
				if (i <= 9)
				{
					hashSet3.Add((FishType)i);
				}
				if (i >= 10)
				{
					hashSet4.Add((FishType)i);
				}
				if (i >= 12)
				{
					hashSet5.Add((FishType)i);
				}
			}
			_validFishTypesList.Add(hashSet);
			_validFishTypesList.Add(hashSet2);
			_validFishTypesList.Add(hashSet3);
			_validFishTypesList.Add(hashSet4);
			_validFishTypesList.Add(hashSet5);
		}

		public void Init()
		{
			if (_numSprite == null)
			{
				_numSprite = this.Instantiate("FishEffect/LightningFish/Number", active: false).GetComponent<NumberSprite>();
				UnityEngine.Object.DontDestroyOnLoad(_numSprite.gameObject);
				HW2_GVars.dontDestroyOnLoadList.Add(_numSprite.gameObject);
				_numSprite.name = "闪电鱼数字";
				_numSprite.SetText("1");
				_numSprite.SetActive(active: false);
				_numSprite.fixFace = true;
			}
		}

		public void SetFaceDown(bool faceDown)
		{
			_numSprite.SetFaceDown(faceDown);
		}

		public void Play(FishBehaviour lightingFish, Action<FishBehaviour, FishDeadInfo> fishDeadAction, int seatID, int bulletPower)
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("闪电鱼logo出现在炮台上旋转的音效");
			GunBehaviour gunById = fiSimpleSingletonBehaviour<GunMgr>.Get().GetGunById(seatID);
			_killerGunPos = gunById.transform.position;
			effect_logo_Lighting = Effect_LogoMgr.Get().SpawnLightingLogo(_killerGunPos, seatID, bulletPower);
			totalScore = 0;
			hasBigFish = false;
			_shouldOver_Log_Lighting = true;
			_fishDeadAction = fishDeadAction;
			if (_lightningfish != null || _actionTask != null || _cachedDeadFishs.Count != 0)
			{
				Stop();
				UnityEngine.Debug.LogWarning("lightning fish overlapped???");
			}
			_killerSeatID = seatID;
			_linkedFishes.Clear();
			_lightningfish = lightingFish;
			lightingFish.Dying(playAni: false);
			_lastDeadFish = new LightningDeadFish(_lightningfish, 0, 0, 0, _numSprite, this);
			_linkedFishes.Add(_lastDeadFish);
			_canContinue = true;
			if (_isOwner)
			{
				_actionTask = new Task(_doAction());
			}
			else
			{
				_actionTask = new Task(_doSyncAction());
			}
		}

		public void Stop()
		{
			if (_actionTask != null)
			{
				_actionTask.Stop();
				_actionTask = null;
			}
			if (_actionEffectLogoShow != null)
			{
				_actionEffectLogoShow.Stop();
				_actionEffectLogoShow = null;
			}
			_hitFishMsgReturned = false;
			_lightningfish = null;
			_shouldOver_Log_Lighting = false;
			_canContinue = true;
			foreach (LightningDeadFish linkedFish in _linkedFishes)
			{
				linkedFish.Stop();
			}
			foreach (LightningDeadFish cachedDeadFish in _cachedDeadFishs)
			{
				cachedDeadFish.Stop();
			}
			_linkedFishes.Clear();
			_cachedDeadFishs.Clear();
			_fishDeadAction = null;
			_lightningfish = null;
			_numSprite.SetActive(active: false);
		}

		public void OnFishDead(FishBehaviour fish, int bulletPower, int fishRate, int score)
		{
			if (_fishDeadAction != null)
			{
				FishDeadInfo fishDeadInfo = new FishDeadInfo();
				fishDeadInfo.addScore = score;
				fishDeadInfo.bulletPower = bulletPower;
				fishDeadInfo.fishId = fish.id;
				fishDeadInfo.deadWay = 4;
				fishDeadInfo.fishType = (int)fish.type;
				fishDeadInfo.killerSeatId = _killerSeatID;
				fishDeadInfo.fishRate = fishRate;
				_fishDeadAction(fish, fishDeadInfo);
				if (_shouldOver_Log_Lighting)
				{
					_actionEffectLogoShow = new Task(EffetLogoShow());
					_shouldOver_Log_Lighting = false;
				}
			}
		}

		private IEnumerator EffetLogoShow()
		{
			float waitTime = hasBigFish ? 9f : 2f;
			yield return new WaitForSeconds(waitTime);
			Vector3 position = effect_logo_Lighting.transform.position;
			ShortcutExtensions.DOMoveY(endValue: position.y + ((_killerSeatID == 1 || _killerSeatID == 2) ? 0.8f : (-0.8f)), target: effect_logo_Lighting.transform, duration: 0.5f);
			Effect_Logo effect_logo_LightingScore = Effect_LogoMgr.Get().SpawnLogoScore(_killerGunPos, _killerSeatID, totalScore, Effect_LogoMgr.LogoScoreTypes.ScoreLighting);
			yield return new WaitForSeconds(3f);
			yield return new WaitForSeconds(1f);
			effect_logo_Lighting.Over();
			effect_logo_LightingScore.Over();
			UnityEngine.Debug.Log(HW2_LogHelper.Orange($"闪电连锁收到死鱼通知后动画效果耗时{stopwatch.Elapsed.TotalSeconds}"));
		}

		public void OnActionMsgReturn(bool canContinue, List<DeadFishData> deadFishs)
		{
			if (_lightningfish == null)
			{
				return;
			}
			if (deadFishs.Count == 1)
			{
				DeadFishData deadFishData = deadFishs[0];
				_canContinue = canContinue;
				if (_isOwner)
				{
					_hitFishMsgReturned = true;
					_curDeadFish = new LightningDeadFish(deadFishData.fish, deadFishData.bulletPower, deadFishData.fishRate, deadFishData.score, _numSprite, this);
				}
				else
				{
					LightningDeadFish lightningDeadFish = new LightningDeadFish(deadFishData.fish, deadFishData.bulletPower, deadFishData.fishRate, deadFishData.score, _numSprite, this);
					lightningDeadFish.index = _cachedDeadFishs.Count + 1;
					_cachedDeadFishs.Enqueue(lightningDeadFish);
				}
			}
			else if (deadFishs.Count == 0)
			{
				_canContinue = false;
			}
			else if (deadFishs.Count > 1)
			{
				UnityEngine.Debug.LogWarning("闪电_Action_失败：闪电鱼一次只能电死1个，但是死鱼列表里超过了1个!");
				Stop();
			}
		}

		protected IEnumerator _doSyncAction()
		{
			UnityEngine.Debug.Log(HW2_LogHelper.Orange("---------- 闪电鱼同步_" + _lightningfish.type + "_Action_启动 ------------"));
			_numSprite.SetPosition(_lastDeadFish.fish.GetPosition());
			_numSprite.SetText("1");
			_numSprite.SetActive();
			yield return _lastDeadFish.Play();
			while (_canContinue)
			{
				_curDeadFish = null;
				float lastTime = Time.realtimeSinceStartup;
				float elapsedTime = 0f;
				while (_canContinue && _cachedDeadFishs.Count <= 0)
				{
					elapsedTime += Time.realtimeSinceStartup - lastTime;
					lastTime = Time.realtimeSinceStartup;
					if (elapsedTime > 4f)
					{
						_canContinue = false;
						break;
					}
					yield return null;
				}
				if (!_canContinue)
				{
					break;
				}
				_curDeadFish = _cachedDeadFishs.Dequeue();
				_curDeadFish.index = _linkedFishes.Count + 1;
				_linkedFishes.Add(_curDeadFish);
				yield return _curDeadFish.Play(_lastDeadFish.fish);
				if (null == _lastDeadFish.moveTarget)
				{
					_lastDeadFish.moveTarget = _curDeadFish.fish;
				}
				_lastDeadFish = _curDeadFish;
			}
			yield return new WaitForSeconds(1f);
			foreach (LightningDeadFish linkedFish in _linkedFishes)
			{
				linkedFish.Stop();
			}
			_linkedFishes.Clear();
			_cachedDeadFishs.Clear();
			_numSprite.SetActive(active: false);
			_lightningfish = null;
			UnityEngine.Debug.Log(HW2_LogHelper.Orange("####### 闪电鱼同步_Action_结束 #########"));
			_actionTask = null;
		}

		protected IEnumerator _doAction()
		{
			UnityEngine.Debug.Log(HW2_LogHelper.Orange("---------- 闪电鱼_" + _lightningfish.type + "_Action_启动 ------------"));
			try
			{
				_numSprite.SetPosition(_lastDeadFish.fish.GetPosition());
				_numSprite.SetText("1");
				_numSprite.SetActive();
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			yield return _lastDeadFish.Play();
			while (_canContinue)
			{
				if (_isOwner)
				{
					int linkTypeIndex = 0;
					if (_linkedFishes.Count - 1 < 3)
					{
						linkTypeIndex = 0;
					}
					else if (_linkedFishes.Count - 1 < 7)
					{
						linkTypeIndex = 1;
					}
					else if (_linkedFishes.Count - 1 < 11)
					{
						linkTypeIndex = ((UnityEngine.Random.Range(0, 10) < 4) ? 2 : 3);
					}
					else
					{
						linkTypeIndex = 4;
					}
					List<FishBehaviour> screenFishList = FishMgr.Get().GetScreenFishList(delegate(FishBehaviour fish)
					{
						bool flag = _validFishTypesList[linkTypeIndex].Contains(fish.type);
						bool flag2 = fish.State == FishState.Live;
						return flag && flag2;
					});
					if (screenFishList.Count == 0)
					{
						HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/gunHitFishInAction", new object[2]
						{
							11,
							string.Empty
						});
						break;
					}
					int index = UnityEngine.Random.Range(0, screenFishList.Count - 1);
					FishBehaviour fishBehaviour = screenFishList[index];
					__targetFishID = fishBehaviour.id;
					string text = $"{fishBehaviour.id}#{(int)fishBehaviour.type}";
					object[] args = new object[2]
					{
						11,
						text
					};
					HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/gunHitFishInAction", args);
					UnityEngine.Debug.Log(HW2_LogHelper.Green("闪电_Action_尝试电死：" + fishBehaviour.type));
				}
				_curDeadFish = null;
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
				if (!_hitFishMsgReturned)
				{
					break;
				}
				_hitFishMsgReturned = false;
				if (_curDeadFish == null)
				{
					break;
				}
				_curDeadFish.index = _linkedFishes.Count + 1;
				_linkedFishes.Add(_curDeadFish);
				if (null == _lastDeadFish.moveTarget)
				{
					_lastDeadFish.moveTarget = _curDeadFish.fish;
				}
				yield return _curDeadFish.Play(_lastDeadFish.fish);
				_lastDeadFish = _curDeadFish;
			}
			stopwatch.Reset();
			stopwatch.Start();
			yield return new WaitForSeconds(1f);
			foreach (LightningDeadFish linkedFish in _linkedFishes)
			{
				linkedFish.Stop();
			}
			_linkedFishes.Clear();
			_numSprite.SetActive(active: false);
			_lightningfish = null;
			UnityEngine.Debug.Log(HW2_LogHelper.Orange("####### 闪电鱼_Action_结束 #########"));
			_actionTask = null;
		}
	}
}
