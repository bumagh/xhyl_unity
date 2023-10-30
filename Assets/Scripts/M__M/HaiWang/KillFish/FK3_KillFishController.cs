using DG.Tweening;
using M__M.HaiWang.Effect;
using M__M.HaiWang.Fish;
using M__M.HaiWang.GameDefine;
using M__M.HaiWang.Player.Gun;
using M__M.HaiWang.Scenario;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.KillFish
{
public class FK3_KillFishController
{
	private FK3_GunBehaviour m_gun;

	private FK3_KillFishData m_curData;

	private MonoBehaviour manager;

	private Stopwatch stopwatch = new Stopwatch();

	public FK3_KillFishController(MonoBehaviour manager)
	{
		this.manager = manager;
	}

	public FK3_KillFishController SetGun(FK3_GunBehaviour gun)
	{
		m_gun = gun;
		return this;
	}

	public void DoDeadFish(FK3_KillFishData data, FK3_FishDeadInfo deadInfo, bool killMutiply = false)
	{
		manager.StartCoroutine(IE_DoDeadFish(data, deadInfo, killMutiply));
	}

	private IEnumerator IE_DoDeadFish(FK3_KillFishData data, FK3_FishDeadInfo deadInfo, bool killMutiply = false)
	{
		m_curData = data;
		int fishId = data.fishId;
		FK3_GunBehaviour gun = FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(data.seatId);
		Vector3 fishPos = data.fish.transform.position;
		Vector3 gunPos = gun.transform.position;
		Action actionOnScoreStop = null;
		actionOnScoreStop = (Action)Delegate.Combine(actionOnScoreStop, (Action)delegate
		{
			FK3_Effect_CoinMgr.Get().SpawnBigFishCoins(deadInfo.fishRate, fishPos, gunPos, deadInfo.killerSeatId);
		});
		bool bigScore = false;
		bool scoreFollow = false;
		bool showScoreBack = false;
		Action actionSimpleScoreEffect = delegate
		{
			try
			{
				Transform transform = data.fish.transform;
				if ((bool)transform)
				{
					FK3_FollowData followData = new FK3_FollowData(transform);
					if (data.fishType == FK3_FishType.Big_Clown_巨型小丑鱼 || data.fishType == FK3_FishType.Big_Rasbora_巨型鲽鱼 || data.fishType == FK3_FishType.Shark_鲨鱼)
					{
						followData = new FK3_FollowData(transform.Find("follow"));
					}
					FK3_Effect_ScoreMgr.Get().DoPlayScore(deadInfo.addScore, data.fishType, deadInfo.bulletPower, data.seatId, followData, false, bigScore, scoreFollow, showScoreBack, deadInfo.deadWay, actionOnScoreStop);
				}
				else
				{
					UnityEngine.Debug.LogError("fishTransform为空");
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("错误: " + ex);
			}
		};
		if (killMutiply)
		{
			actionSimpleScoreEffect();
		}
		else if (data.fishType == FK3_FishType.CrabBoom_连环炸弹蟹)
		{
			UnityEngine.Debug.LogWarning(string.Format("CrabBoom_连环炸弹蟹[id:{0}] 不应在此处理死亡逻辑", data.fishId));
			data.fish.Die();
		}
		else if (data.fishType == FK3_FishType.CrabLaser_电磁蟹)
		{
			data.fish.Die();
		}
		else if (data.fishType == FK3_FishType.CrabDrill_钻头蟹)
		{
			data.fish.Die();
		}
		else if (FK3_FishMgr.bossFishSet.Contains(data.fishType))
		{
			stopwatch.Reset();
			stopwatch.Start();
			if (data.fishType == FK3_FishType.Boss_Dorgan_狂暴火龙 || data.fishType == FK3_FishType.Boss_Dorgan_冰封暴龙 || data.fishType == FK3_FishType.Boss_Kraken_深海八爪鱼 || data.fishType == FK3_FishType.Boss_Lantern_暗夜炬兽 || data.fishType == FK3_FishType.Boss_Crab_霸王蟹 || data.fishType == FK3_FishType.Boss_Crocodil_史前巨鳄)
			{
				UnityEngine.Debug.LogError("====特殊死亡处理===" + data.fishType);
				DoPlayBossFishDyingAnimation(data.fish);
				if (data.fishType == FK3_FishType.Boss_Dorgan_狂暴火龙 || data.fishType == FK3_FishType.Boss_Dorgan_冰封暴龙)
				{
					FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("火龙被击杀时的音效1");
					yield return new WaitForSeconds(0.5f);
					FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("BOSS被击杀音效2");
					FK3_Singleton<FK3_SoundMgr>.Get().SetVolume(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("BOSS被击杀音效2"), 1f);
				}
				if (data.fishType == FK3_FishType.Boss_Lantern_暗夜炬兽 || data.fishType == FK3_FishType.Boss_Crab_霸王蟹 || data.fishType == FK3_FishType.Boss_Kraken_深海八爪鱼 || data.fishType == FK3_FishType.Boss_Crocodil_史前巨鳄)
				{
					FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("霸王蟹、深海八爪鱼、暗夜巨兽被击杀时的音效1");
					yield return new WaitForSeconds(0.5f);
					FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("BOSS被击杀音效2");
					FK3_Singleton<FK3_SoundMgr>.Get().SetVolume(FK3_Singleton<FK3_SoundMgr>.Get().GetClip("BOSS被击杀音效2"), 1f);
				}
				yield return new WaitForSeconds(0.8f);
				for (int i = 0; i < data.fish.GetBoomPositions().Length; i++)
				{
					if (data.fishType == FK3_FishType.Boss_Dorgan_狂暴火龙 || data.fishType == FK3_FishType.Boss_Dorgan_冰封暴龙)
					{
						switch (i)
						{
						case 0:
							FK3_EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 2);
							break;
						case 1:
							FK3_EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 1);
							break;
						case 2:
							FK3_EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 1);
							break;
						case 3:
							FK3_EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 3);
							break;
						case 4:
							FK3_EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 3);
							break;
						case 5:
							FK3_EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 3);
							break;
						}
					}
					else if (data.fishType == FK3_FishType.Boss_Lantern_暗夜炬兽)
					{
						switch (i)
						{
						case 0:
							FK3_EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 0);
							break;
						case 1:
							FK3_EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 1);
							break;
						case 2:
							FK3_EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 1);
							break;
						case 3:
							FK3_EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 2);
							break;
						}
					}
					else
					{
						FK3_EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 0);
					}
				}
				yield return new WaitForSeconds(1f);
				fishPos = data.fish.GetCenterTransform().position;
				data.fish.Die();
				FK3_Effect_LogoMgr.Get().SpawnBossCrystalFire(fishPos);
				yield return new WaitForSeconds(1f);
				FK3_Effect_Logo bossLogo = FK3_Effect_LogoMgr.Get().SpawnBossLogo(data.fishType);
				G.SetActive(bossLogo, false);
				FK3_Effect_Logo bosscrystal = FK3_Effect_LogoMgr.Get().SpawnBossCrystal(data.fishType);
				FK3_GunBehaviour fK3_GunBehaviour = gun;
				List<Transform> list = new List<Transform>();
				list.Add(bossLogo.transform);
				list.Add(bosscrystal.transform);
				Transform bigFishDeadPos = fK3_GunBehaviour.GetTargetdPos(list);
				bosscrystal.Play(fishPos, bigFishDeadPos.position, data.seatId, default(int));
				yield return new WaitForSeconds(1f);
				G.SetActive(bossLogo);
				bossLogo.Play(bigFishDeadPos.position, Vector3.zero, data.seatId, default(int));
				actionOnScoreStop = null;
				actionOnScoreStop = (Action)Delegate.Combine(actionOnScoreStop, (Action)delegate
				{
					Vector3 vector = gunPos;
					gunPos.y += ((data.seatId == 1 || data.seatId == 2) ? 2.3f : (-2.3f));
					FK3_Effect_CoinMgr.Get().SpawnBigFishCoins(deadInfo.fishRate, gunPos, gunPos, data.seatId);
					try
					{
						ShortcutExtensions46.DOFade(bosscrystal.GetComponent<Image>(), 0f, 1f);
						ShortcutExtensions46.DOFade(bossLogo.GetComponent<Image>(), 0f, 1f);
					}
					catch (Exception message)
					{
						UnityEngine.Debug.LogError(message);
					}
					gun.ResetOccupiedTargetPoses();
					UnityEngine.Debug.Log(FK3_LogHelper.Orange(string.Format("Boss死鱼通知后动画效果耗时：{0} 倍率{1}", stopwatch.Elapsed.TotalSeconds, deadInfo.fishRate)));
				});
				FK3_Effect_ScoreMgr.Get().DoPlayScore(deadInfo.addScore, data.fishType, deadInfo.bulletPower, data.seatId, new FK3_FollowData(bosscrystal.transform), true, true, true, showScoreBack, deadInfo.deadWay, actionOnScoreStop);
			}
			else
			{
				data.fish.Die();
			}
		}
		else if (data.fishType == FK3_FishType.GoldShark_霸王鲸)
		{
			bigScore = true;
			scoreFollow = true;
			FK3_GunBehaviour fK3_GunBehaviour2 = gun;
			List<Transform> list = new List<Transform>();
			list.Add(data.fish.transform);
			Transform bigFishDeadPos2 = fK3_GunBehaviour2.GetTargetdPos(list);
			Vector3 targetPos = bigFishDeadPos2.position;
			targetPos.y += ((data.seatId == 1 || data.seatId == 2) ? 0.4f : (-0.4f));
			ShortcutExtensions.DOMove(data.fish.transform, targetPos, 3f);
			actionOnScoreStop = null;
			actionOnScoreStop = (Action)Delegate.Combine(actionOnScoreStop, (Action)delegate
			{
				fishPos = data.fish.transform.position;
				data.fish.Die();
				FK3_Effect_CoinMgr.Get().SpawnBigFishCoins(deadInfo.fishRate, fishPos, gunPos, data.seatId);
				gun.ResetOccupiedTargetPoses();
			});
			FK3_Effect_LogoMgr.Get().SpawnGoldSharkLogo(gunPos, data.seatId);
			actionSimpleScoreEffect();
			manager.StartCoroutine(DoPlayBigFishDyingAnimation(data.fish));
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("霸王鲸被捕获音效1");
			yield return new WaitForSeconds(1f);
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("霸王鲸被捕获音效2");
		}
		else if (FK3_FishMgr.bigFishSet.Contains(data.fishType))
		{
			bigScore = true;
			scoreFollow = true;
			showScoreBack = true;
			FK3_GunBehaviour fK3_GunBehaviour3 = gun;
			List<Transform> list = new List<Transform>();
			list.Add(data.fish.transform);
			Transform targetdPos = fK3_GunBehaviour3.GetTargetdPos(list);
			ShortcutExtensions.DOMove(data.fish.transform, targetdPos.position, 1.5f);
			if (data.fishType != FK3_FishType.KillerWhale_杀人鲸)
			{
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("三种巨型鱼、鲨鱼被捕获时的音效1");
			}
			else
			{
				PlayClip();
			}
			actionOnScoreStop = null;
			actionOnScoreStop = (Action)Delegate.Combine(actionOnScoreStop, (Action)delegate
			{
				fishPos = data.fish.transform.position;
				data.fish.Die();
				FK3_Effect_CoinMgr.Get().SpawnBigFishCoins(deadInfo.fishRate, fishPos, gunPos, data.seatId);
				gun.ResetOccupiedTargetPoses();
			});
			actionSimpleScoreEffect();
			manager.StartCoroutine(DoPlayBigFishDyingAnimation(data.fish));
		}
		else
		{
			scoreFollow = true;
			actionOnScoreStop = (Action)Delegate.Combine(actionOnScoreStop, (Action)delegate
			{
				data.fish.Die();
			});
			manager.StartCoroutine(DoPlayBigFishDyingAnimation(data.fish));
			if (!data.fish.isLightning)
			{
				actionSimpleScoreEffect();
			}
			if (data.fish.isLightning)
			{
				yield return new WaitForSeconds(0.5f);
				data.fish.Die();
				FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("获取金币音效");
			}
		}
	}

	private void PlayClip()
	{
		switch (UnityEngine.Random.Range(0, 3))
		{
		case 0:
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("杀人鲸捕获语音1");
			break;
		case 1:
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("杀人鲸捕获语音2");
			break;
		case 2:
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("杀人鲸捕获语音3");
			break;
		default:
			UnityEngine.Debug.Log("error 杀人鲸捕获 Clip index");
			break;
		}
	}

	private void DoPlaySingleFishDyingAnimation(FK3_FishBehaviour fish)
	{
		fish.Dying(true);
	}

	private void DoPlayBossFishDyingAnimation(FK3_FishBehaviour fish)
	{
		if (fish.type == FK3_FishType.Boss_Lantern_暗夜炬兽)
		{
			manager.StartCoroutine(DoPlayBigFishDyingAnimation(fish));
		}
		else
		{
			fish.Dying(true);
		}
		if (fish.type == FK3_FishType.Boss_Dorgan_狂暴火龙 || fish.type == FK3_FishType.Boss_Dorgan_冰封暴龙 || fish.type == FK3_FishType.Boss_Kraken_深海八爪鱼)
		{
			FK3_fiSimpleSingletonBehaviour<FK3_StoryScenarioMgr>.Get().ShakeScreen(15);
			if (fish.type == FK3_FishType.Boss_Dorgan_狂暴火龙 || fish.type == FK3_FishType.Boss_Dorgan_冰封暴龙)
			{
				ShortcutExtensions.DOScale(fish.transform, Vector3.zero, 1f);
			}
		}
		if (fish.type == FK3_FishType.Boss_Crocodil_史前巨鳄)
		{
			ShortcutExtensions.DOScale(fish.transform, Vector3.zero, 1f);
		}
	}

	private IEnumerator DoPlayBigFishDyingAnimation(FK3_FishBehaviour fish)
	{
		fish.Dying(true);
		if (fish.type == FK3_FishType.Big_Clown_巨型小丑鱼 || fish.type == FK3_FishType.Big_Rasbora_巨型鲽鱼 || fish.type == FK3_FishType.Shark_鲨鱼 || fish.type == FK3_FishType.Boss_Lantern_暗夜炬兽)
		{
			while (fish.gameObject.activeInHierarchy)
			{
				yield return new WaitForSeconds(0.2f);
				Vector3 p = fish.transform.localEulerAngles;
				p.z += 30f;
				ShortcutExtensions.DOLocalRotate(fish.transform, p, 0.2f, RotateMode.FastBeyond360);
			}
		}
		else if (fish.type == FK3_FishType.Grouper_狮子鱼 || fish.type == FK3_FishType.Flounder_比目鱼)
		{
			while (fish.gameObject.activeInHierarchy)
			{
				yield return new WaitForSeconds(0.3f);
				Vector3 p2 = fish.transform.localEulerAngles;
				p2.z += 80f;
				ShortcutExtensions.DOLocalRotate(fish.transform, p2, 0.3f, RotateMode.FastBeyond360);
			}
		}
	}
}
}
