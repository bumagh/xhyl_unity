using System.IO;
using System.Xml;
using UnityEngine;

public class XLDT_localData
{
	private XmlDocument mXmlDoc = new XmlDocument();

	private string mFileName = Application.persistentDataPath + "/Data.xml";

	private static XLDT_localData mSettedManager;

	public XLDT_localData()
	{
		mFileName = Application.persistentDataPath + "/Data.xml";
		loadUserSetting();
	}

	public static XLDT_localData getInstance()
	{
		if (mSettedManager == null)
		{
			mSettedManager = new XLDT_localData();
		}
		return mSettedManager;
	}

	public void loadUserSetting()
	{
		if (!File.Exists(mFileName))
		{
			_createXmlFile(mFileName);
		}
		else
		{
			try
			{
				mXmlDoc.Load(mFileName);
			}
			catch
			{
				File.Delete(mFileName);
				_createXmlFile(mFileName);
			}
		}
		XLDT_GameInfo.getInstance().Setted = ReadUserSetting();
	}

	private void _createXmlFile(string name)
	{
		try
		{
			FileStream fileStream = File.Create(name);
			fileStream.Close();
			XmlElement xmlElement = mXmlDoc.CreateElement("Data");
			mXmlDoc.AppendChild(xmlElement);
			XmlElement xmlElement2 = mXmlDoc.CreateElement("UserSetting");
			xmlElement.AppendChild(xmlElement2);
			XmlElement xmlElement3 = mXmlDoc.CreateElement("GAMEVOLUM");
			xmlElement2.AppendChild(xmlElement3);
			xmlElement3.InnerText = "GameVolum=true";
			xmlElement3 = mXmlDoc.CreateElement("BUTTONVOLUM");
			xmlElement2.AppendChild(xmlElement3);
			xmlElement3.InnerText = "ButtonVolum=true";
			xmlElement3 = mXmlDoc.CreateElement("ISFORBIDPUBLICCHAT");
			xmlElement2.AppendChild(xmlElement3);
			xmlElement3.InnerText = "IsForbidPublicChat=false";
			xmlElement3 = mXmlDoc.CreateElement("ISFORBIDPRIVATECHAT");
			xmlElement2.AppendChild(xmlElement3);
			xmlElement3.InnerText = "IsForbidPrivateChat=false";
			xmlElement3 = mXmlDoc.CreateElement("ISSCREENNEVERSLEEP");
			xmlElement2.AppendChild(xmlElement3);
			xmlElement3.InnerText = "IsScreenNeverSleep=true";
			mXmlDoc.Save(name);
		}
		catch
		{
			UnityEngine.Debug.LogError("DataBase._createXmlFile failed!");
		}
	}

	private string _readData(string strNodeName, string strName)
	{
		string result = string.Empty;
		if (mXmlDoc != null)
		{
			XmlNode xmlNode = mXmlDoc.SelectSingleNode(strNodeName);
			XmlElement xmlElement = (XmlElement)xmlNode;
			result = xmlElement.InnerText.Substring((strName + "=").Length);
			mXmlDoc.Save(mFileName);
		}
		else
		{
			UnityEngine.Debug.Log("XmlDocument load failed.");
		}
		return result;
	}

	private int _writeData(string strNodeName, string strName, string strValue)
	{
		if (mXmlDoc != null)
		{
			XmlNode xmlNode = mXmlDoc.SelectSingleNode(strNodeName);
			XmlElement xmlElement = (XmlElement)xmlNode;
			xmlElement.InnerText = strName + "=" + strValue;
			mXmlDoc.Save(mFileName);
			return 0;
		}
		UnityEngine.Debug.Log("XmlDocument load failed.");
		return 1;
	}

	public void saveUserSetting()
	{
		XLDT_SettedInfo setted = XLDT_GameInfo.getInstance().Setted;
		_writeData("/Data/UserSetting/GAMEVOLUM", "GameVolum", setted.bIsGameVolum.ToString());
		_writeData("/Data/UserSetting/BUTTONVOLUM", "ButtonVolum", setted.bIsButtonVolum.ToString());
		_writeData("/Data/UserSetting/ISFORBIDPUBLICCHAT", "IsForbidPublicChat", setted.bIsForbidPublicChat.ToString());
		_writeData("/Data/UserSetting/ISFORBIDPRIVATECHAT", "IsForbidPrivateChat", setted.bIsForbidPrivateChat.ToString());
		_writeData("/Data/UserSetting/ISSCREENNEVERSLEEP", "IsScreenNeverSleep", setted.bIsScreenNeverSleep.ToString());
	}

	public XLDT_SettedInfo ReadUserSetting()
	{
		XLDT_SettedInfo xLDT_SettedInfo = new XLDT_SettedInfo();
		xLDT_SettedInfo.bIsGameVolum = bool.Parse(_readData("/Data/UserSetting/GAMEVOLUM", "GameVolum"));
		xLDT_SettedInfo.bIsButtonVolum = bool.Parse(_readData("/Data/UserSetting/BUTTONVOLUM", "ButtonVolum"));
		xLDT_SettedInfo.bIsForbidPublicChat = bool.Parse(_readData("/Data/UserSetting/ISFORBIDPUBLICCHAT", "IsForbidPublicChat"));
		xLDT_SettedInfo.bIsForbidPrivateChat = bool.Parse(_readData("/Data/UserSetting/ISFORBIDPRIVATECHAT", "IsForbidPrivateChat"));
		xLDT_SettedInfo.bIsScreenNeverSleep = bool.Parse(_readData("/Data/UserSetting/ISSCREENNEVERSLEEP", "IsScreenNeverSleep"));
		return xLDT_SettedInfo;
	}

	public void applySetting()
	{
		XLDT_SettedInfo setted = XLDT_GameInfo.getInstance().Setted;
		if (setted.bIsScreenNeverSleep)
		{
			Screen.sleepTimeout = -1;
		}
		else
		{
			Screen.sleepTimeout = -2;
		}
		XLDT_SoundManage.getInstance().IsButtonMusic = setted.bIsButtonVolum;
		XLDT_SoundManage.getInstance().IsGameMusic = setted.bIsGameVolum;
	}
}
