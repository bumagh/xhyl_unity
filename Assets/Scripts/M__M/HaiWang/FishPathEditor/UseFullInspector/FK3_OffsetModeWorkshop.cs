using BansheeGz.BGSpline.Curve;
using FullInspector;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FK3_OffsetModeWorkshop
	{
		public int num;

		public Vector3 offset = Vector3.right;

		public BGCurve masterCurve;

		public Action<FK3_OffsetModeWorkshop> doAction;

		[InspectorButton]
		[InspectorName("生成")]
		public void Generate()
		{
			if (CheckValid())
			{
				for (int i = 0; i < num; i++)
				{
					doAction(this);
				}
			}
		}

		private bool CheckValid()
		{
			Assert.raiseExceptions = true;
			return true;
		}

		public void Test()
		{
			UnityEngine.Debug.Log("FK3_OffsetModeWorkshop.Test");
		}
	}
}
