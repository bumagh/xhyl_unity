using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class STTF_FishNet : MonoBehaviour
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
		new Color(1f, 1f, 0.7058824f, 1f),
		new Color(1f, 41f / 51f, 1f, 1f),
		new Color(0.5490196f, 0.8235294f, 1f, 1f),
		new Color(11f / 15f, 226f / 255f, 11f / 15f, 1f)
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
			STTF_FishNetpoolMngr.GetSingleton().DestroyFishNet(base.gameObject);
		}
	}

	public void Shake()
	{
		base.transform.DOShakePosition(0.6f, Vector3.one * 20f, 40, 360f).OnComplete(_shakeEnd).OnUpdate(_onUpdate);
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
