using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
	public abstract class BGCcWithCursorObject : BGCcWithCursor
	{
		private const string ErrorObjectNotSet = "Object To Manipulate is not set.";

		[SerializeField]
		[Tooltip("Object to manipulate.\r\n")]
		private Transform objectToManipulate;

		public Transform ObjectToManipulate
		{
			get
			{
				return objectToManipulate;
			}
			set
			{
				ParamChanged(ref objectToManipulate, value);
			}
		}

		public override string Error => (!(objectToManipulate == null)) ? null : "Object To Manipulate is not set.";
	}
}
