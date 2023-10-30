using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace SWS
{
	public class MoveAnimator : MonoBehaviour
	{
		private splineMove sMove;

		private NavMeshAgent nAgent;

		private Animator animator;

		private float lastRotY;

		private void Start()
		{
			animator = GetComponentInChildren<Animator>();
			sMove = GetComponent<splineMove>();
			if (!sMove)
			{
				nAgent = GetComponent<NavMeshAgent>();
			}
		}

		private void OnAnimatorMove()
		{
			float num = 0f;
			float num2 = 0f;
			if ((bool)sMove)
			{
				num = ((sMove.tween != null && sMove.tween.IsPlaying()) ? sMove.speed : 0f);
				Vector3 eulerAngles = base.transform.eulerAngles;
				num2 = (eulerAngles.y - lastRotY) * 10f;
				Vector3 eulerAngles2 = base.transform.eulerAngles;
				lastRotY = eulerAngles2.y;
			}
			else
			{
				num = nAgent.velocity.magnitude;
				Vector3 vector = Quaternion.Inverse(base.transform.rotation) * nAgent.desiredVelocity;
				num2 = Mathf.Atan2(vector.x, vector.z) * 180f / 3.14159f;
			}
			animator.SetFloat("Speed", num);
			animator.SetFloat("Direction", num2, 0.15f, Time.deltaTime);
		}
	}
}
