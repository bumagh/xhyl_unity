using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DNTG_FishNet : MonoBehaviour
{
	[SerializeField]
	private Image imgNets;

	[SerializeField]
	private Image[] tfRings;

	public bool mIsDead
	{
		get;
		set;
	}

	public void SetPower(int nPlayer, int nPower, DNTG_Bullet bullet)
	{
		int index = nPlayer - 1;
		int num = 0;
		num = ((nPower > 0 && nPower <= 50) ? (IsSuperShoot(index) ? 3 : 0) : ((nPower > 50 && nPower < 200) ? ((!IsSuperShoot(index)) ? 1 : 4) : (IsSuperShoot(index) ? 5 : 2)));
		SetRing(num, bullet);
	}

	private void SetRing(int indexGun, DNTG_Bullet bullet)
	{
		for (int i = 0; i < tfRings.Length; i++)
		{
			tfRings[i].color = new Color(1f, 1f, 1f, 0f);
			tfRings[i].transform.localScale = Vector3.zero;
		}
		imgNets.transform.localScale = Vector3.zero;
		tfRings[indexGun].DOFade(1f, 0.45f);
		imgNets.DOFade(1f, 0.45f);
		imgNets.transform.DOScale(Vector3.one, 0.5f);
		tfRings[indexGun].transform.DOScale(Vector3.one, 0.5f).OnComplete(delegate
		{
			tfRings[indexGun].transform.DOScale(Vector3.one * 1.2f, 0.35f);
			base.transform.DOScale(Vector3.one, 0.25f).OnComplete(delegate
			{
				tfRings[indexGun].DOFade(0f, 0.5f);
				imgNets.DOFade(0f, 0.5f);
			});
		});
	}

	private bool IsSuperShoot(int index)
	{
		return DNTG_GameInfo.getInstance().IsSuperShoot[index];
	}

	public void OnSpawned()
	{
		mIsDead = false;
		Shake();
	}

	public void ObjDestroy()
	{
		if (!mIsDead)
		{
			mIsDead = true;
			DNTG_FishNetpoolMngr.GetSingleton().DestroyFishNet(base.gameObject);
		}
	}

	public void Shake()
	{
		base.transform.DOScale(Vector3.one, 1f).SetEase(Ease.Linear).OnComplete(_shakeEnd)
			.OnUpdate(_onUpdate);
		RingScaleAnim();
	}

	private void _shakeEnd()
	{
		ObjDestroy();
	}

	private void RingScaleAnim()
	{
	}

	private void _onUpdate()
	{
	}
}
