using System.Xml;

namespace wox.serial
{
	public abstract class ObjectWriter : Serial
	{
		public abstract void write(object o, XmlTextWriter writer);
	}
}
