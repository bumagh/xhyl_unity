using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.Effect
{
	public class FK3_Effect_Logo_BossKrankenShadow : FK3_Effect_Logo
	{
		[SerializeField]
		private Animator animator;

		[SerializeField]
		private float speed = 0.7f;

		private List<Vector2> oldPos = new List<Vector2>();

		private void Awake()
		{
			oldPos = new List<Vector2>();
			animator.speed = speed;
			for (int i = 0; i < base.transform.childCount; i++)
			{
				oldPos.Add(base.transform.GetChild(i).localPosition);
			}
		}

		public override void Reset_Logo()
		{
			base.Reset_Logo();
			DOTween.Kill(base.transform);
			for (int i = 0; i < base.transform.childCount; i++)
			{
				base.transform.GetChild(i).localPosition = oldPos[i];
			}
		}

		public override void Play(Vector3 pos, Vector3 posAdd, int seatId, int[] values)
		{
			Vector3 position = base.transform.position;
			float num = pos.z = position.z;
			base.gameObject.SetActive(value: true);
			base.transform.position = pos;
			showTask = new FK3_Task(Show(pos));
		}

		private IEnumerator Show(Vector3 pos)
		{
			float delta = -2f * pos.x / 4f;
			float targetX = pos.x + delta;
			for (int i = 0; i < 4; i++)
			{
				base.transform.DOMoveX(targetX, 1f);
				targetX += delta;
				try
				{
					for (int j = 0; j < base.transform.childCount; j++)
					{
						Transform child = base.transform.GetChild(j);
						Vector2 vector = oldPos[j];
						float x = vector.x + (float)Random.Range(-4, 5);
						Vector2 vector2 = oldPos[j];
						child.DOLocalMove(new Vector2(x, vector2.y + (float)Random.Range(-4, 5)), 1.4f);
					}
				}
				catch
				{
				}
				yield return new WaitForSeconds(1.5f);
			}
			Over();
		}
	}
}
