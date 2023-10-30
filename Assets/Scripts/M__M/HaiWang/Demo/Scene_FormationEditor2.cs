using DG.Tweening;
using M__M.GameHall.Common;
using M__M.GameHall.Net;
using M__M.HaiWang.Bullet;
using M__M.HaiWang.Effect;
using M__M.HaiWang.Fish;
using M__M.HaiWang.Player;
using M__M.HaiWang.Player.Gun;
using M__M.HaiWang.Player.GunState;
using PathSystem;
using SWS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace M__M.HaiWang.Demo
{
	public class Scene_FormationEditor2 : MonoBehaviour
	{
		private bool m_useNet;

		private void Awake()
		{
			FishBehaviour.SEvent_FishOnStart_Handler = delegate
			{
			};
		}

		private void Start()
		{
			Init();
			if (m_useNet)
			{
				StartCoroutine(CoLogin());
			}
			else
			{
				FishMgr.Get().SetFishIndex(20000);
				StartCoroutine(Demo_FishFormation());
			}
			Test();
		}

		private void Test()
		{
		}

		private void Update()
		{
		}

		private void Handle_FishHitByBullet(FishBehaviour fish, Collider bullet)
		{
			BulletController component = bullet.GetComponent<BulletController>();
			if (component == null || component.used)
			{
				return;
			}
			component.used = true;
			if (fish.State == FishState.Live)
			{
				Effect_ExplodeMgr.Get().SpawnExplode(bullet.transform.position);
				component.Over();
				if (m_useNet)
				{
					int num = 0;
					string text = $"{fish.id}#{fish.type}";
					object[] args = new object[2]
					{
						num,
						text
					};
					HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/gunHitFish", args);
				}
				else
				{
					FishDead(fish.id, component.seatId);
				}
			}
		}

		private void Handle_FishOnDoubleClick(FishBehaviour fish)
		{
			UnityEngine.Debug.Log("Handle_FishOnDoubleClick " + fish.identity);
			GunController nativeGun = PlayerMgr.Get().GetNativeGun();
			UnityEngine.Debug.Log($"fishState:[{fish.State}], gunType:[{nativeGun.gunFSM.CurrentState}]");
			if (fish.State == FishState.Live && nativeGun.gunFSM.IsInState<LockingGun>() && nativeGun.lockingFishId != fish.id)
			{
				LockChainController chain = EffectMgr.Get().GetLockChain(nativeGun.seatId);
				chain.SetTarget(nativeGun.transform, fish.transform, fish.GetSpriteOrder());
				nativeGun.lockingFishId = fish.id;
				chain.Show();
				Action<FishBehaviour> action = delegate
				{
					if (nativeGun.lockingFishId == fish.id)
					{
						nativeGun.lockingFishId = 0;
						chain.Hide();
					}
				};
				FishBehaviour fishBehaviour = fish;
				fishBehaviour.Event_FishDie_Handler = (Action<FishBehaviour>)Delegate.Remove(fishBehaviour.Event_FishDie_Handler, action);
				FishBehaviour fishBehaviour2 = fish;
				fishBehaviour2.Event_FishDie_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour2.Event_FishDie_Handler, action);
			}
		}

		private void FishDead(int fishId, int killerSeatId)
		{
			UnityEngine.Debug.Log($"FishDead fishId:[{fishId}], killerId:[{killerSeatId}]");
			FishBehaviour fishById = FishMgr.Get().GetFishById(fishId);
			fishById.Dying();
			AgentData<FishType> agentData = fishById.GetComponent<NavPathAgent>().userData as AgentData<FishType>;
			agentData.formation.RemoveObject(agentData.agent);
			Effect_ScoreMgr.Get().SpawnScore_old(100, 1, FishMgr.Get().GetFishById(fishId).transform.position, BigScore: false, delegate
			{
				Vector3 position = FishMgr.Get().GetFishById(fishId).transform.position;
				Vector3 position2 = PlayerMgr.Get().GetPlayer(killerSeatId).transform.position;
				Effect_CoinMgr.Get().SpawnCoin(position, position2);
				FishMgr.Get().GetFishById(fishId).Die();
			});
		}

		private void Init()
		{
			if (HW2_MB_Singleton<HW2_NetManager>.Get() != null)
			{
				HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("login", HandleNetMsg_Login);
				HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("newFish", HandleNetMsg_NewFish);
				HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("userFired", HandleNetMsg_UserFired);
				HW2_MB_Singleton<HW2_NetManager>.Get().RegisterHandler("newGameScore", HandleNetMsg_NewGameScore);
			}
			if (PlayerMgr.Get() != null && PlayerMgr.Get().GetNativeUI() != null)
			{
				PlayerMgr.Get().SetNativePlayer(1);
				PlayerMgr.Get().GetNativeUI().SetNative(isHost: true);
				PlayerMgr.Get().GetNativeUI().UIEvent_PlayerChangeGunMode += PlayerLogic_Handle_PlayerChangeGunPower;
				GunController nativeGun = PlayerMgr.Get().GetNativeGun();
				nativeGun.Event_Shoot_Handler = (GunController.EventHandler_Shoot)Delegate.Combine(nativeGun.Event_Shoot_Handler, new GunController.EventHandler_Shoot(GunLogic_Handle_GunShoot));
			}
		}

		private IEnumerator CoLogin()
		{
			string ip = "lhl.swccd88.xyz";
			int port = 10100;
			HW2_MB_Singleton<HW2_NetManager>.Get().Connect(ip, port);
			yield return new WaitForSeconds(1f);
			HW2_MB_Singleton<HW2_NetManager>.Get().SendPublicKey();
			yield return new WaitForSeconds(1f);
			Send_Login();
		}

		private void Send_Login()
		{
			UnityEngine.Debug.Log("Send_Login");
			string text = "robot-1";
			string text2 = "123456";
			HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/login", new object[2]
			{
				text,
				text2
			}, check: false);
		}

		private void HandleNetMsg_Login(object[] args)
		{
		}

		private void HandleNetMsg_CeaseFire(object[] args)
		{
		}

		private void HandleNetMsg_AllowFire(object[] args)
		{
		}

		private void HandleNetMsg_NewFish(object[] args)
		{
			UnityEngine.Debug.Log("HandleNetMsg_NewFish");
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			object[] array = dictionary["fishes"] as object[];
			for (int i = 0; i < array.Length; i++)
			{
				Dictionary<string, object> dictionary2 = array[0] as Dictionary<string, object>;
				int num = (int)dictionary2["fishType"];
				int pathId = (int)dictionary2["movePath"];
				int fishIndex = (int)dictionary2["fishStartId"];
				num++;
				num = ((num < 3) ? 2 : 4);
				FishMgr.Get().SetFishIndex(fishIndex);
				FishType fishType = (FishType)num;
				FishCreationInfo info = new FishCreationInfo(fishType);
				FishBehaviour fishBehaviour = FishMgr.Get().GenNewFish(info);
				fishBehaviour.SetPosition(new Vector3(-3f, 0f));
				fishBehaviour.Prepare();
				FishBindEventHandler(fishBehaviour);
				SetupFishMovement(fishBehaviour, pathId);
			}
		}

		private void HandleNetMsg_NewFormation(object[] args)
		{
		}

		private void HandleNetMsg_UserFired(object[] args)
		{
		}

		private void HandleNetMsg_HitFish(object[] args)
		{
		}

		private void HandleNetMsg_NewGameScore(object[] args)
		{
		}

		private void HandleNetMsg_LockFish(object[] args)
		{
		}

		private void HandleNetMsg_UnlockFish(object[] args)
		{
		}

		private IEnumerator Demo_FishLifeCycle_1()
		{
			yield return new WaitForSeconds(0.1f);
			int fishId = FishMgr.Get().IncreaseFishIndex();
			FishCreationInfo info3 = new FishCreationInfo(FishType.Big_Clown_巨型小丑鱼);
			FishBehaviour fish3 = FishMgr.Get().GenNewFish(info3);
			fish3.SetPosition(new Vector3(-3f, 0f));
			fish3.Prepare();
			yield return new WaitForSeconds(1f);
			int fishId2 = FishMgr.Get().IncreaseFishIndex();
			FishCreationInfo info2 = new FishCreationInfo(FishType.Puffer_河豚);
			FishBehaviour fish2 = FishMgr.Get().GenNewFish(info2);
			fish2.SetPosition(new Vector3(3f, 0f));
			fish2.Prepare();
			yield return new WaitForSeconds(1f);
			FishMgr.Get().GetFishById(fishId).Dying();
			Effect_ScoreMgr.Get().SpawnScore_old(100, 1, FishMgr.Get().GetFishById(fishId).transform.position, BigScore: false, delegate
			{
				Vector3 position = FishMgr.Get().GetFishById(fishId).transform.position;
				Vector3 position2 = PlayerMgr.Get().GetPlayer(1).transform.position;
				Effect_CoinMgr.Get().SpawnCoin(position, position2);
			});
			yield return new WaitForSeconds(0.1f);
			FishMgr.Get().GetFishById(fishId2).Dying();
			Effect_ScoreMgr.Get().SpawnScore_old(20, 1, FishMgr.Get().GetFishById(fishId2).transform.position, BigScore: false, delegate
			{
			});
			yield return new WaitForSeconds(0.1f);
			yield return new WaitForSeconds(1f);
			FishMgr.Get().GetFishById(fishId).Die();
			yield return new WaitForSeconds(0.1f);
			FishMgr.Get().GetFishById(fishId2).Die();
			yield return new WaitForSeconds(0.1f);
		}

		private IEnumerator Demo_GunUse_1()
		{
			yield return new WaitForSeconds(0.1f);
			FishMgr.Get().IncreaseFishIndex();
			FishCreationInfo info3 = new FishCreationInfo(FishType.Big_Clown_巨型小丑鱼);
			FishBehaviour fish3 = FishMgr.Get().GenNewFish(info3);
			fish3.SetPosition(new Vector3(-3f, 0f));
			FishBindEventHandler(fish3);
			fish3.Prepare();
			SetupFishMovement(fish3, 1);
			FishMgr.Get().IncreaseFishIndex();
			FishCreationInfo info2 = new FishCreationInfo(FishType.Puffer_河豚);
			FishBehaviour fish2 = FishMgr.Get().GenNewFish(info2);
			fish2.SetPosition(new Vector3(3f, 0f));
			FishBindEventHandler(fish2);
			fish2.Prepare();
			SetupFishMovement(fish2, 8);
			yield return new WaitForSeconds(2f);
			StartCoroutine(Demo_GunUse_1());
		}

		private void SetupFishMovement(FishBehaviour fish, int pathId)
		{
			PathManager path = PathMgr_Mock.Get().pathDic[pathId.ToString()];
			splineMove move = fish.gameObject.GetComponent<splineMove>();
			if (move == null)
			{
				move = fish.gameObject.AddComponent<splineMove>();
			}
			move.enabled = true;
			move.pathMode = PathMode.TopDown2D;
			move.SetPath(path);
			move.offset = Vector3.zero;
			move.offset.z = fish.depth;
			move.speed = 1f;
			move.StartMove();
			UnityEvent unityEvent = move.events[move.events.Count - 1];
			FishBehaviour fishBehaviour = fish;
			fishBehaviour.Event_FishDie_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour.Event_FishDie_Handler, (Action<FishBehaviour>)delegate
			{
				move.events[move.events.Count - 1].RemoveAllListeners();
			});
			unityEvent.AddListener(delegate
			{
				fish.Die();
				move.enabled = false;
				move.events[move.events.Count - 1].RemoveAllListeners();
			});
			FishBehaviour fishBehaviour2 = fish;
			fishBehaviour2.Event_FishMovePause_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour2.Event_FishMovePause_Handler, (Action<FishBehaviour>)delegate
			{
				move.Pause();
			});
		}

		private IEnumerator Demo_FishFormation()
		{
			if (FishFormationMgr.Get() == null && FishFormationFactory.Get() == null)
			{
				UnityEngine.Debug.Log(HW2_LogHelper.Red("FishFormation not ready"));
				yield break;
			}
			FishFormationFactory.Get().createFun = delegate(FishType _fishType, int id)
			{
				int num = FishMgr.Get().IncreaseFishIndex();
				FishCreationInfo info = new FishCreationInfo(_fishType);
				FishBehaviour fishBehaviour = FishMgr.Get().GenNewFish(info);
				fishBehaviour.SetPosition(new Vector3(-3f, 0f));
				FishBindEventHandler(fishBehaviour);
				fishBehaviour.Prepare();
				NavPathAgent navPathAgent = fishBehaviour.gameObject.GetComponent<NavPathAgent>();
				if (navPathAgent == null)
				{
					navPathAgent = fishBehaviour.gameObject.AddComponent<NavPathAgent>();
				}
				else
				{
					navPathAgent.enabled = true;
				}
				return navPathAgent;
			};
			FishFormationFactory.Get().destoryAction = delegate(NavPathAgent _navPathAgent, FishType _fishType)
			{
				FishBehaviour component = _navPathAgent.GetComponent<FishBehaviour>();
				if (component.State == FishState.Live)
				{
					component.Die();
					_navPathAgent.enabled = false;
				}
			};
		}

		private void FishBindEventHandler(FishBehaviour fish)
		{
			fish.Event_FishOnDoubleClick_Handler = (Action<FishBehaviour>)Delegate.Combine(fish.Event_FishOnDoubleClick_Handler, new Action<FishBehaviour>(Handle_FishOnDoubleClick));
			fish.Event_FishOnHit_Handler += Handle_FishHitByBullet;
		}

		private IEnumerator Demo_GunShoot_1()
		{
			yield break;
		}

		private Vector3 _RandomBirthPoint()
		{
			return new Vector3(UnityEngine.Random.Range(-5f, 5f), 0f, 0f);
		}

		private void GunLogic_Handle_GunShoot(GunController gun, Vector3 pos, bool faceDown, float angle, int gunValue, int seatId)
		{
			UnityEngine.Debug.Log(HW2_LogHelper.Aqua("GunLogic_Handle_GunShoot"));
			if (m_useNet)
			{
				int num = 0;
				int num2 = 0;
				string text = $"{num},{gunValue},{angle},{num2}";
				object[] args = new object[1]
				{
					text
				};
				HW2_MB_Singleton<HW2_NetManager>.Get().Send("userService/userFired", args);
			}
		}

		private void PlayerLogic_Handle_PlayerChangeGunPower(PlayerUI playerUI)
		{
			PlayerController player = playerUI.GetPlayer();
			player.GetGun().UserSwitchGun();
		}
	}
}
