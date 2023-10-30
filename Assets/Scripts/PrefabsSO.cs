using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PrefabsSO : ScriptableObject
{
	public List<GameObject> prefabs;

	public GameObject this[int index] => prefabs[index];
}
