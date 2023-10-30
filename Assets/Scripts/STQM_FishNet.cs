using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class STQM_FishNet : MonoBehaviour
{
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
		new Color(1f, 1f, 1f, 0.5f),
		new Color(248f / 255f, 242f / 255f, 86f / 255f, 1f),
		new Color(82f / 85f, 33f / 85f, 223f / 255f, 1f),
		new Color(22f / 85f, 0.4f, 172f / 255f, 1f),
		new Color(33f / 85f, 116f / 255f, 28f / 85f, 1f)
	};

	private bool _isDead;

	private bool bLizi;

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

	public void SetLizi(bool isLizi)
	{
		bLizi = isLizi;
		for (int i = 0; i < 4; i++)
		{
			imgNets[i].sprite = ((!bLizi) ? spiNormalNet : spiLiziNet);
		}
	}

	public void SetPower(int nPower)
	{
		if (nPower > 0 && nPower < 100)
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
		else if (nPower >= 100 && nPower < 1000)
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
		else if (nPower >= 1000)
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
			STQM_FishNetpoolMngr.GetSingleton().DestroyFishNet(base.gameObject);
		}
	}

	public void Shake()
	{
		base.transform.DOShakePosition(0.6f, Vector3.right * 10f + Vector3.up * 10f).SetEase(Ease.Linear).OnComplete(_shakeEnd)
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
