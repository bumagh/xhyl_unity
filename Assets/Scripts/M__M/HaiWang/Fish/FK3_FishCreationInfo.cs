using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class FK3_FishCreationInfo
	{
		public FK3_FishType fishType;

		public int fishId;

		public FK3_FishMovementType movementTpye;

		public int formationId;

		public int pathId;

		public Vector3 position = Vector3.zero;

		public Quaternion rotation = Quaternion.identity;

		public Vector3 scale = Vector3.one;

		public FK3_FishCreationInfo(FK3_FishType fishType = FK3_FishType.Unknown, int fishId = 0, FK3_FishMovementType movementTpye = FK3_FishMovementType.None, int formationId = 0, int pathId = 0)
		{
			this.fishType = fishType;
			this.fishId = fishId;
			this.movementTpye = movementTpye;
			this.formationId = formationId;
			this.pathId = pathId;
		}

		public void SetTransform(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
		}
	}
}
