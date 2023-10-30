using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public interface FK3_IMoveEvent
	{
		void SetupMovement(Transform moveObj);

		void SetupMoveStop();
	}
}
