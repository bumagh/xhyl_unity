using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class STOF_WaterWaveCtrl : MonoBehaviour
{
	[SerializeField]
	private Material[] mats;

	[SerializeField]
	private Image img;

	private WaitForSeconds wait = new WaitForSeconds(0.1f);

	private int index;

	private int len;

	private void Start()
	{
		index = 0;
		len = mats.Length;
		StartCoroutine("WaterWave");
	}

	private IEnumerator WaterWave()
	{
		while (true)
		{
			index %= len;
			img.material = mats[index];
			yield return wait;
			index++;
		}
	}
}
