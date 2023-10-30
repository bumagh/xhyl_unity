using System.Xml;

namespace BCBM_wox.serial
{
	public abstract class ObjectReader : BCBM_Serial
	{
		public abstract object read(XmlReader reader);
	}
}
