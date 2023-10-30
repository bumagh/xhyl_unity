using System.Xml;

namespace wox.serial
{
	public abstract class BaiJiaLe_ObjectWriter : BaiJiaLe_Serial
	{
		public abstract void write(object o, XmlTextWriter writer);
	}
}
