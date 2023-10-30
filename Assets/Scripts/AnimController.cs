using UnityEngine;

public class AnimController : MonoBehaviour
{
	[SerializeField]
	private GameObject _anim;

	private Animator ani;

	private void OnEnable()
	{
		ani = _anim.transform.GetComponent<Animator>();
		ani.ResetTrigger("Show");
		ani.SetTrigger("Show");
	}
}
