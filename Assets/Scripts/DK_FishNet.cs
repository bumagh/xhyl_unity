using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DK_FishNet : MonoBehaviour
{
	private int nPlayerID;

	[SerializeField]
	private Image[] imgNets;

	[SerializeField]
	private Transform[] tfRings;

	[SerializeField]
	private Sprite spiLiziNet;

	[SerializeField]
	private Sprite[] spiNormalNet;

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
		nPlayerID--;
		if (nPlayerID < 0 || nPlayerID >= spiNormalNet.Length)
		{
			nPlayerID = 0;
		}
		this.nPlayerID = nPlayerID;
		for (int i = 0; i < 4; i++)
		{
			imgNets[i].sprite = spiNormalNet[(!bLizi) ? nPlayerID : 0];
		}
	}

	public void SetLizi(bool isLizi)
	{
		bLizi = isLizi;
		if (nPlayerID < 0 || nPlayerID >= spiNormalNet.Length)
		{
			nPlayerID = 0;
		}
		for (int i = 0; i < 4; i++)
		{
			imgNets[i].sprite = ((!bLizi) ? spiNormalNet[nPlayerID] : spiLiziNet);
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
			DK_FishNetpoolMngr.GetSingleton().DestroyFishNet(base.gameObject);
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
