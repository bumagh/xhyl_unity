using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PHG_HUDController : PHG_MB_Singleton<PHG_HUDController>
{
	private GameObject _goContainer;

	private Text pingTime;

	private Image ImgWifi;

	[SerializeField]
	private Sprite[] _sprites = new Sprite[2];

	[SerializeField]
	private PHG_RulePicController ruleController;

	private Color greenColor = new Color(8f / 51f, 1f, 0f);

	private Color yellowColor = new Color(1f, 72f / 85f, 0f);

	private Color redColor = new Color(float.PositiveInfinity, 0f, 0f);

	private bool isNetWorkLose;

	private Ping ping;

	private int delayTime;

	private Coroutine waitPing;

	private void Awake()
	{
		base.transform.localScale = Vector3.one;
		_goContainer = base.gameObject;
		pingTime = base.transform.Find("PingTime").GetComponent<Text>();
		ImgWifi = base.transform.Find("ImgWifi").GetComponent<Image>();
		if (PHG_MB_Singleton<PHG_HUDController>._instance == null)
		{
			PHG_MB_Singleton<PHG_HUDController>.SetInstance(this);
			PreInit();
		}
	}

	private void OnEnable()
	{
		SendPing();
	}

	private void Start()
	{
		Init();
	}

	public void PreInit()
	{
		if (_goContainer == null)
		{
			_goContainer = base.gameObject;
		}
		if (ruleController == null)
		{
			ruleController = base.transform.Find("Canvas/Mask/Rule").GetComponent<PHG_RulePicController>();
		}
	}

	public void Init()
	{
		PHG_MB_Singleton<PHG_OptionsController>.GetInstance().onItemRules = delegate
		{
			ruleController.Show();
		};
		ruleController.Hide();
	}

	public void Show()
	{
		_goContainer.SetActive(value: true);
	}

	public void Hide()
	{
		_goContainer.SetActive(value: false);
	}

	private void OnGUI()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			delayTime = 460;
			SetColor(delayTime);
			isNetWorkLose = true;
		}
		else if ((ping != null && ping.isDone) || isNetWorkLose)
		{
			isNetWorkLose = false;
			delayTime = ping.time;
			ping.DestroyPing();
			ping = null;
			Invoke("SendPing", 1f);
		}
		if (delayTime < 0)
		{
			delayTime = 460;
		}
		if (pingTime != null)
		{
			pingTime.text = delayTime.ToString("00") + " ms";
		}
		SetColor(delayTime);
		if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
		{
			if (ImgWifi != null && _sprites.Length > 0)
			{
				ImgWifi.sprite = _sprites[0];
			}
		}
		else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork && ImgWifi != null && _sprites.Length > 1)
		{
			ImgWifi.sprite = _sprites[1];
		}
	}

	private void SetColor(int pingValue)
	{
		if (pingValue < 100)
		{
			if (pingTime != null)
			{
				pingTime.color = greenColor;
			}
			if (ImgWifi != null)
			{
				ImgWifi.color = greenColor;
			}
		}
		else if (pingValue < 200)
		{
			if (pingTime != null)
			{
				pingTime.color = yellowColor;
			}
			if (ImgWifi != null)
			{
				ImgWifi.color = yellowColor;
			}
		}
		else
		{
			if (pingTime != null)
			{
				pingTime.color = redColor;
			}
			if (ImgWifi != null)
			{
				ImgWifi.color = redColor;
			}
		}
	}

	private void SendPing()
	{
		ping = new Ping(PHG_GVars.IPAddress);
		if (waitPing != null)
		{
			StopCoroutine(waitPing);
		}
		if (base.gameObject.activeSelf)
		{
			waitPing = StartCoroutine(WaitPing());
		}
	}

	private IEnumerator WaitPing()
	{
		yield return new WaitForSeconds(3f);
		UnityEngine.Debug.LogError("=====等待结束,重启Ping=====");
		SendPing();
	}

	private void OnDisable()
	{
		if (waitPing != null)
		{
			StopCoroutine(waitPing);
		}
		waitPing = null;
	}

	public void OnBtnOptions_Click()
	{
		PHG_SoundManager.Instance.PlayClickAudio();
		if (!PHG_LockManager.IsLocked("btn_options"))
		{
			if (PHG_MB_Singleton<PHG_OptionsController>.GetInstance().isShow)
			{
				PHG_MB_Singleton<PHG_OptionsController>.GetInstance().Hide();
			}
			else
			{
				PHG_MB_Singleton<PHG_OptionsController>.GetInstance().Show();
			}
		}
	}

	public void HideRules()
	{
		ruleController.Hide();
	}
}
