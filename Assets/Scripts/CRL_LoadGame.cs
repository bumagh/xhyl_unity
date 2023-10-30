using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CRL_LoadGame : MonoBehaviour
{
	private Slider _progress;

	private Text _textPercent;

	private float percent;

	private void Awake()
	{
		SceneManager.LoadSceneAsync("CRL_Ui");
		_progress = base.transform.Find("Slider").GetComponent<Slider>();
		_textPercent = base.transform.Find("TxtProgress").GetComponent<Text>();
		StartCoroutine(LoadIng());
	}

	private IEnumerator LoadIng()
	{
		while (percent < 1f)
		{
			percent += 0.01f;
			CRL_MySqlConnection.RandomTime = percent;
			_setProgress(percent);
			yield return new WaitForSeconds(0.1f);
		}
	}

	private void _setProgress(float percent)
	{
		_progress.value = percent;
		_textPercent.text = $"{Mathf.Min(100, Mathf.CeilToInt(percent * 100f))}%";
	}
}
