using System.Collections.Generic;
using UnityEngine;

namespace FullInspector.Samples.FullSerializer
{
	[AddComponentMenu("Full Inspector Samples/FullSerializer/Inheritance")]
	public class SampleFullSerializerInheritance : BaseBehavior<FullSerializerSerializer>
	{
		public interface IFace
		{
		}

		public class DerivedIFaceA : IFace
		{
			public int A;
		}

		public class DerivedIFaceB : IFace
		{
			public string B;
		}

		public abstract class AbstractType
		{
		}

		public class DerivedAbstractA : AbstractType
		{
			public int A;
		}

		public class DerivedAbstractB : AbstractType
		{
			public string B;
		}

		public abstract class BaseType
		{
		}

		public class DerivedBaseA : BaseType
		{
			public int A;
		}

		public class DerivedBaseB : BaseType
		{
			public string B;
		}

		public IFace Interface;

		public List<IFace> InterfaceList;

		public AbstractType AbstractValue;

		public List<AbstractType> AbstractTypeList;

		public BaseType BaseValue;

		public List<BaseType> BaseTypeList;
	}
}
