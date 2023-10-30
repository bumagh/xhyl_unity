using UnityEngine;

public class LuckyLightningAnim : MonoBehaviour
{
	private void Start()
	{
	}

	private void OnEnable()
	{
		iTween.MoveTo(base.gameObject, iTween.Hash("position", base.transform.position + Vector3.up, "time", 0.5f, "easetype", iTween.EaseType.easeInOutSine));
		iTween.MoveTo(base.gameObject, iTween.Hash("position", base.transform.position, "time", 0.5f, "delay", 0.5f, "easetype", iTween.EaseType.easeInOutSine));
		iTween.RotateTo(base.gameObject, iTween.Hash("rotation", Vector3.up * 720f + base.transform.localEulerAngles, "delay", 1, "time", 0.5f, "easetype", iTween.EaseType.easeInOutSine));
	}

	private void Update()
	{
	}
}
