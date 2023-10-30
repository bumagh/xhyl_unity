using UnityEngine;

public abstract class DK_IMoveObj : MonoBehaviour
{
	private Vector3 _speed;

	private int _nPlayerID;

	private float _fLimitUp = 3.4f;

	private float _fLimitDown = -3.4f;

	private float _fLimitLeft = 0f - DK_SetCanvas.Width * 6.2f / 1280f;

	private float _fLimitRight = DK_SetCanvas.Width * 6.2f / 1280f;

	private float rot_cos;

	private float rot_sin;

	private bool bLokingZiZan;

	public Vector3 mSpeed
	{
		get
		{
			return _speed;
		}
		set
		{
			_speed = value;
		}
	}

	public int mPlayerID
	{
		get
		{
			return _nPlayerID;
		}
		set
		{
			_nPlayerID = value;
		}
	}

	public bool IsLocking
	{
		get
		{
			return bLokingZiZan;
		}
		set
		{
			bLokingZiZan = value;
		}
	}

	private void Start()
	{
		_fLimitLeft = 0f - DK_SetCanvas.Width * 6.2f / 1280f;
		_fLimitRight = DK_SetCanvas.Width * 6.2f / 1280f;
	}

	public void MoveLogic()
	{
		Vector3 position = base.transform.position;
		if (position.x > _fLimitRight && _speed.x > 0f)
		{
			_speed.x *= -1f;
			SetRotation(_speed);
		}
		else
		{
			Vector3 position2 = base.transform.position;
			if (position2.y > _fLimitUp && _speed.y > 0f)
			{
				_speed.y *= -1f;
				SetRotation(_speed);
			}
			else
			{
				Vector3 position3 = base.transform.position;
				if (position3.x < _fLimitLeft && _speed.x < 0f)
				{
					_speed.x *= -1f;
					SetRotation(_speed);
				}
				else
				{
					Vector3 position4 = base.transform.position;
					if (position4.y < _fLimitDown && _speed.y < 0f)
					{
						_speed.y *= -1f;
						SetRotation(_speed);
					}
				}
			}
		}
		if (!bLokingZiZan)
		{
			base.transform.Translate(_speed * Time.deltaTime, Space.World);
			return;
		}
		for (int i = 0; i < DK_GameInfo.getInstance().UserList.Count; i++)
		{
			if (DK_GameInfo.getInstance().UserList[i].SeatIndex != _nPlayerID)
			{
				continue;
			}
			if ((bool)DK_GameInfo.getInstance().UserList[i].LockFish)
			{
				DK_ISwimObj swimObjByTag = DK_FishPoolMngr.GetSingleton().GetSwimObjByTag(DK_GameInfo.getInstance().UserList[i].LockFish);
				if (DK_GameInfo.getInstance().UserList[i].Lock && !swimObjByTag.bFishDead)
				{
					Vector3 lockFishPos = swimObjByTag.GetLockFishPos();
					Vector3 position5 = base.transform.position;
					rot_cos = (lockFishPos.x - position5.x) / Mathf.Sqrt((position5.x - lockFishPos.x) * (position5.x - lockFishPos.x) + (position5.y - lockFishPos.y) * (position5.y - lockFishPos.y));
					rot_sin = (lockFishPos.y - position5.y) / Mathf.Sqrt((position5.x - lockFishPos.x) * (position5.x - lockFishPos.x) + (position5.y - lockFishPos.y) * (position5.y - lockFishPos.y));
					float num = (lockFishPos.x - position5.x) / (lockFishPos.y - position5.y);
					float num2 = Mathf.Acos(rot_cos);
					float num3 = num2 * 180f / 3.14159f;
					if (lockFishPos.y < position5.y)
					{
						if (lockFishPos.x < position5.x)
						{
							num3 -= 180f;
						}
						else
						{
							num3 += 180f;
						}
					}
					Vector3 position6 = position5;
					position6.x += Mathf.Sqrt(_speed.x * _speed.x + _speed.y * _speed.y) * Time.deltaTime * rot_cos;
					position6.y += Mathf.Sqrt(_speed.x * _speed.x + _speed.y * _speed.y) * Time.deltaTime * rot_sin;
					base.gameObject.transform.position = position6;
				}
				else
				{
					bLokingZiZan = false;
				}
			}
			else
			{
				bLokingZiZan = false;
			}
		}
	}

	public void SetRotation(Vector3 dir)
	{
		Quaternion quaternion = default(Quaternion);
		quaternion.SetFromToRotation(Vector3.up, dir);
		Transform transform = base.transform;
		Vector3 forward = Vector3.forward;
		Vector3 eulerAngles = quaternion.eulerAngles;
		transform.localEulerAngles = forward * eulerAngles.z;
	}

	public void SetRotation(float fAgl)
	{
		base.transform.localEulerAngles = Vector3.forward * fAgl;
	}
}
