using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public interface IMoveEvent
	{
		void SetupMovement(Transform moveObj);

		void SetupMoveStop();
	}
}
