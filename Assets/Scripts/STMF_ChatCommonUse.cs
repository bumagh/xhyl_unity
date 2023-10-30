using UnityEngine;
using UnityEngine.UI;

public class STMF_ChatCommonUse : MonoBehaviour
{
	private int count = 10;

	private int language;

	[HideInInspector]
	public Button[] btnCommonUse;

	[HideInInspector]
	public Text[] txtCommonUse;

	[HideInInspector]
	public string[] words = new string[20]
	{
		"兄台高人啊，佩服佩服。",
		"有缘千里来相会，交个朋友吧！",
		"放开那龙龟，看我手到擒来。",
		"哈哈，这些都是我的囊中之物。",
		"嘿嘿，总算被我逮到条大的。",
		"这么小的鱼，回家炖汤都不够啊！",
		"为何大鱼总是与我擦肩而过？",
		"宝刀屠龙在我手，小样看你往哪游！",
		"青山不改绿水长流，大家后会有期。",
		"不要走，决战到天亮！",
		"It's so enjoyable to play with you.",
		"Don't go away, let's fight until tomorrow.",
		"Fuck! Don't let me over limit, I don't like cash out.",
		"The Dragon turtle can not escape from my hand.",
		"The Luck Fairy has forgotten me.",
		"Keep on going, never give up.",
		"I'm very pleased to play together with you.",
		"Wait for me, I'll be back.",
		"I kown you want to kill two birds with one stone.",
		"Winning is unimportant, enjoy it!"
	};

	private void Awake()
	{
		int num = language * count;
		btnCommonUse = new Button[count];
		txtCommonUse = new Text[count];
		for (int i = 0; i < count; i++)
		{
			btnCommonUse[i] = base.transform.GetChild(i).GetComponent<Button>();
			txtCommonUse[i] = btnCommonUse[i].transform.Find("Text").GetComponent<Text>();
			txtCommonUse[i].text = words[num + i];
		}
	}
}
