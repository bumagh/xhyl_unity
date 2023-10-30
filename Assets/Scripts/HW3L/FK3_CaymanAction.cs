using DG.Tweening;
using LitJson;
using M__M.GameHall.Common;
using M__M.GameHall.Net;
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

namespace HW3L
{
	public class FK3_CaymanAction : MonoBehaviour
	{
		protected int _bombCount;

		protected FK3_NumberSprite _numSprite;

		private Animator _effectRange;

		private Action _cleanAction;

		protected bool _canContinue;

		protected bool _hitFishMsgReturned;

		private Stopwatch stopwatch = new Stopwatch();

		protected FK3_FishBehaviour _bombCrab;

		private Animator animator;

		protected int _killerSeatID;

		private Action<FK3_FishBehaviour, FK3_FishDeadInfo> _fishDeadAction;

		protected Vector3 _killerGunPos;

		private FK3_Effect_Logo _effect_LogobombCrab;

		protected FK3_Task _actionTask;

		public float waitBigFishShowTime
		{
			get;
			set;
		}

		public int totalScore
		{
			get;
			set;
		}

		private void InitDoAction()
		{
			if (_numSprite == null)
			{
				FK3_NumberSprite original = Resources.Load<FK3_NumberSprite>("FishEffect/LightningFish/Number2");
				_numSprite = UnityEngine.Object.Instantiate(original);
				UnityEngine.Object.DontDestroyOnLoad(_numSprite.gameObject);
				FK3_GVars.dontDestroyOnLoadList.Add(_numSprite.gameObject);
				_numSprite.gameObject.transform.localScale = Vector3.one;
				_numSprite.name = "烈焰龟数字";
				_numSprite.SetText("1");
				_numSprite.SetActive(active: false);
				_numSprite.fixFace = true;
			}
		}

		private Animator GetEffectRange()
		{
			string path = "FishEffect/CrabBoom_Range0";
			_effectRange = Resources.Load<Animator>(path);
			return UnityEngine.Object.Instantiate(_effectRange);
		}

		private void PlayAnim(FK3_FishBehaviour fish, string name)
		{
			if (animator == null)
			{
				animator = fish.GetComponent<Animator>();
			}
			if (animator == null)
			{
				animator = fish.transform.GetChild(0).GetComponent<Animator>();
			}
			animator.SetTrigger(name);
		}

		public void Play(FK3_FishBehaviour bombCrab, Action<FK3_FishBehaviour, FK3_FishDeadInfo> fishDeadAction, int seatID, int bulletPower)
		{
			if (!(_bombCrab != null))
			{
				_bombCrab = bombCrab;
				_killerSeatID = seatID;
				_fishDeadAction = fishDeadAction;
				_canContinue = true;
				totalScore = 0;
				waitBigFishShowTime = 0f;
				FK3_GunBehaviour gunById = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(seatID);
				_killerGunPos = gunById.transform.position;
				_effect_LogobombCrab = FK3_Effect_LogoMgr.Get().SpawnCaymanLogo(bombCrab.GetPosition(), _killerSeatID, bulletPower);
				_hitFishMsgReturned = false;
				bombCrab.Dying();
				_actionTask = new FK3_Task(IE_DoAction());
			}
		}

