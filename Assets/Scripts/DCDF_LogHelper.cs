public static class DCDF_LogHelper
{
	public static string Color_NetHandle = "#00C0F0ff";

	public static string ColorStr(string inner, DCDF_RichColor color)
	{
		return inner;
	}

	public static string ColorStr(string inner, string color)
	{
		return inner;
	}

	public static string Key(object o, DCDF_RichColor color = DCDF_RichColor.aqua)
	{
		return ColorStr(string.Empty + o.ToString(), color);
	}

	public static string Key(object o, string color)
	{
		return ColorStr(string.Empty + o.ToString(), color);
	}

	public static string NetHandle(string str)
	{
		return ColorStr(str, Color_NetHandle);
	}

	public static string Aqua(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), DCDF_RichColor.aqua);
	}

	public static string Aqua(string inner)
	{
		return ColorStr(inner, DCDF_RichColor.aqua);
	}

	public static string Black(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), DCDF_RichColor.black);
	}

	public static string Black(string inner)
	{
		return ColorStr(inner, DCDF_RichColor.black);
	}

	public static string Blue(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), DCDF_RichColor.blue);
	}

	public static string Blue(string inner)
	{
		return ColorStr(inner, DCDF_RichColor.blue);
	}

	public static string Brown(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), DCDF_RichColor.brown);
	}

	public static string Brown(string inner)
	{
		return ColorStr(inner, DCDF_RichColor.brown);
	}

	public static string Cyan(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), DCDF_RichColor.cyan);
	}

	public static string Cyan(string inner)
	{
		return ColorStr(inner, DCDF_RichColor.cyan);
	}

	public static string Green(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), DCDF_RichColor.green);
	}

	public static string Green(string inner)
	{
		return ColorStr(inner, DCDF_RichColor.green);
	}

	public static string Lightblue(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), DCDF_RichColor.lightblue);
	}

	public static string Lightblue(string inner)
	{
		return ColorStr(inner, DCDF_RichColor.lightblue);
	}

	public static string Lime(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), DCDF_RichColor.lime);
	}

	public static string Lime(string inner)
	{
		return ColorStr(inner, DCDF_RichColor.lime);
	}

	public static string Magenta(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), DCDF_RichColor.magenta);
	}

	public static string Magenta(string inner)
	{
		return ColorStr(inner, DCDF_RichColor.magenta);
	}

	public static string Olive(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), DCDF_RichColor.olive);
	}

	public static string Olive(string inner)
	{
		return ColorStr(inner, DCDF_RichColor.olive);
	}

	public static string Orange(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), DCDF_RichColor.orange);
	}

	public static string Orange(string inner)
	{
		return ColorStr(inner, DCDF_RichColor.orange);
	}

	public static string Red(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), DCDF_RichColor.red);
	}

	public static string Red(string inner)
	{
		return ColorStr(inner, DCDF_RichColor.red);
	}

	public static string Silver(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), DCDF_RichColor.silver);
	}

	public static string Silver(string inner)
	{
		return ColorStr(inner, DCDF_RichColor.silver);
	}

	public static string Teal(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), DCDF_RichColor.teal);
	}

	public static string Teal(string inner)
	{
		return ColorStr(inner, DCDF_RichColor.teal);
	}

	public static string White(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), DCDF_RichColor.white);
	}

	public static string White(string inner)
	{
		return ColorStr(inner, DCDF_RichColor.white);
	}

	public static string Yellow(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), DCDF_RichColor.yellow);
	}

	public static string Yellow(string inner)
	{
		return ColorStr(inner, DCDF_RichColor.yellow);
	}
}
