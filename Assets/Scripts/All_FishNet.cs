using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class All_FishNet : MonoBehaviour
{
	private int nPlayerID;

	[SerializeField]
	private Image[] imgNets;

	[SerializeField]
	private Transform[] tfRings;

	[SerializeField]
	private Sprite spiLiziNet;

	[SerializeField]
	private Sprite spiNormalNet;

	private Color[] colorNets = new Color[5]
	{
		new Color(1f, 1f, 1f, 0.8f),
        new Color(0.9811321f, 0.9420763f,0.3008188f, 1f),
        new Color(1f, 0.189f,0.9f, 1f),
        new Color(0.2520915F, 0.373437f,0.9716981f, 1f),
        new Color(0.1683873f,0.8301887f,0.6457523f, 1f),
	};

	private bool _isDead;

	private bool bLizi;

	private int[,] powerList = new int[3, 2]
	{
		{
			0,
			100
		},
		{
			100,
			1000
		},
		{
			1000,
			2147483647
		}
	};

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

	public void SetPlayerColor(int nPlayerID)
	{
		for (int i = 0; i < 4; i++)
		{
			imgNets[i].color = colorNets[(!bLizi) ? nPlayerID : 0];
		}
	}

	public void SetLizi(bool isLizi = false)
	{
		bLizi = isLizi;
		for (int i = 0; i < 4; i++)
		{
			imgNets[i].sprite = ((!bLizi) ? spiNormalNet : spiLiziNet);
		}
	}

	public void SetPower(int nPower, ZH2_GVars.GameType gameType)
	{
		switch (gameType)
		{
		case ZH2_GVars.GameType.mermaid_desk:
		case ZH2_GVars.GameType.fish_desk:
		case ZH2_GVars.GameType.golden_cicada_fish_desk:
			powerList = new int[3, 2]
			{
				{
					0,
					50
				},
				{
					50,
					200
				},
				{
					2147483647,
					2147483647
				}
			};
			break;
		case ZH2_GVars.GameType.bullet_fish_desk:
		case ZH2_GVars.GameType.li_kui_fish_desk:
			powerList = new int[3, 2]
			{
				{
					0,
					50
				},
				{
					50,
					200
				},
				{
					200,
					2147483647
				}
			};
			break;
		default:
			powerList = new int[3, 2]
			{
				{
					0,
					100
				},
				{
					100,
					1000
				},
				{
					1000,
					2147483647
				}
			};
			break;
		}
		if (nPower > powerList[0, 0] && nPower <= powerList[0, 1])
		{
			imgNets[0].gameObject.SetActive(value: false);
			imgNets[1].gameObject.SetActive(value: false);
			tfRings[0].gameObject.SetActive(value: false);
			tfRings[1].gameObject.SetActive(value: false);
			imgNets[2].transform.localPosition = Vector3.left * 57.5f;
			imgNets[3].transform.localPosition = Vector3.right * 57.5f;
			tfRings[2].transform.localPosition = Vector3.left * 57.5f;
			tfRings[3].transform.localPosition = Vector3.right * 57.5f;
		}
		else if (nPower > powerList[1, 0] && nPower < powerList[1, 1])
		{
			imgNets[0].gameObject.SetActive(value: false);
			imgNets[1].gameObject.SetActive(value: true);
			tfRings[0].gameObject.SetActive(value: false);
			tfRings[1].gameObject.SetActive(value: true);
			imgNets[1].transform.localPosition = Vector3.up * 57.5f;
			imgNets[2].transform.localPosition = Vector3.left * 57.5f + Vector3.down * 57.5f;
			imgNets[3].transform.localPosition = Vector3.right * 57.5f + Vector3.down * 57.5f;
			tfRings[1].transform.localPosition = Vector3.up * 57.5f;
			tfRings[2].transform.localPosition = Vector3.left * 57.5f + Vector3.down * 57.5f;
			tfRings[3].transform.localPosition = Vector3.right * 57.5f + Vector3.down * 57.5f;
		}
		else if (nPower >= powerList[2, 0])
		{
			imgNets[0].gameObject.SetActive(value: true);
			imgNets[1].gameObject.SetActive(value: true);
			tfRings[0].gameObject.SetActive(value: true);
			tfRings[1].gameObject.SetActive(value: true);
			imgNets[0].transform.localPosition = Vector3.left * 57.5f + Vector3.up * 57.5f;
			imgNets[1].transform.localPosition = Vector3.right * 57.5f + Vector3.up * 57.5f;
			imgNets[2].transform.localPosition = Vector3.left * 57.5f + Vector3.down * 57.5f;
			imgNets[3].transform.localPosition = Vector3.right * 57.5f + Vector3.down * 57.5f;
			tfRings[0].transform.localPosition = Vector3.left * 57.5f + Vector3.up * 57.5f;
			tfRings[1].transform.localPosition = Vector3.right * 57.5f + Vector3.up * 57.5f;
			tfRings[2].transform.localPosition = Vector3.left * 57.5f + Vector3.down * 57.5f;
			tfRings[3].transform.localPosition = Vector3.right * 57.5f + Vector3.down * 57.5f;
		}
		else
		{
			imgNets[0].gameObject.SetActive(value: false);
			imgNets[1].gameObject.SetActive(value: true);
			tfRings[0].gameObject.SetActive(value: false);
			tfRings[1].gameObject.SetActive(value: true);
			imgNets[1].transform.localPosition = Vector3.up * 57.5f;
			imgNets[2].transform.localPosition = Vector3.left * 57.5f + Vector3.down * 57.5f;
			imgNets[3].transform.localPosition = Vector3.right * 57.5f + Vector3.down * 57.5f;
			tfRings[1].transform.localPosition = Vector3.up * 57.5f;
			tfRings[2].transform.localPosition = Vector3.left * 57.5f + Vector3.down * 57.5f;
			tfRings[3].transform.localPosition = Vector3.right * 57.5f + Vector3.down * 57.5f;
		}
	}

	private void OnEnable()
	{
		SetImgNets();
	}

	private void SetImgNets()
	{
		for (int i = 0; i < imgNets.Length; i++)
		{
			if (imgNets[i] != null)
			{
				TweenScale component = imgNets[i].GetComponent<TweenScale>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
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
			if (ZH2_GVars.DestroyFishNet != null)
			{
				ZH2_GVars.DestroyFishNet(base.gameObject);
				return;
			}
			UnityEngine.Debug.LogError("==========注意!委托不存在!!!!");
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Shake()
	{
		base.transform.DOShakePosition(0.1f, Vector3.right * 20f + Vector3.up * 10f).SetEase(Ease.Linear).SetLoops(7)
			.OnComplete(_shakeEnd)
			.OnUpdate(_onUpdate);
		RingScaleAnim();
	}

	private void _shakeEnd()
	{
		ObjDestroy();
	}

	private void RingScaleAnim()
	{
		for (int i = 0; i < 4; i++)
		{
			int num = i;
			if (tfRings[num].gameObject.activeSelf)
			{
				tfRings[num].DOKill();
				tfRings[num].DOScale(0.5f, 0f).SetDelay(0.4f);
				tfRings[num].DOScale(0.6f, 0.2f).SetEase(Ease.Linear).SetDelay(0.4f);
				tfRings[num].DOScale(0f, 0f).SetDelay(0.6f);
			}
		}
	}

	private void _onUpdate()
	{
		if (!bLizi)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			if (imgNets[i].gameObject.activeSelf)
			{
				imgNets[i].transform.Rotate(Vector3.forward, 200f * Time.deltaTime);
			}
		}
	}
}
