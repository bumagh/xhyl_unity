using System;
using System.Collections.Generic;
using UnityEngine;

namespace FullSerializer.Speedup
{
	public class UnityEngine_Vector3_DirectConverter : fsDirectConverter<Vector3>, fsIAotConverter
	{
		private fsAotVersionInfo _versionInfo = new fsAotVersionInfo
		{
			IsConstructorPublic = true,
			Members = new fsAotVersionInfo.Member[3]
			{
				new fsAotVersionInfo.Member
				{
					MemberName = "x",
					JsonName = "x",
					StorageType = "float"
				},
				new fsAotVersionInfo.Member
				{
					MemberName = "y",
					JsonName = "y",
					StorageType = "float"
				},
				new fsAotVersionInfo.Member
				{
					MemberName = "z",
					JsonName = "z",
					StorageType = "float"
				}
			}
		};

		fsAotVersionInfo fsIAotConverter.VersionInfo => _versionInfo;

		protected override fsResult DoSerialize(Vector3 model, Dictionary<string, fsData> serialized)
		{
			fsResult success = fsResult.Success;
			success += SerializeMember(serialized, null, "x", model.x);
			success += SerializeMember(serialized, null, "y", model.y);
			return success + SerializeMember(serialized, null, "z", model.z);
		}

		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Vector3 model)
		{
			fsResult success = fsResult.Success;
			float value = model.x;
			success += DeserializeMember(data, null, "x", out value);
			model.x = value;
			float value2 = model.y;
			success += DeserializeMember(data, null, "y", out value2);
			model.y = value2;
			float value3 = model.z;
			success += DeserializeMember(data, null, "z", out value3);
			model.z = value3;
			return success;
		}

		public override object CreateInstance(fsData data, Type storageType)
		{
			return default(Vector3);
		}
	}
}
