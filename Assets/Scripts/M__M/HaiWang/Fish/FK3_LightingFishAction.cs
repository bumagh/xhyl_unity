using DG.Tweening;
using Framework;
using HW3L;
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
using UnityEngine.UI;

namespace M__M.HaiWang.Fish
{
	public class FK3_LightingFishAction : FK3_ObjectBase
	{
		protected int _killerSeatID;

		protected Vector3 _killerGunPos;

		protected List<HashSet<FK3_FishType>> _validFishTypesList = new List<HashSet<FK3_FishType>>();

		protected FK3_FishBehaviour _lightningfish;

		protected List<FK3_LightningDeadFish> _linkedFishes;

		private FK3_LightningDeadFish _lastDeadFish;

		private FK3_LightningDeadFish _curDeadFish;

		protected FK3_Task _actionTask;

		protected bool _canContinue;

		protected bool _hitFishMsgReturned;

		protected Queue<FK3_LightningDeadFish> _cachedDeadFishs;

		protected FK3_NumberSprite _numSprite;

		private Action<FK3_FishBehaviour, FK3_FishDeadInfo> _fishDeadAction;

		private int __targetFishID;

		private FK3_Effect_Logo effect_logo_Lighting;

		private FK3_Task _actionEffectLogoShow;

		private bool _shouldOver_Log_Lighting;

		private bool _hasBigFish;

		private int _totalScore;

		private Stopwatch stopwatch = new Stopwatch();

		protected bool _isOwner => _killerSeatID == FK3_GVars.game.curSeatId;

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

