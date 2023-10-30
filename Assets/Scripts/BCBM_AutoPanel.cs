using UnityEngine;

public class BCBM_AutoPanel : MonoBehaviour
{
	public void ClickOk()
	{
		base.gameObject.SetActive(value: false);
		BCBM_BetScene.publicBetScene.AutoGame();
	}
}
