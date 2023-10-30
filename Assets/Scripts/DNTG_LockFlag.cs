using UnityEngine;
using UnityEngine.UI;

public class DNTG_LockFlag : MonoBehaviour
{
	[HideInInspector]
	public int seatId;

	private SpriteRenderer imgLockFlag;

	private Image imgLockFlag2;

	[SerializeField]
	private DNTG_LockFlagSpis sptLockFlagSpis;

	private Color[] colLock = new Color[4]
	{
		new Color(1f, 146f / 255f, 0f, 1f),
		new Color(0.8235294f, 0f, 1f, 1f),
		new Color(0f, 32f / 255f, 248f / 255f, 1f),
		new Color(0f, 1f, 7f / 85f, 1f)
	};

	private void Awake()
	{
		imgLockFlag = GetComponent<SpriteRenderer>();
		imgLockFlag2 = GetComponent<Image>();
	}

	public void InitLockFlag()
	{
		if (imgLockFlag == null)
		{
			imgLockFlag = GetComponent<SpriteRenderer>();
		}
		if (seatId <= 0)
		{
			seatId = 1;
		}
		if (imgLockFlag != null)
		{
			imgLockFlag.sprite = sptLockFlagSpis.spiFishLockFlags[seatId - 1];
			imgLockFlag.color = colLock[seatId - 1];
			return;
		}
		if (imgLockFlag2 == null)
		{
			imgLockFlag2 = GetComponent<Image>();
		}
		if (imgLockFlag2 != null)
		{
			imgLockFlag2.sprite = sptLockFlagSpis.spiFishLockFlags[seatId - 1];
			imgLockFlag2.color = colLock[seatId - 1];
		}
	}

	private void Update()
	{
		if (DNTG_GameInfo.getInstance() != null && (DNTG_GameInfo.getInstance().User.SeatIndex == 3 || DNTG_GameInfo.getInstance().User.SeatIndex == 4))
		{
			base.transform.eulerAngles = Vector3.forward * 180f;
		}
		else
		{
			base.transform.eulerAngles = Vector3.zero;
		}
	}
}
