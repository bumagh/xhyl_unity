public static class LogHelper
{
	public static string Color_NetHandle = "#00C0F0ff";

	public static string ColorStr(string inner, RichColor color)
	{
		return inner;
	}

	public static string ColorStr(string inner, string color)
	{
		return inner;
	}

	public static string Key(object o, RichColor color = RichColor.aqua)
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
		return ColorStr(string.Format(format, args), RichColor.aqua);
	}

	public static string Aqua(string inner)
	{
		return ColorStr(inner, RichColor.aqua);
	}

	public static string Black(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), RichColor.black);
	}

	public static string Black(string inner)
	{
		return ColorStr(inner, RichColor.black);
	}

	public static string Blue(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), RichColor.blue);
	}

	public static string Blue(string inner)
	{
		return ColorStr(inner, RichColor.blue);
	}

	public static string Brown(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), RichColor.brown);
	}

	public static string Brown(string inner)
	{
		return ColorStr(inner, RichColor.brown);
	}

	public static string Cyan(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), RichColor.cyan);
	}

	public static string Cyan(string inner)
	{
		return ColorStr(inner, RichColor.cyan);
	}

	public static string Green(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), RichColor.green);
	}

	public static string Green(string inner)
	{
		return ColorStr(inner, RichColor.green);
	}

	public static string Lightblue(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), RichColor.lightblue);
	}

	public static string Lightblue(string inner)
	{
		return ColorStr(inner, RichColor.lightblue);
	}

	public static string Lime(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), RichColor.lime);
	}

	public static string Lime(string inner)
	{
		return ColorStr(inner, RichColor.lime);
	}

	public static string Magenta(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), RichColor.magenta);
	}

	public static string Magenta(string inner)
	{
		return ColorStr(inner, RichColor.magenta);
	}

	public static string Olive(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), RichColor.olive);
	}

	public static string Olive(string inner)
	{
		return ColorStr(inner, RichColor.olive);
	}

	public static string Orange(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), RichColor.orange);
	}

	public static string Orange(string inner)
	{
		return ColorStr(inner, RichColor.orange);
	}

	public static string Red(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), RichColor.red);
	}

	public static string Red(string inner)
	{
		return ColorStr(inner, RichColor.red);
	}

	public static string Silver(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), RichColor.silver);
	}

	public static string Silver(string inner)
	{
		return ColorStr(inner, RichColor.silver);
	}

	public static string Teal(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), RichColor.teal);
	}

	public static string Teal(string inner)
	{
		return ColorStr(inner, RichColor.teal);
	}

	public static string White(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), RichColor.white);
	}

	public static string White(string inner)
	{
		return ColorStr(inner, RichColor.white);
	}

	public static string Yellow(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), RichColor.yellow);
	}

	public static string Yellow(string inner)
	{
		return ColorStr(inner, RichColor.yellow);
	}
}
