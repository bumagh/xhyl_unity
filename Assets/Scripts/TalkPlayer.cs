using UnityEngine;

public class TalkPlayer
{
	public string username;

	public int operatorType;

	public int qrcodeType;

	public string info;

	public bool notQRCode;

	public Texture2D qrcodeTexute;

	public bool isOnLine;

	private float loadAvatarTime = -100f;

	private bool isLoadAvatar;

	private Texture2D m_avatar;

	public void Copy(TalkPlayer copyData)
	{
		if (copyData.username.ToLower() != username.ToLower())
		{
			qrcodeTexute = copyData.qrcodeTexute;
			info = copyData.info;
		}
		username = copyData.username;
		operatorType = copyData.operatorType;
		qrcodeType = copyData.qrcodeType;
		info = copyData.info;
		isOnLine = copyData.isOnLine;
	}

	public override string ToString()
	{
		return username + "," + operatorType + "," + qrcodeType + "," + info;
	}
}
