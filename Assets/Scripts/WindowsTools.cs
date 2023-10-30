using System.Runtime.InteropServices;
using UnityEngine;

public class WindowsTools
{
	public static Texture2D GetImage()
	{
		OpenFileName openFileName = new OpenFileName();
		openFileName.structSize = Marshal.SizeOf(openFileName);
		openFileName.filter = "二维码文件(*.png)\0*.png";
		openFileName.file = new string(new char[256]);
		openFileName.maxFile = openFileName.file.Length;
		openFileName.fileTitle = new string(new char[64]);
		openFileName.maxFileTitle = openFileName.fileTitle.Length;
		openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');
		openFileName.title = "窗口标题";
		openFileName.flags = 530440;
		if (LocalDialog.GetOpenFileName(openFileName))
		{
			UnityEngine.Debug.Log(openFileName.file);
			UnityEngine.Debug.Log(openFileName.fileTitle);
			Texture2D texture2D = ServiceTools.ReadTexturFromPath(openFileName.file, 512);
			if (texture2D != null)
			{
				return texture2D;
			}
		}
		return null;
	}
}
