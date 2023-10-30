using M__M.HaiWang.Player.Gun;

namespace M__M.HaiWang.Player.GunState
{
	public class FK3_NormalGun : FK3_GunState<FK3_GunController>
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
