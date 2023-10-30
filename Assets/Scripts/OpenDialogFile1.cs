using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class OpenDialogFile1 : MonoBehaviour
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class OpenDialogFile
	{
		public int structSize;

		public IntPtr dlgOwner = IntPtr.Zero;

		public IntPtr instance = IntPtr.Zero;

		public string filter;

		public string customFilter;

		public int maxCustFilter;

		public int filterIndex;

		public string file;

		public int maxFile;

		public string fileTitle;

		public int maxFileTitle;

		public string initialDir;

		public string title;

		public int flags;

		public short fileOffset;

		public short fileExtension;

		public string defExt;

		public IntPtr custData = IntPtr.Zero;

		public IntPtr hook = IntPtr.Zero;

		public string templateName;

		public IntPtr reservedPtr = IntPtr.Zero;

		public int reservedInt;

		public int flagsEx;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class OpenDialogDir
	{
		public IntPtr hwndOwner = IntPtr.Zero;

		public IntPtr pidlRoot = IntPtr.Zero;

		public string pszDisplayName;

		public string lpszTitle;

		public uint ulFlags;

		public IntPtr lpfn = IntPtr.Zero;

		public IntPtr lParam = IntPtr.Zero;

		public int iImage;
	}

	public class DllOpenFileDialog
	{
		[DllImport("Comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern bool GetOpenFileName([In] [Out] OpenDialogFile ofn);

		[DllImport("Comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern bool GetSaveFileName([In] [Out] OpenDialogFile ofn);

		[DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern IntPtr SHBrowseForFolder([In] [Out] OpenDialogDir ofn);

		[DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern bool SHGetPathFromIDList([In] IntPtr pidl, [In] [Out] char[] fileName);
	}

	private string fullDirPath;

	private void Start()
	{
		GetFiles();
	}

	public void QQ()
	{
		OpenDialogDir openDialogDir = new OpenDialogDir();
		openDialogDir.pszDisplayName = new string(new char[2000]);
		openDialogDir.lpszTitle = "Open Project";
		IntPtr pidl = DllOpenFileDialog.SHBrowseForFolder(openDialogDir);
		char[] array = new char[2000];
		for (int i = 0; i < 2000; i++)
		{
			array[i] = '\0';
		}
		DllOpenFileDialog.SHGetPathFromIDList(pidl, array);
		fullDirPath = new string(array);
		fullDirPath = fullDirPath.Substring(0, fullDirPath.IndexOf('\0'));
		UnityEngine.Debug.Log("***选择储存的文件夹，将要储存到**" + fullDirPath);
	}

	public void GetFiles()
	{
		string text = "I:/Hall_my/Assets/CaiShenFa/Res/Texture/Load";
		UnityEngine.Debug.Log("想要读取的文件夹是" + text);
		string text2 = "*.BMP|*.JPG|*.GIF|*.PNG";
		string[] array = text2.Split('|');
		if (!Directory.Exists(text))
		{
			return;
		}
		DirectoryInfo directoryInfo = new DirectoryInfo(text);
		FileInfo[] files = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
		UnityEngine.Debug.Log("该文件夹文件总数为" + files.Length);
		for (int i = 0; i < array.Length; i++)
		{
			string[] files2 = Directory.GetFiles(text, array[i]);
			for (int j = 0; j < files2.Length; j++)
			{
				if (!files[j].Name.EndsWith(".meta"))
				{
					UnityEngine.Debug.Log("文件夹图片格式的Name:" + files[j].Name);
				}
			}
		}
	}
}
