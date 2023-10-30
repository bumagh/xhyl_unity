using System;
using System.Collections;

namespace wox.serial
{
	public class Serial
	{
		public const string OBJECT = "object";

		public const string FIELD = "field";

		public const string NAME = "name";

		public const string TYPE = "type";

		public const string VALUE = "value";

		public const string ARRAY = "array";

		public const string ARRAYLIST = "list";

		public const string ELEMENT_TYPE = "elementType";

		public const string MAP = "map";

		public const string KEY_TYPE = "keyType";

		public const string VALUE_TYPE = "valueType";

		public const string ENTRY = "entry";

		public const string KEY = "key";

		public const string LENGTH = "length";

		public const string ID = "id";

		public const string IDREF = "idref";

		public const string DECLARED = "declaredClass";

		public string[] primitiveArrays = new string[9]
		{
			"System.Int32[]",
			"System.Boolean[]",
			"System.Byte[]",
			"System.Int16[]",
			"System.Int64[]",
			"System.Char[]",
			"System.Single[]",
			"System.Double[]",
			"System.Type[]"
		};

		public string[] primitiveArraysWOX = new string[9]
		{
			"int",
			"boolean",
			"byte",
			"short",
			"long",
			"char",
			"float",
			"double",
			"class"
		};

		public static Type[] primitives;

		public static Hashtable mapWOXToCSharp;

		public static Hashtable mapCSharpToWOX;

		public static Hashtable mapArrayCSharpToWOX;

		public static Hashtable mapArrayWOXToCSharp;

		public static Hashtable mapWrapper;

