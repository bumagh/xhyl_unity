using System;
using UnityEngine;

[Serializable]
public class GameSound : MonoBehaviour
{
	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = 1f;
}
