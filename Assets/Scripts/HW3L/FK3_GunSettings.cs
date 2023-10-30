using FullInspector;
using M__M.HaiWang.Player.Gun;
using System.Collections.Generic;
using UnityEngine;

namespace HW3L
{
	[CreateAssetMenu]
	public class FK3_GunSettings : BaseScriptableObject<FullSerializerSerializer>
	{
		public FK3_GunConfig gunDefaultConfig;

		public Dictionary<int, FK3_GunConfig> gunConfigs;

		public FK3_GunConfig GetConfig(int id)
		{
			FK3_GunConfig value = null;
			gunConfigs.TryGetValue(id, out value);
			return value;
		}
	}
}
