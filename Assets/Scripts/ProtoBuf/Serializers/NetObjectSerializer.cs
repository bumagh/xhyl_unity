using ProtoBuf.Meta;
using System;

namespace ProtoBuf.Serializers
{
	internal sealed class NetObjectSerializer : IProtoSerializer
	{
		private readonly int key;

		private readonly Type type;

		private readonly BclHelpers.NetObjectOptions options;

		public Type ExpectedType => type;

		public bool ReturnsValue => true;

		public bool RequiresOldValue => true;

		public NetObjectSerializer(TypeModel model, Type type, int key, BclHelpers.NetObjectOptions options)
		{
			bool flag = (options & BclHelpers.NetObjectOptions.DynamicType) != BclHelpers.NetObjectOptions.None;
			this.key = ((!flag) ? key : (-1));
			this.type = ((!flag) ? type : model.MapType(typeof(object)));
			this.options = options;
		}

		public object Read(object value, ProtoReader source)
		{
			return BclHelpers.ReadNetObject(value, source, key, (!(type == typeof(object))) ? type : null, options);
		}

		public void Write(object value, ProtoWriter dest)
		{
			BclHelpers.WriteNetObject(value, dest, key, options);
		}
	}
}
