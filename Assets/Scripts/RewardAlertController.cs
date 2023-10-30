using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RewardAlertController : MonoBehaviour
{
	[SerializeField]
	private Image rewardType;

	[SerializeField]
	private Text rewardText;

	[SerializeField]
	private Image[] rewardImage;

	public void ShowReward(int type, int count)
	{
		base.gameObject.SetActive(value: true);
		rewardText.text = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? $"Gold X{count}" : ((ZH2_GVars.language_enum != 0) ? $"ช\u0e37\u0e48อเกม X{count}" : $"游戏币 X{count}"));
		StartCoroutine(Diappear(2f));
	}

	public void BeClick()
	{
		base.gameObject.SetActive(value: false);
	}

	private IEnumerator Diappear(float t)
	{
		yield return new WaitForSeconds(t);
		base.gameObject.SetActive(value: false);
	}
}
