using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using FullInspector;
using M__M.HaiWang.Message;
using PathologicalGames;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FK3_BGPathMgr : BaseBehavior<FullSerializerSerializer>
	{
		private static FK3_BGPathMgr s_instance;

		[SerializeField]
		private FK3_SpawnPool m_pathPool;

		public List<BGCurve> curveList = new List<BGCurve>();

		[InspectorCollapsedFoldout]
		[SerializeField]
		public Dictionary<string, BGCurve> curveDic = new Dictionary<string, BGCurve>();

		private Dictionary<string, FK3_CurveUsage> usageDic = new Dictionary<string, FK3_CurveUsage>();

		private List<FK3_CursorUsage> activeCursors = new List<FK3_CursorUsage>();

		private Queue<UnityEngine.Object> destroyQueue = new Queue<UnityEngine.Object>();

		private int num = 8;

		private UnityEngine.Object @object;

		public FK3_SpawnPool PathPool => m_pathPool;

		public static FK3_BGPathMgr Get()
		{
			return s_instance;
		}

		protected override void Awake()
		{
			ListToDic();
			base.Awake();
			s_instance = this;
			FK3_CursorUsage.destroyAction = DestroyOne;
		}

		private void ListToDic()
		{
			UnityEngine.Debug.LogError("==========开始字典转换=========");
			try
			{
				curveDic = new Dictionary<string, BGCurve>();
				int num = 0;
				for (int i = 0; i < curveList.Count - 7; i++)
				{
					if (curveList[i].PointsCount > 9)
					{
						curveDic.Add(num.ToString(), curveList[i]);
						num++;
					}
				}
				curveDic.Add("999", curveList[curveList.Count - 7]);
				curveDic.Add("888", curveList[curveList.Count - 6]);
				curveDic.Add("777", curveList[curveList.Count - 5]);
				curveDic.Add("666", curveList[curveList.Count - 4]);
				curveDic.Add("555", curveList[curveList.Count - 3]);
				curveDic.Add("444", curveList[curveList.Count - 2]);
				curveDic.Add("333", curveList[curveList.Count - 1]);
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("字典转换错误: " + arg);
			}
		}

		private void OnEnable()
		{
			FK3_MessageCenter.RegisterHandle("SetCursorUsageLinearSpeed", OnSpeedUpAll);
		}

		private void OnDisable()
		{
			FK3_MessageCenter.UnRegisterHandle("SetCursorUsageLinearSpeed", OnSpeedUpAll);
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

		public FK3_CurveUsage GetCurveUsageById(int id)
		{
			FK3_CurveUsage value = null;
			usageDic.TryGetValue(id.ToString(), out value);
			if (value == null)
			{
				BGCurve curveById = GetCurveById(id);
				if (curveById == null)
				{
					UnityEngine.Debug.LogError("路径为空!");
					return null;
				}
				value = new FK3_CurveUsage(curveById, id.ToString());
				value.disableOnNotUsed = true;
				FK3_CurveUsage fK3_CurveUsage = value;
				fK3_CurveUsage.onCreateUsage = (Action<FK3_CurveUsage, FK3_CursorUsage>)Delegate.Combine(fK3_CurveUsage.onCreateUsage, new Action<FK3_CurveUsage, FK3_CursorUsage>(OnCreateCursorUsage));
				value.Prepare();
				usageDic.Add(id.ToString(), value);
			}
			return value;
		}

		public void SpeedUpAllCursors()
		{
			activeCursors.ForEach(delegate(FK3_CursorUsage cursorUsage)
			{
				cursorUsage.linear.Speed = 7f;
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
						UnityEngine.Debug.Log(FK3_LogHelper.Red($"fishPath[id:{name}] repeat"));
					}
					else
					{
						dic.Add(name, item);
					}
				}
			}
		}

		private void OnCreateCursorUsage(FK3_CurveUsage curveUsage, FK3_CursorUsage cursorUsage)
		{
			activeCursors.Add(cursorUsage);
			cursorUsage.FreeAction = (Action<FK3_CursorUsage>)Delegate.Remove(cursorUsage.FreeAction, new Action<FK3_CursorUsage>(OnCursorFree));
			cursorUsage.FreeAction = (Action<FK3_CursorUsage>)Delegate.Combine(cursorUsage.FreeAction, new Action<FK3_CursorUsage>(OnCursorFree));
		}

		private void OnCursorFree(FK3_CursorUsage cursorUsage)
		{
			activeCursors.Remove(cursorUsage);
			cursorUsage.FreeAction = (Action<FK3_CursorUsage>)Delegate.Remove(cursorUsage.FreeAction, new Action<FK3_CursorUsage>(OnCursorFree));
		}

		private void OnSpeedUpAll(FK3_KeyValueInfo keyValueInfo)
		{
			UnityEngine.Debug.Log("执行鱼清场，加速");
			SpeedUpAllCursors();
		}
	}
}
