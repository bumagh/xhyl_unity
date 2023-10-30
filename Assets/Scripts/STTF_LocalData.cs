using GameConfig;
using System;
using System.IO;
using System.Xml;
using UnityEngine;

public class STTF_LocalData
{
	private XmlDocument mXmlDoc = new XmlDocument();

	private string mFileName = Application.persistentDataPath + "/STTFData.xml";

	private static STTF_LocalData mSettedManager;

	public static STTF_LocalData getInstance()
	{
		if (mSettedManager == null)
		{
			mSettedManager = new STTF_LocalData();
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
			catch (Exception)
			{
				File.Delete(mFileName);
				_createXmlFile(mFileName);
			}
		}
		STTF_GameInfo.getInstance().Setted = ReadUserSetting();
	}

	private void _createXmlFile(string name)
	{
		try
		{
			FileStream fileStream = File.Create(name);
			fileStream.Close();
			XmlElement xmlElement = mXmlDoc.CreateElement("STTFData");
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
		STTF_SettedInfo setted = STTF_GameInfo.getInstance().Setted;
		_writeData("/STTFData/UserSetting/GAMEVOLUM", "GameVolum", setted.bIsGameVolum.ToString());
		_writeData("/STTFData/UserSetting/BUTTONVOLUM", "ButtonVolum", setted.bIsButtonVolum.ToString());
		_writeData("/STTFData/UserSetting/ISFORBIDPUBLICCHAT", "IsForbidPublicChat", setted.bIsForbidPublicChat.ToString());
		_writeData("/STTFData/UserSetting/ISFORBIDPRIVATECHAT", "IsForbidPrivateChat", setted.bIsForbidPrivateChat.ToString());
		_writeData("/STTFData/UserSetting/ISSCREENNEVERSLEEP", "IsScreenNeverSleep", setted.bIsScreenNeverSleep.ToString());
	}

	private STTF_SettedInfo ReadUserSetting()
	{
		STTF_SettedInfo sTTF_SettedInfo = new STTF_SettedInfo();
		sTTF_SettedInfo.bIsGameVolum = bool.Parse(_readData("/STTFData/UserSetting/GAMEVOLUM", "GameVolum"));
		sTTF_SettedInfo.bIsButtonVolum = bool.Parse(_readData("/STTFData/UserSetting/BUTTONVOLUM", "ButtonVolum"));
		sTTF_SettedInfo.bIsForbidPublicChat = bool.Parse(_readData("/STTFData/UserSetting/ISFORBIDPUBLICCHAT", "IsForbidPublicChat"));
		sTTF_SettedInfo.bIsForbidPrivateChat = bool.Parse(_readData("/STTFData/UserSetting/ISFORBIDPRIVATECHAT", "IsForbidPrivateChat"));
		sTTF_SettedInfo.bIsScreenNeverSleep = bool.Parse(_readData("/STTFData/UserSetting/ISSCREENNEVERSLEEP", "IsScreenNeverSleep"));
		return sTTF_SettedInfo;
	}

	public void applySetting()
	{
		STTF_SettedInfo setted = STTF_GameInfo.getInstance().Setted;
		if (setted.bIsScreenNeverSleep)
		{
			Screen.sleepTimeout = -1;
		}
		else
		{
			Screen.sleepTimeout = -2;
		}
		STTF_SoundManage.getInstance().IsButtonMusic = setted.bIsButtonVolum;
		if (STTF_GameInfo.getInstance().currentState == STTF_GameState.On_Game)
		{
			STTF_MusicMngr.GetSingleton().SetMusicOnOff(setted.bIsButtonVolum);
			STTF_MusicMngr.GetSingleton().SetMusicFxOnOff(setted.bIsButtonVolum);
		}
		STTF_SoundManage.getInstance().setBgMusic(setted.bIsGameVolum);
	}
}
