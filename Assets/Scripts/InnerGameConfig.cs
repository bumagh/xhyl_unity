using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InnerGameConfig : ScriptableObject
{
	public List<InnerGame> list;

	public InnerGame this[int index] => list[index];
}
