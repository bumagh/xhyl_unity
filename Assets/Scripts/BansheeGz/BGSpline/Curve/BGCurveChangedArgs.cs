using System;

namespace BansheeGz.BGSpline.Curve
{
	public class BGCurveChangedArgs : EventArgs, ICloneable
	{
		public enum ChangeTypeEnum
		{
			Multiple,
			CurveTransform,
			Points,
			Point,
			Fields,
			Snap,
			Curve
		}

		public class BeforeChange : EventArgs
		{
			public static readonly BeforeChange BeforeChangeInstance = new BeforeChange();

			public string Operation;

			private BeforeChange()
			{
			}

			public static BeforeChange GetInstance(string operation)
			{
				BeforeChangeInstance.Operation = operation;
				return BeforeChangeInstance;
			}
		}

		private static readonly BGCurveChangedArgs Instance = new BGCurveChangedArgs();

		private ChangeTypeEnum changeType;

		private BGCurve curve;

		private BGCurvePointI point;

		private string message;

		private BGCurveChangedArgs[] multipleChanges;

		public ChangeTypeEnum ChangeType => changeType;

		public BGCurve Curve => curve;

		public string Message => message;

		public BGCurveChangedArgs[] MultipleChanges => multipleChanges;

		private BGCurveChangedArgs()
		{
		}

		public static BGCurveChangedArgs GetInstance(BGCurve curve, ChangeTypeEnum type, string message)
		{
			Instance.curve = curve;
			Instance.changeType = type;
			Instance.message = message;
			Instance.multipleChanges = null;
			Instance.point = null;
			return Instance;
		}

		public static BGCurveChangedArgs GetInstance(BGCurve curve, BGCurveChangedArgs[] changes, string changesInTransaction)
		{
			Instance.curve = curve;
			Instance.changeType = ChangeTypeEnum.Multiple;
			Instance.message = "changes in transaction";
			Instance.multipleChanges = changes;
			Instance.point = null;
			return Instance;
		}

		public static BGCurveChangedArgs GetInstance(BGCurve curve, BGCurvePointI point, string changesInTransaction)
		{
			Instance.curve = curve;
			Instance.changeType = ChangeTypeEnum.Point;
			Instance.message = "changes in transaction";
			Instance.point = point;
			return Instance;
		}

		public object Clone()
		{
			BGCurveChangedArgs bGCurveChangedArgs = new BGCurveChangedArgs();
			bGCurveChangedArgs.changeType = changeType;
			bGCurveChangedArgs.curve = curve;
			bGCurveChangedArgs.multipleChanges = multipleChanges;
			bGCurveChangedArgs.message = message;
			bGCurveChangedArgs.point = point;
			return bGCurveChangedArgs;
		}

		protected bool Equals(BGCurveChangedArgs other)
		{
			return changeType == other.changeType && object.Equals(curve, other.curve);
		}

		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(null, obj))
			{
				return false;
			}
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((BGCurveChangedArgs)obj);
		}

		public override int GetHashCode()
		{
			int num = (int)changeType;
			return (num * 397) ^ ((curve != null) ? curve.GetHashCode() : 0);
		}
	}
}
