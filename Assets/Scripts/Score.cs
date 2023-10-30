using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	private static int score;

	private static int highScore;

	private static Score instance;

	public Text scoreText;

	public MyButton myButton;

	private Fish fish;

	private bool isOnClick;

	public static void AddPoint()
	{
		if (!instance.fish.dead)
		{
			score++;
			if (score > highScore)
			{
				highScore = score;
			}
		}
	}

	private void Awake()
	{
		UnityEngine.Debug.LogError("开始等待登录");
		if (PlayerPrefs.GetString("isCanjump", "不能跳") == "可以跳")
		{
			UnityEngine.Debug.LogError("不是第一次打开,直接跳场景");
			SceneManager.LoadScene("MainScene");
		}
	}

	private void OnEnable()
	{
		isOnClick = false;
	}

	private void Start()
	{
		instance = this;
		GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
		if (gameObject == null)
		{
			UnityEngine.Debug.LogError("Could not find an object with tag 'Player'.");
		}
		fish = gameObject.GetComponent<Fish>();
		score = 0;
		highScore = PlayerPrefs.GetInt("highScore", 0);
		myButton.onClick.AddListener(delegate
		{
			UnityEngine.Debug.Log(" myButton.onClick");
		});
		myButton.OnDoubleClick.AddListener(delegate
		{
			UnityEngine.Debug.Log(" myButton.OnDoubleClick");
		});
		myButton.OnLongPress.AddListener(delegate
		{
			OnLongPress();
		});
	}

	private void OnDestroy()
	{
		instance = null;
		PlayerPrefs.SetInt("highScore", highScore);
	}

	private void Update()
	{
		scoreText.text = "Score: " + score + "\nHigh Score: " + highScore;
	}

	private void OnLongPress()
	{
		UnityEngine.Debug.LogError("长按");
		SceneManager.LoadScene("MainScene");
	}
}
