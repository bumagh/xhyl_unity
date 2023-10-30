namespace M__M.HaiWang.Player.Gun
{
	public static class GunUtils
	{
		public static float GetGunDirRotation(GunDir dir)
		{
			switch (dir)
			{
			case GunDir.Up:
				return 0f;
			case GunDir.Down:
				return 180f;
			case GunDir.Right:
				return 90f;
			case GunDir.Left:
				return 270f;
			default:
				return 0f;
			}
		}
	}
}
