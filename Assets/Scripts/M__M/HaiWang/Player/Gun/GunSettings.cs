using FullInspector;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Player.Gun
{
	[CreateAssetMenu]
	public class GunSettings : BaseScriptableObject<FullSerializerSerializer>
	{
		public GunConfig gunDefaultConfig;

		public Dictionary<int, GunConfig> gunConfigs;

		public GunConfig GetConfig(int id)
		{
			GunConfig value = null;
			gunConfigs.TryGetValue(id, out value);
			return value;
		}
	}
}
