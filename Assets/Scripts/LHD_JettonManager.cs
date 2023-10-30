using System.Collections.Generic;
using UnityEngine;

public class LHD_JettonManager : MonoBehaviour
{
	public GameObject Jetton;

	public Queue<GameObject> JettonQueue = new Queue<GameObject>();

	public List<GameObject> UseList = new List<GameObject>();

	private void CreateJetton()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Jetton, base.transform);
		gameObject.SetActive(value: false);
		JettonQueue.Enqueue(gameObject);
	}

	public GameObject GetUseJetton(Transform pre)
	{
		if (JettonQueue.Count <= 1)
		{
			CreateJetton();
		}
		GameObject gameObject = JettonQueue.Dequeue();
		gameObject.transform.SetParent(pre);
		gameObject.SetActive(value: true);
		UseList.Add(gameObject);
		return gameObject;
	}

	public void ReMoveUseJetton(GameObject obj)
	{
		obj.transform.SetParent(base.transform);
		obj.SetActive(value: false);
		JettonQueue.Enqueue(obj);
		UseList.Remove(obj);
	}

	public void RecycleJetton()
	{
		for (int i = 0; i < UseList.Count; i++)
		{
			UseList[i].transform.SetParent(base.transform);
			UseList[i].SetActive(value: false);
			JettonQueue.Enqueue(UseList[i]);
		}
		UseList.Clear();
	}
}
