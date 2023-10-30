using DG.Tweening;
using UnityEngine;

public class STMF_FishNet : MonoBehaviour
{
	[SerializeField]
	private GameObject _NetUp;

	[SerializeField]
	private GameObject _NetLeft;

	[SerializeField]
	private GameObject _NetRight;

	[SerializeField]
	private Transform tfNetRing;

	[SerializeField]
	private Transform[] tfNetRings;

	[SerializeField]
	private SpriteRenderer srNetUp;

	[SerializeField]
	private SpriteRenderer srNetLeft;

	[SerializeField]
	private SpriteRenderer srNetRight;

	[SerializeField]
	private Sprite spiLiziNet;

	[SerializeField]
	private Sprite spiNormalNet;

	private Color[] colorNets = new Color[5]
	{
		Color.white,
		Color.white,
		Color.white,
		Color.white,
		Color.white
	};

	private bool _isDead;

	private bool _isLizi;

	public bool mIsDead
	{
		get
		{
			return _isDead;
		}
		set
		{
			_isDead = value;
		}
	}

	public bool mIsLizi
	{
		get
		{
			return _isLizi;
		}
		set
		{
			_isLizi = value;
		}
	}

	public void SetPlayerColor(int nPlayerID)
	{
		if (mIsLizi)
		{
			srNetUp.color = colorNets[0];
			srNetLeft.color = colorNets[0];
			srNetRight.color = colorNets[0];
		}
		else
		{
			srNetUp.color = colorNets[nPlayerID];
			srNetLeft.color = colorNets[nPlayerID];
			srNetRight.color = colorNets[nPlayerID];
		}
	}

	public void SetLizi(bool isLizi)
	{
		mIsLizi = isLizi;
		if (isLizi)
		{
			srNetUp.sprite = spiLiziNet;
			srNetLeft.sprite = spiLiziNet;
			srNetRight.sprite = spiLiziNet;
		}
		else
		{
			srNetUp.sprite = spiNormalNet;
			srNetLeft.sprite = spiNormalNet;
			srNetRight.sprite = spiNormalNet;
		}
	}

	public void SetPower(int nPower)
	{
		if (nPower <= 50 && nPower > 0)
		{
			_NetUp.SetActive(value: false);
			tfNetRings[0].gameObject.SetActive(value: false);
		}
		else if (nPower > 50)
		{
			_NetUp.SetActive(value: true);
			tfNetRings[0].gameObject.SetActive(value: true);
		}
	}

	public void OnSpawned()
	{
		_isDead = false;
		Shake();
	}

	public void ObjDestroy()
	{
		if (!_isDead)
		{
			_isDead = true;
			STMF_FishNetpoolMngr.GetSingleton().DestroyFishNet(base.gameObject);
		}
	}

	public void Shake()
	{
		base.transform.DOShakePosition(0.6f, Vector3.right * 0.1f + Vector3.up * 0.1f).SetEase(Ease.Linear).OnComplete(_shakeEnd)
			.OnUpdate(_onUpdate);
		RingScaleAnim();
	}

	private void _shakeEnd()
	{
		ObjDestroy();
	}

	private void RingScaleAnim()
	{
		for (int i = 0; i < 3; i++)
		{
			int num = i;
			if (tfNetRings[num].gameObject.activeSelf)
			{
				tfNetRings[num].DOKill();
				tfNetRings[num].DOScale(0.5f, 0f).SetDelay(0.4f);
				tfNetRings[num].DOScale(0.6f, 0.2f).SetEase(Ease.Linear).SetDelay(0.4f);
				tfNetRings[num].DOScale(0f, 0f).SetDelay(0.6f);
			}
		}
	}

	private void _onUpdate()
	{
		if (mIsLizi)
		{
			_NetUp.transform.Rotate(Vector3.forward, 200f * Time.deltaTime);
			_NetLeft.transform.Rotate(Vector3.forward, 200f * Time.deltaTime);
			_NetRight.transform.Rotate(Vector3.forward, 200f * Time.deltaTime);
		}
	}
}
