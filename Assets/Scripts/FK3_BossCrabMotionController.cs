using DG.Tweening;
using UnityEngine;

public class FK3_BossCrabMotionController : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void Move()
	{
		Tweener tweener = base.transform.DOLocalMove(new Vector3(5f, 0f, 0f), 2f);
		Tweener tweener2 = base.transform.DOLocalMove(new Vector3(-5f, 0f, 0f), 2f);
	}
}
