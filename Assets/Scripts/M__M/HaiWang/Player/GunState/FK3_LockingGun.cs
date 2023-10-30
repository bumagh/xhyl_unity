using M__M.HaiWang.FK3_FSM;
using M__M.HaiWang.Player.Gun;

namespace M__M.HaiWang.Player.GunState
{
	public class FK3_LockingGun : FK3_State<FK3_GunController>
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
