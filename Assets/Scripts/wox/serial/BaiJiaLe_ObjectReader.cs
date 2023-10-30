using System.Xml;

namespace wox.serial
{
	public abstract class BaiJiaLe_ObjectReader : BaiJiaLe_Serial
	{
		public abstract object read(XmlReader reader);
	}
}
