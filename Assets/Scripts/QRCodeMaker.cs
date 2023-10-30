using UnityEngine;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

public class QRCodeMaker : MonoBehaviour
{
	public static Texture2D encoded = new Texture2D(512, 512);

	private static Color32[] Encode(string textForEncoding, int width, int height)
	{
		BarcodeWriter barcodeWriter = new BarcodeWriter();
		barcodeWriter.Format = BarcodeFormat.QR_CODE;
		barcodeWriter.Options = new QrCodeEncodingOptions
		{
			Height = height,
			Width = width,
			Margin = 0,
			ErrorCorrection = ErrorCorrectionLevel.H
		};
		BarcodeWriter barcodeWriter2 = barcodeWriter;
		return barcodeWriter2.Write(textForEncoding);
	}

	public static void QRCodeCreate(string url)
	{
		if (url == null)
		{
			return;
		}
		Color32[] array = Encode(url, 256, 256);
		Color32[] array2 = new Color32[262144];
		for (int i = 0; i < 256; i++)
		{
			for (int j = 0; j < 256; j++)
			{
				array2[i * 1024 + 2 * j] = array[i * 256 + j];
				array2[i * 1024 + 2 * j + 1] = array[i * 256 + j];
				array2[i * 1024 + 2 * j + 512] = array[i * 256 + j];
				array2[i * 1024 + 2 * j + 513] = array[i * 256 + j];
			}
		}
		encoded.SetPixels32(array2);
		encoded.Apply();
	}
}
