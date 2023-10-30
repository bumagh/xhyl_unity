using BansheeGz.BGSpline.Curve;
using FullInspector;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector.Assistance
{
	public class SpawnerCurveItem
	{
		public delegate void OperateCallback(SpawnerCurveItem pair);

		public BGCurve curve;

		public FishSpawnerBehaviour spawner;

		public static bool printLog = true;

		[NotSerialized]
		public OperateCallback addSpawnerAction;

		[NotSerialized]
		public OperateCallback removeSpawnerAction;

		public bool hasSpawner => spawner != null;

		[InspectorButton]
		[InspectorHideIf("hasSpawner")]
		public void AddSpawner()
		{
			if (addSpawnerAction == null)
			{
				if (printLog)
				{
					UnityEngine.Debug.Log(string.Format("addSpawnerAction is {0}", HW2_LogHelper.Red("null")));
				}
			}
			else
			{
				addSpawnerAction(this);
			}
		}

		[InspectorShowIf("hasSpawner")]
		[InspectorButton]
		public void RemoveSpawner()
		{
			if (removeSpawnerAction == null)
			{
				if (printLog)
				{
					UnityEngine.Debug.Log(string.Format("removeSpawnerAction is {0}", HW2_LogHelper.Red("null")));
				}
			}
			else
			{
				removeSpawnerAction(this);
			}
		}
	}
}
