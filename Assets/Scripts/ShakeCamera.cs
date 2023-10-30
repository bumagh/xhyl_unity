using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
	private float shakeLevel = 1.5f;

	public float setShakeTime = 0.5f;

	public float shakeFps = 45f;

	private bool isshakeCamera;

	private float fps;

	private float shakeTime;

	private float frameTime;

	private float shakeDelta = 0.005f;

	private Camera selfCamera;

	private void OnEnable()
	{
		isshakeCamera = true;
		selfCamera = base.gameObject.GetComponent<Camera>();
		shakeTime = setShakeTime;
		fps = shakeFps;
		frameTime = 0.03f;
		shakeDelta = 0.005f;
		base.enabled = false;
	}

	private void OnDisable()
	{
		selfCamera.rect = new Rect(0f, 0f, 1f, 1f);
		isshakeCamera = false;
	}

	private void Update()
	{
		if (!isshakeCamera || !(shakeTime > 0f))
		{
			return;
		}
		shakeTime -= Time.deltaTime;
		if (shakeTime <= 0f)
		{
			base.enabled = false;
			return;
		}
		frameTime += Time.deltaTime;
		if ((double)frameTime > 1.0 / (double)fps)
		{
			frameTime = 0f;
			selfCamera.rect = new Rect(shakeDelta * (-1f + shakeLevel * UnityEngine.Random.value), shakeDelta * (-1f + shakeLevel * UnityEngine.Random.value), 1f, 1f);
		}
	}
}
