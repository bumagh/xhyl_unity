using System;
using System.Collections.Generic;
using UnityEngine;

namespace FullSerializer.Internal.DirectConverters
{
	public class Gradient_DirectConverter : fsDirectConverter<Gradient>
	{
		protected override fsResult DoSerialize(Gradient model, Dictionary<string, fsData> serialized)
		{
			fsResult success = fsResult.Success;
			success += SerializeMember(serialized, null, "alphaKeys", model.alphaKeys);
			return success + SerializeMember(serialized, null, "colorKeys", model.colorKeys);
		}

		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Gradient model)
		{
			fsResult success = fsResult.Success;
			GradientAlphaKey[] value = model.alphaKeys;
			success += DeserializeMember(data, null, "alphaKeys", out value);
			model.alphaKeys = value;
			GradientColorKey[] value2 = model.colorKeys;
			success += DeserializeMember(data, null, "colorKeys", out value2);
			model.colorKeys = value2;
			return success;
		}

		public override object CreateInstance(fsData data, Type storageType)
		{
			return new Gradient();
		}
	}
}
