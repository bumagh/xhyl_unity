using M__M.HaiWang.Player.Gun;

namespace M__M.HaiWang.KillFish
{
	public class FK3_KillFishMgr : FK3_SimpleSingletonBehaviour<FK3_KillFishMgr>
	{
		private FK3_KillFishController[] _controllers;

		private void Awake()
		{
			FK3_SimpleSingletonBehaviour<FK3_KillFishMgr>.s_instance = this;
		}

		public void Init()
		{
			_controllers = new FK3_KillFishController[4];
			for (int i = 0; i < 4; i++)
			{
				FK3_KillFishController fK3_KillFishController = new FK3_KillFishController(this);
				fK3_KillFishController.SetGun(FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.Get().GetGunById(i + 1));
				_controllers[i] = fK3_KillFishController;
			}
		}

		public FK3_KillFishController GetControllerById(int seatId)
		{
			return _controllers[seatId - 1];
		}
	}
}
