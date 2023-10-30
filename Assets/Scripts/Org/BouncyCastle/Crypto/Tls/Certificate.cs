using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	public class Certificate
	{
		public static readonly Certificate EmptyChain = new Certificate(new X509CertificateStructure[0]);

		protected readonly X509CertificateStructure[] mCertificateList;

		public virtual int Length => mCertificateList.Length;

		public virtual bool IsEmpty => mCertificateList.Length == 0;

		public Certificate(X509CertificateStructure[] certificateList)
		{
			if (certificateList == null)
			{
				throw new ArgumentNullException("certificateList");
			}
			mCertificateList = certificateList;
		}

		public virtual X509CertificateStructure[] GetCertificateList()
		{
			return CloneCertificateList();
		}

		public virtual X509CertificateStructure GetCertificateAt(int index)
		{
			return mCertificateList[index];
		}

		public virtual void Encode(Stream output)
		{
			IList list = Platform.CreateArrayList(mCertificateList.Length);
			int num = 0;
			X509CertificateStructure[] array = mCertificateList;
			foreach (Asn1Encodable asn1Encodable in array)
			{
				byte[] encoded = asn1Encodable.GetEncoded("DER");
				list.Add(encoded);
				num += encoded.Length + 3;
			}
			TlsUtilities.CheckUint24(num);
			TlsUtilities.WriteUint24(num, output);
			IEnumerator enumerator = list.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					byte[] buf = (byte[])enumerator.Current;
					TlsUtilities.WriteOpaque24(buf, output);
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

		public static Certificate Parse(Stream input)
		{
			int num = TlsUtilities.ReadUint24(input);
			if (num == 0)
			{
				return EmptyChain;
			}
			byte[] buffer = TlsUtilities.ReadFully(num, input);
			MemoryStream memoryStream = new MemoryStream(buffer, writable: false);
			IList list = Platform.CreateArrayList();
			while (memoryStream.Position < memoryStream.Length)
			{
				byte[] encoding = TlsUtilities.ReadOpaque24(memoryStream);
				Asn1Object obj = TlsUtilities.ReadDerObject(encoding);
				list.Add(X509CertificateStructure.GetInstance(obj));
			}
			X509CertificateStructure[] array = new X509CertificateStructure[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				array[i] = (X509CertificateStructure)list[i];
			}
			return new Certificate(array);
		}

		protected virtual X509CertificateStructure[] CloneCertificateList()
		{
			return (X509CertificateStructure[])mCertificateList.Clone();
		}
	}
}
