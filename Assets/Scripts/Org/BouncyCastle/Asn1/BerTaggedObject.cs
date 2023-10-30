using Org.BouncyCastle.Utilities;
using System;
using System.Collections;

namespace Org.BouncyCastle.Asn1
{
	public class BerTaggedObject : DerTaggedObject
	{
		public BerTaggedObject(int tagNo, Asn1Encodable obj)
			: base(tagNo, obj)
		{
		}

		public BerTaggedObject(bool explicitly, int tagNo, Asn1Encodable obj)
			: base(explicitly, tagNo, obj)
		{
		}

		public BerTaggedObject(int tagNo)
			: base(explicitly: false, tagNo, BerSequence.Empty)
		{
		}

		internal override void Encode(DerOutputStream derOut)
		{
			if (derOut is Asn1OutputStream || derOut is BerOutputStream)
			{
				derOut.WriteTag(160, tagNo);
				derOut.WriteByte(128);
				if (!IsEmpty())
				{
					if (!explicitly)
					{
						IEnumerable enumerable;
						if (base.obj is Asn1OctetString)
						{
							if (base.obj is BerOctetString)
							{
								enumerable = (BerOctetString)base.obj;
							}
							else
							{
								Asn1OctetString asn1OctetString = (Asn1OctetString)base.obj;
								enumerable = new BerOctetString(asn1OctetString.GetOctets());
							}
						}
						else if (base.obj is Asn1Sequence)
						{
							enumerable = (Asn1Sequence)base.obj;
						}
						else
						{
							if (!(base.obj is Asn1Set))
							{
								throw Platform.CreateNotImplementedException(Platform.GetTypeName(base.obj));
							}
							enumerable = (Asn1Set)base.obj;
						}
						IEnumerator enumerator = enumerable.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								Asn1Encodable obj = (Asn1Encodable)enumerator.Current;
								derOut.WriteObject(obj);
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
					}
					else
					{
						derOut.WriteObject(base.obj);
					}
				}
				derOut.WriteByte(0);
				derOut.WriteByte(0);
			}
			else
			{
				base.Encode(derOut);
			}
		}
	}
}
