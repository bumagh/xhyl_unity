using System;
using System.Collections;
using UnityEngine;

namespace PathologicalGames
{
	[AddComponentMenu("")]
	[ExecuteInEditMode]
	public class ConstraintFrameworkBaseClass : MonoBehaviour
	{
		protected virtual void Awake()
		{
		}

		protected virtual void OnEnable()
		{
			InitConstraint();
		}

		protected virtual void OnDisable()
		{
			StopCoroutine("Constrain");
		}

		protected virtual void InitConstraint()
		{
			StartCoroutine("Constrain");
		}

		protected virtual IEnumerator Constrain()
		{
			while (true)
			{
				OnConstraintUpdate();
				yield return null;
			}
		}

		protected virtual void OnConstraintUpdate()
		{
			throw new NotImplementedException();
		}
	}
}
