using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System;
using System.Collections.Generic;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FK3_CurveUsage
	{
		public string id;

		public BGCurve curve;

		public BGCcMath math;

		public float speed;

		public float startDelay;

		public bool disableOnNotUsed;

		internal List<FK3_CursorUsage> cursorList = new List<FK3_CursorUsage>();

		public Action<FK3_CurveUsage, FK3_CursorUsage> onCreateUsage;

		public int cursorCount => cursorList.Count;

		public FK3_CurveUsage(BGCurve curve, string id)
		{
			this.curve = curve;
			this.id = id;
		}

		public void Prepare()
		{
			BGCcMath bGCcMath = curve.GetComponent<BGCcMath>();
			if (bGCcMath == null)
			{
				bGCcMath = curve.gameObject.AddComponent<BGCcMath>();
			}
			bGCcMath.Fields = BGCurveBaseMath.Fields.PositionAndTangent;
			math = bGCcMath;
			math.Fields = BGCurveBaseMath.Fields.PositionAndTangent;
		}

		public FK3_CursorUsage GetCursorUsage(int subId)
		{
			return GetCursorUsage(subId.ToString());
		}

		public FK3_CursorUsage GetCursorUsage(string subId)
		{
			curve.gameObject.SetActive(value: true);
			FK3_CursorUsage cursorUsage = new FK3_CursorUsage();
			cursorUsage.Prepare(curve, id, subId, speed, startDelay);
			cursorList.Add(cursorUsage);
			FK3_CursorUsage fK3_CursorUsage = cursorUsage;
			fK3_CursorUsage.FreeAction = (Action<FK3_CursorUsage>)Delegate.Combine(fK3_CursorUsage.FreeAction, (Action<FK3_CursorUsage>)delegate
			{
				cursorList.Remove(cursorUsage);
				if (cursorCount == 0 && disableOnNotUsed)
				{
					curve.gameObject.SetActive(value: false);
				}
			});
			if (onCreateUsage != null)
			{
				onCreateUsage(this, cursorUsage);
			}
			return cursorUsage;
		}

		public void ForEachCursorUsage(Action<FK3_CursorUsage> action)
		{
			if (cursorList != null)
			{
				cursorList.ForEach(action);
			}
		}
	}
}
