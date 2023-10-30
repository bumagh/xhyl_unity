using DG.Tweening;
using UnityEngine;

public class STQM_LockFlag : MonoBehaviour
{
	public int seatId;

	[SerializeField]
	private SpriteRenderer srLockFlag;

	[SerializeField]
	private STQM_LockFlagSpis sptLockFlagSpis;

	private Color[] colLock = new Color[4]
	{
		new Color(1f, 146f / 255f, 0f, 1f),
		new Color(0.8235294f, 0f, 1f, 1f),
		new Color(0f, 32f / 255f, 248f / 255f, 1f),
		new Color(0f, 1f, 7f / 85f, 1f)
	};

	public void InitLockFlag()
	{
		srLockFlag.sprite = sptLockFlagSpis.spiFishLockFlags[seatId - 1];
		srLockFlag.color = colLock[seatId - 1];
	}

	private void Awake()
	{
		base.transform.DOLocalRotate(Vector3.back * 90f + Vector3.down * 90f, 0.5f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
	}
}
