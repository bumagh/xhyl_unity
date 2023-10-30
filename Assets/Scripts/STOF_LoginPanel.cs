using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class STOF_LoginPanel : MonoBehaviour
{
	public InputField UserID;

	public InputField Password;

	public static string userid = string.Empty;

	public static string pwd = string.Empty;

	public Button Enter;

	private void Start()
	{
		if (PlayerPrefs.HasKey("userid"))
		{
			UserID.text = PlayerPrefs.GetString("userid");
			Password.text = PlayerPrefs.GetString("pwd");
		}
		Enter.onClick.AddListener(delegate
		{
			userid = UserID.text;
			pwd = Password.text;
			if (userid != string.Empty)
			{
				PlayerPrefs.SetString("userid", userid);
			}
			if (pwd != string.Empty)
			{
				PlayerPrefs.SetString("pwd", pwd);
			}
			SceneManager.LoadSceneAsync(1);
		});
	}
}
