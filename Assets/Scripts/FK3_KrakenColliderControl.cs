using DG.Tweening;
using UnityEngine;

public class FK3_KrakenColliderControl : MonoBehaviour
{
	public delegate void EventHandler_KrakenOnHit(Collider collider);

	[SerializeField]
	private GameObject SightBeadPrefab;

	[SerializeField]
	private float changeTime = 0.2f;

	[SerializeField]
	private float speed = 0.1f;

	public EventHandler_KrakenOnHit Event_KrakenOnHit;

	private void OnTriggerEnter(Collider collider)
	{
		UnityEngine.Debug.Log("kraken be shoot!");
		OnKrakenBeHit();
	}

	private void Update()
	{
		base.transform.Rotate(Vector3.forward * speed * Time.deltaTime);
	}

	private void OnKrakenBeHit()
	{
		GameObject SightBead = UnityEngine.Object.Instantiate(SightBeadPrefab);
		SightBead.transform.parent = base.transform;
		SightBead.transform.localPosition = base.transform.localPosition;
		SightBead.transform.DOScale(Vector3.one * 1.4f, changeTime).SetEase(Ease.OutQuad);
		SightBead.GetComponent<SpriteRenderer>().DOColor(new Color(0f, 0f, 0f, 0f), changeTime * 5f).SetEase(Ease.OutQuad)
			.OnComplete(delegate
			{
				UnityEngine.Object.Destroy(SightBead);
			});
	}
}
