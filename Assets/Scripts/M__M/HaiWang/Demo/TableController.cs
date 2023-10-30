using M__M.HaiWang.GameDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Demo
{
	public class TableController : SimpleSingletonBehaviour<TableController>
	{
		private static TableController instance;

		private Action<int> chooseTableAct;

		private List<DeskInfo> deskInfo = new List<DeskInfo>();

		public List<GameObject> itemList = new List<GameObject>();

		private Button btnArrow;

		private Button btnParent;

		private RectTransform rtContent;

		private GameObject ScrollView;

		[HideInInspector]
		public List<Button> btnChilds = new List<Button>();

		private List<GameObject> objChoose = new List<GameObject>();

		[HideInInspector]
		public List<Text> txtTableName = new List<Text>();

		[HideInInspector]
		public List<Text> txtTableInfo = new List<Text>();

		private float offset = 50f;

		private int activeCount;

		private int chooseIndex = -1;

		private WaitForSeconds wait = new WaitForSeconds(0.02f);

		[HideInInspector]
		public bool isOpening
		{
			get;
			private set;
		}

		[HideInInspector]
		public bool isCanClick
		{
			get;
			set;
		}

		public new static TableController Get()
		{
			return instance;
		}

		private void Awake()
		{
			instance = this;
			SimpleSingletonBehaviour<TableController>.s_instance = this;
			isOpening = false;
			isCanClick = true;
			btnArrow = base.transform.Find("BtnArrow").GetComponent<Button>();
			ScrollView = base.transform.Find("Scroll View").gameObject;
			rtContent = base.transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();
			for (int i = 0; i < 10; i++)
			{
				btnChilds.Add(rtContent.GetChild(i).GetComponent<Button>());
				objChoose.Add(rtContent.GetChild(i).Find("BgChoose").gameObject);
				txtTableName.Add(rtContent.GetChild(i).Find("TxtTableName").GetComponent<Text>());
				txtTableInfo.Add(rtContent.GetChild(i).Find("TxtTableInfo").GetComponent<Text>());
			}
			btnArrow.onClick.AddListener(ClickBtnArrow);
		}

		public void ClickBtnArrow()
		{
			if (isCanClick)
			{
				HW2_Singleton<SoundMgr>.Get().PlayClip("选座选厅自动发炮");
				HW2_Singleton<SoundMgr>.Get().SetVolume("选座选厅自动发炮", 1f);
				if (!isOpening)
				{
					StartCoroutine(ShowChildMenu());
				}
				else
				{
					StartCoroutine(HideChildMenu());
				}
			}
		}

		private IEnumerator ShowChildMenu()
		{
			ScrollView.SetActive(value: true);
			isCanClick = false;
			for (int i = 0; i < activeCount; i++)
			{
				btnChilds[i].transform.localPosition -= Vector3.up * ((float)i * offset + offset);
				yield return wait;
			}
			isCanClick = true;
			isOpening = true;
		}

		private IEnumerator HideChildMenu()
		{
			isCanClick = false;
			for (int i = activeCount - 1; i >= 0; i--)
			{
				btnChilds[i].transform.localPosition += Vector3.up * ((float)i * offset + offset);
				yield return wait;
			}
			isCanClick = true;
			isOpening = false;
			ScrollView.SetActive(value: false);
		}

		public void InitTableList(Action<int> act)
		{
			chooseTableAct = act;
			UpdateTableList(HW2_GVars.lobby.desks);
		}

		public void UpdateListUI(int index)
		{
			ShowTableList(index);
		}

		private void UpdateTableList(List<DeskInfo> desk)
		{
			int count = desk.Count;
			ShowTableList(count);
			for (int i = 0; i < count; i++)
			{
				txtTableName[i].text = desk[i].name;
				txtTableInfo[i].text = $"({desk[i].GetSeatUseCount()}/4)";
				int num = i;
				btnChilds[i].onClick.RemoveAllListeners();
			}
		}

		public void ShowTableList(int tableCount)
		{
			activeCount = tableCount;
			int count = btnChilds.Count;
			if (count < activeCount)
			{
				for (int i = count; i < activeCount; i++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(rtContent.GetChild(0).gameObject, rtContent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = rtContent.GetChild(0).localPosition;
					btnChilds.Add(gameObject.GetComponent<Button>());
					objChoose.Add(gameObject.transform.Find("BgChoose").gameObject);
					txtTableName.Add(gameObject.transform.Find("TxtTableName").GetComponent<Text>());
					txtTableInfo.Add(gameObject.transform.Find("TxtTableInfo").GetComponent<Text>());
				}
			}
			for (int j = 0; j < btnChilds.Count; j++)
			{
				btnChilds[j].gameObject.SetActive(value: false);
			}
			for (int k = 0; k < activeCount; k++)
			{
				btnChilds[k].gameObject.SetActive(value: true);
			}
			rtContent.sizeDelta = Vector3.up * offset * activeCount;
		}
	}
}
