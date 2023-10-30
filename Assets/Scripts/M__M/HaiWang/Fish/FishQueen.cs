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
	public class FishQueen : BaseBehavior<FullSerializerSerializer>
	{
		public bool playOnStart;

		[SerializeField]
		private FishQueenData m_fishData;

		[SerializeField]
		private FishPathBase m_pathData;

		private int m_count;

		private int m_startFishID;

		private float m_openTime;

		public Action<FishBehaviour> setupFishAction;

		public Action<FishQueen> onFinish;

		public static event Action<FishType, Action<Transform>> OnCreate;

		private void Start()
		{
			if (playOnStart)
			{
				Play();
			}
		}

		public FishQueen Setup(FishQueenData fishData, FishPathBase pathData, int count, int startFishID, int openTime = 0)
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
			bool isTeam = m_pathData.type == FishPathType.Team;
			FishTeam team = null;
			int fishId = m_startFishID;
			int pathId;
			if (isTeam)
			{
				FP_Team fP_Team = (FP_Team)m_pathData;
				pathId = fP_Team.pathId;
				int teamId = fP_Team.teamId;
				team = FishTeamMgr.Get().GetTeamById(teamId);
			}
			else
			{
				pathId = ((FP_Sequence)m_pathData).pathId;
			}
			while (m_fishData.generator.HasNext())
			{
				FishType fishType = m_fishData.generator.GetNext();
				FishMgr fishMgr = FishMgr.Get();
				int num;
				int fishIndex = num = fishId;
				fishId = num + 1;
				fishMgr.SetFishIndex(fishIndex);
				FishBehaviour fish = FishMgr.Get().SpawnSingleFish(fishType);
				if (fish != null)
				{
					if (fish.type == FishType.Puffer_河豚 || fishType == FishType.Big_Puffer_巨型河豚)
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
				else
				{
					UnityEngine.Debug.LogError($"fishType {fishType} 为空");
				}
			}
			if (onFinish != null)
			{
				onFinish(this);
			}
		}

		public Transform Create(FishType fishType)
		{
			FishBehaviour fishBehaviour = FishMgr.Get().SpawnSingleFish(fishType);
			return fishBehaviour.transform;
		}

		private void SetupFishMovement_SWS(FishBehaviour fish, int pathId, bool isTeam, Vector3 offset)
		{
			UnityEngine.Debug.LogError("设置路线: " + fish.name + " 路径: " + pathId + "  是否团队: " + isTeam);
			PathManager pathById = FishPathMgr.Get().GetPathById(pathId);
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
			FishBehaviour fishBehaviour = fish;
			fishBehaviour.Event_FishDie_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour.Event_FishDie_Handler, (Action<FishBehaviour>)delegate
			{
				move.enabled = false;
				move.Stop();
				move.events[move.events.Count - 1].RemoveAllListeners();
			});
			unityEvent.AddListener(delegate
			{
				fish.Die();
			});
			FishBehaviour fishBehaviour2 = fish;
			fishBehaviour2.Event_FishMovePause_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour2.Event_FishMovePause_Handler, (Action<FishBehaviour>)delegate
			{
				move.Pause();
			});
			move.StartMove();
			if (fish.type == FishType.Tortoise_海龟 || fish.type == FishType.CrabLaser_电磁蟹 || fish.type == FishType.CrabBoom_连环炸弹蟹)
			{
				move.tween.DOTimeScale(0.3f, 3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
				unityEvent.AddListener(delegate
				{
					UnityEngine.Debug.Log(HW2_LogHelper.Cyan("变速鱼>fish[{0}],pathId:[{1}],time:{2}", fish.identity, pathId, fish.ageInSec));
				});
			}
		}

		private void SetupFishMovement_BGCurve(FishBehaviour fish, int pathId, bool isTeam, Vector3 offset)
		{
			try
			{
				CurveUsage curveUsageById = BGPathMgr.Get().GetCurveUsageById(pathId);
				if (curveUsageById == null)
				{
					UnityEngine.Debug.LogError("鱼该死了");
					fish.Die();
				}
				else
				{
					CursorUsage cursorUsage = curveUsageById.GetCursorUsage(fish.id);
					cursorUsage.translate.ObjectToManipulate = fish.transform;
					cursorUsage.rotate.ObjectToManipulate = fish.transform;
					cursorUsage.linear.Speed = m_fishData.speed;
					cursorUsage.translate.ForceUpdate();
					if (isTeam)
					{
						cursorUsage.translate.SetOffset(offset, adjustByRotation: true);
					}
					Action<FishBehaviour> b = delegate
					{
						cursorUsage.Free();
					};
					FishBehaviour fishBehaviour = fish;
					fishBehaviour.RemoveMovementAction = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour.RemoveMovementAction, b);
					FishBehaviour fishBehaviour2 = fish;
					fishBehaviour2.Event_FishDie_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour2.Event_FishDie_Handler, b);
					cursorUsage.ListenOnReachEnd(delegate
					{
						fish.Die();
					});
					if (fish.type == FishType.Flounder_比目鱼)
					{
						cursorUsage.linear.Speed = 2f;
						fish.animator.speed = 2f;
						FishBehaviour fishBehaviour3 = fish;
						fishBehaviour3.Event_FishOnAniEvent_Handler = (Action<FishBehaviour, string>)Delegate.Combine(fishBehaviour3.Event_FishOnAniEvent_Handler, (Action<FishBehaviour, string>)delegate(FishBehaviour _fish, string _name)
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
					if (fish.type == FishType.Big_Puffer_巨型河豚 || fish.type == FishType.Puffer_河豚)
					{
						FishBehaviour fishBehaviour4 = fish;
						fishBehaviour4.Event_ShowSize_Handler = (Action)Delegate.Combine(fishBehaviour4.Event_ShowSize_Handler, (Action)delegate
						{
							cursorUsage.linear.Speed = 0.4f;
						});
						FishBehaviour fishBehaviour5 = fish;
						fishBehaviour5.Event_FishOnAniEvent_Handler = (Action<FishBehaviour, string>)Delegate.Combine(fishBehaviour5.Event_FishOnAniEvent_Handler, (Action<FishBehaviour, string>)delegate(FishBehaviour _fish, string _name)
						{
							if (_name.Equals("Size") && _fish.IsLive() && _fish.GetComponent<Collider>().enabled)
							{
								cursorUsage.linear.Speed = 1f;
							}
						});
					}
					if (fish.type == FishType.Tortoise_海龟 || fish.originalType == FishType.Octopus_章鱼)
					{
						FishBehaviour fishBehaviour6 = fish;
						fishBehaviour6.Event_FishOnAniEvent_Handler = (Action<FishBehaviour, string>)Delegate.Combine(fishBehaviour6.Event_FishOnAniEvent_Handler, (Action<FishBehaviour, string>)delegate(FishBehaviour _fish, string _name)
						{
							if (_name.Equals("Flap") && _fish.IsLive())
							{
								FlapSpeedControl(_fish, cursorUsage);
							}
						});
					}
					else if (fish.type == FishType.CrabBoom_连环炸弹蟹 || fish.type == FishType.CrabLaser_电磁蟹)
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

		private void FlapSpeedControl(FishBehaviour fish, CursorUsage cursorUsage)
		{
			if (fish.originalType == FishType.Tortoise_海龟)
			{
				fish.StartCoroutine(IE_FlapSpeedControl(fish, cursorUsage, 3f, 1f, 2f));
			}
			else if (fish.originalType == FishType.Octopus_章鱼)
			{
				fish.StartCoroutine(IE_FlapSpeedControl(fish, cursorUsage, 3f, 1f, 1.3f));
			}
		}

		private IEnumerator IE_FlapSpeedControl(FishBehaviour fish, CursorUsage cursorUsage, float startSpeed, float targetSpeed, float duration)
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

		private IEnumerator IE_BlinkSpeedControl(FishBehaviour fish, CursorUsage cursorUsage, float startSpeed, float targetSpeed, float duration, float maxSpeed)
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

		private void ShiftSpeedControl(FishBehaviour fish, CursorUsage cursorUsage)
		{
			fish.StartCoroutine(IE_ShiftSpeedControl(fish, cursorUsage, 2.5f, 0.5f, 3f));
		}

		private IEnumerator IE_ShiftSpeedControl(FishBehaviour fish, CursorUsage cursorUsage, float maxSpeed, float minSpeed, float period)
		{
			UnityEngine.Debug.Log($"[{fish.identity}] shiftSpeed");
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
				fish.animator.speed = speed / maxSpeed;
				cursorUsage.linear.Speed = speed;
				yield return null;
			}
		}

		private bool HackFish(FishType fishType, FishBehaviour fish, int arg)
		{
			bool result = true;
			switch (fishType)
			{
			case FishType.Boss_Crab_霸王蟹:
				Hack_BossCrab(fish, 0);
				break;
			case FishType.Boss_Dorgan_狂暴火龙:
				Hack_BossDragon(fish, arg);
				break;
			default:
				result = false;
				break;
			}
			return result;
		}

		private void Hack_BossCrab(FishBehaviour fish, int arg)
		{
			try
			{
				BossCrabMovmentController controller = fish.gameObject.GetComponent<BossCrabMovmentController>();
				controller.Play();
				FishBehaviour fishBehaviour = fish;
				fishBehaviour.Event_FishDie_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour.Event_FishDie_Handler, (Action<FishBehaviour>)delegate
				{
					UnityEngine.Object.Destroy(controller);
				});
				BossCrabMovmentController bossCrabMovmentController = controller;
				bossCrabMovmentController.onFinish = (Action)Delegate.Combine(bossCrabMovmentController.onFinish, (Action)delegate
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

		private void Hack_BossDragon(FishBehaviour fish, int arg)
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
