using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;

namespace BCBM_wox.serial
{
	public class SimpleWriter : ObjectWriter
	{
		private Hashtable map;

		private int count;

		private bool writePrimitiveTypes = true;

		private bool doStatic = true;

		private bool doFinal;

		public SimpleWriter()
		{
			map = new Hashtable();
			count = 0;
		}

		public override void write(object ob, XmlTextWriter el)
		{
			if (ob == null)
			{
				el.WriteElementString("object", null);
				return;
			}
			if (map.ContainsKey(ob))
			{
				el.WriteStartElement("object");
				string value = (string)BCBM_Serial.mapCSharpToWOX[ob.GetType().ToString()];
				el.WriteAttributeString("type", value);
				el.WriteAttributeString("value", stringify(ob));
				el.WriteAttributeString("id", map[ob].ToString());
				el.WriteEndElement();
				return;
			}
			map.Add(ob, count++);
			if (Util.stringable(ob))
			{
				el.WriteStartElement("object");
				string value2 = (string)BCBM_Serial.mapCSharpToWOX[ob.GetType().ToString()];
				el.WriteAttributeString("type", value2);
				el.WriteAttributeString("value", stringify(ob));
				el.WriteAttributeString("id", map[ob].ToString());
				el.WriteEndElement();
			}
			else if (ob is Array)
			{
				writeArray(ob, el);
			}
			else if (ob is ArrayList)
			{
				writeArrayList(ob, el);
			}
			else if (ob is Hashtable)
			{
				writeHashtable(ob, el);
			}
			else
			{
				el.WriteStartElement("object");
				el.WriteAttributeString("type", ob.GetType().ToString());
				el.WriteAttributeString("id", map[ob].ToString());
				writeFields(ob, el);
				el.WriteEndElement();
			}
		}

		public void writeHashtable(object ob, XmlTextWriter el)
		{
			el.WriteStartElement("object");
			el.WriteAttributeString("type", "map");
			el.WriteAttributeString("id", map[ob].ToString());
			Hashtable hashtable = (Hashtable)ob;
			IDictionaryEnumerator enumerator = hashtable.GetEnumerator();
			while (enumerator.MoveNext())
			{
				writeMapEntry(enumerator.Entry, el);
			}
			el.WriteEndElement();
		}

		public void writeMapEntry(object ob, XmlTextWriter el)
		{
			el.WriteStartElement("object");
			el.WriteAttributeString("type", "entry");
			DictionaryEntry dictionaryEntry = (DictionaryEntry)ob;
			writeMapEntryKey(dictionaryEntry.Key, el);
			writeMapEntryKey(dictionaryEntry.Value, el);
			el.WriteEndElement();
		}

		public void writeMapEntryKey(object ob, XmlTextWriter el)
		{
			write(ob, el);
		}

		public void writeMapEntryValue(object ob, XmlTextWriter el)
		{
			write(ob, el);
		}

		public void writeArrayList(object ob, XmlTextWriter el)
		{
			el.WriteStartElement("object");
			el.WriteAttributeString("type", "list");
			ArrayList arrayList = (ArrayList)ob;
			object ob2 = arrayList.ToArray();
			writeObjectArrayGeneric(ob, ob2, el);
		}

		public void writeArray(object ob, XmlTextWriter el)
		{
			if (isPrimitiveArray(ob.GetType().ToString()))
			{
				writePrimitiveArray(ob, el);
			}
			else
			{
				writeObjectArray(ob, el);
			}
		}

		public void writeObjectArray(object ob, XmlTextWriter el)
		{
			el.WriteStartElement("object");
			el.WriteAttributeString("type", "array");
			writeObjectArrayGeneric(ob, ob, el);
		}

		public void writeObjectArrayGeneric(object obStored, object ob, XmlTextWriter el)
		{
			string text = (string)BCBM_Serial.mapCSharpToWOX[ob.GetType().GetElementType().ToString()];
			if (text == null)
			{
				text = (string)BCBM_Serial.mapArrayCSharpToWOX[ob.GetType().GetElementType().ToString()];
				if (text != null)
				{
					el.WriteAttributeString("elementType", text);
				}
				else if (ob.GetType().GetElementType().ToString()
					.Equals("System.Object"))
				{
					el.WriteAttributeString("elementType", "Object");
				}
				else
				{
					el.WriteAttributeString("elementType", ob.GetType().GetElementType().ToString());
				}
			}
			else
			{
				el.WriteAttributeString("elementType", text);
			}
			Array array = (Array)ob;
			int length = array.GetLength(0);
			el.WriteAttributeString("length", string.Empty + length);
			el.WriteAttributeString("id", map[obStored].ToString());
			for (int i = 0; i < length; i++)
			{
				write(array.GetValue(i), el);
			}
			el.WriteEndElement();
		}

		public void writePrimitiveArray(object ob, XmlTextWriter el)
		{
			el.WriteStartElement("object");
			el.WriteAttributeString("type", "array");
			string value = (string)BCBM_Serial.mapCSharpToWOX[ob.GetType().GetElementType().ToString()];
			el.WriteAttributeString("elementType", value);
			Array array = (Array)ob;
			int length = array.GetLength(0);
			if (!(array is sbyte[]))
			{
				el.WriteAttributeString("length", string.Empty + length);
				el.WriteAttributeString("id", map[ob].ToString());
				el.WriteString(arrayString(array, length));
				el.WriteEndElement();
			}
		}

