using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using FullInspector;
using M__M.HaiWang.Message;
using M__M.HaiWang.Scenario;
using PathologicalGames;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class BGPathMgr : BaseBehavior<FullSerializerSerializer>
	{
		private static BGPathMgr s_instance;

		[SerializeField]
		private HW2_SpawnPool m_pathPool;

		public List<BGCurve> curveList = new List<BGCurve>();

		[InspectorCollapsedFoldout]
		[SerializeField]
		public Dictionary<string, BGCurve> curveDic = new Dictionary<string, BGCurve>();

		private Dictionary<string, CurveUsage> usageDic = new Dictionary<string, CurveUsage>();

		private List<CursorUsage> activeCursors = new List<CursorUsage>();

		private Queue<UnityEngine.Object> destroyQueue = new Queue<UnityEngine.Object>();

		private int num = 8;

		private UnityEngine.Object @object;

		public HW2_SpawnPool PathPool => m_pathPool;

		public static BGPathMgr Get()
		{
			return s_instance;
		}

		protected override void Awake()
		{
			ListToDic();
			base.Awake();
			s_instance = this;
			CursorUsage.destroyAction = DestroyOne;
		}

		private void ListToDic()
		{
			UnityEngine.Debug.LogError("==========开始字典转换=========");
			try
			{
				curveDic = new Dictionary<string, BGCurve>();
				int num = 0;
				for (int i = 0; i < curveList.Count; i++)
				{
					if (curveList[i].PointsCount > 9)
					{
						curveDic.Add(num.ToString(), curveList[i]);
						num++;
					}
				}
				curveDic.Add("999", curveList[curveList.Count - 1]);
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("字典转换错误: " + arg);
			}
		}

		private void Start()
		{
		}

		private void OnEnable()
		{
			HW2_MessageCenter.RegisterHandle("SetCursorUsageLinearSpeed", OnSpeedUpAll);
		}

		private void OnDisable()
		{
			HW2_MessageCenter.UnRegisterHandle("SetCursorUsageLinearSpeed", OnSpeedUpAll);
		}

		private void CheckQueueCount()
		{
			if (destroyQueue.Count <= num)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				@object = destroyQueue.Dequeue();
				if ((bool)@object)
				{
					UnityEngine.Object.Destroy(@object);
				}
			}
		}

		public BGCurve GetCurveById(int id)
		{
			BGCurve value = null;
			curveDic.TryGetValue(id.ToString(), out value);
			if (value == null)
			{
				UnityEngine.Debug.LogError("路径为空: " + id);
				return null;
			}
			return value;
		}

		public float GetCurveLength(int id)
		{
			UnityEngine.Debug.LogError("获取路径2");
			BGCurve curveById = GetCurveById(id);
			if (curveById == null)
			{
				return 0f;
			}
			BGCcMath bGCcMath = curveById.GetComponent<BGCcMath>();
			if (bGCcMath == null)
			{
				bGCcMath = curveById.gameObject.AddComponent<BGCcMath>();
			}
			return bGCcMath.GetDistance();
		}

		public CurveUsage GetCurveUsageById(int id)
		{
			CurveUsage value = null;
			usageDic.TryGetValue(id.ToString(), out value);
			if (value == null)
			{
				BGCurve curveById = GetCurveById(id);
				if (curveById == null)
				{
					UnityEngine.Debug.LogError("路径为空!");
					return null;
				}
				value = new CurveUsage(curveById, id.ToString());
				value.disableOnNotUsed = true;
				CurveUsage curveUsage = value;
				curveUsage.onCreateUsage = (Action<CurveUsage, CursorUsage>)Delegate.Combine(curveUsage.onCreateUsage, new Action<CurveUsage, CursorUsage>(OnCreateCursorUsage));
				value.Prepare();
				usageDic.Add(id.ToString(), value);
			}
			return value;
		}

		public void SpeedUpAllCursors()
		{
			activeCursors.ForEach(delegate(CursorUsage cursorUsage)
			{
				cursorUsage.linear.Speed = 7.5f;
			});
		}

		public Dictionary<string, BGCurve> GetCurveMap()
		{
			if (curveDic == null)
			{
				ListToDic();
			}
			return curveDic;
		}

		public List<BGCurve> GetCurveList()
		{
			List<BGCurve> list = new List<BGCurve>();
			return GetCurveList(ref list);
		}

		public List<BGCurve> GetCurveList(ref List<BGCurve> list)
		{
			if (curveDic == null)
			{
				ListToDic();
				return list;
			}
			foreach (KeyValuePair<string, BGCurve> item in curveDic)
			{
				if (!(item.Value == null))
				{
					list.Add(item.Value);
				}
			}
			return list;
		}

		private void DestroyOne(UnityEngine.Object obj)
		{
			if (obj != null)
			{
				destroyQueue.Enqueue(obj);
			}
			CheckQueueCount();
		}

		private void MakeDicFromList(ref Dictionary<string, BGCurve> dic, ref List<BGCurve> list)
		{
			UnityEngine.Debug.Log("MakeDicFromList");
			foreach (BGCurve item in list)
			{
				if (item != null)
				{
					string name = item.name;
					if (dic.ContainsKey(name))
					{
						UnityEngine.Debug.Log(HW2_LogHelper.Red($"fishPath[id:{name}] repeat"));
					}
					else
					{
						dic.Add(name, item);
					}
				}
			}
		}

		private void OnCreateCursorUsage(CurveUsage curveUsage, CursorUsage cursorUsage)
		{
			activeCursors.Add(cursorUsage);
			cursorUsage.FreeAction = (Action<CursorUsage>)Delegate.Remove(cursorUsage.FreeAction, new Action<CursorUsage>(OnCursorFree));
			cursorUsage.FreeAction = (Action<CursorUsage>)Delegate.Combine(cursorUsage.FreeAction, new Action<CursorUsage>(OnCursorFree));
		}

		private void OnCursorFree(CursorUsage cursorUsage)
		{
			activeCursors.Remove(cursorUsage);
			cursorUsage.FreeAction = (Action<CursorUsage>)Delegate.Remove(cursorUsage.FreeAction, new Action<CursorUsage>(OnCursorFree));
		}

		private void OnSpeedUpAll(KeyValueInfo keyValueInfo)
		{
			UnityEngine.Debug.Log("执行鱼清场，加速");
			if (fiSimpleSingletonBehaviour<StoryScenarioMgr>.Get() != null)
			{
				UnityEngine.Debug.LogError("杀鱼了");
				fiSimpleSingletonBehaviour<StoryScenarioMgr>.Get().DieFish();
			}
			else
			{
				UnityEngine.Debug.LogError("FK3_StoryScenarioMgr为空");
			}
			SpeedUpAllCursors();
		}
	}
}
