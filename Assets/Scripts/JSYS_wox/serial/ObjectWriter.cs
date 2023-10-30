using System.Xml;

namespace JSYS_wox.serial
{
	public abstract class ObjectWriter : JSYS_Serial
	{
		public abstract void write(object o, XmlTextWriter writer);
	}
}
