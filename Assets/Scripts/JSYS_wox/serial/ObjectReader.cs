using System.Xml;

namespace JSYS_wox.serial
{
	public abstract class ObjectReader : JSYS_Serial
	{
		public abstract object read(XmlReader reader);
	}
}
