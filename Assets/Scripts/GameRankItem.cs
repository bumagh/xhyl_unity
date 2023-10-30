using UnityEngine;
using UnityEngine.UI;

public class GameRankItem : MonoBehaviour
{
	public void Init(TopRank tr)
	{
		base.transform.Find("Nickname").GetComponent<Text>().text = tr.nickname;
		base.transform.Find("Value").GetComponent<Text>().text = ((ZH2_GVars.language_enum == ZH2_GVars.LanguageType.English) ? $"{tr.gold}" : ((ZH2_GVars.language_enum != 0) ? $"{tr.gold}" : $"{tr.gold}"));
		base.transform.Find("Type").GetComponent<Text>().text = tr.awardName;
		base.transform.Find("Time").GetComponent<Text>().text = tr.datetime;
	}
}
