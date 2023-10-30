using UnityEngine;

public class MeshUICamera : MonoBehaviour
{
	private float m_sizeNormal;

	private float m_sizeLogin;

	private Camera dealerCamera;

	private static MeshUICamera _instance;

	private GameObject m_activeDealer;

	private void Awake()
	{
		dealerCamera = GetComponent<Camera>();
		m_sizeNormal = dealerCamera.orthographicSize;
		m_sizeNormal = 4f;
		m_sizeLogin = 5.2f;
		_instance = this;
		MB_Singleton<AppManager>.GetInstance().Register(UIGameMsgType.UINotify_FreshDealer, this, FreshDealer);
	}

	public static MeshUICamera Get()
	{
		return _instance;
	}

	private void FreshDealer(object obj)
	{
		string[] array = new string[4]
		{
			"dealer00",
			"dealer01",
			"dealer02",
			"dealer03"
		};
		int num = (int)obj;
		if (num == -1)
		{
			num = 0;
			dealerCamera.orthographicSize = m_sizeLogin;
		}
		else
		{
			dealerCamera.orthographicSize = m_sizeNormal;
		}
		for (int i = 0; i < 4; i++)
		{
			GameObject gameObject = base.transform.Find(array[i]).gameObject;
			gameObject.SetActive(i == num);
			m_activeDealer = ((i == num) ? gameObject : m_activeDealer);
		}
		switch (num)
		{
		case 0:
			MB_Singleton<SoundManager>.Get().PlaySound(SoundType.BG_First);
			break;
		case 1:
			MB_Singleton<SoundManager>.Get().PlaySound(SoundType.BG_Second);
			break;
		case 2:
			MB_Singleton<SoundManager>.Get().PlaySound(SoundType.BG_Third);
			break;
		case 3:
			MB_Singleton<SoundManager>.Get().PlaySound(SoundType.BG_Four);
			break;
		}
	}

	public void ClickDealer()
	{
		if ((bool)m_activeDealer)
		{
			m_activeDealer.GetComponent<BeautyDealer>().Greet();
		}
	}
}
