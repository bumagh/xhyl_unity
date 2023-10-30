using System;

namespace FullInspector
{
	[Flags]
	public enum VerifyPrefabTypeFlags
	{
		None = 0x1,
		Prefab = 0x2,
		ModelPrefab = 0x4,
		PrefabInstance = 0x8,
		ModelPrefabInstance = 0x10,
		MissingPrefabInstance = 0x20,
		DisconnectedPrefabInstance = 0x40,
		DisconnectedModelPrefabInstance = 0x80
	}
}