		public FK3_LightingFishAction()
		{
			_lightningfish = null;
			_linkedFishes = new List<FK3_LightningDeadFish>();
			HashSet<FK3_FishType> hashSet = new HashSet<FK3_FishType>();
			HashSet<FK3_FishType> hashSet2 = new HashSet<FK3_FishType>();
			HashSet<FK3_FishType> hashSet3 = new HashSet<FK3_FishType>();
			HashSet<FK3_FishType> hashSet4 = new HashSet<FK3_FishType>();
			HashSet<FK3_FishType> hashSet5 = new HashSet<FK3_FishType>();
			_cachedDeadFishs = new Queue<FK3_LightningDeadFish>();
			for (int i = 1; i <= 19; i++)
			{
				hashSet2.Add((FK3_FishType)i);
				if (i <= 12)
				{
					hashSet.Add((FK3_FishType)i);
				}
				if (i <= 9)
				{
					hashSet3.Add((FK3_FishType)i);
				}
				if (i >= 10)
				{
					hashSet4.Add((FK3_FishType)i);
				}
				if (i >= 12)
				{
					hashSet5.Add((FK3_FishType)i);
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
				_numSprite = this.Instantiate("FishEffect/LightningFish/Number", active: false).GetComponent<FK3_NumberSprite>();
				UnityEngine.Object.DontDestroyOnLoad(_numSprite.gameObject);
				FK3_GVars.dontDestroyOnLoadList.Add(_numSprite.gameObject);
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

		public void Play(FK3_FishBehaviour lightingFish, Action<FK3_FishBehaviour, FK3_FishDeadInfo> fishDeadAction, int seatID, int bulletPower)
		{
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("闪电鱼logo出现在炮台上旋转的音效");
			FK3_GunBehaviour gunById = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(seatID);
			_killerGunPos = gunById.transform.position;
			effect_logo_Lighting = FK3_Effect_LogoMgr.Get().SpawnLightingLogo(_killerGunPos, seatID, bulletPower);
			totalScore = 0;
			hasBigFish = false;
			_shouldOver_Log_Lighting = true;
			if (_lightningfish != null || _actionTask != null || _cachedDeadFishs.Count != 0)
			{
				Stop();
				UnityEngine.Debug.LogError("==========lightning fish overlapped???================");
			}
			_fishDeadAction = fishDeadAction;
			_killerSeatID = seatID;
			_linkedFishes.Clear();
			_lightningfish = lightingFish;
			lightingFish.Dying(playAni: false);
			_lastDeadFish = new FK3_LightningDeadFish(_lightningfish, 0, 0, 0, _numSprite, this);
			_linkedFishes.Add(_lastDeadFish);
			_canContinue = true;
			if (_isOwner)
			{
				_actionTask = new FK3_Task(_doAction());
			}
			else
			{
				_actionTask = new FK3_Task(_doSyncAction());
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
			_shouldOver_Log_Lighting = false;
			_canContinue = true;
			foreach (FK3_LightningDeadFish linkedFish in _linkedFishes)
			{
				linkedFish.Stop();
			}
			foreach (FK3_LightningDeadFish cachedDeadFish in _cachedDeadFishs)
			{
				cachedDeadFish.Stop();
			}
			_linkedFishes.Clear();
			_cachedDeadFishs.Clear();
			_fishDeadAction = null;
			_lightningfish = null;
			_numSprite.SetActive(active: false);
		}

		public void OnFishDead(FK3_FishBehaviour fish, int bulletPower, int fishRate, int score)
		{
			if (_fishDeadAction == null)
			{
				UnityEngine.Debug.LogError("========this._fishDeadAction为空==========");
				return;
			}
			FK3_FishDeadInfo fK3_FishDeadInfo = new FK3_FishDeadInfo();
			fK3_FishDeadInfo.addScore = score;
			fK3_FishDeadInfo.bulletPower = bulletPower;
			fK3_FishDeadInfo.fishId = fish.id;
			fK3_FishDeadInfo.deadWay = 4;
			fK3_FishDeadInfo.fishType = (int)fish.type;
			fK3_FishDeadInfo.killerSeatId = _killerSeatID;
			fK3_FishDeadInfo.fishRate = fishRate;
			_fishDeadAction(fish, fK3_FishDeadInfo);
			if (_shouldOver_Log_Lighting)
			{
				_actionEffectLogoShow = new FK3_Task(EffetLogoShow());
				_shouldOver_Log_Lighting = false;
			}
		}

		private IEnumerator EffetLogoShow()
		{
			float waitTime = hasBigFish ? 9f : 2f;
			yield return new WaitForSeconds(waitTime);
			Vector3 position = effect_logo_Lighting.transform.position;
			ShortcutExtensions.DOMoveY(endValue: position.y + ((_killerSeatID == 1 || _killerSeatID == 2) ? 0.8f : (-0.8f)), target: effect_logo_Lighting.transform, duration: 0.5f);
			FK3_Effect_Logo effect_logo_LightingScore = FK3_Effect_LogoMgr.Get().SpawnLogoScore(_killerGunPos, _killerSeatID, totalScore, FK3_Effect_LogoMgr.LogoScoreTypes.ScoreLighting);
			yield return new WaitForSeconds(3f);
			try
			{
				effect_logo_LightingScore.GetComponent<Image>().DOFade(0f, 1f);
				effect_logo_Lighting.GetComponent<Image>().DOFade(0f, 1f);
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
			}
			yield return new WaitForSeconds(1f);
			effect_logo_Lighting.Over();
			effect_logo_LightingScore.Over();
			UnityEngine.Debug.Log(FK3_LogHelper.Orange($"闪电连锁收到死鱼通知后动画效果耗时{stopwatch.Elapsed.TotalSeconds}"));
		}

		public void OnActionMsgReturn(bool canContinue, List<FK3_DeadFishData> deadFishs)
		{
			if (_lightningfish == null)
			{
				return;
			}
			if (deadFishs.Count == 1)
			{
				FK3_DeadFishData fK3_DeadFishData = deadFishs[0];
				_canContinue = canContinue;
				if (_isOwner)
				{
					_hitFishMsgReturned = true;
					_curDeadFish = new FK3_LightningDeadFish(fK3_DeadFishData.fish, fK3_DeadFishData.bulletPower, fK3_DeadFishData.fishRate, fK3_DeadFishData.score, _numSprite, this);
				}
				else
				{
					FK3_LightningDeadFish fK3_LightningDeadFish = new FK3_LightningDeadFish(fK3_DeadFishData.fish, fK3_DeadFishData.bulletPower, fK3_DeadFishData.fishRate, fK3_DeadFishData.score, _numSprite, this);
					fK3_LightningDeadFish.index = _cachedDeadFishs.Count + 1;
					_cachedDeadFishs.Enqueue(fK3_LightningDeadFish);
				}
			}
			else if (deadFishs.Count == 0)
			{
				_canContinue = false;
			}
			else if (deadFishs.Count > 1)
			{
				UnityEngine.Debug.LogError("闪电_Action_失败：闪电鱼一次只能电死1个，但是死鱼列表里超过了1个!");
				Stop();
			}
		}

		protected IEnumerator _doSyncAction()
		{
			UnityEngine.Debug.Log(FK3_LogHelper.Orange("---------- 闪电鱼同步_" + _lightningfish.type + "_Action_启动 ------------"));
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
			foreach (FK3_LightningDeadFish linkedFish in _linkedFishes)
			{
				linkedFish.Stop();
			}
			_linkedFishes.Clear();
			_cachedDeadFishs.Clear();
			_numSprite.SetActive(active: false);
			_lightningfish = null;
			UnityEngine.Debug.Log(FK3_LogHelper.Orange("####### 闪电鱼同步_Action_结束 #########"));
			_actionTask = null;
		}

		protected IEnumerator _doAction()
		{
			UnityEngine.Debug.Log(FK3_LogHelper.Orange("---------- 闪电鱼_" + _lightningfish.type + "_Action_启动 ------------"));
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
					List<FK3_FishBehaviour> screenFishList = FK3_FishMgr.Get().GetScreenFishList(delegate(FK3_FishBehaviour fish)
					{
						bool flag = _validFishTypesList[linkTypeIndex].Contains(fish.type);
						bool flag2 = fish.State == FK3_FishState.Live;
						return flag && flag2;
					});
					if (screenFishList.Count == 0)
					{
						FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/gunHitFishInAction", new object[2]
						{
							11,
							string.Empty
						});
						break;
					}
					int index = UnityEngine.Random.Range(0, screenFishList.Count - 1);
					FK3_FishBehaviour fK3_FishBehaviour = screenFishList[index];
					__targetFishID = fK3_FishBehaviour.id;
					string text = $"{fK3_FishBehaviour.id}#{(int)fK3_FishBehaviour.type}";
					object[] args = new object[2]
					{
						11,
						text
					};
					FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/gunHitFishInAction", args);
					UnityEngine.Debug.Log(FK3_LogHelper.Green("闪电_Action_尝试电死：" + fK3_FishBehaviour.type));
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
			foreach (FK3_LightningDeadFish linkedFish in _linkedFishes)
			{
				linkedFish.Stop();
			}
			_linkedFishes.Clear();
			_numSprite.SetActive(active: false);
			_lightningfish = null;
			UnityEngine.Debug.Log(FK3_LogHelper.Orange("####### 闪电鱼_Action_结束 #########"));
			_actionTask = null;
		}
	}
}
