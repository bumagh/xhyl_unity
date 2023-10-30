using UnityEngine;

namespace STDT_GameConfig
{
	public class XLDT_DanTiaoCommon
	{
		public static bool G_TEST;

		public static bool G_IS_SPECIAL = true;

		public static string ChangeNumber(long number)
		{
			string text = number.ToString();
			if (XLDT_GameInfo.getInstance().IsSpecial && text.Length > 6)
			{
				text = text.Substring(text.Length - 6, 6);
			}
			return text;
		}

		public static void SetViewport(float fWidth, float fHeight)
		{
			float num = Screen.width;
			float num2 = Screen.height;
			float x = 0f;
			float y = 0f;
			float num3 = 1f;
			float num4 = 1f;
			if (fWidth / fHeight > num / num2)
			{
				num3 = 1f;
				num4 = fHeight * num / fWidth / num2;
				y = (1f - num4) / 2f;
			}
			else if (fWidth / fHeight < num / num2)
			{
				num4 = 1f;
				num3 = fWidth * num2 / fHeight / num;
				x = (1f - num3) / 2f;
			}
			for (int i = 0; i < Camera.allCameras.Length; i++)
			{
				Camera camera = Camera.allCameras[i];
				camera.rect = new Rect(x, y, num3, num4);
			}
		}
	}
}
