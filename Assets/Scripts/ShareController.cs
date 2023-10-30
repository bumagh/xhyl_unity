using UnityEngine;
using UnityEngine.UI;

public class ShareController : MonoBehaviour
{
	private Button[] btns = new Button[7];

	private GameObject[] goImgs = new GameObject[7];

	private GameObject[] contents = new GameObject[7];

	private int curIndex;

	private void Awake()
	{
		for (int i = 0; i < 7; i++)
		{
			btns[i] = base.transform.Find("ShareButtons").GetChild(i).GetComponent<Button>();
			goImgs[i] = btns[i].transform.Find("Image").gameObject;
			contents[i] = base.transform.Find("Content").GetChild(i).gameObject;
			int temp = i;
			btns[i].onClick.AddListener(delegate
			{
				RechargeButtonToggle(temp);
			});
		}
	}

	private void OnEnable()
	{
		curIndex = -1;
		RechargeButtonToggle(0);
	}

	private void RechargeButtonToggle(int i)
	{
		if (curIndex != i)
		{
			goImgs[curIndex].SetActive(value: false);
			contents[curIndex].SetActive(value: false);
			curIndex = i;
			goImgs[curIndex].SetActive(value: true);
			contents[curIndex].SetActive(value: true);
		}
	}
}
