using M__M.HaiWang.GameDefine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace M__M.HaiWang.Demo
{
	public class TableListController : SimpleSingletonBehaviour<TableListController>
	{
		private static TableListController instance;

		[SerializeField]
		private Transform _gridRoot;

		[SerializeField]
		private ScrollRect _scrollRect;

		[SerializeField]
		private GameObject _blank;

		[SerializeField]
		private Scrollbar _scrollBar;

		[SerializeField]
		private GameObject _tableItem;

		[SerializeField]
		private Sprite[] _arrowSprite;

		[SerializeField]
		private Image _arrow;

		[SerializeField]
		private Sprite[] _selectSprite;

		[SerializeField]
		private Button _btnList;

		[SerializeField]
		private GameObject _bottom;

		public GameObject _bottom2;

		[SerializeField]
		private bool useFake = true;

		private Action<int> chooseTableAct;

		private List<DeskInfo> deskinfo = new List<DeskInfo>();

		public List<GameObject> itemList = new List<GameObject>();

		public new static TableListController Get()
		{
			return instance;
		}

		private void Awake()
		{
			instance = this;
			SimpleSingletonBehaviour<TableListController>.s_instance = this;
		}

		private void Start()
		{
			if (!useFake)
			{
				return;
			}
			for (int i = 0; i < 15; i++)
			{
				DeskInfo deskInfo = new DeskInfo(string.Empty);
				deskInfo.name = "desk" + i;
				for (int j = 0; j < 4; j++)
				{
					deskInfo.seats[j].isUsed = (j % 2 == 0);
				}
				deskinfo.Add(deskInfo);
			}
		}

		public void InitTableList(Action<int> act)
		{
			chooseTableAct = act;
			_arrow.sprite = _arrowSprite[1];
			_scrollRect.verticalScrollbar = null;
			if (useFake)
			{
				GenerateTableItem(deskinfo);
			}
			else
			{
				GenerateTableItem(HW2_GVars.lobby.desks);
			}
			_scrollBar.gameObject.SetActive(value: false);
			_gridRoot.gameObject.SetActive(value: false);
			_blank.SetActive(value: false);
		}

		public void ShowTableList()
		{
			_btnList.enabled = _gridRoot.gameObject.activeSelf;
			_bottom.SetActive(!_bottom.activeSelf);
			_gridRoot.gameObject.SetActive(!_gridRoot.gameObject.activeSelf);
			_scrollRect.GetComponent<Mask>().showMaskGraphic = _gridRoot.gameObject.activeSelf;
			_bottom2.SetActive(!_bottom2.activeSelf);
			_blank.SetActive(!_blank.activeSelf);
			if (useFake)
			{
				_scrollRect.verticalScrollbar = ((!_blank.activeSelf) ? null : ((deskinfo.Count > 8) ? _scrollBar : null));
			}
			else
			{
				_scrollRect.verticalScrollbar = ((!_blank.activeSelf) ? null : ((HW2_GVars.lobby.desks.Count > 8) ? _scrollBar : null));
			}
			_scrollBar.gameObject.SetActive(value: false);
			if ((bool)_scrollBar)
			{
				_scrollBar.value = 1f;
			}
			_arrow.sprite = (_blank.activeSelf ? _arrowSprite[0] : _arrowSprite[1]);
		}

		public void UpdateListUI(int index)
		{
			if (_gridRoot.childCount > 1)
			{
				string name = HW2_GVars.lobby.desks[index].name;
				int seatUseCount = HW2_GVars.lobby.desks[index].GetSeatUseCount();
				_gridRoot.GetChild(index + 1).Find("Text").GetComponent<Text>()
					.text = $"{name}";
					_gridRoot.GetChild(index + 1).Find("Text2").GetComponent<Text>()
						.text = $"{seatUseCount}/4";
					}
				}

				private void GenerateTableItem(List<DeskInfo> desk)
				{
					if (_gridRoot.childCount > 1)
					{
						int childCount = _gridRoot.childCount;
						for (int i = 1; i < childCount; i++)
						{
							UnityEngine.Object.Destroy(_gridRoot.GetChild(i).gameObject);
							itemList.Clear();
						}
					}
					int count = desk.Count;
					float y = (count <= 8) ? (count * 57) : (57 * (count - 8) + 456);
					float num = (count <= 8) ? (count * 57) : 456;
					_gridRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(227f, y);
					_scrollRect.GetComponent<RectTransform>().sizeDelta = new Vector2(230f, num);
					Transform component = _bottom2.GetComponent<Transform>();
					Vector3 localPosition = _scrollRect.transform.localPosition;
					component.localPosition = new Vector2(localPosition.x, 0f - num + 43f);
					for (int j = 0; j < count; j++)
					{
						GameObject item = UnityEngine.Object.Instantiate(_tableItem);
						item.SetActive(value: true);
						item.transform.SetParent(_gridRoot);
						item.transform.localScale = Vector3.one;
						item.name = "deskItem" + j;
						itemList.Add(item);
						int seatUseCount = desk[j].GetSeatUseCount();
						item.transform.Find("Text").GetComponent<Text>().text = $"{desk[j].name}";
						if (item.transform.Find("Text").GetComponent<Text>().text.Length > 10)
						{
							item.transform.Find("Text").GetComponent<Text>().text = item.transform.Find("Text").GetComponent<Text>().text.Substring(0, 8) + "..";
						}
						item.transform.Find("Text2").GetComponent<Text>().text = $"{seatUseCount}/4";
						int index = j;
						item.GetComponent<Button>().onClick.AddListener(delegate
						{
							UnityEngine.Debug.Log("index: " + index);
							chooseTableAct(index);
							foreach (GameObject item2 in itemList)
							{
								item2.GetComponent<Image>().sprite = _selectSprite[0];
							}
							item.GetComponent<Image>().sprite = _selectSprite[1];
						});
					}
				}
			}
		}
