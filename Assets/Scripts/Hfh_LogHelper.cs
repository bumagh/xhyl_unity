public class Hfh_LogHelper
{
	public static string Color_NetHandle = "000CFFFF";

	public static string ColorStr(string inner, Dzb_RichColor color)
	{
		return inner;
	}

	public static string ColorStr(string inner, string color)
	{
		return inner;
	}

	public static string Key(object o, Dzb_RichColor color = Dzb_RichColor.aqua)
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
		return ColorStr(string.Format(format, args), Dzb_RichColor.aqua);
	}

	public static string Aqua(string inner)
	{
		return ColorStr(inner, Dzb_RichColor.aqua);
	}

	public static string Black(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), Dzb_RichColor.black);
	}

	public static string Black(string inner)
	{
		return ColorStr(inner, Dzb_RichColor.black);
	}

	public static string Blue(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), Dzb_RichColor.blue);
	}

	public static string Blue(string inner)
	{
		return ColorStr(inner, Dzb_RichColor.blue);
	}

	public static string Brown(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), Dzb_RichColor.brown);
	}

	public static string Brown(string inner)
	{
		return ColorStr(inner, Dzb_RichColor.brown);
	}

	public static string Cyan(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), Dzb_RichColor.cyan);
	}

	public static string Cyan(string inner)
	{
		return ColorStr(inner, Dzb_RichColor.cyan);
	}

	public static string Green(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), Dzb_RichColor.green);
	}

	public static string Green(string inner)
	{
		return ColorStr(inner, Dzb_RichColor.green);
	}

	public static string Lightblue(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), Dzb_RichColor.lightblue);
	}

	public static string Lightblue(string inner)
	{
		return ColorStr(inner, Dzb_RichColor.lightblue);
	}

	public static string Lime(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), Dzb_RichColor.lime);
	}

	public static string Lime(string inner)
	{
		return ColorStr(inner, Dzb_RichColor.lime);
	}

	public static string Magenta(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), Dzb_RichColor.magenta);
	}

	public static string Magenta(string inner)
	{
		return ColorStr(inner, Dzb_RichColor.magenta);
	}

	public static string Olive(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), Dzb_RichColor.olive);
	}

	public static string Olive(string inner)
	{
		return ColorStr(inner, Dzb_RichColor.olive);
	}

	public static string Orange(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), Dzb_RichColor.orange);
	}

	public static string Orange(string inner)
	{
		return ColorStr(inner, Dzb_RichColor.orange);
	}

	public static string Red(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), Dzb_RichColor.red);
	}

	public static string Red(string inner)
	{
		return ColorStr(inner, Dzb_RichColor.red);
	}

	public static string Silver(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), Dzb_RichColor.silver);
	}

	public static string Silver(string inner)
	{
		return ColorStr(inner, Dzb_RichColor.silver);
	}

	public static string Teal(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), Dzb_RichColor.teal);
	}

	public static string Teal(string inner)
	{
		return ColorStr(inner, Dzb_RichColor.teal);
	}

	public static string White(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), Dzb_RichColor.white);
	}

	public static string White(string inner)
	{
		return ColorStr(inner, Dzb_RichColor.white);
	}

	public static string Yellow(string format, params object[] args)
	{
		return ColorStr(string.Format(format, args), Dzb_RichColor.yellow);
	}

	public static string Yellow(string inner)
	{
		return ColorStr(inner, Dzb_RichColor.yellow);
	}
}
