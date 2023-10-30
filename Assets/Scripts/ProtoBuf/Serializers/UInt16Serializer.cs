using ProtoBuf.Meta;
using System;

namespace ProtoBuf.Serializers
{
	internal class UInt16Serializer : IProtoSerializer
	{
		private static readonly Type expectedType = typeof(ushort);

		bool IProtoSerializer.RequiresOldValue => false;

		bool IProtoSerializer.ReturnsValue => true;

		public virtual Type ExpectedType => expectedType;

		public UInt16Serializer(TypeModel model)
		{
		}

		public virtual object Read(object value, ProtoReader source)
		{
			return source.ReadUInt16();
		}

		public virtual void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteUInt16((ushort)value, dest);
		}
	}
}
