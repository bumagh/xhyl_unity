using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using FullInspector;
using HW3L;
using M__M.HaiWang.Fish;
using M__M.HaiWang.FishPathEditor.UseFullInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Tests.SpawnFishTest
{
	public class FK3_FishSpawnLabScript : BaseBehavior<FullSerializerSerializer>
	{
		[SerializeField]
		private FK3_FishSpawnConfig fishSpawnConfig;

		[SerializeField]
		private FK3_BGPathMgr pathMgr;

		[InspectorShowIf("isRunning")]
		[ShowInInspector]
		private FK3_PathFishPlayer pathFishPlayer = new FK3_PathFishPlayer();

		private bool isRunning => Application.isPlaying;

		protected override void Awake()
		{
			base.Awake();
		}

		private void Start()
		{
			CheckDependencies();
			Setup_PathFishPlayer();
		}

		private void CheckDependencies()
		{
		}

		private void Setup_PathFishPlayer()
		{
			pathFishPlayer.playAction = PathFishPlay_PlayAction;
		}

		private float GetSpeedByFishType(FK3_FishType fishType)
		{
			if (fishSpawnConfig == null)
			{
				return 2f;
			}
			return fishSpawnConfig.GetConstSpeed(fishType);
		}

		private FK3_BGPathMgr GetPathMgr()
		{
			if (pathMgr != null)
			{
				return pathMgr;
			}
			if (isRunning)
			{
				return FK3_BGPathMgr.Get();
			}
			return UnityEngine.Object.FindObjectOfType<FK3_BGPathMgr>();
		}

		[InspectorButton]
		private void Print_AllCurves_Length()
		{
			FK3_BGPathMgr fK3_BGPathMgr = GetPathMgr();
			List<BGCurve> curveList = fK3_BGPathMgr.GetCurveList();
			int count = 0;
			float totalLength = 0f;
			float minLength = 0f;
			float maxLength = 0f;
			curveList.ForEach(delegate(BGCurve _curve)
			{
				if (!(_curve == null))
				{
					int result = -1;
					int.TryParse(_curve.name, out result);
					if (result != -1)
					{
						BGCcMath bGCcMath = _curve.GetComponent<BGCcMath>();
						if (bGCcMath == null)
						{
							bGCcMath = _curve.gameObject.AddComponent<BGCcMath>();
						}
						float distance = bGCcMath.GetDistance();
						if (minLength == 0f || minLength > distance)
						{
							minLength = distance;
						}
						if (maxLength < distance)
						{
							maxLength = distance;
						}
						UnityEngine.Debug.Log($"path:{result}, length:{distance}");
						totalLength += distance;
						count++;
					}
				}
			});
			float num = totalLength / (float)Mathf.Max(1, count);
			UnityEngine.Debug.Log(FK3_LogHelper.Green("path count:{0},length [avg:{1},min:{2},max:{3}]", count, num, minLength, maxLength));
		}

		private List<FK3_PathLengthItem> GetLengthListByCurveList(List<BGCurve> curveList)
		{
			List<FK3_PathLengthItem> ret = new List<FK3_PathLengthItem>();
			curveList.ForEach(delegate(BGCurve _curve)
			{
				if (!(_curve == null))
				{
					int result = -1;
					int.TryParse(_curve.name, out result);
					if (result != -1)
					{
						BGCcMath bGCcMath = _curve.GetComponent<BGCcMath>();
						if (bGCcMath == null)
						{
							bGCcMath = _curve.gameObject.AddComponent<BGCcMath>();
						}
						float distance = bGCcMath.GetDistance();
						ret.Add(new FK3_PathLengthItem(result, distance));
					}
				}
			});
			return ret;
		}

		[InspectorButton]
		private void CheckPathIdCompletion()
		{
			FK3_BGPathMgr fK3_BGPathMgr = GetPathMgr();
			CheckPathIdCompletion(fK3_BGPathMgr.GetCurveMap());
		}

		private void CheckPathIdCompletion(Dictionary<string, BGCurve> map)
		{
			List<FK3_IdRange> list = new List<FK3_IdRange>();
			list.Add(new FK3_IdRange(1, 100, "迦魶鱼（匀速曲线路径）"));
			list.Add(new FK3_IdRange(101, 180, "小丑鱼（匀速曲线路径）"));
			list.Add(new FK3_IdRange(201, 250, "刺豚（匀速曲线路径）"));
			list.Add(new FK3_IdRange(301, 350, "旗鱼、锯齿鲨、蝠魟（匀速直线路径）"));
			list.Add(new FK3_IdRange(441, 590, "狮子鱼、比目鱼、龙虾、章鱼、灯笼鱼、海龟（匀速直线路径）"));
			list.Add(new FK3_IdRange(591, 670, "巨型小丑鱼（匀速直线路径）"));
			list.Add(new FK3_IdRange(671, 750, "巨型鲽鱼（匀速直线路径）"));
			list.Add(new FK3_IdRange(751, 830, "巨型刺豚（匀速直线路径）"));
			list.Add(new FK3_IdRange(831, 870, "鲨鱼（匀速直线路径）"));
			list.Add(new FK3_IdRange(871, 910, "杀人鲸（匀速直线路径）"));
			list.Add(new FK3_IdRange(911, 950, "霸王鲸（匀速直线路径）"));
			list.Add(new FK3_IdRange(951, 970, "暗夜巨兽（匀速直线路径）"));
			list.Add(new FK3_IdRange(971, 990, "道具蟹（变速直线路径）"));
			list.Add(new FK3_IdRange(361, 400, "迦魶鱼组（匀速直线路径）"));
			list.Add(new FK3_IdRange(999, 999, "霸王蟹（椭圆）"));
			List<FK3_IdRange> list2 = list;
			int count = 0;
			List<int> invalidList = new List<int>();
			list2.ForEach(delegate(FK3_IdRange range)
			{
				range.ForEach(delegate(int id)
				{
					bool flag = true;
					if (map.ContainsKey(id.ToString()))
					{
						BGCurve x = map[id.ToString()];
						if (!(x != null))
						{
							flag = false;
						}
					}
					else
					{
						flag = false;
					}
					if (!flag)
					{
						UnityEngine.Debug.Log(FK3_LogHelper.Red("{0} path[{1}] not correct implement", range.info, FK3_LogHelper.White(id.ToString())));
						invalidList.Add(id);
					}
					count++;
				});
			});
			UnityEngine.Debug.Log($"total check {count} paths, {invalidList.Count} invalid");
		}

		private void PathFishPlay_PlayAction(FK3_PathFishPlayerParam param)
		{
			PathFishPlay_PlayAction(param.fishType, param.pathId, param.player, param.fishId, param.isTeam, param.teamId);
		}

		private void PathFishPlay_PlayAction(FK3_FishType fishType, int pathId, FK3_PathFishPlayer player, int fishId = 0, bool isTeam = false, int teamId = 0)
		{
			int num = fishId;
			player.stopAction = null;
			if (isTeam)
			{
				FK3_FishTeam teamById = FK3_FishTeamMgr.Get().GetTeamById(teamId);
				if (teamById == null)
				{
					return;
				}
				int i = 0;
				for (int num2 = teamById.Count(); i < num2; i++)
				{
					FK3_FishMgr.Get().SetFishIndex(num++);
					Vector3 pointByIndex = teamById.GetPointByIndex(i);
					FK3_FishBehaviour fish = FK3_FishMgr.Get().SpawnSingleFish(fishType);
					if (!(fish == null))
					{
						if (!SetupFishMovement_BGCurve(fish, pathId, isTeam: true, pointByIndex))
						{
							return;
						}
						FK3_FishBehaviour fK3_FishBehaviour = fish;
						fK3_FishBehaviour.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour.Event_FishDie_Handler, (Action<FK3_FishBehaviour>)delegate
						{
							player.isPlaying = false;
						});
						FK3_PathFishPlayer fK3_PathFishPlayer = player;
						fK3_PathFishPlayer.stopAction = (Action)Delegate.Combine(fK3_PathFishPlayer.stopAction, (Action)delegate
						{
							if (fish != null && fish.IsLive())
							{
								fish.Die();
							}
						});
					}
				}
			}
			else
			{
				FK3_FishMgr.Get().SetFishIndex(num);
				FK3_FishBehaviour fish2 = FK3_FishMgr.Get().SpawnSingleFish(fishType);
				if (!SetupFishMovement_BGCurve(fish2, pathId, isTeam: false, Vector3.zero))
				{
					return;
				}
				FK3_FishBehaviour fK3_FishBehaviour2 = fish2;
				fK3_FishBehaviour2.Event_FishDie_Handler = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishBehaviour2.Event_FishDie_Handler, (Action<FK3_FishBehaviour>)delegate
				{
					player.isPlaying = false;
				});
				player.stopAction = delegate
				{
					if (fish2 != null && fish2.IsLive())
					{
						fish2.Die();
					}
				};
			}
			player.isPlaying = true;
		}

		private bool SetupFishMovement_BGCurve(FK3_FishBehaviour fish, int pathId, bool isTeam, Vector3 offset)
		{
			FK3_CurveUsage curveUsageById = FK3_BGPathMgr.Get().GetCurveUsageById(pathId);
			if (curveUsageById == null)
			{
				fish.Die();
				return false;
			}
			FK3_CursorUsage cursorUsage = curveUsageById.GetCursorUsage(fish.id);
			cursorUsage.translate.ObjectToManipulate = fish.transform;
			cursorUsage.rotate.ObjectToManipulate = fish.transform;
			cursorUsage.linear.Speed = GetSpeedByFishType(fish.originalType);
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
			return true;
		}

		[InspectorButton]
		[InspectorShowIf("isRunning")]
		private void Play_Test_1()
		{
			FK3_FishType[] array = new FK3_FishType[4]
			{
				FK3_FishType.Clown_小丑鱼,
				FK3_FishType.Gurnard_迦魶鱼,
				FK3_FishType.Grouper_狮子鱼,
				FK3_FishType.KillerWhale_杀人鲸
			};
			int[] array2 = new int[4];
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				FK3_PathFishPlayer fK3_PathFishPlayer = new FK3_PathFishPlayer();
				fK3_PathFishPlayer.playAction = PathFishPlay_PlayAction;
				fK3_PathFishPlayer.pathId = array2[i];
				fK3_PathFishPlayer.fishType = array[i];
				fK3_PathFishPlayer.fishId = i;
				fK3_PathFishPlayer.Play();
			}
		}

		[InspectorShowIf("isRunning")]
		[InspectorButton]
		private void Play_变速鱼_UsingFishQueen()
		{
			UnityEngine.Debug.LogError("本来是开放的");
			FK3_FishType[] array = new FK3_FishType[4]
			{
				FK3_FishType.CrabBoom_连环炸弹蟹,
				FK3_FishType.CrabLaser_电磁蟹,
				FK3_FishType.Tortoise_海龟,
				FK3_FishType.Octopus_章鱼
			};
			int[] array2 = new int[4];
			int num = array.Length;
			int i;
			for (i = 0; i < num; i++)
			{
				PlayPathFishByFishQueen(array[i], array2[i], i, delegate(FK3_FishBehaviour _fish)
				{
					_fish.lifetimeDic.Add("pathId", i);
					_fish.lifetimeDic.Add("pathLength", FK3_BGPathMgr.Get().GetCurveLength(i));
				});
			}
		}

		private void PlayPathFishByFishQueen(FK3_FishType fishType, int pathId, int fishId, Action<FK3_FishBehaviour> onSpawnFishAction = null)
		{
			FK3_FishQueenData fK3_FishQueenData = new FK3_FishQueenData();
			FK3_FP_Sequence fK3_FP_Sequence = new FK3_FP_Sequence();
			FK3_FG_SingleType<FK3_FishType> fK3_FG_SingleType = new FK3_FG_SingleType<FK3_FishType>();
			fK3_FG_SingleType.type = fishType;
			fK3_FG_SingleType.count = 1;
			fK3_FishQueenData.delay = 0f;
			fK3_FishQueenData.generator = fK3_FG_SingleType;
			fK3_FishQueenData.speed = FK3_FishQueenMgr.Get().GetSpeedByFishType(fK3_FG_SingleType.type.BasicType());
			fK3_FP_Sequence.pathId = pathId;
			FK3_FishQueen fishQueen = FK3_FishQueenMgr.Get().GetFishQueen();
			fishQueen.Setup(fK3_FishQueenData, fK3_FP_Sequence, 1, fishId);
			if (onSpawnFishAction != null)
			{
				FK3_FishQueen fK3_FishQueen = fishQueen;
				fK3_FishQueen.setupFishAction = (Action<FK3_FishBehaviour>)Delegate.Combine(fK3_FishQueen.setupFishAction, onSpawnFishAction);
			}
			fishQueen.Play();
		}
	}
}
