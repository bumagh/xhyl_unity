using DG.Tweening;
using FullInspector;
using M__M.HaiWang.FishPathEditor.UseFullInspector;
using SWS;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace M__M.HaiWang.Fish
{
	public class FK3_FishQueen : BaseBehavior<FullSerializerSerializer>
	{
		public bool playOnStart;

		[SerializeField]
		private FK3_FishQueenData m_fishData;

		[SerializeField]
		private FK3_FishPathBase m_pathData;

		private int m_count;

		private int m_startFishID;

		private float m_openTime;

		public Action<FK3_FishBehaviour> setupFishAction;

		public Action<FK3_FishQueen> onFinish;

		public static event Action<FK3_FishType, Action<Transform>> OnCreate;

		private void Start()
		{
			if (playOnStart)
			{
				Play();
			}
		}

		public FK3_FishQueen Setup(FK3_FishQueenData fishData, FK3_FishPathBase pathData, int count, int startFishID, int openTime = 0)
		{
			m_fishData = fishData;
			m_pathData = pathData;
			m_startFishID = startFishID;
			m_openTime = openTime;
			return this;
		}

		public void Play()
		{
			StartCoroutine(IE_Play());
		}

		private IEnumerator IE_Play()
		{
			yield return new WaitForSeconds(m_fishData.delay);
			bool isTeam = m_pathData.type == FK3_FishPathType.Team;
			FK3_FishTeam team = null;
			int fishId = m_startFishID;
			int pathId;
			if (isTeam)
			{
				FK3_FP_Team fK3_FP_Team = (FK3_FP_Team)m_pathData;
				pathId = fK3_FP_Team.pathId;
				int teamId = fK3_FP_Team.teamId;
				team = FK3_FishTeamMgr.Get().GetTeamById(teamId);
			}
			else
			{
				pathId = ((FK3_FP_Sequence)m_pathData).pathId;
			}
			while (m_fishData.generator.HasNext())
			{
				FK3_FishType fishType = m_fishData.generator.GetNext();
				FK3_FishMgr fishMgr = FK3_FishMgr.Get();
				int num;
				int fishIndex = num = fishId;
				fishId = num + 1;
				fishMgr.SetFishIndex(fishIndex);
				FK3_FishBehaviour fish = FK3_FishMgr.Get().SpawnSingleFish(fishType);
				if (fish != null)
				{
					if (fish.type == FK3_FishType.Puffer_河豚 || fishType == FK3_FishType.Big_Puffer_巨型河豚)
					{
						fish.OpenTime = m_openTime;
					}
					if (setupFishAction != null)
					{
						setupFishAction(fish);
					}
					if (HackFish(fishType, fish, pathId))
					{
						break;
					}
					Vector3 offset = Vector3.zero;
					if (isTeam)
					{
						offset = team.GetPointByIndex(m_count);
					}
					SetupFishMovement_BGCurve(fish, pathId, isTeam, offset);
					m_count++;
					if (!isTeam)
					{
						yield return new WaitForSeconds(m_fishData.interval);
					}
				}
			}
			if (onFinish != null)
			{
				onFinish(this);
			}
		}

		public Transform Create(FK3_FishType fishType)
		{
			FK3_FishBehaviour fK3_FishBehaviour = FK3_FishMgr.Get().SpawnSingleFish(fishType);
			return fK3_FishBehaviour.transform;
		}

		private void SetupFishMovement_SWS(FK3_FishBehaviour fish, int pathId, bool isTeam, Vector3 offset)
		{
			UnityEngine.Debug.LogError("设置路线: " + fish.name + " 路径: " + pathId + "  是否团队: " + isTeam);
			PathManager pathById = FK3_FishPathMgr.Get().GetPathById(pathId);
			if (pathById == null)
			{
				fish.Die();
				return;
			}
			splineMove move = fish.gameObject.GetComponent<splineMove>();
			if (move == null)
			{
				move = fish.gameObject.AddComponent<splineMove>();
			}
			move.enabled = true;
			move.pathMode = PathMode.TopDown2D;
			move.SetPath(pathById);
			if (isTeam)
			{
				offset = pathById.GetBeginRotation(Vector3.right) * offset;
			}
			move.offset = offset;
			move.offset.z = fish.depth;
			move.speed = m_fishData.speed;
			move.InitializeEvents();
			UnityEvent unityEvent = move.events[move.events.Count - 1];
			FK3_FishBehaviour fK3_FishBehaviour = fish;
			fK3_FishBehaviour.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour.Event_FishDie_Handler, (Action<FK3_FishBehaviour>)delegate
			{
				move.enabled = false;
				move.Stop();
				move.events[move.events.Count - 1].RemoveAllListeners();
			});
			unityEvent.AddListener(delegate
			{
				fish.Die();
			});
			FK3_FishBehaviour fK3_FishBehaviour2 = fish;
			fK3_FishBehaviour2.Event_FishMovePause_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour2.Event_FishMovePause_Handler, (Action<FK3_FishBehaviour>)delegate
			{
				move.Pause();
			});
			move.StartMove();
			if (fish.type == FK3_FishType.Tortoise_海龟 || fish.type == FK3_FishType.CrabLaser_电磁蟹 || fish.type == FK3_FishType.CrabBoom_连环炸弹蟹 || fish.type == FK3_FishType.CrabDrill_钻头蟹)
			{
				move.tween.DOTimeScale(0.3f, 3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
				unityEvent.AddListener(delegate
				{
					UnityEngine.Debug.Log(FK3_LogHelper.Cyan("变速鱼>fish[{0}],pathId:[{1}],time:{2}", fish.identity, pathId, fish.ageInSec));
				});
			}
		}

		private void SetupFishMovement_BGCurve(FK3_FishBehaviour fish, int pathId, bool isTeam, Vector3 offset)
		{
			try
			{
				FK3_CurveUsage curveUsageById = FK3_BGPathMgr.Get().GetCurveUsageById(pathId);
				if (curveUsageById == null)
				{
					UnityEngine.Debug.LogError("鱼该死了");
					fish.Die();
				}
				else
				{
					FK3_CursorUsage cursorUsage = curveUsageById.GetCursorUsage(fish.id);
					cursorUsage.translate.ObjectToManipulate = fish.transform;
					cursorUsage.rotate.ObjectToManipulate = fish.transform;
					cursorUsage.linear.Speed = m_fishData.speed;
					cursorUsage.translate.ForceUpdate();
					if (isTeam)
					{
						cursorUsage.translate.SetOffset(offset, adjustByRotation: true);
					}
					Action<FK3_FishBehaviour> b = delegate
					{
						cursorUsage.Free();
					};
					FK3_FishBehaviour fK3_FishBehaviour = fish;
					fK3_FishBehaviour.RemoveMovementAction = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour.RemoveMovementAction, b);
					FK3_FishBehaviour fK3_FishBehaviour2 = fish;
					fK3_FishBehaviour2.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour2.Event_FishDie_Handler, b);
					cursorUsage.ListenOnReachEnd(delegate
					{
						fish.Die();
					});
					if (fish.type == FK3_FishType.Flounder_比目鱼)
					{
						cursorUsage.linear.Speed = 2f;
						fish.animator.speed = 2f;
						FK3_FishBehaviour fK3_FishBehaviour3 = fish;
						fK3_FishBehaviour3.Event_FishOnAniEvent_Handler = (Action<FK3_FishBehaviour, string>)Delegate.Combine(fK3_FishBehaviour3.Event_FishOnAniEvent_Handler, (Action<FK3_FishBehaviour, string>)delegate(FK3_FishBehaviour _fish, string _name)
						{
							if (_fish.IsLive() && _fish.GetComponent<Collider>().enabled)
							{
								if (_name.Equals("Blink"))
								{
									fish.StartCoroutine(IE_BlinkSpeedControl(fish, cursorUsage, 2f, 0.5f, 0.3f, 2f));
								}
								else if (_name.Equals("BlinkOut"))
								{
									fish.StartCoroutine(IE_BlinkSpeedControl(fish, cursorUsage, 0.5f, 2f, 0.3f, 2f));
								}
							}
						});
					}
					if (fish.type == FK3_FishType.Big_Puffer_巨型河豚 || fish.type == FK3_FishType.Puffer_河豚)
					{
						FK3_FishBehaviour fK3_FishBehaviour4 = fish;
						fK3_FishBehaviour4.Event_ShowSize_Handler = (Action)Delegate.Combine(fK3_FishBehaviour4.Event_ShowSize_Handler, (Action)delegate
						{
							cursorUsage.linear.Speed = 0.4f;
						});
						FK3_FishBehaviour fK3_FishBehaviour5 = fish;
						fK3_FishBehaviour5.Event_FishOnAniEvent_Handler = (Action<FK3_FishBehaviour, string>)Delegate.Combine(fK3_FishBehaviour5.Event_FishOnAniEvent_Handler, (Action<FK3_FishBehaviour, string>)delegate(FK3_FishBehaviour _fish, string _name)
						{
							UnityEngine.Debug.LogError("河豚: " + base.name);
							if (_name.Equals("Size") && _fish.IsLive() && _fish.GetComponent<Collider>().enabled)
							{
								cursorUsage.linear.Speed = 1f;
							}
						});
					}
					if (fish.type == FK3_FishType.Tortoise_海龟 || fish.originalType == FK3_FishType.Octopus_章鱼)
					{
						FK3_FishBehaviour fK3_FishBehaviour6 = fish;
						fK3_FishBehaviour6.Event_FishOnAniEvent_Handler = (Action<FK3_FishBehaviour, string>)Delegate.Combine(fK3_FishBehaviour6.Event_FishOnAniEvent_Handler, (Action<FK3_FishBehaviour, string>)delegate(FK3_FishBehaviour _fish, string _name)
						{
							UnityEngine.Debug.LogError("Tortoise_海龟: " + base.name);
							if (_name.Equals("Flap") && _fish.IsLive())
							{
								FlapSpeedControl(_fish, cursorUsage);
							}
						});
					}
					else if (fish.type == FK3_FishType.CrabBoom_连环炸弹蟹 || fish.type == FK3_FishType.CrabLaser_电磁蟹 || fish.type == FK3_FishType.CrabDrill_钻头蟹)
					{
						ShiftSpeedControl(fish, cursorUsage);
					}
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("错误: " + arg);
			}
		}

		private void FlapSpeedControl(FK3_FishBehaviour fish, FK3_CursorUsage cursorUsage)
		{
			if (fish.originalType == FK3_FishType.Tortoise_海龟)
			{
				fish.StartCoroutine(IE_FlapSpeedControl(fish, cursorUsage, 3f, 1f, 2f));
			}
			else if (fish.originalType == FK3_FishType.Octopus_章鱼)
			{
				fish.StartCoroutine(IE_FlapSpeedControl(fish, cursorUsage, 3f, 1f, 1.3f));
			}
		}

		private IEnumerator IE_FlapSpeedControl(FK3_FishBehaviour fish, FK3_CursorUsage cursorUsage, float startSpeed, float targetSpeed, float duration)
		{
			cursorUsage.linear.Speed = 0f;
			float time = 0f;
			while (time <= duration)
			{
				if (!fish.IsLive())
				{
					yield break;
				}
				time += Time.deltaTime;
				cursorUsage.linear.Speed = Mathf.Lerp(startSpeed, targetSpeed, time / duration);
				yield return null;
			}
			MonoBehaviour.print("=============IL_CC被注销了============");
		}

		private IEnumerator IE_BlinkSpeedControl(FK3_FishBehaviour fish, FK3_CursorUsage cursorUsage, float startSpeed, float targetSpeed, float duration, float maxSpeed)
		{
			float time = 0f;
			while (time <= duration)
			{
				if (!fish.IsLive())
				{
					yield break;
				}
				time += Time.deltaTime;
				float speed = Mathf.Lerp(startSpeed, targetSpeed, time / duration);
				cursorUsage.linear.Speed = speed;
				fish.animator.speed = speed / maxSpeed;
				yield return null;
			}
			MonoBehaviour.print("===============IL_E0被注销了=============");
		}

		private void ShiftSpeedControl(FK3_FishBehaviour fish, FK3_CursorUsage cursorUsage)
		{
			fish.StartCoroutine(IE_ShiftSpeedControl(fish, cursorUsage, 4.5f, 0.5f, 3f));
		}

		private IEnumerator IE_ShiftSpeedControl(FK3_FishBehaviour fish, FK3_CursorUsage cursorUsage, float maxSpeed, float minSpeed, float period)
		{
			float timer2 = 0f;
			bool increase = true;
			float beginSpeed = minSpeed;
			float endSpeed = maxSpeed;
			while (fish.IsLive())
			{
				timer2 += Time.deltaTime;
				if (timer2 > period)
				{
					increase = !increase;
					beginSpeed = (increase ? minSpeed : maxSpeed);
					endSpeed = ((!increase) ? minSpeed : maxSpeed);
					timer2 = 0f;
				}
				timer2 += Time.deltaTime;
				float speed = Mathf.Lerp(beginSpeed, endSpeed, timer2 / period);
				try
				{
					fish.animator.speed = speed / maxSpeed;
				}
				catch (Exception)
				{
				}
				cursorUsage.linear.Speed = speed;
				yield return null;
			}
		}

		private bool HackFish(FK3_FishType fishType, FK3_FishBehaviour fish, int arg)
		{
			bool result = true;
			switch (fishType)
			{
			case FK3_FishType.Boss_Crab_霸王蟹:
				Hack_BossCrab(fish, 0);
				break;
			case FK3_FishType.Boss_Dorgan_狂暴火龙:
			case FK3_FishType.Boss_Dorgan_冰封暴龙:
				Hack_BossDragon(fish, arg);
				break;
			case FK3_FishType.烈焰龟:
				Hack_CaymanCrab(fish, arg);
				break;
			case FK3_FishType.Boss_Crocodil_史前巨鳄:
				Hack_CrocodilCrab(fish, arg);
				break;
			default:
				result = false;
				break;
			}
			return result;
		}

		private void Hack_BossCrab(FK3_FishBehaviour fish, int arg)
		{
			try
			{
				FK3_BossCrabMovmentController controller = fish.gameObject.GetComponent<FK3_BossCrabMovmentController>();
				controller.enabled = true;
				controller.Play();
				FK3_FishBehaviour fK3_FishBehaviour = fish;
				fK3_FishBehaviour.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour.Event_FishDie_Handler, (Action<FK3_FishBehaviour>)delegate
				{
					controller.enabled = false;
				});
				FK3_BossCrabMovmentController fK3_BossCrabMovmentController = controller;
				fK3_BossCrabMovmentController.onFinish = (Action)Delegate.Combine(fK3_BossCrabMovmentController.onFinish, (Action)delegate
				{
					UnityEngine.Debug.LogError(fish.name + " 死亡");
					fish.Die();
				});
			}
			catch (Exception arg2)
			{
				UnityEngine.Debug.LogError("错误: " + arg2);
			}
		}

		private void Hack_CaymanCrab(FK3_FishBehaviour fish, int arg)
		{
			try
			{
				FK3_CaymanMovController component = fish.gameObject.GetComponent<FK3_CaymanMovController>();
				if ((bool)component)
				{
					component.SetPhtch(arg);
					component.Play();
					FK3_FishBehaviour fK3_FishBehaviour = fish;
					fK3_FishBehaviour.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour.Event_FishDie_Handler, (Action<FK3_FishBehaviour>)delegate
					{
						UnityEngine.Debug.LogError("处理烈焰龟死亡后");
					});
					FK3_CaymanMovController fK3_CaymanMovController = component;
					fK3_CaymanMovController.onFinish = (Action)Delegate.Combine(fK3_CaymanMovController.onFinish, (Action)delegate
					{
						UnityEngine.Debug.LogError(fish.name + " 死亡");
						fish.Die();
					});
				}
			}
			catch (Exception arg2)
			{
				UnityEngine.Debug.LogError("错误: " + arg2);
			}
		}

		private void Hack_CrocodilCrab(FK3_FishBehaviour fish, int arg)
		{
			try
			{
				FK3_CrocodileBeheviour component = fish.gameObject.GetComponent<FK3_CrocodileBeheviour>();
				if ((bool)component)
				{
					component.SetPhtch(arg);
					component.Play();
					FK3_FishBehaviour fK3_FishBehaviour = fish;
					fK3_FishBehaviour.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour.Event_FishDie_Handler, (Action<FK3_FishBehaviour>)delegate
					{
						UnityEngine.Debug.LogError("处理史前巨鳄死亡后");
					});
					FK3_CrocodileBeheviour fK3_CrocodileBeheviour = component;
					fK3_CrocodileBeheviour.onFinish = (Action)Delegate.Combine(fK3_CrocodileBeheviour.onFinish, (Action)delegate
					{
						UnityEngine.Debug.LogError(fish.name + " 死亡");
						fish.Die();
					});
				}
			}
			catch (Exception arg2)
			{
				UnityEngine.Debug.LogError("错误: " + arg2);
			}
		}

		private void Hack_BossDragon(FK3_FishBehaviour fish, int arg)
		{
			switch (arg)
			{
			case 0:
				fish.transform.rotation = Quaternion.Euler(0f, 0f, -30f);
				break;
			case 1:
				fish.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
				break;
			case 2:
				fish.transform.rotation = Quaternion.Euler(0f, 0f, 30f);
				break;
			case 3:
				fish.transform.rotation = Quaternion.Euler(0f, 0f, 150f);
				break;
			case 4:
				fish.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
				break;
			case 5:
				fish.transform.rotation = Quaternion.Euler(0f, 0f, 210f);
				break;
			case 6:
				fish.transform.rotation = Quaternion.Euler(0f, 0f, -60f);
				break;
			case 7:
				fish.transform.rotation = Quaternion.Euler(0f, 0f, -120f);
				break;
			case 8:
				fish.transform.rotation = Quaternion.Euler(0f, 0f, 60f);
				break;
			case 9:
				fish.transform.rotation = Quaternion.Euler(0f, 0f, 120f);
				break;
			default:
				fish.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
				break;
			}
		}
	}
}
