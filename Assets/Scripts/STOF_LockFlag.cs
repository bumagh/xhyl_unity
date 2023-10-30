using UnityEngine;

public class STOF_LockFlag : MonoBehaviour
{
	public int seatId;

	[SerializeField]
	private SpriteRenderer imgLockFlag;

	[SerializeField]
	private STOF_LockFlagSpis sptLockFlagSpis;

	private Color[] colLock = new Color[4]
	{
		new Color(1f, 146f / 255f, 0f, 1f),
		new Color(0.8235294f, 0f, 1f, 1f),
		new Color(0f, 32f / 255f, 248f / 255f, 1f),
		new Color(0f, 1f, 7f / 85f, 1f)
	};

	public void InitLockFlag()
	{
		imgLockFlag.sprite = sptLockFlagSpis.spiFishLockFlags[seatId - 1];
		imgLockFlag.color = colLock[seatId - 1];
	}

	private void Update()
	{
		if (STOF_GameInfo.getInstance().User.SeatIndex == 3 || STOF_GameInfo.getInstance().User.SeatIndex == 4)
		{
			base.transform.eulerAngles = Vector3.forward * 180f;
		}
		else
		{
			base.transform.eulerAngles = Vector3.zero;
		}
	}
}