		private IEnumerator IE_DoAction()
		{
			UnityEngine.Debug.LogError("烈焰龟开始");
			PlayAnim(_bombCrab, "Die");
			InitDoAction();
			FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().caymanDieEffect.gameObject.SetActive(value: true);
			Transform transform = _bombCrab.transform;
			Vector3 localPosition = _bombCrab.transform.localPosition;
			transform.DOLocalMove(new Vector3(0f, 0f, localPosition.z), 2f);
			_bombCrab.transform.localScale = Vector3.one * 200f;
			_bombCrab.transform.DORotate(Vector3.zero, 0.1f);
			float waitDuration = 0.5f;
			_bombCount = 1;
			_numSprite.transform.SetParent(_bombCrab.transform);
			_numSprite.transform.localPosition = Vector3.zero;
			_numSprite.SetText(_bombCount.ToString());
			_cleanAction = (Action)Delegate.Combine(_cleanAction, (Action)delegate
			{
				_cleanAction = null;
			});
			yield return new WaitForSeconds(waitDuration + 1.5f);
			while (_canContinue)
			{
				PlayAnim(_bombCrab, "Boom");
				_numSprite.SetActive();
				yield return new WaitForSeconds(waitDuration + 1.2f);
				List<FK3_FishBehaviour> fishes = FK3_FishMgr.Get().GetScreenFishList(delegate(FK3_FishBehaviour _fish)
				{
					if (!_fish.IsLive())
					{
						return false;
					}
					if (_fish.id == _bombCrab.id)
					{
						return false;
					}
					bool flag = Vector2.Distance(_bombCrab.transform.position, _fish.transform.position) < 3f;
					return (!flag) ? flag : (_fish.type <= FK3_FishType.Mobula_蝠魟 || _fish.type == FK3_FishType.Big_Clown_巨型小丑鱼 || _fish.type == FK3_FishType.Big_Rasbora_巨型鲽鱼 || _fish.type == FK3_FishType.Big_Puffer_巨型河豚);
				});
				List<FK3_FishData4Hit> list = (from _fish in fishes
					select new FK3_FishData4Hit(_fish)).ToList();
				if (list.Count == 0)
				{
					UnityEngine.Debug.LogError($"烈焰龟 查找失败. 次数:{_bombCount}");
				}
				string text = (from _fish in list
					select $"{_fish.fishId}#{_fish.fishType}").JoinStrings("|");
				object[] args = new object[2]
				{
					15,
					text
				};
				UnityEngine.Debug.LogError("发送烈焰龟: " + JsonMapper.ToJson(args));
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/gunHitFishInAction", args);
				yield return new WaitUntil(() => _hitFishMsgReturned || !_canContinue);
				_hitFishMsgReturned = false;
				FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().ShakeScreen();
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("连环炸弹爆炸音效");
				if (!_canContinue)
				{
					break;
				}
				float flyDuration = 0.8f;
				_bombCount++;
				_numSprite.SetText(_bombCount.ToString());
				_numSprite.SetActive(active: false);
				UnityEngine.Debug.LogError(FK3_LogHelper.Lime($"烈焰龟bombCount:{_bombCount}"));
				yield return new WaitForSeconds(flyDuration + waitDuration);
				yield return null;
			}
			yield return new WaitForSeconds(waitDuration);
			UnityEngine.Debug.LogError("烈焰龟 end");
			FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().caymanDieEffect.gameObject.SetActive(value: false);
			_bombCrab.Die();
			_numSprite.transform.SetParent(null);
			_numSprite.SetActive(active: false);
			_bombCrab = null;
			_cleanAction = null;
			yield return new WaitForSeconds(2f);
			FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetNativeGun();
			if (waitBigFishShowTime > 0f)
			{
				yield return new WaitForSeconds(waitBigFishShowTime);
			}
			Vector3 position = _effect_LogobombCrab.transform.position;
			ShortcutExtensions.DOMoveY(endValue: position.y + ((_killerSeatID == 1 || _killerSeatID == 2) ? 0.2f : (-0.2f)), target: _effect_LogobombCrab.transform, duration: 0.5f);
			FK3_Effect_Logo effect_logo_BombCrabScore = FK3_Effect_LogoMgr.Get().SpawnLogoScore(_killerGunPos, _killerSeatID, totalScore, FK3_Effect_LogoMgr.LogoScoreTypes.ScoreBombCrab);
			effect_logo_BombCrabScore.GetComponent<Image>().DOFade(1f, 0.1f);
			_effect_LogobombCrab.GetComponent<Image>().DOFade(1f, 0.1f);
			yield return new WaitForSeconds(3f);
			effect_logo_BombCrabScore.GetComponent<Image>().DOFade(0f, 1f);
			_effect_LogobombCrab.GetComponent<Image>().DOFade(0f, 1f);
			yield return new WaitForSeconds(1f);
			effect_logo_BombCrabScore.Over();
			_effect_LogobombCrab.Over();
			stopwatch.Stop();
			UnityEngine.Debug.Log(FK3_LogHelper.Orange($"连环炸弹最后一次收到死鱼通知后动画效果耗时：{stopwatch.Elapsed.TotalSeconds}"));
		}

		public void Stop()
		{
			if (_actionTask != null)
			{
				_actionTask.Stop();
				_actionTask = null;
			}
			if (_effect_LogobombCrab != null)
			{
				_effect_LogobombCrab.Over();
				_effect_LogobombCrab = null;
			}
			if (_bombCrab != null)
			{
				_bombCrab.Die();
			}
			_fishDeadAction = null;
			if ((bool)_numSprite)
			{
				_numSprite.transform.SetParent(null);
				_numSprite.SetActive(active: false);
			}
			DoReset();
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

		public void OnActionMsgReturn(bool canContinue, List<FK3_DeadFishData> deadFishs)
		{
			UnityEngine.Debug.Log($"BombCrabAction.OnActionMsgReturn> continue:{canContinue}, deadFishs:{deadFishs.Count}");
			_canContinue = canContinue;
			_hitFishMsgReturned = true;
			stopwatch.Reset();
			stopwatch.Start();
			foreach (FK3_DeadFishData deadFish in deadFishs)
			{
				FK3_FishDeadInfo fK3_FishDeadInfo = new FK3_FishDeadInfo();
				fK3_FishDeadInfo.fishId = deadFish.fish.id;
				fK3_FishDeadInfo.bulletPower = deadFish.bulletPower;
				fK3_FishDeadInfo.fishType = (int)deadFish.fish.type;
				fK3_FishDeadInfo.killerSeatId = _killerSeatID;
				fK3_FishDeadInfo.addScore = deadFish.score;
				fK3_FishDeadInfo.deadWay = 5;
				fK3_FishDeadInfo.fishRate = deadFish.fishRate;
				_fishDeadAction(deadFish.fish, fK3_FishDeadInfo);
			}
		}
	}
}
