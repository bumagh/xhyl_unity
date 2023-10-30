using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace wox.serial
{
	public class BaiJiaLe_SimpleReader : BaiJiaLe_ObjectReader
	{
		private Hashtable map;

		public BaiJiaLe_SimpleReader()
		{
			map = new Hashtable();
		}

		public override object read(XmlReader xob)
		{
			if (empty(xob))
			{
				return null;
			}
			if (reference(xob))
			{
				return map[xob.GetAttribute("idref")];
			}
			string attribute = xob.GetAttribute("id");
			if (isPrimitiveArray(xob))
			{
				return readPrimitiveArray(xob, attribute);
			}
			if (isObjectArray(xob))
			{
				return readObjectArray(xob, attribute);
			}
			if (isArrayList(xob))
			{
				return readArrayList(xob, attribute);
			}
			if (isMap(xob))
			{
				return readMap(xob, attribute);
			}
			if (BaiJiaLe_Util.stringable(xob.GetAttribute("type")))
			{
				return readStringObject(xob, attribute);
			}
			return readObject(xob, attribute);
		}

		public bool empty(XmlReader xob)
		{
			return !xob.HasAttributes && xob.IsEmptyElement;
		}

		public bool reference(XmlReader xob)
		{
			return xob.GetAttribute("idref") != null;
		}

		public bool isPrimitiveArray(XmlReader xob)
		{
			if (!xob.GetAttribute("type").Equals("array"))
			{
				return false;
			}
			string attribute = xob.GetAttribute("elementType");
			for (int i = 0; i < primitiveArraysWOX.Length; i++)
			{
				if (primitiveArraysWOX[i].Equals(attribute))
				{
					return true;
				}
			}
			return false;
		}

		public bool isObjectArray(XmlReader xob)
		{
			return xob.GetAttribute("type").Equals("array");
		}

		public bool isArrayList(XmlReader xob)
		{
			return xob.GetAttribute("type").Equals("list");
		}

		public bool isMap(XmlReader xob)
		{
			return xob.GetAttribute("type").Equals("map");
		}

		public object readPrimitiveArray(XmlReader xob, object id)
		{
			try
			{
				Type type = (Type)BaiJiaLe_Serial.mapWOXToCSharp[xob.GetAttribute("elementType")];
				int length = int.Parse(xob.GetAttribute("length"));
				Array array = Array.CreateInstance(type, length);
				string text = xob.ReadString();
				char[] separator = new char[1]
				{
					' '
				};
				string[] s = text.Split(separator);
				if (type.Equals(typeof(sbyte)))
				{
					map.Add(id, array);
					return array;
				}
				if (type.Equals(typeof(short)))
				{
					object obj = readShortArray((short[])array, s);
					map.Add(id, obj);
					return obj;
				}
				if (type.Equals(typeof(int)))
				{
					object obj2 = readIntArray((int[])array, s);
					map.Add(id, obj2);
					return obj2;
				}
				if (type.Equals(typeof(long)))
				{
					object obj3 = readLongArray((long[])array, s);
					map.Add(id, obj3);
					return obj3;
				}
				if (type.Equals(typeof(float)))
				{
					object obj4 = readFloatArray((float[])array, s);
					map.Add(id, obj4);
					return obj4;
				}
				if (type.Equals(typeof(double)))
				{
					object obj5 = readDoubleArray((double[])array, s);
					map.Add(id, obj5);
					return obj5;
				}
				if (type.Equals(typeof(char)))
				{
					object obj6 = readCharArray((char[])array, s);
					map.Add(id, obj6);
					return obj6;
				}
				if (type.Equals(typeof(bool)))
				{
					object obj7 = readBooleanArray((bool[])array, s);
					map.Add(id, obj7);
					return obj7;
				}
				if (type.Equals(typeof(Type)))
				{
					object obj8 = readClassArray((Type[])array, s);
					map.Add(id, obj8);
					return obj8;
				}
				return null;
			}
			catch (Exception ex)
			{
				Console.Out.WriteLine("The exception is: " + ex.Message);
			}
			return string.Empty;
		}

		public object readShortArray(short[] a, string[] s)
		{
			int num = 0;
			for (int i = 0; i < a.Length; i++)
			{
				a[num++] = short.Parse(s[i]);
			}
			return a;
		}

		public object readIntArray(int[] a, string[] s)
		{
			int num = 0;
			for (int i = 0; i < a.Length; i++)
			{
				a[num++] = int.Parse(s[i]);
			}
			return a;
		}

		public object readLongArray(long[] a, string[] s)
		{
			int num = 0;
			for (int i = 0; i < a.Length; i++)
			{
				a[num++] = long.Parse(s[i]);
			}
			return a;
		}

		public object readFloatArray(float[] a, string[] s)
		{
			int num = 0;
			for (int i = 0; i < a.Length; i++)
			{
				a[num++] = float.Parse(s[i]);
			}
			return a;
		}

		public object readDoubleArray(double[] a, string[] s)
		{
			int num = 0;
			for (int i = 0; i < a.Length; i++)
			{
				a[num++] = double.Parse(s[i]);
			}
			return a;
		}

		public object readCharArray(char[] a, string[] s)
		{
			int num = 0;
			for (int i = 0; i < a.Length; i++)
			{
				int decimalValue = getDecimalValue(s[i]);
				a[num++] = (char)decimalValue;
			}
			return a;
		}

		public object readBooleanArray(bool[] a, string[] s)
		{
			int num = 0;
			for (int i = 0; i < a.Length; i++)
			{
				a[num++] = bool.Parse(s[i]);
			}
			return a;
		}

		private static int getDecimalValue(string unicodeValue)
		{
			string hexString = unicodeValue.Substring(2, 4);
			return HexToInt(hexString);
		}

		private static int HexToInt(string hexString)
		{
			return int.Parse(hexString, NumberStyles.HexNumber, null);
		}

		public object readClassArray(Type[] a, string[] s)
		{
			int num = 0;
			for (int i = 0; i < a.Length; i++)
			{
				if (s[i].Equals("null"))
				{
					a[num++] = null;
					continue;
				}
				Type type = (Type)BaiJiaLe_Serial.mapWOXToCSharp[s[i]];
				if (type == null)
				{
					type = (Type)BaiJiaLe_Serial.mapArrayWOXToCSharp[s[i]];
					if (type == null)
					{
						try
						{
							Type type2 = Type.GetType(s[i]);
							if (type2 == null)
							{
								Assembly entryAssembly = Assembly.GetEntryAssembly();
								type2 = entryAssembly.GetType(s[i]);
							}
							a[num++] = type2;
						}
						catch (Exception ex)
						{
							Console.Out.WriteLine(ex.Message);
						}
					}
					else
					{
						a[num++] = type;
					}
				}
				else
				{
					a[num++] = type;
				}
			}
			return a;
		}

		public object readMap(XmlReader xob, object id)
		{
			Hashtable hashtable = new Hashtable();
			XmlReader xmlReader = xob.ReadSubtree();
			xmlReader.Read();
			while (xmlReader.Read())
			{
				if (xmlReader.NodeType == XmlNodeType.Element)
				{
					xmlReader.Read();
					object key = read(xmlReader);
					xmlReader.Read();
					object value = read(xmlReader);
					hashtable.Add(key, value);
				}
			}
			map.Add(id, hashtable);
			return hashtable;
		}

		public object readArrayList(XmlReader xob, object id)
		{
			Array array = (Array)readObjectArrayGeneric(xob, id);
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < array.GetLength(0); i++)
			{
				arrayList.Add(array.GetValue(i));
			}
			map.Add(id, arrayList);
			return arrayList;
		}

		public object readObjectArray(XmlReader xob, object id)
		{
			object obj = readObjectArrayGeneric(xob, id);
			map.Add(id, obj);
			return obj;
		}

		public object readObjectArrayGeneric(XmlReader xob, object id)
		{
			try
			{
				string attribute = xob.GetAttribute("elementType");
				int length = int.Parse(xob.GetAttribute("length"));
				Type objectArrayComponentType = getObjectArrayComponentType(attribute);
				Array array = Array.CreateInstance(objectArrayComponentType, length);
				XmlReader xmlReader = xob.ReadSubtree();
				xmlReader.Read();
				int num = 0;
				while (xmlReader.Read())
				{
					if (xmlReader.NodeType == XmlNodeType.Element)
					{
						object value = read(xmlReader);
						array.SetValue(value, num++);
					}
				}
				return array;
			}
			catch (Exception ex)
			{
				Console.Out.WriteLine("Exception is: " + ex.Message);
				return null;
			}
		}

		public Type getObjectArrayComponentType(string arrayTypeName)
		{
			Type type = (Type)BaiJiaLe_Serial.mapWOXToCSharp[arrayTypeName];
			if (type != null)
			{
				return type;
			}
			type = (Type)BaiJiaLe_Serial.mapArrayWOXToCSharp[arrayTypeName];
			if (type == null)
			{
				if (arrayTypeName.Equals("Object"))
				{
					arrayTypeName = "System.Object";
				}
				Type type2 = Type.GetType(arrayTypeName);
				if (type2 == null)
				{
					Assembly entryAssembly = Assembly.GetEntryAssembly();
					type2 = entryAssembly.GetType(arrayTypeName);
				}
				return type2;
			}
			return type;
		}

		public object readStringObject(XmlReader xob, object id)
		{
			try
			{
				Type type = (Type)BaiJiaLe_Serial.mapWOXToCSharp[xob.GetAttribute("type")];
				if (type.Equals(typeof(Type)))
				{
					Type type2 = (Type)BaiJiaLe_Serial.mapWOXToCSharp[xob.GetAttribute("value")];
					if (type2 == null)
					{
						type2 = (Type)BaiJiaLe_Serial.mapArrayWOXToCSharp[xob.GetAttribute("value")];
						if (type2 == null)
						{
							object type3 = Type.GetType(xob.GetAttribute("value"));
							if (type3 == null)
							{
								Assembly entryAssembly = Assembly.GetEntryAssembly();
								type3 = entryAssembly.GetType(xob.GetAttribute("value"));
							}
							map.Add(id, type3);
							return type3;
						}
						map.Add(id, type2);
						return type2;
					}
					map.Add(id, type2);
					return type2;
				}
				if (type.Equals(typeof(char)))
				{
					int decimalValue = getDecimalValue(xob.GetAttribute("value"));
					char c = (char)decimalValue;
					return c;
				}
				if (type.Equals(typeof(sbyte)))
				{
					return sbyte.Parse(xob.GetAttribute("value"));
				}
				if (type.Equals(typeof(short)))
				{
					return short.Parse(xob.GetAttribute("value"));
				}
				if (type.Equals(typeof(int)))
				{
					return int.Parse(xob.GetAttribute("value"));
				}
				if (type.Equals(typeof(long)))
				{
					return long.Parse(xob.GetAttribute("value"));
				}
				if (type.Equals(typeof(float)))
				{
					return float.Parse(xob.GetAttribute("value"));
				}
				if (type.Equals(typeof(double)))
				{
					return double.Parse(xob.GetAttribute("value"));
				}
				if (type.Equals(typeof(bool)))
				{
					return bool.Parse(xob.GetAttribute("value"));
				}
				if (type.Equals(typeof(string)))
				{
					return xob.GetAttribute("value");
				}
				return null;
			}
			catch (Exception ex)
			{
				Console.Out.WriteLine(ex.Message);
				return null;
			}
		}

		public object readObject(XmlReader xob, object id)
		{
			try
			{
				Type type = Type.GetType(xob.GetAttribute("type"));
				if (type == null)
				{
					Assembly entryAssembly = Assembly.GetEntryAssembly();
					type = entryAssembly.GetType(xob.GetAttribute("type"));
				}
				ConstructorInfo cons = BaiJiaLe_Util.forceDefaultConstructor(type);
				object obj = makeObject(cons, new object[0], id);
				setFields(obj, xob);
				return obj;
			}
			catch (Exception ex)
			{
				Console.Out.WriteLine(ex.Message);
				return null;
			}
		}

		public object makeObject(ConstructorInfo cons, object[] args, object key)
		{
			object obj = cons.Invoke(args);
			map.Add(key, obj);
			return obj;
		}

		public FieldInfo getField(Type typeObject, string name)
		{
			if (typeObject == null)
			{
				return null;
			}
			try
			{
				FieldInfo[] fields = typeObject.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				FieldInfo result = null;
				for (int i = 0; i < fields.Length; i++)
				{
					if (fields[i].Name.Equals(name))
					{
						result = fields[i];
						break;
					}
				}
				return result;
			}
			catch (Exception ex)
			{
				Console.Out.WriteLine(ex.Message);
				return null;
			}
		}

		public void setFields(object ob, XmlReader xob)
		{
			Type type = ob.GetType();
			XmlReader xmlReader = xob.ReadSubtree();
			xmlReader.Read();
			while (xmlReader.Read())
			{
				if (xmlReader.NodeType == XmlNodeType.Element)
				{
					string attribute = xmlReader.GetAttribute("name");
					try
					{
						Type typeObject = type;
						FieldInfo field = getField(typeObject, attribute);
						object obj;
						if (BaiJiaLe_Util.primitive(field.FieldType))
						{
							string attribute2 = xmlReader.GetAttribute("type");
							if (attribute2.Equals("char"))
							{
								int decimalValue = getDecimalValue(xmlReader.GetAttribute("value"));
								char c = (char)decimalValue;
								obj = c;
							}
							else if (attribute2.Equals("byte"))
							{
								obj = sbyte.Parse(xmlReader.GetAttribute("value"));
							}
							else if (attribute2.Equals("short"))
							{
								obj = short.Parse(xmlReader.GetAttribute("value"));
							}
							else if (attribute2.Equals("int"))
							{
								obj = int.Parse(xmlReader.GetAttribute("value"));
							}
							else if (attribute2.Equals("long"))
							{
								obj = long.Parse(xmlReader.GetAttribute("value"));
							}
							else if (attribute2.Equals("float"))
							{
								obj = float.Parse(xmlReader.GetAttribute("value"));
							}
							else if (attribute2.Equals("double"))
							{
								obj = double.Parse(xmlReader.GetAttribute("value"));
							}
							else if (attribute2.Equals("boolean"))
							{
								obj = bool.Parse(xmlReader.GetAttribute("value"));
							}
							else if (attribute2.Equals("string"))
							{
								obj = xmlReader.GetAttribute("value");
							}
							else if (attribute2.Equals("class"))
							{
								obj = Type.GetType(xmlReader.GetAttribute("value"));
								if (obj == null)
								{
									Assembly entryAssembly = Assembly.GetEntryAssembly();
									obj = entryAssembly.GetType(xmlReader.GetAttribute("value"));
								}
							}
							else
							{
								obj = null;
							}
						}
						else
						{
							xmlReader.Read();
							XmlReader xmlReader2 = xmlReader.ReadSubtree();
							xmlReader2.Read();
							obj = read(xmlReader2);
						}
						field.SetValue(ob, obj);
					}
					catch (Exception)
					{
					}
				}
			}
		}
	}
}
