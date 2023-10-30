using Org.BouncyCastle.Math.Field;
using System;

namespace Org.BouncyCastle.Math.EC
{
	public abstract class AbstractFpCurve : ECCurve
	{
		protected AbstractFpCurve(BigInteger q)
			: base(FiniteFields.GetPrimeField(q))
		{
		}

		public override bool IsValidFieldElement(BigInteger x)
		{
			return x != null && x.SignValue >= 0 && x.CompareTo(Field.Characteristic) < 0;
		}

		protected override ECPoint DecompressPoint(int yTilde, BigInteger X1)
		{
			ECFieldElement eCFieldElement = FromBigInteger(X1);
			ECFieldElement eCFieldElement2 = eCFieldElement.Square().Add(A).Multiply(eCFieldElement)
				.Add(B);
			ECFieldElement eCFieldElement3 = eCFieldElement2.Sqrt();
			if (eCFieldElement3 == null)
			{
				throw new ArgumentException("Invalid point compression");
			}
			if (eCFieldElement3.TestBitZero() != (yTilde == 1))
			{
				eCFieldElement3 = eCFieldElement3.Negate();
			}
			return CreateRawPoint(eCFieldElement, eCFieldElement3, withCompression: true);
		}
	}
}