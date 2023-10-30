using M__M.HaiWang.FSM;
using M__M.HaiWang.Player.Gun;

namespace M__M.HaiWang.Player.GunState
{
	public class LockingGun : State<GunController>
	{
		public override void OnEnter(ArgList args)
		{
			Owner.TurnToLockingGun();
		}

		public override void OnExit()
		{
			Owner.TurnFromLockingGun();
		}
	}
}
