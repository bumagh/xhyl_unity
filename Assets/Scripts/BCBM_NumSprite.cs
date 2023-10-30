using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BCBM_NumSprite : MonoBehaviour
{
	public bool isChoose;

	private IEnumerator ChooseSprite(float time)
	{
		while (isChoose)
		{
			int randomNum = UnityEngine.Random.Range(0, 10);
			base.transform.GetComponent<Image>().sprite = BCBM_NumSpriteControl.Instances.num_Sprite[randomNum];
			yield return new WaitForSeconds(time);
			if (!isChoose)
			{
				break;
			}
		}
	}

	private void Update()
	{
	}
}
