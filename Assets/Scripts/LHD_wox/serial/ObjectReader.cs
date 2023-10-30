using System.Xml;

namespace LHD_wox.serial
{
	public abstract class ObjectReader : LHD_Serial
	{
		public abstract object read(XmlReader reader);
	}
}
