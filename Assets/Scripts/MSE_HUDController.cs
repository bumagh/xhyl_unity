using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MSE_HUDController : MSE_MB_Singleton<MSE_HUDController>
{
	private GameObject _goContainer;

	private Text pingTime;

	private Image ImgWifi;

	[SerializeField]
	private Sprite[] _sprites = new Sprite[2];

	[SerializeField]
	private MSE_RulePicController ruleController;

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
		if (MSE_MB_Singleton<MSE_HUDController>._instance == null)
		{
			MSE_MB_Singleton<MSE_HUDController>.SetInstance(this);
			PreInit();
		}
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
			ruleController = base.transform.Find("Canvas/Mask/Rule").GetComponent<MSE_RulePicController>();
		}
	}

	public void Init()
	{
		MSE_MB_Singleton<MSE_OptionsController>.GetInstance().onItemRules = delegate
		{
			ruleController.Show();
		};
		ruleController.Hide();
	}

	public void Update()
	{
	}

	public void Show()
	{
		_goContainer.SetActive(value: true);
	}

	public void Hide()
	{
		_goContainer.SetActive(value: false);
	}

	private void _updateCurrentTime()
	{
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
			if (ImgWifi != null)
			{
				ImgWifi.sprite = _sprites[0];
			}
		}
		else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork && ImgWifi != null)
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
		ping = new Ping(MSE_GVars.IPAddress);
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
		UnityEngine.Debug.LogError("等待结束,重启Ping");
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
		MSE_SoundManager.Instance.PlayClickAudio();
		if (!MSE_LockManager.IsLocked("btn_options"))
		{
			if (MSE_MB_Singleton<MSE_OptionsController>.GetInstance().isShow)
			{
				MSE_MB_Singleton<MSE_OptionsController>.GetInstance().Hide();
			}
			else
			{
				MSE_MB_Singleton<MSE_OptionsController>.GetInstance().Show();
			}
		}
	}

	public void ResetSprite()
	{
	}

	public void HideRules()
	{
		ruleController.Hide();
	}

	public void SetBtnOptionsEnable(bool isEnable)
	{
	}
}
