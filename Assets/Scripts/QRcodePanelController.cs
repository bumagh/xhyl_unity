using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class QRcodePanelController : MonoBehaviour
{
	private bool IsSave;

	private void OnEnable()
	{
		StartCoroutine(CaptureScreenshot2());
	}

	private IEnumerator CaptureScreenshot2()
	{
		yield return new WaitForEndOfFrame();
		Rect rect = GetComponent<RectTransform>().rect;
		float x = (float)Screen.width / 1280f;
		float y = (float)Screen.height / 720f;
		Texture2D screenShot = new Texture2D((int)(rect.width * x), (int)(rect.height * y), TextureFormat.RGB24, mipChain: false);
		screenShot.ReadPixels(new Rect((float)(Screen.width / 2) - rect.width * x / 2f, (float)(Screen.height / 2) - rect.height * y / 2f, (float)(int)rect.width * x, (float)(int)rect.height * y), 0, 0);
		screenShot.Apply();
		byte[] bytes = screenShot.EncodeToPNG();
		IsSave = false;
		string path2 = Application.persistentDataPath + "/";
		Debug.Log(path2);
		try
		{
			path2 = "/storage/emulated/0/DCIM/";
			if (!Directory.Exists(path2))
			{
				Directory.CreateDirectory(path2);
			}
			File.WriteAllBytes(path2 + "picture.png", bytes);
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.xxyl.BitmapTools");
			AndroidJavaObject @static = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
			androidJavaObject.Call("refreshMedia", @static, path2 + "picture.png");
			IsSave = true;
		}
		catch (Exception arg)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip( "请检查是否已经打开相册获取权限", "Please check if the album has been opened to obtain permissions", "โปรดตรวจสอบว่าได้เปิดอัลบั้มเพื่อขออนุญาตแล้วหรือไม่", "Kiểm tra xem album đã được mở chưa"));
			Debug.LogError("保存错误: " + arg);
		}
		yield return new WaitForSeconds(0.5f);
		if (IsSave)
		{
			MB_Singleton<AlertDialog>.GetInstance().ShowDialog(ZH2_GVars.ShowTip("二维码保存成功", "QR code saved successfully", "บันทึกรหัส QR สำเร็จ", "Mã QR được lưu thành công"));
		}
		base.gameObject.SetActive(value: false);
	}
}
