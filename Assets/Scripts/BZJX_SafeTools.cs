using System.Collections;

public class BZJX_SafeTools
{
	public static string Adjust_String_1(string source)
	{
		return (string)GetValue(IE_Adjust_String_1(source), toEnd: true);
	}

	private static IEnumerator IE_Adjust_String_1(string source)
	{
		char[] chars = source.ToCharArray();
		int begin2 = 1;
		int end2;
		for (end2 = chars.Length - 2; begin2 < end2; begin2 += 2)
		{
			char c = chars[begin2];
			chars[begin2] = chars[begin2 + 1];
			chars[begin2 + 1] = c;
		}
		begin2 = 1;
		end2 = chars.Length - 2;
		while (begin2 < end2)
		{
			char c2 = chars[begin2];
			chars[begin2] = chars[end2];
			chars[end2] = c2;
			begin2++;
			end2--;
		}
		yield return new string(chars);
	}

	public static T GetValue<T>(IEnumerator ie, bool toEnd = false)
	{
		while (ie.MoveNext() && (toEnd || ie.Current == null))
		{
		}
		return (T)ie.Current;
	}

	public static object GetValue(IEnumerator ie, bool toEnd = false)
	{
		while (ie.MoveNext() && (toEnd || ie.Current == null))
		{
		}
		return ie.Current;
	}
}
