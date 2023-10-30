using System.Runtime.InteropServices;

public class LocalDialog
{
	[DllImport("Comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
	public static extern bool GetOpenFileName([In] [Out] OpenFileName ofn);

	public static bool GetOFN([In] [Out] OpenFileName ofn)
	{
		return GetOpenFileName(ofn);
	}

	[DllImport("Comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
	public static extern bool GetSaveFileName([In] [Out] OpenFileName ofn);

	public static bool GetSFN([In] [Out] OpenFileName ofn)
	{
		return GetSaveFileName(ofn);
	}
}
