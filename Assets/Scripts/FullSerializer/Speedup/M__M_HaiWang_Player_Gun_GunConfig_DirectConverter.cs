using M__M.HaiWang.Player.Gun;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FullSerializer.Speedup
{
	public class M__M_HaiWang_Player_Gun_GunConfig_DirectConverter : fsDirectConverter<GunConfig>, fsIAotConverter
	{
		private fsAotVersionInfo _versionInfo = new fsAotVersionInfo
		{
			IsConstructorPublic = true,
			Members = new fsAotVersionInfo.Member[5]
			{
				new fsAotVersionInfo.Member
				{
					MemberName = "id",
					JsonName = "id",
					StorageType = "int"
				},
				new fsAotVersionInfo.Member
				{
					MemberName = "pos",
					JsonName = "pos",
					StorageType = "UnityEngine.Vector3"
				},
				new fsAotVersionInfo.Member
				{
					MemberName = "dir",
					JsonName = "dir",
					StorageType = "M__M.HaiWang.Player.Gun.GunDir"
				},
				new fsAotVersionInfo.Member
				{
					MemberName = "labelSprite",
					JsonName = "labelSprite",
					StorageType = "UnityEngine.Sprite"
				},
				new fsAotVersionInfo.Member
				{
					MemberName = "laserRangeSprite",
					JsonName = "laserRangeSprite",
					StorageType = "UnityEngine.Sprite"
				}
			}
		};

		fsAotVersionInfo fsIAotConverter.VersionInfo => _versionInfo;

		protected override fsResult DoSerialize(GunConfig model, Dictionary<string, fsData> serialized)
		{
			fsResult success = fsResult.Success;
			success += SerializeMember(serialized, null, "id", model.id);
			success += SerializeMember(serialized, null, "pos", model.pos);
			success += SerializeMember(serialized, null, "dir", model.dir);
			success += SerializeMember(serialized, null, "labelSprite", model.labelSprite);
			return success + SerializeMember(serialized, null, "laserRangeSprite", model.laserRangeSprite);
		}

		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref GunConfig model)
		{
			fsResult success = fsResult.Success;
			int value = model.id;
			success += DeserializeMember(data, null, "id", out value);
			model.id = value;
			Vector3 value2 = model.pos;
			success += DeserializeMember(data, null, "pos", out value2);
			model.pos = value2;
			GunDir value3 = model.dir;
			success += DeserializeMember(data, null, "dir", out value3);
			model.dir = value3;
			Sprite value4 = model.labelSprite;
			success += DeserializeMember(data, null, "labelSprite", out value4);
			model.labelSprite = value4;
			Sprite value5 = model.laserRangeSprite;
			success += DeserializeMember(data, null, "laserRangeSprite", out value5);
			model.laserRangeSprite = value5;
			return success;
		}

		public override object CreateInstance(fsData data, Type storageType)
		{
			return new GunConfig();
		}
	}
}
