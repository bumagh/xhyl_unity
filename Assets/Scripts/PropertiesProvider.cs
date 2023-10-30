using FullSerializer;
using System;
using System.Collections.Generic;

public class PropertiesProvider : TestProvider<object>
{
	public struct PublicGetPublicSet
	{
		public int Value
		{
			get;
			set;
		}

		public PublicGetPublicSet(int value)
		{
			this = default(PublicGetPublicSet);
			Value = value;
		}
	}

	public struct PrivateGetPublicSet
	{
		[fsProperty]
		public int Value
		{
			private get;
			set;
		}

		public PrivateGetPublicSet(int value)
		{
			this = default(PrivateGetPublicSet);
			Value = value;
		}

		public static bool Compare(PrivateGetPublicSet a, PrivateGetPublicSet b)
		{
			return a.Value == b.Value;
		}
	}

	public struct PublicGetPrivateSet
	{
		public int Value
		{
			get;
			private set;
		}

		public PublicGetPrivateSet(int value)
		{
			this = default(PublicGetPrivateSet);
			Value = value;
		}
	}

	public struct PrivateGetPrivateSet
	{
		private int Value
		{
			get;
			set;
		}

		public PrivateGetPrivateSet(int value)
		{
			this = default(PrivateGetPrivateSet);
			Value = value;
		}

		public bool Verify()
		{
			if (Value != 0)
			{
				throw new Exception("Private autoproperty was deserialized");
			}
			return true;
		}
	}

	public override bool Compare(object before, object after)
	{
		if (before is PublicGetPublicSet)
		{
			PublicGetPublicSet publicGetPublicSet = (PublicGetPublicSet)before;
			PublicGetPublicSet publicGetPublicSet2 = (PublicGetPublicSet)after;
			return publicGetPublicSet.Value == publicGetPublicSet2.Value;
		}
		if (before is PrivateGetPublicSet)
		{
			PrivateGetPublicSet a = (PrivateGetPublicSet)before;
			PrivateGetPublicSet b = (PrivateGetPublicSet)after;
			return PrivateGetPublicSet.Compare(a, b);
		}
		if (before is PublicGetPrivateSet)
		{
			PublicGetPrivateSet publicGetPrivateSet = (PublicGetPrivateSet)before;
			PublicGetPrivateSet publicGetPrivateSet2 = (PublicGetPrivateSet)after;
			return publicGetPrivateSet.Value == publicGetPrivateSet2.Value;
		}
		if (after is PrivateGetPrivateSet)
		{
			return ((PrivateGetPrivateSet)after).Verify();
		}
		throw new Exception("Unknown type");
	}

	public override IEnumerable<object> GetValues()
	{
		for (int i = -1; i <= 1; i++)
		{
			yield return new PublicGetPublicSet(i);
			yield return new PrivateGetPublicSet(i);
			yield return new PublicGetPrivateSet(i);
			yield return new PrivateGetPrivateSet(i);
		}
	}
}
