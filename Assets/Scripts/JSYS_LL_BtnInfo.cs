using UnityEngine;
using UnityEngine.UI;

public class JSYS_LL_BtnInfo : MonoBehaviour
{
	public new string name = string.Empty;

	public string minGlod = string.Empty;

	public string onlinePeople = string.Empty;

	public int hallId;

	public int hallType = 1;

	public Text nameText;

	public Text minGoldText;

	public Text onlinePeopleText;

	public void UpdateText()
	{
		nameText.text = name;
		minGoldText.text = ZH2_GVars.ShowTip($"最少携带: {minGlod}金币", $"MiniCarry: {minGlod}Gold", string.Empty);
		onlinePeopleText.text = onlinePeople;
	}
}
