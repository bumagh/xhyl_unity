using UnityEngine;

public abstract class STMF_IMoveObj : MonoBehaviour
{
	private Vector3 _speed;

	private float _fLimitUp = 3.4f;

	private float _fLimitDown = -3.4f;

	private float _fLimitLeft = 0f - STMF_SetCanvas.Width * 6.2f / 1280f;

	private float _fLimitRight = STMF_SetCanvas.Width * 6.2f / 1280f;

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

	private void Start()
	{
		_fLimitLeft = 0f - STMF_SetCanvas.Width * 6.2f / 1280f;
		_fLimitRight = STMF_SetCanvas.Width * 6.2f / 1280f;
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
		base.transform.Translate(_speed * Time.deltaTime, Space.World);
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
