using DG.Tweening;
using M__M.HaiWang.Effect;
using M__M.HaiWang.Fish;
using M__M.HaiWang.GameDefine;
using M__M.HaiWang.Player.Gun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace M__M.HaiWang.KillFish
{

public class KillFishController
{
	private GunBehaviour m_gun;

	private KillFishData m_curData;

	private MonoBehaviour manager;

	private Stopwatch stopwatch = new Stopwatch();

	public KillFishController(MonoBehaviour manager)
	{
		this.manager = manager;
	}

	public KillFishController SetGun(GunBehaviour gun)
	{
		m_gun = gun;
		return this;
	}

	public void DoDeadFish(KillFishData data, FishDeadInfo deadInfo, bool killMutiply = false)
	{
		manager.StartCoroutine(IE_DoDeadFish(data, deadInfo, killMutiply));
	}

	private IEnumerator IE_DoDeadFish(KillFishData data, FishDeadInfo deadInfo, bool killMutiply = false)
	{
		m_curData = data;
		int fishId = data.fishId;
		GunBehaviour gun = fiSimpleSingletonBehaviour<GunMgr>.Get().GetGunById(data.seatId);
		Vector3 fishPos = data.fish.transform.position;
		Vector3 gunPos = gun.transform.position;
		Action actionOnScoreStop = null;
		actionOnScoreStop = (Action)Delegate.Combine(actionOnScoreStop, (Action)delegate
		{
			Effect_CoinMgr.Get().SpawnBigFishCoins(deadInfo.fishRate, fishPos, gunPos, deadInfo.killerSeatId);
		});
		bool bigScore = false;
		bool scoreFollow = false;
		bool showScoreBack = false;
		Action actionSimpleScoreEffect = delegate
		{
			try
			{
				if ((bool)data.fish.transform)
				{
					FollowData followData = new FollowData(data.fish.transform);
					if (data.fishType == FishType.Big_Clown_巨型小丑鱼 || data.fishType == FishType.Big_Rasbora_巨型鲽鱼 || data.fishType == FishType.Shark_鲨鱼)
					{
						followData = new FollowData(data.fish.transform.Find("follow"));
					}
					Effect_ScoreMgr.Get().DoPlayScore(deadInfo.addScore, data.fishType, deadInfo.bulletPower, data.seatId, followData, false, bigScore, scoreFollow, showScoreBack, deadInfo.deadWay, actionOnScoreStop);
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
		else if (data.fishType == FishType.CrabBoom_连环炸弹蟹)
		{
			UnityEngine.Debug.LogWarning(string.Format("CrabBoom_连环炸弹蟹[id:{0}] 不应在此处理死亡逻辑", data.fishId));
			data.fish.Die();
		}
		else if (data.fishType == FishType.CrabLaser_电磁蟹)
		{
			data.fish.Die();
		}
		else if (FishMgr.bossFishSet.Contains(data.fishType))
		{
			stopwatch.Reset();
			stopwatch.Start();
			if (data.fishType == FishType.Boss_Dorgan_狂暴火龙 || data.fishType == FishType.Boss_Kraken_深海八爪鱼 || data.fishType == FishType.Boss_Lantern_暗夜炬兽 || data.fishType == FishType.Boss_Crab_霸王蟹)
			{
				DoPlayBossFishDyingAnimation(data.fish);
				if (data.fishType == FishType.Boss_Dorgan_狂暴火龙)
				{
					HW2_Singleton<SoundMgr>.Get().PlayClip("火龙被击杀时的音效1");
					yield return new WaitForSeconds(0.5f);
					HW2_Singleton<SoundMgr>.Get().PlayClip("BOSS被击杀音效2");
					HW2_Singleton<SoundMgr>.Get().SetVolume(HW2_Singleton<SoundMgr>.Get().GetClip("BOSS被击杀音效2"), 1f);
				}
				if (data.fishType == FishType.Boss_Lantern_暗夜炬兽 || data.fishType == FishType.Boss_Crab_霸王蟹 || data.fishType == FishType.Boss_Kraken_深海八爪鱼)
				{
					HW2_Singleton<SoundMgr>.Get().PlayClip("霸王蟹、深海八爪鱼、暗夜巨兽被击杀时的音效1");
					yield return new WaitForSeconds(0.5f);
					HW2_Singleton<SoundMgr>.Get().PlayClip("BOSS被击杀音效2");
					HW2_Singleton<SoundMgr>.Get().SetVolume(HW2_Singleton<SoundMgr>.Get().GetClip("BOSS被击杀音效2"), 1f);
				}
				yield return new WaitForSeconds(0.8f);
				for (int i = 0; i < data.fish.GetBoomPositions().Length; i++)
				{
					if (data.fishType == FishType.Boss_Dorgan_狂暴火龙)
					{
						switch (i)
						{
						case 0:
							EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 2);
							break;
						case 1:
							EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 1);
							break;
						case 2:
							EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 1);
							break;
						case 3:
							EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 3);
							break;
						case 4:
							EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 3);
							break;
						case 5:
							EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 3);
							break;
						}
					}
					else if (data.fishType == FishType.Boss_Lantern_暗夜炬兽)
					{
						switch (i)
						{
						case 0:
							EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 0);
							break;
						case 1:
							EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 1);
							break;
						case 2:
							EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 1);
							break;
						case 3:
							EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 2);
							break;
						}
					}
					else
					{
						EffectMgr.Get().PlayBossBoom1(data.fish.GetBoomPositions()[i].position, 0);
					}
				}
				yield return new WaitForSeconds(1f);
				fishPos = data.fish.GetCenterTransform().position;
				data.fish.Die();
				Effect_LogoMgr.Get().SpawnBossCrystalFire(fishPos);
				yield return new WaitForSeconds(1f);
				Effect_Logo bossLogo = Effect_LogoMgr.Get().SpawnBossLogo(data.fishType);
				G.SetActive(bossLogo, false);
				Effect_Logo bosscrystal = Effect_LogoMgr.Get().SpawnBossCrystal(data.fishType);
				GunBehaviour gunBehaviour = gun;
				List<Transform> list = new List<Transform>();
				list.Add(bossLogo.transform);
				list.Add(bosscrystal.transform);
				Transform bigFishDeadPos = gunBehaviour.GetTargetdPos(list);
				bosscrystal.Play(fishPos, bigFishDeadPos.position, data.seatId, default(int));
				yield return new WaitForSeconds(1f);
				G.SetActive(bossLogo);
				bossLogo.Play(bigFishDeadPos.position, Vector3.zero, data.seatId, default(int));
				actionOnScoreStop = null;
				actionOnScoreStop = (Action)Delegate.Combine(actionOnScoreStop, (Action)delegate
				{
					Vector3 vector = gunPos;
					gunPos.y += ((data.seatId == 1 || data.seatId == 2) ? 2.3f : (-2.3f));
					Effect_CoinMgr.Get().SpawnBigFishCoins(deadInfo.fishRate, gunPos, gunPos, data.seatId);
					bosscrystal.DoFade();
					bossLogo.DoFade();
					gun.ResetOccupiedTargetPoses();
					UnityEngine.Debug.Log(HW2_LogHelper.Orange(string.Format("Boss死鱼通知后动画效果耗时：{0} 倍率{1}", stopwatch.Elapsed.TotalSeconds, deadInfo.fishRate)));
				});
				Effect_ScoreMgr.Get().DoPlayScore(deadInfo.addScore, data.fishType, deadInfo.bulletPower, data.seatId, new FollowData(bosscrystal.transform), true, true, true, showScoreBack, deadInfo.deadWay, actionOnScoreStop);
			}
			else
			{
				data.fish.Die();
			}
		}
		else if (data.fishType == FishType.GoldShark_霸王鲸)
		{
			bigScore = true;
			scoreFollow = true;
			GunBehaviour gunBehaviour2 = gun;
			List<Transform> list = new List<Transform>();
			list.Add(data.fish.transform);
			Transform bigFishDeadPos2 = gunBehaviour2.GetTargetdPos(list);
			Vector3 targetPos = bigFishDeadPos2.position;
			targetPos.y += ((data.seatId == 1 || data.seatId == 2) ? 0.4f : (-0.4f));
			ShortcutExtensions.DOMove(data.fish.transform, targetPos, 3f);
			actionOnScoreStop = null;
			actionOnScoreStop = (Action)Delegate.Combine(actionOnScoreStop, (Action)delegate
			{
				fishPos = data.fish.transform.position;
				data.fish.Die();
				Effect_CoinMgr.Get().SpawnBigFishCoins(deadInfo.fishRate, fishPos, gunPos, data.seatId);
				gun.ResetOccupiedTargetPoses();
			});
			Effect_LogoMgr.Get().SpawnGoldSharkLogo(gunPos, data.seatId);
			actionSimpleScoreEffect();
			manager.StartCoroutine(DoPlayBigFishDyingAnimation(data.fish));
			HW2_Singleton<SoundMgr>.Get().PlayClip("霸王鲸被捕获音效1");
			yield return new WaitForSeconds(1f);
			HW2_Singleton<SoundMgr>.Get().PlayClip("霸王鲸被捕获音效2");
		}
		else if (FishMgr.bigFishSet.Contains(data.fishType))
		{
			bigScore = true;
			scoreFollow = true;
			showScoreBack = true;
			GunBehaviour gunBehaviour3 = gun;
			List<Transform> list = new List<Transform>();
			list.Add(data.fish.transform);
			Transform targetdPos = gunBehaviour3.GetTargetdPos(list);
			ShortcutExtensions.DOMove(data.fish.transform, targetdPos.position, 1.5f);
			if (data.fishType != FishType.KillerWhale_杀人鲸)
			{
				HW2_Singleton<SoundMgr>.Get().PlayClip("三种巨型鱼、鲨鱼被捕获时的音效1");
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
				Effect_CoinMgr.Get().SpawnBigFishCoins(deadInfo.fishRate, fishPos, gunPos, data.seatId);
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
				yield return new WaitForSeconds(1f);
				data.fish.Die();
				HW2_Singleton<SoundMgr>.Get().PlayClip("获取金币音效");
			}
		}
	}

	private void PlayClip()
	{
		switch (UnityEngine.Random.Range(0, 3))
		{
		case 0:
			HW2_Singleton<SoundMgr>.Get().PlayClip("杀人鲸捕获语音1");
			break;
		case 1:
			HW2_Singleton<SoundMgr>.Get().PlayClip("杀人鲸捕获语音2");
			break;
		case 2:
			HW2_Singleton<SoundMgr>.Get().PlayClip("杀人鲸捕获语音3");
			break;
		default:
			UnityEngine.Debug.Log("error 杀人鲸捕获 Clip index");
			break;
		}
	}

	private void DoPlaySingleFishDyingAnimation(FishBehaviour fish)
	{
		fish.Dying(true);
	}

	private void DoPlayBossFishDyingAnimation(FishBehaviour fish)
	{
		if (fish.type == FishType.Boss_Lantern_暗夜炬兽)
		{
			manager.StartCoroutine(DoPlayBigFishDyingAnimation(fish));
		}
		else
		{
			fish.Dying(true);
		}
		if (fish.type == FishType.Boss_Dorgan_狂暴火龙 || fish.type == FishType.Boss_Kraken_深海八爪鱼)
		{
			ShortcutExtensions.DOShakePosition(fish.transform, 2f, new Vector3(0.5f, 0f, 0f), 1000, 20f);
		}
	}

	private IEnumerator DoPlayBigFishDyingAnimation(FishBehaviour fish)
	{
		fish.Dying(true);
		if (fish.type == FishType.Big_Clown_巨型小丑鱼 || fish.type == FishType.Big_Rasbora_巨型鲽鱼 || fish.type == FishType.Shark_鲨鱼 || fish.type == FishType.Boss_Lantern_暗夜炬兽)
		{
			while (fish.gameObject.activeInHierarchy)
			{
				yield return new WaitForSeconds(0.2f);
				Vector3 p = fish.transform.localEulerAngles;
				p.z += 30f;
				ShortcutExtensions.DOLocalRotate(fish.transform, p, 0.2f, RotateMode.FastBeyond360);
			}
		}
		else if (fish.type == FishType.Grouper_狮子鱼 || fish.type == FishType.Flounder_比目鱼)
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
