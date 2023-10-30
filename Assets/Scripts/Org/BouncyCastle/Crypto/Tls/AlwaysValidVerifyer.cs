using Org.BouncyCastle.Asn1.X509;
using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	public class AlwaysValidVerifyer : ICertificateVerifyer
	{
		public bool IsValid(Uri targetUri, X509CertificateStructure[] certs)
		{
			return true;
		}
	}
}
