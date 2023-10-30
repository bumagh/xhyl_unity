using ProtoBuf.Meta;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ProtoBuf.Serializers
{
	internal sealed class ListDecorator : ProtoDecoratorBase
	{
		private readonly byte options;

		private const byte OPTIONS_IsList = 1;

		private const byte OPTIONS_SuppressIList = 2;

		private const byte OPTIONS_WritePacked = 4;

		private const byte OPTIONS_ReturnList = 8;

		private const byte OPTIONS_OverwriteList = 16;

		private const byte OPTIONS_SupportNull = 32;

		private readonly Type declaredType;

		private readonly Type concreteType;

		private readonly MethodInfo add;

		private readonly int fieldNumber;

		private readonly WireType packedWireType;

		private static readonly Type ienumeratorType = typeof(IEnumerator);

		private static readonly Type ienumerableType = typeof(IEnumerable);

		private bool IsList => (options & 1) != 0;

		private bool SuppressIList => (options & 2) != 0;

		private bool WritePacked => (options & 4) != 0;

		private bool SupportNull => (options & 0x20) != 0;

		private bool ReturnList => (options & 8) != 0;

		public override Type ExpectedType => declaredType;

		public override bool RequiresOldValue => AppendToCollection;

		public override bool ReturnsValue => ReturnList;

		private bool AppendToCollection => (options & 0x10) == 0;

		public ListDecorator(TypeModel model, Type declaredType, Type concreteType, IProtoSerializer tail, int fieldNumber, bool writePacked, WireType packedWireType, bool returnList, bool overwriteList, bool supportNull)
			: base(tail)
		{
			if (returnList)
			{
				options |= 8;
			}
			if (overwriteList)
			{
				options |= 16;
			}
			if (supportNull)
			{
				options |= 32;
			}
			if ((writePacked || packedWireType != WireType.None) && fieldNumber <= 0)
			{
				throw new ArgumentOutOfRangeException("fieldNumber");
			}
			if (!CanPack(packedWireType))
			{
				if (writePacked)
				{
					throw new InvalidOperationException("Only simple data-types can use packed encoding");
				}
				packedWireType = WireType.None;
			}
			this.fieldNumber = fieldNumber;
			if (writePacked)
			{
				options |= 4;
			}
			this.packedWireType = packedWireType;
			if (declaredType == null)
			{
				throw new ArgumentNullException("declaredType");
			}
			if (declaredType.IsArray)
			{
				throw new ArgumentException("Cannot treat arrays as lists", "declaredType");
			}
			this.declaredType = declaredType;
			this.concreteType = concreteType;
			add = TypeModel.ResolveListAdd(model, declaredType, tail.ExpectedType, out bool isList);
			if (isList)
			{
				options |= 1;
				string fullName = declaredType.FullName;
				if (fullName != null && fullName.StartsWith("System.Data.Linq.EntitySet`1[["))
				{
					options |= 2;
				}
			}
			if (add == null)
			{
				throw new InvalidOperationException("Unable to resolve a suitable Add method for " + declaredType.FullName);
			}
		}

		internal static bool CanPack(WireType wireType)
		{
			switch (wireType)
			{
			case WireType.Variant:
			case WireType.Fixed64:
			case WireType.Fixed32:
			case WireType.SignedVariant:
				return true;
			default:
				return false;
			}
		}

		private MethodInfo GetEnumeratorInfo(TypeModel model, out MethodInfo moveNext, out MethodInfo current)
		{
			Type type = null;
			Type expectedType = ExpectedType;
			MethodInfo instanceMethod = Helpers.GetInstanceMethod(expectedType, "GetEnumerator", null);
			Type expectedType2 = Tail.ExpectedType;
			Type type2 = null;
			Type type3;
			if (instanceMethod != null)
			{
				type2 = instanceMethod.ReturnType;
				type3 = type2;
				moveNext = Helpers.GetInstanceMethod(type3, "MoveNext", null);
				PropertyInfo property = Helpers.GetProperty(type3, "Current", nonPublic: false);
				current = ((!(property == null)) ? Helpers.GetGetMethod(property, nonPublic: false, allowInternal: false) : null);
				if (moveNext == null && model.MapType(ienumeratorType).IsAssignableFrom(type3))
				{
					moveNext = Helpers.GetInstanceMethod(model.MapType(ienumeratorType), "MoveNext", null);
				}
				if (moveNext != null && moveNext.ReturnType == model.MapType(typeof(bool)) && current != null && current.ReturnType == expectedType2)
				{
					return instanceMethod;
				}
				moveNext = (current = (instanceMethod = null));
			}
			Type type4 = model.MapType(typeof(IEnumerable<>), demand: false);
			if (type4 != null)
			{
				type4 = type4.MakeGenericType(expectedType2);
				type = type4;
			}
			if (type != null && type.IsAssignableFrom(expectedType))
			{
				instanceMethod = Helpers.GetInstanceMethod(type, "GetEnumerator");
				type2 = instanceMethod.ReturnType;
				type3 = type2;
				moveNext = Helpers.GetInstanceMethod(model.MapType(ienumeratorType), "MoveNext");
				current = Helpers.GetGetMethod(Helpers.GetProperty(type3, "Current", nonPublic: false), nonPublic: false, allowInternal: false);
				return instanceMethod;
			}
			type = model.MapType(ienumerableType);
			instanceMethod = Helpers.GetInstanceMethod(type, "GetEnumerator");
			type2 = instanceMethod.ReturnType;
			type3 = type2;
			moveNext = Helpers.GetInstanceMethod(type3, "MoveNext");
			current = Helpers.GetGetMethod(Helpers.GetProperty(type3, "Current", nonPublic: false), nonPublic: false, allowInternal: false);
			return instanceMethod;
		}

		public override void Write(object value, ProtoWriter dest)
		{
			bool writePacked = WritePacked;
			SubItemToken token;
			if (writePacked)
			{
				ProtoWriter.WriteFieldHeader(fieldNumber, WireType.String, dest);
				token = ProtoWriter.StartSubItem(value, dest);
				ProtoWriter.SetPackedField(fieldNumber, dest);
			}
			else
			{
				token = default(SubItemToken);
			}
			bool flag = !SupportNull;
			IEnumerator enumerator = ((IEnumerable)value).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					if (flag && current == null)
					{
						throw new NullReferenceException();
					}
					Tail.Write(current, dest);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			if (writePacked)
			{
				ProtoWriter.EndSubItem(token, dest);
			}
		}

		public override object Read(object value, ProtoReader source)
		{
			int field = source.FieldNumber;
			object obj = value;
			if (value == null)
			{
				value = Activator.CreateInstance(concreteType);
			}
			bool flag = IsList && !SuppressIList;
			if (packedWireType != WireType.None && source.WireType == WireType.String)
			{
				SubItemToken token = ProtoReader.StartSubItem(source);
				if (flag)
				{
					IList list = (IList)value;
					while (ProtoReader.HasSubValue(packedWireType, source))
					{
						list.Add(Tail.Read(null, source));
					}
				}
				else
				{
					object[] array = new object[1];
					while (ProtoReader.HasSubValue(packedWireType, source))
					{
						array[0] = Tail.Read(null, source);
						add.Invoke(value, array);
					}
				}
				ProtoReader.EndSubItem(token, source);
			}
			else if (flag)
			{
				IList list2 = (IList)value;
				do
				{
					list2.Add(Tail.Read(null, source));
				}
				while (source.TryReadFieldHeader(field));
			}
			else
			{
				object[] array2 = new object[1];
				do
				{
					array2[0] = Tail.Read(null, source);
					add.Invoke(value, array2);
				}
				while (source.TryReadFieldHeader(field));
			}
			return (obj != value) ? value : null;
		}
	}
}
