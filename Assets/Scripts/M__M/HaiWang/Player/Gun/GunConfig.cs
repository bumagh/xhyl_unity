using System;
using UnityEngine;

namespace M__M.HaiWang.Player.Gun
{
	[Serializable]
	public class GunConfig
	{
		public int id;

		public Vector3 pos;

		public GunDir dir;

		public Sprite labelSprite;

		public Sprite laserRangeSprite;
	}
}
