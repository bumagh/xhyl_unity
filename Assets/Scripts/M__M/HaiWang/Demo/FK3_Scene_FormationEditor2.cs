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
	public class FK3_Scene_FormationEditor2 : MonoBehaviour
	{
		private bool m_useNet;

		private void Awake()
		{
			FK3_FishBehaviour.SEvent_FishOnStart_Handler = delegate
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
				FK3_FishMgr.Get().SetFishIndex(20000);
				StartCoroutine(Demo_FishFormation());
			}
			Test();
		}

		private void Test()
		{
		}

		private void Handle_FishHitByBullet(FK3_FishBehaviour fish, Collider bullet)
		{
			FK3_BulletController component = bullet.GetComponent<FK3_BulletController>();
			if (component == null || component.used)
			{
				return;
			}
			component.used = true;
			if (fish.State == FK3_FishState.Live)
			{
				FK3_Effect_ExplodeMgr.Get().SpawnExplode(bullet.transform.position, component);
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
					FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/gunHitFish", args);
				}
				else
				{
					FishDead(fish.id, component.seatId);
				}
			}
		}

		private void Handle_FishOnDoubleClick(FK3_FishBehaviour fish)
		{
			UnityEngine.Debug.Log("Handle_FishOnDoubleClick " + fish.identity);
			FK3_GunController nativeGun = FK3_PlayerMgr.Get().GetNativeGun();
			UnityEngine.Debug.Log($"fishState:[{fish.State}], gunType:[{nativeGun.gunFSM.CurrentState}]");
			if (fish.State == FK3_FishState.Live && nativeGun.gunFSM.IsInState<FK3_LockingGun>() && nativeGun.lockingFishId != fish.id)
			{
				FK3_LockChainController chain = FK3_EffectMgr.Get().GetLockChain(nativeGun.seatId);
				chain.SetTarget(nativeGun.transform, fish.transform, fish.GetSpriteOrder());
				nativeGun.lockingFishId = fish.id;
				chain.Show();
				Action<FK3_FishBehaviour> action = delegate
				{
					if (nativeGun.lockingFishId == fish.id)
					{
						nativeGun.lockingFishId = 0;
						chain.Hide();
					}
				};
				FK3_FishBehaviour fK3_FishBehaviour = fish;
				fK3_FishBehaviour.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Remove(fK3_FishBehaviour.Event_FishDie_Handler, action);
				FK3_FishBehaviour fK3_FishBehaviour2 = fish;
				fK3_FishBehaviour2.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour2.Event_FishDie_Handler, action);
			}
		}

		private void FishDead(int fishId, int killerSeatId)
		{
			UnityEngine.Debug.Log($"FishDead fishId:[{fishId}], killerId:[{killerSeatId}]");
			FK3_FishBehaviour fishById = FK3_FishMgr.Get().GetFishById(fishId);
			fishById.Dying();
			FK3_AgentData<FK3_FishType> fK3_AgentData = fishById.GetComponent<FK3_NavPathAgent>().userData as FK3_AgentData<FK3_FishType>;
			fK3_AgentData.formation.RemoveObject(fK3_AgentData.agent);
			FK3_Effect_ScoreMgr.Get().SpawnScore_old(100, 1, FK3_FishMgr.Get().GetFishById(fishId).transform.position, BigScore: false, delegate
			{
				Vector3 position = FK3_FishMgr.Get().GetFishById(fishId).transform.position;
				Vector3 position2 = FK3_PlayerMgr.Get().GetPlayer(killerSeatId).transform.position;
				FK3_Effect_CoinMgr.Get().SpawnCoin(position, position2);
				FK3_FishMgr.Get().GetFishById(fishId).Die();
			});
		}

		private void Init()
		{
			if (FK3_MB_Singleton<FK3_NetManager>.Get() != null)
			{
				FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("login", HandleNetMsg_Login);
				FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("newFish", HandleNetMsg_NewFish);
				FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("userFired", HandleNetMsg_UserFired);
				FK3_MB_Singleton<FK3_NetManager>.Get().RegisterHandler("newGameScore", HandleNetMsg_NewGameScore);
			}
			if (FK3_PlayerMgr.Get() != null && FK3_PlayerMgr.Get().GetNativeUI() != null)
			{
				FK3_PlayerMgr.Get().SetNativePlayer(1);
				FK3_PlayerMgr.Get().GetNativeUI().SetNative(isHost: true);
				FK3_PlayerMgr.Get().GetNativeUI().UIEvent_PlayerChangeGunMode += PlayerLogic_Handle_PlayerChangeGunPower;
				FK3_GunController nativeGun = FK3_PlayerMgr.Get().GetNativeGun();
				nativeGun.Event_Shoot_Handler = (FK3_GunController.EventHandler_Shoot)Delegate.Combine(nativeGun.Event_Shoot_Handler, new FK3_GunController.EventHandler_Shoot(GunLogic_Handle_GunShoot));
			}
		}

		private IEnumerator CoLogin()
		{
			string ip = "lhl.swccd88.xyz";
			int port = 10100;
			UnityEngine.Debug.LogError($"==========IE_Flow: {ip}:{port}");
			FK3_MB_Singleton<FK3_NetManager>.Get().Connect(ip, port);
			yield return new WaitForSeconds(1f);
			FK3_MB_Singleton<FK3_NetManager>.Get().SendPublicKey();
			yield return new WaitForSeconds(1f);
			Send_Login();
		}

		private void Send_Login()
		{
			UnityEngine.Debug.Log("Send_Login");
			string text = "robot-1";
			string text2 = "123456";
			FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/login", new object[2]
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
				FK3_FishMgr.Get().SetFishIndex(fishIndex);
				FK3_FishType fishType = (FK3_FishType)num;
				FK3_FishCreationInfo info = new FK3_FishCreationInfo(fishType);
				FK3_FishBehaviour fK3_FishBehaviour = FK3_FishMgr.Get().GenNewFish(info);
				fK3_FishBehaviour.SetPosition(new Vector3(-3f, 0f));
				fK3_FishBehaviour.Prepare();
				FishBindEventHandler(fK3_FishBehaviour);
				SetupFishMovement(fK3_FishBehaviour, pathId);
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
			int fishId = FK3_FishMgr.Get().IncreaseFishIndex();
			FK3_FishCreationInfo info3 = new FK3_FishCreationInfo(FK3_FishType.Big_Clown_巨型小丑鱼);
			FK3_FishBehaviour fish3 = FK3_FishMgr.Get().GenNewFish(info3);
			fish3.SetPosition(new Vector3(-3f, 0f));
			fish3.Prepare();
			yield return new WaitForSeconds(1f);
			int fishId2 = FK3_FishMgr.Get().IncreaseFishIndex();
			FK3_FishCreationInfo info2 = new FK3_FishCreationInfo(FK3_FishType.Puffer_河豚);
			FK3_FishBehaviour fish2 = FK3_FishMgr.Get().GenNewFish(info2);
			fish2.SetPosition(new Vector3(3f, 0f));
			fish2.Prepare();
			yield return new WaitForSeconds(1f);
			FK3_FishMgr.Get().GetFishById(fishId).Dying();
			FK3_Effect_ScoreMgr.Get().SpawnScore_old(100, 1, FK3_FishMgr.Get().GetFishById(fishId).transform.position, BigScore: false, delegate
			{
				Vector3 position = FK3_FishMgr.Get().GetFishById(fishId).transform.position;
				Vector3 position2 = FK3_PlayerMgr.Get().GetPlayer(1).transform.position;
				FK3_Effect_CoinMgr.Get().SpawnCoin(position, position2);
			});
			yield return new WaitForSeconds(0.1f);
			FK3_FishMgr.Get().GetFishById(fishId2).Dying();
			FK3_Effect_ScoreMgr.Get().SpawnScore_old(20, 1, FK3_FishMgr.Get().GetFishById(fishId2).transform.position, BigScore: false, delegate
			{
			});
			yield return new WaitForSeconds(0.1f);
			yield return new WaitForSeconds(1f);
			FK3_FishMgr.Get().GetFishById(fishId).Die();
			yield return new WaitForSeconds(0.1f);
			FK3_FishMgr.Get().GetFishById(fishId2).Die();
			yield return new WaitForSeconds(0.1f);
		}

		private IEnumerator Demo_GunUse_1()
		{
			yield return new WaitForSeconds(0.1f);
			FK3_FishMgr.Get().IncreaseFishIndex();
			FK3_FishCreationInfo info3 = new FK3_FishCreationInfo(FK3_FishType.Big_Clown_巨型小丑鱼);
			FK3_FishBehaviour fish3 = FK3_FishMgr.Get().GenNewFish(info3);
			fish3.SetPosition(new Vector3(-3f, 0f));
			FishBindEventHandler(fish3);
			fish3.Prepare();
			SetupFishMovement(fish3, 1);
			FK3_FishMgr.Get().IncreaseFishIndex();
			FK3_FishCreationInfo info2 = new FK3_FishCreationInfo(FK3_FishType.Puffer_河豚);
			FK3_FishBehaviour fish2 = FK3_FishMgr.Get().GenNewFish(info2);
			fish2.SetPosition(new Vector3(3f, 0f));
			FishBindEventHandler(fish2);
			fish2.Prepare();
			SetupFishMovement(fish2, 8);
			yield return new WaitForSeconds(2f);
			StartCoroutine(Demo_GunUse_1());
		}

		private void SetupFishMovement(FK3_FishBehaviour fish, int pathId)
		{
			PathManager path = FK3_PathMgr_Mock.Get().pathDic[pathId.ToString()];
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
			FK3_FishBehaviour fK3_FishBehaviour = fish;
			fK3_FishBehaviour.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour.Event_FishDie_Handler, (Action<FK3_FishBehaviour>)delegate
			{
				move.events[move.events.Count - 1].RemoveAllListeners();
			});
			unityEvent.AddListener(delegate
			{
				fish.Die();
				move.enabled = false;
				move.events[move.events.Count - 1].RemoveAllListeners();
			});
			FK3_FishBehaviour fK3_FishBehaviour2 = fish;
			fK3_FishBehaviour2.Event_FishMovePause_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour2.Event_FishMovePause_Handler, (Action<FK3_FishBehaviour>)delegate
			{
				move.Pause();
			});
		}

		private IEnumerator Demo_FishFormation()
		{
			if (FK3_FishFormationMgr.Get() == null && FK3_FishFormationFactory.Get() == null)
			{
				UnityEngine.Debug.Log(FK3_LogHelper.Red("FishFormation not ready"));
				yield break;
			}
			FK3_FishFormationFactory.Get().createFun = delegate(FK3_FishType _fishType, int id)
			{
				int num = FK3_FishMgr.Get().IncreaseFishIndex();
				FK3_FishCreationInfo info = new FK3_FishCreationInfo(_fishType);
				FK3_FishBehaviour fK3_FishBehaviour = FK3_FishMgr.Get().GenNewFish(info);
				fK3_FishBehaviour.SetPosition(new Vector3(-3f, 0f));
				FishBindEventHandler(fK3_FishBehaviour);
				fK3_FishBehaviour.Prepare();
				FK3_NavPathAgent fK3_NavPathAgent = fK3_FishBehaviour.gameObject.GetComponent<FK3_NavPathAgent>();
				if (fK3_NavPathAgent == null)
				{
					fK3_NavPathAgent = fK3_FishBehaviour.gameObject.AddComponent<FK3_NavPathAgent>();
				}
				else
				{
					fK3_NavPathAgent.enabled = true;
				}
				return fK3_NavPathAgent;
			};
			FK3_FishFormationFactory.Get().destoryAction = delegate(FK3_NavPathAgent _navPathAgent, FK3_FishType _fishType)
			{
				FK3_FishBehaviour component = _navPathAgent.GetComponent<FK3_FishBehaviour>();
				if (component.State == FK3_FishState.Live)
				{
					component.Die();
					_navPathAgent.enabled = false;
				}
			};
		}

		private void FishBindEventHandler(FK3_FishBehaviour fish)
		{
			fish.Event_FishOnDoubleClick_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fish.Event_FishOnDoubleClick_Handler, new Action<FK3_FishBehaviour>(Handle_FishOnDoubleClick));
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

		private void GunLogic_Handle_GunShoot(FK3_GunController gun, Vector3 pos, bool faceDown, float angle, int gunValue, int seatId)
		{
			UnityEngine.Debug.Log(FK3_LogHelper.Aqua("GunLogic_Handle_GunShoot"));
			if (m_useNet)
			{
				int num = 0;
				int num2 = 0;
				string text = $"{num},{gunValue},{angle},{num2}";
				object[] args = new object[1]
				{
					text
				};
				FK3_MB_Singleton<FK3_NetManager>.Get().Send("userService/userFired", args);
			}
		}

		private void PlayerLogic_Handle_PlayerChangeGunPower(FK3_PlayerUI playerUI)
		{
			FK3_PlayerController player = playerUI.GetPlayer();
			player.GetGun().UserSwitchGun();
		}
	}
}
