using DP_GameCommon;
using UnityEngine;

public class DP_SpinCtrl : MonoBehaviour
{
	private float acceleration = 60f;

	public float maxSpinSpeed = 260f;

	public SPIN_STATE spinState = SPIN_STATE.ST_STOP;

	private int no = -1;

	private float curSpinSpeed;

	private bool bClockwise = true;

	private bool bSpin;

	private bool bGoToStop;

	private float fTotalSpinTime;

	private float fTotalGoToStopTime;

	private float aglDistance;

	private float fCurSpinTime;

	public void Reset()
	{
		curSpinSpeed = 0f;
		bSpin = false;
		bGoToStop = false;
		fTotalSpinTime = 0f;
		fTotalGoToStopTime = 0f;
		aglDistance = 0f;
		fCurSpinTime = 0f;
	}

	public void SpinTo(int nNo, float time, bool isClockwise = true)
	{
		Reset();
		spinState = SPIN_STATE.ST_ACCELERATION;
		no = nNo;
		bClockwise = isClockwise;
		curSpinSpeed = 0f;
		fTotalSpinTime = time;
		fCurSpinTime = 0f;
		bSpin = true;
	}

	public void GoToStop(float timeLen, float speed = 100f)
	{
		fTotalGoToStopTime = timeLen;
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		float y = localEulerAngles.y;
		float num = 2f;
		if (bClockwise)
		{
			float num2 = ((float)no * 360f / (float)LL_Parameter.G_nAnimalNumber - y + 360f) % 360f;
			num = (int)(timeLen * speed) / 360;
			aglDistance = num2 + num * 360f;
		}
		else
		{
			float num3 = (y - (float)no * 360f / (float)LL_Parameter.G_nAnimalNumber + 360f) % 360f;
			aglDistance = num3 + num * 360f;
		}
		bGoToStop = true;
		bSpin = false;
	}

	private void spinUpdate()
	{
		fCurSpinTime += Time.deltaTime;
		if (fCurSpinTime > fTotalSpinTime / 3f)
		{
			GoToStop(fTotalSpinTime - fCurSpinTime);
			return;
		}
		if (curSpinSpeed < maxSpinSpeed)
		{
			curSpinSpeed += acceleration * Time.deltaTime;
		}
		float rotation = getRotation(curSpinSpeed, bClockwise);
		base.transform.Rotate(Vector3.up * rotation, Space.Self);
	}

	private void gotoStopUpdate()
	{
		if (fTotalGoToStopTime > 0f)
		{
			float num = 2f * aglDistance / (fTotalGoToStopTime * fTotalGoToStopTime);
			curSpinSpeed = num * fTotalGoToStopTime;
			float rotation = getRotation(curSpinSpeed, bClockwise);
			base.transform.Rotate(Vector3.up * rotation, Space.Self);
			aglDistance -= Mathf.Abs(rotation);
			fTotalGoToStopTime -= Time.deltaTime;
		}
		else
		{
			bGoToStop = false;
			spinState = SPIN_STATE.ST_STOP;
			SetAglToNo(no);
		}
	}

	private float getRotation(float speed, bool isClockwize)
	{
		if (isClockwize)
		{
			return speed * Time.deltaTime;
		}
		return (0f - speed) * Time.deltaTime;
	}

	public bool IsStop()
	{
		return spinState == SPIN_STATE.ST_STOP;
	}

	public void SetAglToNo(int nNo)
	{
		float d = (float)nNo * 15f;
		base.transform.localEulerAngles = Vector3.up * d;
	}

	private void Update()
	{
		if (bSpin)
		{
			spinUpdate();
		}
		if (bGoToStop)
		{
			gotoStopUpdate();
		}
	}
}
