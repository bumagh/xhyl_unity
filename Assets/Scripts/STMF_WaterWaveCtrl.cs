using System.Collections;
using UnityEngine;

public class STMF_WaterWaveCtrl : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer srWater;

	[SerializeField]
	private Sprite[] spiWaters;

	private WaitForSeconds wait = new WaitForSeconds(0.1f);

	public int index;

	private void Start()
	{
		StartCoroutine("WaterWaveAnim");
	}

	private IEnumerator WaterWaveAnim()
	{
		while (true)
		{
			index %= spiWaters.Length;
			srWater.sprite = spiWaters[index];
			yield return wait;
			index++;
		}
	}
}
