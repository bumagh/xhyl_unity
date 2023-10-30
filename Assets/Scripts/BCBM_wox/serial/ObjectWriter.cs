using System.Xml;

namespace BCBM_wox.serial
{
	public abstract class ObjectWriter : BCBM_Serial
	{
		public abstract void write(object o, XmlTextWriter writer);
	}
}
