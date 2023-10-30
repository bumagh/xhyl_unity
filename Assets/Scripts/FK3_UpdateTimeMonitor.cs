using UnityEngine;

public class FK3_UpdateTimeMonitor : MonoBehaviour
{
	private float _lastTime = -1f;

	public float maxDeltaTime = 0.1f;

	private void Start()
	{
	}

	private void Update()
	{
		if (_lastTime < 0f)
		{
			_lastTime = Time.time;
			return;
		}
		float num = Time.time - _lastTime;
		if (num > maxDeltaTime)
		{
			UnityEngine.Debug.Log(FK3_LogHelper.Magenta("deltaTime is beyond standard value. actualDelta:{0}, Time.deltaTime:{1}", num, Time.deltaTime));
		}
		_lastTime = Time.time;
	}
}
