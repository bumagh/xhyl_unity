using System.Xml;

namespace LHD_wox.serial
{
	public abstract class ObjectWriter : LHD_Serial
	{
		public abstract void write(object o, XmlTextWriter writer);
	}
}
