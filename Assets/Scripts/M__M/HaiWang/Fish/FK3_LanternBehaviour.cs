using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class FK3_LanternBehaviour : FK3_FishBehaviour
	{
		[SerializeField]
		private Transform[] _boomPositions;

		public override Transform[] GetBoomPositions()
		{
			return _boomPositions;
		}
	}
}
