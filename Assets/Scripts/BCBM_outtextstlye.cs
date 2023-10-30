using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class BCBM_outtextstlye : MonoBehaviour
{
	public GameObject leftTextListGO;

	public GameObject rightTextListGO;

	private void settextInList(string datatext, bool isleftorright, Transform tagetGO)
	{
		if (!(datatext != string.Empty))
		{
			return;
		}
		JsonData jsonData = JsonMapper.ToObject(datatext);
		if (!jsonData["code"].ToString().Equals("200"))
		{
			return;
		}
		Transform child = tagetGO.transform.GetChild(1);
		for (int i = 0; i < jsonData["List"].Count; i++)
		{
			child.GetChild(i).GetChild(0).GetComponent<Text>()
				.text = jsonData["List"][i]["dates"].ToString();
				child.GetChild(i).GetChild(1).GetChild(0)
					.GetComponent<Text>()
					.text = jsonData["List"][i]["A"].ToString();
					child.GetChild(i).GetChild(1).GetChild(1)
						.GetComponent<Text>()
						.text = jsonData["List"][i]["B"].ToString();
						child.GetChild(i).GetChild(1).GetChild(2)
							.GetComponent<Text>()
							.text = jsonData["List"][i]["C"].ToString();
							child.GetChild(i).GetChild(1).GetChild(3)
								.GetComponent<Text>()
								.text = jsonData["List"][i]["D"].ToString();
								if (isleftorright)
								{
									child.GetChild(i).GetChild(1).GetChild(4)
										.GetComponent<Text>()
										.text = jsonData["List"][i]["E"].ToString();
									}
									string[] array = jsonData["List"][i]["win_total"].ToString().Split('.');
									child.GetChild(i).GetChild(2).GetComponent<Text>()
										.text = array[0].ToString();
									}
								}

								private static string setinputtextlist(string text, int lengthcheak)
								{
									string text2 = text;
									if (text.Length <= lengthcheak)
									{
										for (int i = 0; i < lengthcheak - text.Length; i++)
										{
											text2 = ((i % 2 != 0) ? (text2 + "  ") : ("  " + text2));
										}
									}
									return text2;
								}

								public void addtexttoui(string text, bool leftorright)
								{
									if (text != string.Empty)
									{
										if (leftorright)
										{
											settextInList(text, leftorright, leftTextListGO.transform);
										}
										else
										{
											settextInList(text, leftorright, rightTextListGO.transform);
										}
									}
								}
							}
