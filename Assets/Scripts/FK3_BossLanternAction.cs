using DG.Tweening;
using HW3L;
using LitJson;
using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.Effect;
using M__M.HaiWang.Fish;
using M__M.HaiWang.GameDefine;
using M__M.HaiWang.KillFish;
using M__M.HaiWang.Scenario;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FK3_BossLanternAction : MonoBehaviour
{
	protected FK3_Task _actionTask;

	private Action _cleanAction;

	protected bool _canContinue;

	protected bool _hitFishMsgReturned;

	protected int _killerSeatID;

	protected int _bombCount;

	protected FK3_FishBehaviour _bombCrab;

	protected Vector3 _killerGunPos;

	private Transform _effect_LogobombCrab_Pre;

	private Transform _effect_LogobombCrab;

	private Transform _effect_LogobombCrab_Hole;

	private Text _text;

	private FK3_KillFishController _fishDeadAction;

	private Animator _effect_LogobombCrab_Anim;

	private Tween tween;

	private Tween textTween;

	public int totalScore
	{
		get;
		set;
	}

	public void Play(FK3_FishBehaviour bombCrab, FK3_KillFishController fK3_KillFish, int seatID, int bulletPower)
	{
		if (!(_bombCrab != null))
		{
			_fishDeadAction = fK3_KillFish;
			_bombCrab = bombCrab;
			_killerSeatID = seatID;
			_canContinue = true;
			totalScore = 0;
			UnityEngine.Debug.LogError("暗夜巨兽死亡攻击准备");
			_effect_LogobombCrab_Pre = FK3_Effect_LogoMgr.Get().SpawnBossCrabLogo("Logo_BossLanternDie");
			_effect_LogobombCrab = _effect_LogobombCrab_Pre.Find("Logo_BossLanternDie");
			_effect_LogobombCrab_Anim = _effect_LogobombCrab.GetComponent<Animator>();
			_effect_LogobombCrab_Hole = _effect_LogobombCrab_Pre.Find("Logo_BossLanternDie_Hole");
			_effect_LogobombCrab_Hole.localScale = Vector3.zero;
			_effect_LogobombCrab.localScale = Vector3.one;
			_effect_LogobombCrab_Pre.localPosition = _bombCrab.transform.localPosition;
			_effect_LogobombCrab_Pre.localScale = Vector3.zero;
			_text = _effect_LogobombCrab_Pre.Find("Logo_BossLanternDie/Text").GetComponent<Text>();
			tween = null;
			tween = _effect_LogobombCrab_Pre.DOScale(Vector3.one, 1f);
			tween.OnComplete(delegate
			{
				_effect_LogobombCrab_Pre.DOLocalMove(Vector3.zero, 2f);
				_hitFishMsgReturned = false;
				_actionTask = new FK3_Task(IE_DoAction());
			});
		}
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

	private void PlayAnim(Animator animator, string playName)
	{
		animator.SetTrigger(playName);
	}

	private IEnumerator IE_DoAction()
	{
		UnityEngine.Debug.LogError("暗夜巨兽死亡攻击开始");
		_bombCount = 1;
		_cleanAction = (Action)Delegate.Combine(_cleanAction, (Action)delegate
		{
			_cleanAction = null;
		});
		yield return new WaitForSeconds(2f);
		_effect_LogobombCrab_Hole.DOScale(Vector3.one * 2.5f, 2f);
		bool isCanAttack = true;
		while ((_canContinue || isCanAttack) && _bombCount <= 5)
		{
			_effect_LogobombCrab.localScale = Vector3.one;
			PlayAnim(_effect_LogobombCrab_Anim, "Boom");
			_effect_LogobombCrab.DOScale(Vector3.one * 2f, 0.3f);
			yield return new WaitForSeconds(1f);
			List<FK3_FishBehaviour> fishes = FK3_FishMgr.Get().GetScreenFishList(delegate(FK3_FishBehaviour _fish)
			{
				if (!_fish.IsLive())
				{
					return false;
				}
				bool flag = Vector2.Distance(_effect_LogobombCrab_Pre.transform.position, _fish.transform.position) < 8f;
				return (!flag) ? flag : (_fish.type <= FK3_FishType.Mobula_蝠魟 || _fish.type == FK3_FishType.Big_Clown_巨型小丑鱼 || _fish.type == FK3_FishType.Big_Rasbora_巨型鲽鱼 || _fish.type == FK3_FishType.Big_Puffer_巨型河豚);
			});
			List<FK3_FishData4Hit> list = (from _fish in fishes
				select new FK3_FishData4Hit(_fish)).ToList();
			string text = (from _fish in list
				select $"{_fish.fishId}#{_fish.fishType}").JoinStrings("|");
			object[] args = new object[2]
			{
				17,
				text
			};
			UnityEngine.Debug.LogError("发送暗夜巨兽1: " + JsonMapper.ToJson(args));
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/gunHitFishInAction", args);
			FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().ShakeScreen(2);
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("连环炸弹爆炸音效");
			_bombCount++;
			SetText(_text, "X " + _bombCount.ToString("00"));
			yield return new WaitForSeconds(0.6f);
			_effect_LogobombCrab.localScale = Vector3.one;
			yield return new WaitUntil(() => _hitFishMsgReturned || !_canContinue);
			_hitFishMsgReturned = false;
			if (_bombCount >= 5)
			{
				isCanAttack = false;
			}
			yield return new WaitForSeconds(2f);
		}
		UnityEngine.Object.Destroy(_effect_LogobombCrab_Pre.gameObject);
		UnityEngine.Debug.LogError("暗夜巨兽 end");
		_cleanAction = null;
		_bombCrab = null;
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
