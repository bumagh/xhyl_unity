using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using FullInspector;
using M__M.HaiWang.Fish;
using M__M.HaiWang.FishPathEditor.UseFullInspector;
using M__M.HaiWang.FishSpawn;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Tests.SpawnFishTest
{
	public class FishSpawnLabScript : BaseBehavior<FullSerializerSerializer>
	{
		[SerializeField]
		private FishSpawnConfig fishSpawnConfig;

		[SerializeField]
		private BGPathMgr pathMgr;

		[InspectorShowIf("isRunning")]
		[ShowInInspector]
		private PathFishPlayer pathFishPlayer = new PathFishPlayer();

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

		private float GetSpeedByFishType(FishType fishType)
		{
			if (fishSpawnConfig == null)
			{
				return 2f;
			}
			return fishSpawnConfig.GetConstSpeed(fishType);
		}

		private BGPathMgr GetPathMgr()
		{
			if (pathMgr != null)
			{
				return pathMgr;
			}
			if (isRunning)
			{
				return BGPathMgr.Get();
			}
			return UnityEngine.Object.FindObjectOfType<BGPathMgr>();
		}

		[InspectorButton]
		private void Print_AllCurves_Length()
		{
			BGPathMgr bGPathMgr = GetPathMgr();
			List<BGCurve> curveList = bGPathMgr.GetCurveList();
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
			UnityEngine.Debug.Log(HW2_LogHelper.Green("path count:{0},length [avg:{1},min:{2},max:{3}]", count, num, minLength, maxLength));
		}

		private List<PathLengthItem> GetLengthListByCurveList(List<BGCurve> curveList)
		{
			List<PathLengthItem> ret = new List<PathLengthItem>();
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
						ret.Add(new PathLengthItem(result, distance));
					}
				}
			});
			return ret;
		}

		[InspectorButton]
		private void CheckPathIdCompletion()
		{
			BGPathMgr bGPathMgr = GetPathMgr();
			CheckPathIdCompletion(bGPathMgr.GetCurveMap());
		}

		private void CheckPathIdCompletion(Dictionary<string, BGCurve> map)
		{
			List<IdRange> list = new List<IdRange>();
			list.Add(new IdRange(1, 100, "迦魶鱼（匀速曲线路径）"));
			list.Add(new IdRange(101, 180, "小丑鱼（匀速曲线路径）"));
			list.Add(new IdRange(201, 250, "刺豚（匀速曲线路径）"));
			list.Add(new IdRange(301, 350, "旗鱼、锯齿鲨、蝠魟（匀速直线路径）"));
			list.Add(new IdRange(441, 590, "狮子鱼、比目鱼、龙虾、章鱼、灯笼鱼、海龟（匀速直线路径）"));
			list.Add(new IdRange(591, 670, "巨型小丑鱼（匀速直线路径）"));
			list.Add(new IdRange(671, 750, "巨型鲽鱼（匀速直线路径）"));
			list.Add(new IdRange(751, 830, "巨型刺豚（匀速直线路径）"));
			list.Add(new IdRange(831, 870, "鲨鱼（匀速直线路径）"));
			list.Add(new IdRange(871, 910, "杀人鲸（匀速直线路径）"));
			list.Add(new IdRange(911, 950, "霸王鲸（匀速直线路径）"));
			list.Add(new IdRange(951, 970, "暗夜巨兽（匀速直线路径）"));
			list.Add(new IdRange(971, 990, "道具蟹（变速直线路径）"));
			list.Add(new IdRange(361, 400, "迦魶鱼组（匀速直线路径）"));
			list.Add(new IdRange(999, 999, "霸王蟹（椭圆）"));
			List<IdRange> list2 = list;
			int count = 0;
			List<int> invalidList = new List<int>();
			list2.ForEach(delegate(IdRange range)
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
						UnityEngine.Debug.Log(HW2_LogHelper.Red("{0} path[{1}] not correct implement", range.info, HW2_LogHelper.White(id.ToString())));
						invalidList.Add(id);
					}
					count++;
				});
			});
			UnityEngine.Debug.Log($"total check {count} paths, {invalidList.Count} invalid");
		}

		private void PathFishPlay_PlayAction(PathFishPlayerParam param)
		{
			PathFishPlay_PlayAction(param.fishType, param.pathId, param.player, param.fishId, param.isTeam, param.teamId);
		}

		private void PathFishPlay_PlayAction(FishType fishType, int pathId, PathFishPlayer player, int fishId = 0, bool isTeam = false, int teamId = 0)
		{
			int num = fishId;
			player.stopAction = null;
			if (isTeam)
			{
				FishTeam teamById = FishTeamMgr.Get().GetTeamById(teamId);
				if (teamById == null)
				{
					return;
				}
				int i = 0;
				for (int num2 = teamById.Count(); i < num2; i++)
				{
					FishMgr.Get().SetFishIndex(num++);
					Vector3 pointByIndex = teamById.GetPointByIndex(i);
					FishBehaviour fish = FishMgr.Get().SpawnSingleFish(fishType);
					if (!(fish == null))
					{
						if (!SetupFishMovement_BGCurve(fish, pathId, isTeam: true, pointByIndex))
						{
							return;
						}
						FishBehaviour fishBehaviour = fish;
						fishBehaviour.Event_FishDie_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour.Event_FishDie_Handler, (Action<FishBehaviour>)delegate
						{
							player.isPlaying = false;
						});
						PathFishPlayer pathFishPlayer = player;
						pathFishPlayer.stopAction = (Action)Delegate.Combine(pathFishPlayer.stopAction, (Action)delegate
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
				FishMgr.Get().SetFishIndex(num);
				FishBehaviour fish2 = FishMgr.Get().SpawnSingleFish(fishType);
				if (!SetupFishMovement_BGCurve(fish2, pathId, isTeam: false, Vector3.zero))
				{
					return;
				}
				FishBehaviour fishBehaviour2 = fish2;
				fishBehaviour2.Event_FishDie_Handler = (Action<FishBehaviour>)Delegate.Combine(fishBehaviour2.Event_FishDie_Handler, (Action<FishBehaviour>)delegate
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

		private bool SetupFishMovement_BGCurve(FishBehaviour fish, int pathId, bool isTeam, Vector3 offset)
		{
			CurveUsage curveUsageById = BGPathMgr.Get().GetCurveUsageById(pathId);
			if (curveUsageById == null)
			{
				fish.Die();
				return false;
			}
			CursorUsage cursorUsage = curveUsageById.GetCursorUsage(fish.id);
			cursorUsage.translate.ObjectToManipulate = fish.transform;
			cursorUsage.rotate.ObjectToManipulate = fish.transform;
			cursorUsage.linear.Speed = GetSpeedByFishType(fish.originalType);
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
			return true;
		}

		[InspectorButton]
		[InspectorShowIf("isRunning")]
		private void Play_Test_1()
		{
			FishType[] array = new FishType[4]
			{
				FishType.Clown_小丑鱼,
				FishType.Gurnard_迦魶鱼,
				FishType.Grouper_狮子鱼,
				FishType.KillerWhale_杀人鲸
			};
			int[] array2 = new int[4];
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				PathFishPlayer pathFishPlayer = new PathFishPlayer();
				pathFishPlayer.playAction = PathFishPlay_PlayAction;
				pathFishPlayer.pathId = array2[i];
				pathFishPlayer.fishType = array[i];
				pathFishPlayer.fishId = i;
				pathFishPlayer.Play();
			}
		}

		[InspectorShowIf("isRunning")]
		[InspectorButton]
		private void Play_变速鱼_UsingFishQueen()
		{
			UnityEngine.Debug.LogError("本来是开放的");
			FishType[] array = new FishType[4]
			{
				FishType.CrabBoom_连环炸弹蟹,
				FishType.CrabLaser_电磁蟹,
				FishType.Tortoise_海龟,
				FishType.Octopus_章鱼
			};
			int[] array2 = new int[4];
			int num = array.Length;
			int i;
			for (i = 0; i < num; i++)
			{
				PlayPathFishByFishQueen(array[i], array2[i], i, delegate(FishBehaviour _fish)
				{
					_fish.lifetimeDic.Add("pathId", i);
					_fish.lifetimeDic.Add("pathLength", BGPathMgr.Get().GetCurveLength(i));
				});
			}
		}

		private void PlayPathFishByFishQueen(FishType fishType, int pathId, int fishId, Action<FishBehaviour> onSpawnFishAction = null)
		{
			FishQueenData fishQueenData = new FishQueenData();
			FP_Sequence fP_Sequence = new FP_Sequence();
			FG_SingleType<FishType> fG_SingleType = new FG_SingleType<FishType>();
			fG_SingleType.type = fishType;
			fG_SingleType.count = 1;
			fishQueenData.delay = 0f;
			fishQueenData.generator = fG_SingleType;
			fishQueenData.speed = FishQueenMgr.Get().GetSpeedByFishType(fG_SingleType.type.BasicType());
			fP_Sequence.pathId = pathId;
			FishQueen fishQueen = FishQueenMgr.Get().GetFishQueen();
			fishQueen.Setup(fishQueenData, fP_Sequence, 1, fishId);
			if (onSpawnFishAction != null)
			{
				FishQueen fishQueen2 = fishQueen;
				fishQueen2.setupFishAction = (Action<FishBehaviour>)Delegate.Combine(fishQueen2.setupFishAction, onSpawnFishAction);
			}
			fishQueen.Play();
		}
	}
}
