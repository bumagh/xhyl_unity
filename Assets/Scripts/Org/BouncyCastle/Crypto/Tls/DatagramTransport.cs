namespace Org.BouncyCastle.Crypto.Tls
{
	public interface DatagramTransport
	{
		int GetReceiveLimit();

		int GetSendLimit();

		int Receive(byte[] buf, int off, int len, int waitMillis);

		void Send(byte[] buf, int off, int len);

		void Close();
	}
}
