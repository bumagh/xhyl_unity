using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JSYS_DisconnectPanel : MonoBehaviour
{
	private static JSYS_DisconnectPanel instance;

	private Text TitleText;

	private Text ContentText;

	private Button ResetButton;

	private Button ReturnButton;

	public static JSYS_DisconnectPanel GetInstance()
	{
		if (instance == null)
		{
			instance = UnityEngine.Object.Instantiate(Resources.Load<JSYS_DisconnectPanel>("DisconnectPanel"), GameObject.Find("Canvas").transform);
			instance.TitleText = instance.transform.Find("Title").GetComponent<Text>();
			instance.ContentText = instance.transform.Find("Content").GetComponent<Text>();
			instance.ResetButton = instance.transform.Find("Reset").GetComponent<Button>();
			instance.ReturnButton = instance.transform.Find("Return").GetComponent<Button>();
			instance.ResetButton.onClick.AddListener(instance.Reset_Method);
			instance.ReturnButton.onClick.AddListener(instance.Return_Method);
			instance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			instance.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
			instance.GetComponent<RectTransform>().localScale = Vector3.one;
		}
		return instance;
	}

	public void Show()
	{
		base.transform.SetAsLastSibling();
		base.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	public void Modification(string title, string Content)
	{
		TitleText.text = title;
		ContentText.text = Content;
	}

	private void Reset_Method()
	{
	}

	public void Return_Method()
	{
		if (JSYS_NewTcpNet.instance != null)
		{
			if (JSYS_NewTcpNet.instance.GetConnectionStatus())
			{
				JSYS_NewTcpNet.instance.SocketQuit();
				JSYS_Audiomanger._instenc.GetComponent<AudioSource>().clip = JSYS_Audiomanger._instenc.LobbyBG[Random.Range(0, 2)];
				JSYS_Audiomanger._instenc.GetComponent<AudioSource>().Play();
				SceneManager.LoadScene(1);
			}
			else
			{
				JSYS_NewTcpNet.instance.SocketQuit();
				JSYS_Audiomanger._instenc.GetComponent<AudioSource>().clip = JSYS_Audiomanger._instenc.LobbyBG[Random.Range(0, 2)];
				JSYS_Audiomanger._instenc.GetComponent<AudioSource>().Play();
				SceneManager.LoadScene(1);
			}
		}
		else
		{
			JSYS_Audiomanger._instenc.GetComponent<AudioSource>().clip = JSYS_Audiomanger._instenc.LobbyBG[Random.Range(0, 2)];
			JSYS_Audiomanger._instenc.GetComponent<AudioSource>().Play();
			SceneManager.LoadScene(1);
		}
	}
}