		static Serial()
		{
			primitives = new Type[10]
			{
				typeof(int),
				typeof(bool),
				typeof(sbyte),
				typeof(short),
				typeof(long),
				typeof(char),
				typeof(float),
				typeof(double),
				typeof(Type),
				typeof(string)
			};
			mapWOXToCSharp = new Hashtable();
			mapCSharpToWOX = new Hashtable();
			mapArrayCSharpToWOX = new Hashtable();
			mapArrayWOXToCSharp = new Hashtable();
			mapWrapper = new Hashtable();
			mapWOXToCSharp.Add("byte", typeof(sbyte));
			mapWOXToCSharp.Add("short", typeof(short));
			mapWOXToCSharp.Add("int", typeof(int));
			mapWOXToCSharp.Add("long", typeof(long));
			mapWOXToCSharp.Add("float", typeof(float));
			mapWOXToCSharp.Add("double", typeof(double));
			mapWOXToCSharp.Add("char", typeof(char));
			mapWOXToCSharp.Add("boolean", typeof(bool));
			mapWOXToCSharp.Add("string", typeof(string));
			mapWOXToCSharp.Add("class", typeof(Type));
			mapWOXToCSharp.Add("byteWrapper", typeof(sbyte));
			mapWOXToCSharp.Add("shortWrapper", typeof(short));
			mapWOXToCSharp.Add("intWrapper", typeof(int));
			mapWOXToCSharp.Add("longWrapper", typeof(long));
			mapWOXToCSharp.Add("floatWrapper", typeof(float));
			mapWOXToCSharp.Add("doubleWrapper", typeof(double));
			mapWOXToCSharp.Add("charWrapper", typeof(char));
			mapWOXToCSharp.Add("booleanWrapper", typeof(bool));
			mapArrayWOXToCSharp.Add("byte[]", typeof(sbyte[]));
			mapArrayWOXToCSharp.Add("byte[][]", typeof(sbyte[][]));
			mapArrayWOXToCSharp.Add("byte[][][]", typeof(sbyte[][][]));
			mapArrayWOXToCSharp.Add("short[]", typeof(short[]));
			mapArrayWOXToCSharp.Add("short[][]", typeof(short[][]));
			mapArrayWOXToCSharp.Add("short[][][]", typeof(short[][][]));
			mapArrayWOXToCSharp.Add("int[]", typeof(int[]));
			mapArrayWOXToCSharp.Add("int[][]", typeof(int[][]));
			mapArrayWOXToCSharp.Add("int[][][]", typeof(int[][][]));
			mapArrayWOXToCSharp.Add("long[]", typeof(long[]));
			mapArrayWOXToCSharp.Add("long[][]", typeof(long[][]));
			mapArrayWOXToCSharp.Add("long[][][]", typeof(long[][][]));
			mapArrayWOXToCSharp.Add("float[]", typeof(float[]));
			mapArrayWOXToCSharp.Add("float[][]", typeof(float[][]));
			mapArrayWOXToCSharp.Add("float[][][]", typeof(float[][][]));
			mapArrayWOXToCSharp.Add("double[]", typeof(double[]));
			mapArrayWOXToCSharp.Add("double[][]", typeof(double[][]));
			mapArrayWOXToCSharp.Add("double[][][]", typeof(double[][][]));
			mapArrayWOXToCSharp.Add("char[]", typeof(char[]));
			mapArrayWOXToCSharp.Add("char[][]", typeof(char[][]));
			mapArrayWOXToCSharp.Add("char[][][]", typeof(char[][][]));
			mapArrayWOXToCSharp.Add("boolean[]", typeof(bool[]));
			mapArrayWOXToCSharp.Add("boolean[][]", typeof(bool[][]));
			mapArrayWOXToCSharp.Add("boolean[][][]", typeof(bool[][][]));
			mapArrayWOXToCSharp.Add("class[]", typeof(Type[]));
			mapArrayWOXToCSharp.Add("class[][]", typeof(Type[][]));
			mapArrayWOXToCSharp.Add("class[][][]", typeof(Type[][][]));
			mapArrayWOXToCSharp.Add("string[]", typeof(string[]));
			mapArrayWOXToCSharp.Add("string[][]", typeof(string[][]));
			mapArrayWOXToCSharp.Add("string[][][]", typeof(string[][][]));
			mapCSharpToWOX.Add("System.SByte", "byte");
			mapCSharpToWOX.Add("System.Int16", "short");
			mapCSharpToWOX.Add("System.Int32", "int");
			mapCSharpToWOX.Add("System.Int64", "long");
			mapCSharpToWOX.Add("System.Single", "float");
			mapCSharpToWOX.Add("System.Double", "double");
			mapCSharpToWOX.Add("System.Char", "char");
			mapCSharpToWOX.Add("System.Boolean", "boolean");
			mapCSharpToWOX.Add("System.String", "string");
			mapCSharpToWOX.Add("System.Type", "class");
			mapCSharpToWOX.Add("System.RuntimeType", "class");
			mapCSharpToWOX.Add("System.Array", "array");
			mapArrayCSharpToWOX.Add("System.SByte[]", "byte[]");
			mapArrayCSharpToWOX.Add("System.SByte[][]", "byte[][]");
			mapArrayCSharpToWOX.Add("System.SByte[][][]", "byte[][][]");
			mapArrayCSharpToWOX.Add("System.Int16[]", "short[]");
			mapArrayCSharpToWOX.Add("System.Int16[][]", "short[][]");
			mapArrayCSharpToWOX.Add("System.Int16[][][]", "short[][][]");
			mapArrayCSharpToWOX.Add("System.Int32[]", "int[]");
			mapArrayCSharpToWOX.Add("System.Int32[][]", "int[][]");
			mapArrayCSharpToWOX.Add("System.Int32[][][]", "int[][][]");
			mapArrayCSharpToWOX.Add("System.Int64[]", "long[]");
			mapArrayCSharpToWOX.Add("System.Int64[][]", "long[][]");
			mapArrayCSharpToWOX.Add("System.Int64[][][]", "long[][][]");
			mapArrayCSharpToWOX.Add("System.Single[]", "float[]");
			mapArrayCSharpToWOX.Add("System.Single[][]", "float[][]");
			mapArrayCSharpToWOX.Add("System.Single[][][]", "float[][][]");
			mapArrayCSharpToWOX.Add("System.Double[]", "double[]");
			mapArrayCSharpToWOX.Add("System.Double[][]", "double[][]");
			mapArrayCSharpToWOX.Add("System.Double[][][]", "double[][][]");
			mapArrayCSharpToWOX.Add("System.Char[]", "char[]");
			mapArrayCSharpToWOX.Add("System.Char[][]", "char[][]");
			mapArrayCSharpToWOX.Add("System.Char[][][]", "char[][][]");
			mapArrayCSharpToWOX.Add("System.Boolean[]", "boolean[]");
			mapArrayCSharpToWOX.Add("System.Boolean[][]", "boolean[][]");
			mapArrayCSharpToWOX.Add("System.Boolean[][][]", "boolean[][][]");
			mapArrayCSharpToWOX.Add("System.Type[]", "class[]");
			mapArrayCSharpToWOX.Add("System.Type[][]", "class[][]");
			mapArrayCSharpToWOX.Add("System.Type[][][]", "class[][][]");
			mapArrayCSharpToWOX.Add("System.String[]", "string[]");
			mapArrayCSharpToWOX.Add("System.String[][]", "string[][]");
			mapArrayCSharpToWOX.Add("System.String[][][]", "string[][][]");
			mapWrapper.Add("byte", "byteWrapper");
			mapWrapper.Add("short", "shortWrapper");
			mapWrapper.Add("int", "intWrapper");
			mapWrapper.Add("long", "longWrapper");
			mapWrapper.Add("float", "floatWrapper");
			mapWrapper.Add("double", "doubleWrapper");
			mapWrapper.Add("char", "charWrapper");
			mapWrapper.Add("boolean", "booleanWrapper");
		}
	}
}