		public string arrayString(Array obArray, int len)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < len; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(" ");
				}
				if (obArray is Type[])
				{
					Type type = (Type)obArray.GetValue(i);
					if (type != null)
					{
						string text = (string)BCBM_Serial.mapCSharpToWOX[type.ToString()];
						if (text != null)
						{
							stringBuilder.Append(text);
							continue;
						}
						text = (string)BCBM_Serial.mapArrayCSharpToWOX[type.ToString()];
						if (text != null)
						{
							stringBuilder.Append(text);
						}
						else
						{
							stringBuilder.Append(type.ToString());
						}
					}
					else
					{
						stringBuilder.Append("null");
					}
				}
				else if (obArray is char[])
				{
					object value = obArray.GetValue(i);
					if (value != null)
					{
						char character = (char)obArray.GetValue(i);
						stringBuilder.Append(getUnicodeValue(character));
					}
					else
					{
						stringBuilder.Append("null");
					}
				}
				else if (obArray is bool[])
				{
					object value2 = obArray.GetValue(i);
					string value3 = string.Empty;
					if (value2 != null)
					{
						bool flag = (bool)obArray.GetValue(i);
						if (flag.ToString().Equals("True"))
						{
							value3 = "true";
						}
						else if (flag.ToString().Equals("False"))
						{
							value3 = "false";
						}
						stringBuilder.Append(value3);
					}
					else
					{
						stringBuilder.Append("null");
					}
				}
				else
				{
					object value4 = obArray.GetValue(i);
					if (value4 != null)
					{
						stringBuilder.Append(value4.ToString());
					}
					else
					{
						stringBuilder.Append("null");
					}
				}
			}
			return stringBuilder.ToString();
		}

		public void writeFields(object o, XmlTextWriter parent)
		{
			Type type = o.GetType();
			FieldInfo[] fields = getFields(type);
			for (int i = 0; i < fields.Length; i++)
			{
				try
				{
					string name = fields[i].Name;
					object obj = fields[i].GetValue(o);
					parent.WriteStartElement("field");
					parent.WriteAttributeString("name", name);
					if (BCBM_Serial.mapCSharpToWOX.ContainsKey(fields[i].FieldType.ToString()))
					{
						if (writePrimitiveTypes)
						{
							parent.WriteAttributeString("type", (string)BCBM_Serial.mapCSharpToWOX[fields[i].FieldType.ToString()]);
						}
						if (fields[i].FieldType.ToString().Equals("System.Char"))
						{
							char character = (char)obj;
							string unicodeValue = getUnicodeValue(character);
							parent.WriteAttributeString("value", unicodeValue);
							parent.WriteEndElement();
						}
						else if (fields[i].FieldType.ToString().Equals("System.Boolean"))
						{
							string value = string.Empty;
							if (obj == null)
							{
								value = string.Empty;
							}
							if (obj.ToString().Equals("True"))
							{
								value = "true";
							}
							else if (obj.ToString().Equals("False"))
							{
								value = "false";
							}
							parent.WriteAttributeString("value", value);
							parent.WriteEndElement();
						}
						else
						{
							if (obj == null)
							{
								obj = string.Empty;
							}
							parent.WriteAttributeString("value", obj.ToString());
							parent.WriteEndElement();
						}
					}
					else
					{
						write(obj, parent);
						parent.WriteEndElement();
					}
				}
				catch (Exception ex)
				{
					Console.Out.WriteLine("error: " + ex.Message);
				}
			}
		}

		private static string getUnicodeValue(char character)
		{
			string hexValue = IntToHex(character);
			return "\\u" + fillWithZeros(hexValue);
		}

		private static string fillWithZeros(string hexValue)
		{
			switch (hexValue.Length)
			{
			case 1:
				return "000" + hexValue;
			case 2:
				return "00" + hexValue;
			case 3:
				return "0" + hexValue;
			default:
				return hexValue;
			}
		}

		private static string IntToHex(int number)
		{
			return $"{number:x}";
		}

		private static int HexToInt(string hexString)
		{
			return int.Parse(hexString, NumberStyles.HexNumber, null);
		}

		public static string stringify(object ob)
		{
			if (ob is Type)
			{
				Type type = (Type)ob;
				string text = (string)BCBM_Serial.mapCSharpToWOX[type.ToString()];
				if (text != null)
				{
					return text;
				}
				return type.ToString();
			}
			if (ob is char)
			{
				return getUnicodeValue((char)ob);
			}
			if (!(ob is bool))
			{
				return ob.ToString();
			}
			if (ob.ToString().Equals("True"))
			{
				return "true";
			}
			return "false";
		}

		public static FieldInfo[] getFields(Type c)
		{
			ArrayList arrayList = new ArrayList();
			while (c != null)
			{
				FieldInfo[] fields = c.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				for (int i = 0; i < fields.Length; i++)
				{
					arrayList.Add(fields[i]);
				}
				c = null;
			}
			FieldInfo[] array = new FieldInfo[arrayList.Count];
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = (FieldInfo)arrayList[j];
			}
			return array;
		}

		public bool isPrimitiveArray(string c)
		{
			for (int i = 0; i < primitiveArrays.Length; i++)
			{
				if (c.Equals(primitiveArrays[i]))
				{
					return true;
				}
			}
			return false;
		}
	}
}
