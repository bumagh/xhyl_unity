using HW3L;

namespace M__M.HaiWang.Player.Gun
{
	public static class FK3_GunUtils
	{
		public static float GetGunDirRotation(FK3_GunDir dir)
		{
			switch (dir)
			{
			case FK3_GunDir.Up:
				return 0f;
			case FK3_GunDir.Down:
				return 180f;
			case FK3_GunDir.Right:
				return 90f;
			case FK3_GunDir.Left:
				return 270f;
			default:
				return 0f;
			}
		}
	}
}
