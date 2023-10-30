using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class LanternBehaviour : FishBehaviour
	{
		[SerializeField]
		private Transform[] _boomPositions;

		public override Transform[] GetBoomPositions()
		{
			return _boomPositions;
		}
	}
}
