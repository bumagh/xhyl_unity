using UnityEngine;
using UnityEngine.UI;

public class RankItem : MonoBehaviour
{
	private Text m_textCoin;

	private GameObject m_imageCoin;

	public void Init(UserIconDataConfig uIdc, WealthLevel tr, int type)
	{
		if (tr.photoId < 0 || tr.photoId >= uIdc.list.Count)
		{
			tr.photoId = 0;
		}
		base.transform.Find("Head").GetComponent<Image>().sprite = uIdc.list[tr.photoId].sprite;
		base.transform.Find("Nickname").GetComponent<Text>().text = tr.nickname;
		m_textCoin = base.transform.Find("Value").GetComponent<Text>();
		m_imageCoin = base.transform.Find("Wealth").gameObject;
		if (type == 1)
		{
			m_textCoin.text = tr.gameGoldOrLevel.ToString();
			m_imageCoin.SetActive(value: true);
		}
		if (type == 2)
		{
			m_textCoin.text = tr.gameGoldOrLevel + "级";
			m_imageCoin.SetActive(value: false);
		}
	}
}
