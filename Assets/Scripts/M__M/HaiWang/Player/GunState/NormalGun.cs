using M__M.HaiWang.Player.Gun;

namespace M__M.HaiWang.Player.GunState
{
	public class NormalGun : GunState<GunController>
	{
		public override void OnEnter(ArgList args)
		{
			Owner.TurnToNormalGun();
		}

		public override void OnExit()
		{
			Owner.TurnFromNormalGun();
		}

		public override void Shoot()
		{
			Owner.NormalGunShoot();
		}

		public override void RotateByInput()
		{
			Owner.NormalGunRotateByInput();
		}
	}
}
