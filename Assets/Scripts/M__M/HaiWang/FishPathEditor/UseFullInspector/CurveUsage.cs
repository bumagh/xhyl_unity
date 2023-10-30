using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System;
using System.Collections.Generic;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class CurveUsage
	{
		public string id;

		public BGCurve curve;

		public BGCcMath math;

		public float speed;

		public float startDelay;

		public bool disableOnNotUsed;

		internal List<CursorUsage> cursorList = new List<CursorUsage>();

		public Action<CurveUsage, CursorUsage> onCreateUsage;

		public int cursorCount => cursorList.Count;

		public CurveUsage(BGCurve curve, string id)
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

		public CursorUsage GetCursorUsage(int subId)
		{
			return GetCursorUsage(subId.ToString());
		}

		public CursorUsage GetCursorUsage(string subId)
		{
			curve.gameObject.SetActive(value: true);
			CursorUsage cursorUsage = new CursorUsage();
			cursorUsage.Prepare(curve, id, subId, speed, startDelay);
			cursorList.Add(cursorUsage);
			CursorUsage cursorUsage2 = cursorUsage;
			cursorUsage2.FreeAction = (Action<CursorUsage>)Delegate.Combine(cursorUsage2.FreeAction, (Action<CursorUsage>)delegate
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

		public void ForEachCursorUsage(Action<CursorUsage> action)
		{
			if (cursorList != null)
			{
				cursorList.ForEach(action);
			}
		}
	}
}
