using DG.Tweening;
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
	public class FK3_BossCrocodileAction : MonoBehaviour
	{
		protected FK3_Task _actionTask;

		private Action _cleanAction;

		protected bool _canContinue;

		protected bool _hitFishMsgReturned;

		protected int _killerSeatID;

		protected int _bombCount;

		protected FK3_FishBehaviour _bombCrab;

		protected Vector3 _killerGunPos;

		private Transform _effect_LogobombCrab;

		private Text _text;

		private FK3_KillFishController _fishDeadAction;

		private FK3_SpawnPool m_fishPool;

		private Transform boss_Crab_Die;

		private Tween tween;

		private Transform boss_Crab_史前巨鳄_Attack1;

		private Transform boss_Crab_史前巨鳄_Attack2;

		private Transform myEffect;

		private GameObject Attack1_Attack1;

		private GameObject Attack1_Attack2;

		private GameObject Attack2_Attack1;

		private GameObject Attack2_Attack2;

		public Transform boss_Crab_史前巨鳄_Attack1_OldPos;

		public Transform boss_Crab_史前巨鳄_Attack2_OldPos;

		public Transform boss_Crab_史前巨鳄_Attack1_TagPos;

		public Transform boss_Crab_史前巨鳄_Attack2_TagPos;

		private Tween textTween;

		public void Play(FK3_FishBehaviour bombCrab, FK3_KillFishController fK3_KillFish, int seatID, int bulletPower)
		{
			if (!(_bombCrab != null))
			{
				_fishDeadAction = fK3_KillFish;
				_bombCrab = bombCrab;
				_killerSeatID = seatID;
				_canContinue = true;
				UnityEngine.Debug.LogError("史前巨鳄死亡攻击准备");
				if (m_fishPool == null)
				{
					m_fishPool = FK3_PoolManager.Pools["BossDie"];
				}
				if (boss_Crab_Die != null)
				{
					UnityEngine.Object.Destroy(boss_Crab_Die.gameObject);
					boss_Crab_Die = null;
				}
				boss_Crab_Die = m_fishPool.Spawn(m_fishPool.prefabs["Boss_Crocodile_Die"], m_fishPool.transform);
				boss_Crab_Die.gameObject.SetActive(value: true);
				_effect_LogobombCrab = FK3_Effect_LogoMgr.Get().SpawnBossCrabLogo("Logo_BossCrocodileDie");
				_effect_LogobombCrab.localPosition = _bombCrab.transform.localPosition;
				_effect_LogobombCrab.localScale = Vector3.zero;
				_text = _effect_LogobombCrab.Find("Text").GetComponent<Text>();
				tween = null;
				tween = _effect_LogobombCrab.DOScale(Vector3.one * 1.5f, 1f);
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
			boss_Crab_史前巨鳄_Attack1 = boss_Crab_Die.Find("Boss_Crocodile_Attack1");
			Attack1_Attack1 = boss_Crab_史前巨鳄_Attack1.Find("Attack").gameObject;
			Attack1_Attack2 = boss_Crab_史前巨鳄_Attack1.Find("Attack2").gameObject;
			Attack1_Attack2.SetActive(value: false);
			boss_Crab_史前巨鳄_Attack2 = boss_Crab_Die.Find("Boss_Crocodile_Attack2");
			Attack2_Attack1 = boss_Crab_史前巨鳄_Attack2.Find("Attack").gameObject;
			Attack2_Attack2 = boss_Crab_史前巨鳄_Attack2.Find("Attack2").gameObject;
			Attack2_Attack2.SetActive(value: false);
			myEffect = boss_Crab_Die.Find("MyEffect");
			myEffect.gameObject.SetActive(value: false);
		}

		private void SetText(Text text, string str)
		{
			text.text = str;
			textTween = text.transform.DOScale(Vector3.one * 1.5f, 0.5f);
			textTween.OnComplete(delegate
			{
				text.transform.DOScale(Vector3.one * 0.8f, 1.5f);
			});
		}

		private void PlayAnim(GameObject object1, GameObject object2)
		{
			object1.SetActive(value: true);
			object2.SetActive(value: false);
		}

		private IEnumerator IE_DoAction()
		{
			UnityEngine.Debug.LogError("史前巨鳄死亡攻击开始");
			InItAttack();
			_bombCount = 1;
			_cleanAction = (Action)Delegate.Combine(_cleanAction, (Action)delegate
			{
				_cleanAction = null;
			});
			yield return new WaitForSeconds(2f);
			bool isCanAttack = true;
			while (_canContinue && isCanAttack)
			{
				PlayAnim(Attack1_Attack1, Attack1_Attack2);
				Transform target = boss_Crab_史前巨鳄_Attack1;
				Vector3 localPosition = boss_Crab_史前巨鳄_Attack1_TagPos.localPosition;
				target.DOLocalMoveX(localPosition.x, 0.6f);
				yield return new WaitForSeconds(0.35f);
				PlayAnim(Attack1_Attack2, Attack1_Attack1);
				Transform target2 = boss_Crab_史前巨鳄_Attack1;
				Vector3 localPosition2 = boss_Crab_史前巨鳄_Attack2_OldPos.localPosition;
				Tween tweenAttackRight = target2.DOLocalMoveX(localPosition2.x * 2.5f, 1.5f);
				tweenAttackRight.OnComplete(delegate
				{
					Transform transform2 = boss_Crab_史前巨鳄_Attack1;
					Vector3 localPosition9 = boss_Crab_史前巨鳄_Attack1_OldPos.localPosition;
					float x2 = localPosition9.x;
					Vector3 localPosition10 = boss_Crab_史前巨鳄_Attack1.localPosition;
					transform2.localPosition = new Vector3(x2, localPosition10.y);
				});
				yield return new WaitForSeconds(0.2f);
				PlayAnim(Attack1_Attack1, Attack1_Attack2);
				Boom();
				myEffect.gameObject.SetActive(value: true);
				List<FK3_FishBehaviour> fishes4 = FK3_FishMgr.Get().GetScreenFishList(delegate(FK3_FishBehaviour _fish)
				{
					if (!_fish.IsLive())
					{
						return false;
					}
					bool flag3 = Vector2.Distance(myEffect.transform.position, _fish.transform.position) < 5f;
					return (!flag3) ? flag3 : (_fish.type <= FK3_FishType.Mobula_蝠魟 || _fish.type == FK3_FishType.Big_Clown_巨型小丑鱼 || _fish.type == FK3_FishType.Big_Rasbora_巨型鲽鱼 || _fish.type == FK3_FishType.Big_Puffer_巨型河豚);
				});
				List<FK3_FishData4Hit> list5 = (from _fish in fishes4
					select new FK3_FishData4Hit(_fish)).ToList();
				string text5 = (from _fish in list5
					select $"{_fish.fishId}#{_fish.fishType}").JoinStrings("|");
				object[] args5 = new object[2]
				{
					18,
					text5
				};
				UnityEngine.Debug.LogError("发送史前巨鳄1: " + JsonMapper.ToJson(args5));
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/gunHitFishInAction", args5);
				FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().ShakeScreen();
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("连环炸弹爆炸音效");
				_bombCount++;
				SetText(_text, "X " + _bombCount.ToString("00"));
				yield return new WaitForSeconds(0.5f);
				myEffect.SetActive(active: false);
				yield return new WaitForSeconds(1.5f);
				PlayAnim(Attack2_Attack1, Attack2_Attack2);
				Transform target3 = boss_Crab_史前巨鳄_Attack2;
				Vector3 localPosition3 = boss_Crab_史前巨鳄_Attack2_TagPos.localPosition;
				target3.DOLocalMoveX(localPosition3.x, 0.6f);
				yield return new WaitForSeconds(0.35f);
				PlayAnim(Attack2_Attack2, Attack2_Attack1);
				Transform target4 = boss_Crab_史前巨鳄_Attack2;
				Vector3 localPosition4 = boss_Crab_史前巨鳄_Attack1_OldPos.localPosition;
				Tween tweenAttackLeft = target4.DOLocalMoveX(localPosition4.x * 2.5f, 2f);
				tweenAttackLeft.OnComplete(delegate
				{
					Transform transform = boss_Crab_史前巨鳄_Attack2;
					Vector3 localPosition7 = boss_Crab_史前巨鳄_Attack2_OldPos.localPosition;
					float x = localPosition7.x;
					Vector3 localPosition8 = boss_Crab_史前巨鳄_Attack2.localPosition;
					transform.localPosition = new Vector3(x, localPosition8.y);
				});
				yield return new WaitForSeconds(0.2f);
				PlayAnim(Attack2_Attack1, Attack2_Attack2);
				Boom();
				myEffect.SetActive();
				List<FK3_FishBehaviour> fishes5 = FK3_FishMgr.Get().GetScreenFishList(delegate(FK3_FishBehaviour _fish2)
				{
					if (!_fish2.IsLive())
					{
						return false;
					}
					bool flag2 = Vector2.Distance(myEffect.transform.position, _fish2.transform.position) < 5f;
					return (!flag2) ? flag2 : (_fish2.type <= FK3_FishType.Mobula_蝠魟 || _fish2.type == FK3_FishType.Big_Clown_巨型小丑鱼 || _fish2.type == FK3_FishType.Big_Rasbora_巨型鲽鱼 || _fish2.type == FK3_FishType.Big_Puffer_巨型河豚);
				});
				List<FK3_FishData4Hit> list4 = (from _fish in fishes5
					select new FK3_FishData4Hit(_fish)).ToList();
				string text4 = (from _fish in list4
					select $"{_fish.fishId}#{_fish.fishType}").JoinStrings("|");
				object[] args4 = new object[2]
				{
					18,
					text4
				};
				UnityEngine.Debug.LogError("发送史前巨鳄2: " + JsonMapper.ToJson(args4));
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/gunHitFishInAction", args4);
				FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().ShakeScreen();
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("连环炸弹爆炸音效");
				_bombCount++;
				SetText(_text, "X " + _bombCount.ToString("00"));
				yield return new WaitForSeconds(0.5f);
				myEffect.SetActive(active: false);
				yield return new WaitForSeconds(0.5f);
				if (!_canContinue)
				{
					break;
				}
				yield return new WaitUntil(() => _hitFishMsgReturned || !_canContinue);
				_hitFishMsgReturned = false;
				if (_bombCount > 6)
				{
					isCanAttack = false;
				}
				UnityEngine.Debug.LogError(FK3_LogHelper.Lime($"史前巨鳄bombCount:{_bombCount}"));
				yield return null;
			}
			yield return new WaitForSeconds(1f);
			PlayAnim(Attack1_Attack1, Attack1_Attack2);
			PlayAnim(Attack2_Attack1, Attack2_Attack2);
			Transform target5 = boss_Crab_史前巨鳄_Attack1;
			Vector3 localPosition5 = boss_Crab_史前巨鳄_Attack2_OldPos.localPosition;
			target5.DOLocalMoveX(localPosition5.x * 2f, 4.5f);
			Transform target6 = boss_Crab_史前巨鳄_Attack2;
			Vector3 localPosition6 = boss_Crab_史前巨鳄_Attack1_OldPos.localPosition;
			target6.DOLocalMoveX(localPosition6.x * 2f, 4.5f);
			yield return new WaitForSeconds(0.5f);
			PlayAnim(Attack1_Attack2, Attack1_Attack1);
			PlayAnim(Attack2_Attack2, Attack2_Attack1);
			yield return new WaitForSeconds(0.2f);
			Boom();
			myEffect.SetActive();
			List<FK3_FishBehaviour> fishes3 = FK3_FishMgr.Get().GetScreenFishList(delegate(FK3_FishBehaviour _fish)
			{
				if (!_fish.IsLive())
				{
					return false;
				}
				bool flag = Vector2.Distance(myEffect.transform.position, _fish.transform.position) < 7f;
				return (!flag) ? flag : (_fish.type <= FK3_FishType.Mobula_蝠魟 || _fish.type == FK3_FishType.Big_Clown_巨型小丑鱼 || _fish.type == FK3_FishType.Big_Rasbora_巨型鲽鱼 || _fish.type == FK3_FishType.Big_Puffer_巨型河豚);
			});
			List<FK3_FishData4Hit> list3 = (from _fish in fishes3
				select new FK3_FishData4Hit(_fish)).ToList();
			string text3 = (from _fish in list3
				select $"{_fish.fishId}#{_fish.fishType}").JoinStrings("|");
			object[] args3 = new object[2]
			{
				18,
				text3
			};
			UnityEngine.Debug.LogError("发送史前巨鳄3: " + JsonMapper.ToJson(args3));
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/gunHitFishInAction", args3);
			Boom();
			UnityEngine.Object.Destroy(_effect_LogobombCrab.gameObject);
			UnityEngine.Object.Destroy(boss_Crab_Die.gameObject);
			UnityEngine.Debug.LogError("史前巨鳄 end");
			_cleanAction = null;
			_bombCrab = null;
		}

		private void Boom()
		{
			HTExplosion effectBomb = GetEffectBomb();
			effectBomb.transform.SetParent(base.transform);
			Transform transform = effectBomb.transform;
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
