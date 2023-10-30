using System.Xml;

namespace wox.serial
{
	public abstract class ObjectReader : Serial
	{
		public abstract object read(XmlReader reader);
	}
}
