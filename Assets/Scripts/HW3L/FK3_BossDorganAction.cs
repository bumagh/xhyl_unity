using DG.Tweening;
using DragonBones;
using LitJson;
using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.Effect;
using M__M.HaiWang.Fish;
using M__M.HaiWang.GameDefine;
using M__M.HaiWang.KillFish;
using M__M.HaiWang.Scenario;
using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HW3L
{
	public class FK3_BossDorganAction : MonoBehaviour
	{
		protected FK3_Task _actionTask;

		private Action _cleanAction;

		protected bool _canContinue;

		protected bool _hitFishMsgReturned;

		protected int _killerSeatID;

		protected int _bombCount;

		protected FK3_FishBehaviour _bombCrab;

		protected Vector3 _killerGunPos;

		private UnityEngine.Transform _effect_LogobombCrab;

		private Text _text;

		private FK3_KillFishController _fishDeadAction;

		private FK3_SpawnPool m_fishPool;

		private UnityEngine.Transform boss_Crab_Die;

		private Tween tween;

		private UnityEngine.Transform myEffect;

		private UnityArmatureComponent[] animators = new UnityArmatureComponent[4];

		private Tween textTween;

		public void Play(FK3_FishBehaviour bombCrab, FK3_KillFishController fK3_KillFish, int seatID, int bulletPower)
		{
			if (!(_bombCrab != null))
			{
				_fishDeadAction = fK3_KillFish;
				_bombCrab = bombCrab;
				_killerSeatID = seatID;
				_canContinue = true;
				UnityEngine.Debug.LogError("龙死亡攻击准备");
				if (m_fishPool == null)
				{
					m_fishPool = FK3_PoolManager.Pools["BossDie"];
				}
				if (boss_Crab_Die != null)
				{
					UnityEngine.Object.Destroy(boss_Crab_Die.gameObject);
					boss_Crab_Die = null;
				}
				boss_Crab_Die = m_fishPool.Spawn(m_fishPool.prefabs[(_bombCrab.type != FK3_FishType.Boss_Dorgan_冰封暴龙) ? "Boss_Dorgan火_Die" : "Boss_Dorgan冰_Die"], m_fishPool.transform);
				InItAttack();
				boss_Crab_Die.gameObject.SetActive(value: true);
				_effect_LogobombCrab = FK3_Effect_LogoMgr.Get().SpawnBossCrabLogo("Logo_BossCrabDie");
				_effect_LogobombCrab.localPosition = _bombCrab.transform.localPosition;
				_effect_LogobombCrab.localScale = Vector3.zero;
				_text = _effect_LogobombCrab.Find("Text").GetComponent<Text>();
				tween = null;
				tween = _effect_LogobombCrab.DOScale(Vector3.one, 1f);
				tween.OnComplete(delegate
				{
					_effect_LogobombCrab.DOLocalMove(Vector3.zero, 2f);
					_hitFishMsgReturned = false;
					_actionTask = new FK3_Task(IE_DoAction());
				});
			}
		}

		private void InItAttack()
		{
			for (int i = 0; i < 4; i++)
			{
				animators[i] = boss_Crab_Die.Find("Boss_Dorgan" + (i + 1)).GetChild(0).GetComponent<UnityArmatureComponent>();
				StopAnim(animators[i]);
				animators[i].gameObject.SetActive(value: false);
			}
			myEffect = boss_Crab_Die.Find("MyEffect");
			myEffect.gameObject.SetActive(value: false);
		}

		private void SetText(Text text, string str)
		{
			text.text = str;
			textTween = text.transform.DOScale(Vector3.one * 2f, 0.5f);
			textTween.OnComplete(delegate
			{
				text.transform.DOScale(Vector3.one, 1.5f);
			});
		}

		private void PlayAnim(UnityArmatureComponent animator)
		{
			animator.gameObject.SetActive(value: true);
			animator.animation.Play();
		}

		private void StopAnim(UnityArmatureComponent animator)
		{
			animator.animation.Stop();
			animator.gameObject.SetActive(value: false);
		}

		private IEnumerator IE_DoAction()
		{
			UnityEngine.Debug.LogError("龙死亡攻击开始");
			_bombCount = 1;
			_cleanAction = (Action)Delegate.Combine(_cleanAction, (Action)delegate
			{
				_cleanAction = null;
			});
			yield return new WaitForSeconds(2f);
			bool isCanAttack = true;
			while (_canContinue && isCanAttack)
			{
				if (_bombCount % 2 != 0)
				{
					PlayAnim(animators[0]);
					PlayAnim(animators[1]);
					StopAnim(animators[2]);
					StopAnim(animators[3]);
				}
				else
				{
					PlayAnim(animators[2]);
					PlayAnim(animators[3]);
					StopAnim(animators[0]);
					StopAnim(animators[1]);
				}
				yield return new WaitForSeconds((_bombCrab.type != FK3_FishType.Boss_Dorgan_冰封暴龙) ? 1f : 0.5f);
				myEffect.SetActive();
				List<FK3_FishBehaviour> fishes = FK3_FishMgr.Get().GetScreenFishList(delegate(FK3_FishBehaviour _fish)
				{
					if (!_fish.IsLive())
					{
						return false;
					}
					bool flag2 = Vector2.Distance(myEffect.transform.position, _fish.transform.position) < 5f;
					return (!flag2) ? flag2 : (_fish.type <= FK3_FishType.Mobula_蝠魟 || _fish.type == FK3_FishType.Big_Clown_巨型小丑鱼 || _fish.type == FK3_FishType.Big_Rasbora_巨型鲽鱼 || _fish.type == FK3_FishType.Big_Puffer_巨型河豚);
				});
				List<FK3_FishData4Hit> list4 = (from _fish in fishes
					select new FK3_FishData4Hit(_fish)).ToList();
				string text4 = (from _fish in list4
					select $"{_fish.fishId}#{_fish.fishType}").JoinStrings("|");
				object[] args4 = new object[2]
				{
					20,
					text4
				};
				UnityEngine.Debug.LogError("发送龙1: " + JsonMapper.ToJson(args4));
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/gunHitFishInAction", args4);
				FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().ShakeScreen();
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("连环炸弹爆炸音效");
				_bombCount++;
				SetText(_text, "X " + _bombCount.ToString("00"));
				yield return new WaitForSeconds(0.6f);
				myEffect.SetActive(active: false);
				yield return new WaitForSeconds((_bombCrab.type != FK3_FishType.Boss_Dorgan_冰封暴龙) ? 2f : 1.5f);
				if (_bombCount % 2 == 0)
				{
					StopAnim(animators[0]);
					StopAnim(animators[1]);
				}
				else
				{
					StopAnim(animators[2]);
					StopAnim(animators[3]);
				}
				if (!_canContinue)
				{
					break;
				}
				yield return new WaitUntil(() => _hitFishMsgReturned || !_canContinue);
				_hitFishMsgReturned = false;
				if (_bombCount > 5)
				{
					isCanAttack = false;
				}
				UnityEngine.Debug.LogError(FK3_LogHelper.Lime($"龙bombCount:{_bombCount}"));
				yield return null;
			}
			yield return new WaitForSeconds(1f);
			for (int i = 0; i < animators.Length; i++)
			{
				PlayAnim(animators[i]);
			}
			yield return new WaitForSeconds(0.4f);
			myEffect.SetActive();
			List<FK3_FishBehaviour> fishes2 = FK3_FishMgr.Get().GetScreenFishList(delegate(FK3_FishBehaviour _fish)
			{
				if (!_fish.IsLive())
				{
					return false;
				}
				bool flag = Vector2.Distance(myEffect.transform.position, _fish.transform.position) < 8.5f;
				return (!flag) ? flag : (_fish.type <= FK3_FishType.Mobula_蝠魟 || _fish.type == FK3_FishType.Big_Clown_巨型小丑鱼 || _fish.type == FK3_FishType.Big_Rasbora_巨型鲽鱼 || _fish.type == FK3_FishType.Big_Puffer_巨型河豚);
			});
			List<FK3_FishData4Hit> list3 = (from _fish in fishes2
				select new FK3_FishData4Hit(_fish)).ToList();
			string text3 = (from _fish in list3
				select $"{_fish.fishId}#{_fish.fishType}").JoinStrings("|");
			object[] args3 = new object[2]
			{
				20,
				text3
			};
			UnityEngine.Debug.LogError("发送龙3: " + JsonMapper.ToJson(args3));
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/gunHitFishInAction", args3);
			Boom();
			yield return new WaitForSeconds(0.6f);
			myEffect.SetActive(active: false);
			UnityEngine.Object.Destroy(_effect_LogobombCrab.gameObject);
			UnityEngine.Object.Destroy(boss_Crab_Die.gameObject);
			UnityEngine.Debug.LogError("龙 end");
			_cleanAction = null;
			_bombCrab = null;
		}

		private void Boom()
		{
			HTExplosion effectBomb = GetEffectBomb();
			effectBomb.transform.SetParent(base.transform);
			UnityEngine.Transform transform = effectBomb.transform;
			Vector3 position = base.transform.position;
			float x = position.x;
			Vector3 position2 = base.transform.position;
			transform.position = new Vector3(x, position2.y, 0f);
			effectBomb.name = "爆炸特效";
			FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().ShakeScreen(2);
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("连环炸弹爆炸音效");
		}

		private HTExplosion GetEffectBomb()
		{
			HTExplosion hTExplosion = Resources.Load<HTExplosion>("VFX/VFX_Boom_01");
			if (hTExplosion == null)
			{
				UnityEngine.Debug.LogError("_effectBomb为空!");
				return null;
			}
			HTExplosion hTExplosion2 = UnityEngine.Object.Instantiate(hTExplosion);
			hTExplosion2.SetActive();
			return hTExplosion2;
		}

		public void OnActionMsgReturn(bool canContinue, List<FK3_DeadFishData> deadFishs)
		{
			UnityEngine.Debug.LogError($"continue:{canContinue}, deadFishs:{deadFishs.Count}");
			_canContinue = canContinue;
			_hitFishMsgReturned = true;
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
				_fishDeadAction.DoDeadFish(new FK3_KillFishData
				{
					fish = deadFish.fish,
					bulletPower = deadFish.bulletPower,
					fishId = deadFish.fish.id,
					seatId = _killerSeatID,
					fishType = deadFish.fish.type
				}, fK3_FishDeadInfo);
			}
		}

		public void Stop()
		{
			if (_actionTask != null)
			{
				_actionTask.Stop();
				_actionTask = null;
			}
			DoReset();
		}

		public void DoReset()
		{
			_canContinue = false;
			if (_cleanAction != null)
			{
				_cleanAction();
			}
		}
	}
}
