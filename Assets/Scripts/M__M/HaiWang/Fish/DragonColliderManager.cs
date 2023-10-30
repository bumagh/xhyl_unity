using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace M__M.HaiWang.Fish
{
	public class DragonColliderManager : MonoBehaviour
	{
		[SerializeField]
		private Collider[] dragonColliders;

		[SerializeField]
		private Transform[] boomPosition;

		[SerializeField]
		private GameObject boomPrefab;

		[SerializeField]
		private FishBehaviour fishBehaviour;

		private void Awake()
		{
			for (int i = 0; i < dragonColliders.Length; i++)
			{
				DragonColliderInit(dragonColliders[i]);
			}
		}

		private void DragonColliderInit(Collider col)
		{
			DragonColliderControl dragonColliderControl = col.gameObject.AddComponent<DragonColliderControl>();
			dragonColliderControl.Event_DragonOnHit_Handler += Test;
		}

		private IEnumerator DragonDie()
		{
			yield return new WaitForSeconds(3f);
			base.transform.DOShakePosition(1f, new Vector3(0.5f, 0f, 0f), 1000, 20f);
			yield return new WaitForSeconds(0.5f);
			for (int i = 0; i < boomPosition.Length; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(boomPrefab);
				gameObject.transform.parent = boomPosition[i].transform;
				gameObject.transform.localPosition = boomPosition[i].transform.localPosition;
			}
		}

		private void Test(Collider c)
		{
			UnityEngine.Debug.Log(c.name);
		}
	}
}
