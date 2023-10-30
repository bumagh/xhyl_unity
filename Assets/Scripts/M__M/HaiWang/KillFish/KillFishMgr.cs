using M__M.HaiWang.Player.Gun;

namespace M__M.HaiWang.KillFish
{
	public class KillFishMgr : SimpleSingletonBehaviour<KillFishMgr>
	{
		private KillFishController[] _controllers;

		private void Awake()
		{
			SimpleSingletonBehaviour<KillFishMgr>.s_instance = this;
		}

		public void Init()
		{
			_controllers = new KillFishController[4];
			for (int i = 0; i < 4; i++)
			{
				KillFishController killFishController = new KillFishController(this);
				killFishController.SetGun(fiSimpleSingletonBehaviour<GunMgr>.Get().GetGunById(i + 1));
				_controllers[i] = killFishController;
			}
		}

		public KillFishController GetControllerById(int seatId)
		{
			return _controllers[seatId - 1];
		}
	}
}
