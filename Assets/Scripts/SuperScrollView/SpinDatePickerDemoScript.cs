using UnityEngine;

namespace SuperScrollView
{
	public class SpinDatePickerDemoScript : MonoBehaviour
	{
		public LoopListView2 mLoopListViewYear;

		public LoopListView2 mLoopListViewMonth;

		public LoopListView2 mLoopListViewDay;

		private static int[] mYearCountArray = new int[2]
		{
			2020,
			2021
		};

		private static int[] mMonthDayCountArray = new int[12]
		{
			31,
			28,
			31,
			30,
			31,
			30,
			31,
			31,
			30,
			31,
			30,
			31
		};

		private static int[] mMonthNameArray = new int[12]
		{
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			10,
			11,
			12
		};

		public int CurSelectedMonth
		{
			get;
			private set;
		}

		public int CurSelectedYear
		{
			get;
			private set;
		}

		public int CurSelectedDay
		{
			get;
			private set;
		}

		public int CurSelectedHour
		{
			get;
			private set;
		}

		public string Year
		{
			get;
			set;
		}

		public string Month
		{
			get;
			set;
		}

		public string Day
		{
			get;
			set;
		}

		private void Start()
		{
			CurSelectedYear = 2;
			CurSelectedDay = 2;
			CurSelectedMonth = 2;
			mLoopListViewYear.InitListView(-1, OnGetItemByIndexForYear);
			mLoopListViewMonth.InitListView(-1, OnGetItemByIndexForMonth);
			mLoopListViewDay.InitListView(-1, OnGetItemByIndexForDay);
			mLoopListViewYear.mOnSnapNearestChanged = OnYearSnapTargetChanged;
			mLoopListViewMonth.mOnSnapNearestChanged = OnMonthSnapTargetChanged;
			mLoopListViewDay.mOnSnapNearestChanged = OnDaySnapTargetChanged;
		}

		private LoopListViewItem2 OnGetItemByIndexForHour(LoopListView2 listView, int index)
		{
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("ItemPrefab1");
			ListItem7 component = loopListViewItem.GetComponent<ListItem7>();
			if (!loopListViewItem.IsInitHandlerCalled)
			{
				loopListViewItem.IsInitHandlerCalled = true;
				component.Init();
			}
			int num = 1;
			int num2 = 24;
			int num3 = 0;
			num3 = ((index < 0) ? (num2 + (index + 1) % num2 - 1) : (index % num2));
			num3 = (component.Value = num3 + num);
			component.mText.text = num3.ToString();
			return loopListViewItem;
		}

		private LoopListViewItem2 OnGetItemByIndexForYear(LoopListView2 listView, int index)
		{
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("ItemPrefab1");
			ListItem7 component = loopListViewItem.GetComponent<ListItem7>();
			if (!loopListViewItem.IsInitHandlerCalled)
			{
				loopListViewItem.IsInitHandlerCalled = true;
				component.Init();
			}
			int num = 1;
			int num2 = mYearCountArray.Length;
			int num3 = 0;
			num3 = ((index < 0) ? (num2 + (index + 1) % num2 - 1) : (index % num2));
			num3 = (component.Value = num3 + num);
			component.mText.text = mYearCountArray[num3 - 1].ToString();
			return loopListViewItem;
		}

		private LoopListViewItem2 OnGetItemByIndexForMonth(LoopListView2 listView, int index)
		{
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("ItemPrefab1");
			ListItem7 component = loopListViewItem.GetComponent<ListItem7>();
			if (!loopListViewItem.IsInitHandlerCalled)
			{
				loopListViewItem.IsInitHandlerCalled = true;
				component.Init();
			}
			int num = 1;
			int num2 = 12;
			int num3 = 0;
			num3 = ((index < 0) ? (num2 + (index + 1) % num2 - 1) : (index % num2));
			num3 = (component.Value = num3 + num);
			component.mText.text = mMonthNameArray[num3 - 1].ToString();
			return loopListViewItem;
		}

		private LoopListViewItem2 OnGetItemByIndexForDay(LoopListView2 listView, int index)
		{
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("ItemPrefab1");
			ListItem7 component = loopListViewItem.GetComponent<ListItem7>();
			if (!loopListViewItem.IsInitHandlerCalled)
			{
				loopListViewItem.IsInitHandlerCalled = true;
				component.Init();
			}
			int num = 1;
			int num2 = mMonthDayCountArray[CurSelectedMonth - 1];
			int num3 = 0;
			num3 = ((index < 0) ? (num2 + (index + 1) % num2 - 1) : (index % num2));
			num3 = (component.Value = num3 + num);
			component.mText.text = num3.ToString();
			return loopListViewItem;
		}

		private void OnYearSnapTargetChanged(LoopListView2 listView, LoopListViewItem2 item)
		{
			int indexInShownItemList = listView.GetIndexInShownItemList(item);
			if (indexInShownItemList >= 0)
			{
				ListItem7 component = item.GetComponent<ListItem7>();
				CurSelectedYear = mYearCountArray[component.Value - 1];
				OnListViewSnapTargetChanged(listView, indexInShownItemList, 0);
			}
		}

		private void OnMonthSnapTargetChanged(LoopListView2 listView, LoopListViewItem2 item)
		{
			int indexInShownItemList = listView.GetIndexInShownItemList(item);
			if (indexInShownItemList >= 0)
			{
				ListItem7 component = item.GetComponent<ListItem7>();
				CurSelectedMonth = component.Value;
				OnListViewSnapTargetChanged(listView, indexInShownItemList, 1);
			}
		}

		private void OnDaySnapTargetChanged(LoopListView2 listView, LoopListViewItem2 item)
		{
			int indexInShownItemList = listView.GetIndexInShownItemList(item);
			if (indexInShownItemList >= 0)
			{
				ListItem7 component = item.GetComponent<ListItem7>();
				CurSelectedDay = component.Value;
				OnListViewSnapTargetChanged(listView, indexInShownItemList, 2);
			}
		}

		private void OnHourSnapTargetChanged(LoopListView2 listView, LoopListViewItem2 item)
		{
			int indexInShownItemList = listView.GetIndexInShownItemList(item);
			if (indexInShownItemList >= 0)
			{
				ListItem7 component = item.GetComponent<ListItem7>();
				CurSelectedHour = component.Value;
				OnListViewSnapTargetChanged(listView, indexInShownItemList, 3);
			}
		}

		private void OnMonthSnapTargetFinished(LoopListView2 listView, LoopListViewItem2 item)
		{
			LoopListViewItem2 shownItemByIndex = mLoopListViewDay.GetShownItemByIndex(0);
			ListItem7 component = shownItemByIndex.GetComponent<ListItem7>();
			int firstItemIndex = component.Value - 1;
			mLoopListViewDay.RefreshAllShownItemWithFirstIndex(firstItemIndex);
		}

		private void OnYearSnapTargetFinished(LoopListView2 listView, LoopListViewItem2 item)
		{
			LoopListViewItem2 shownItemByIndex = mLoopListViewDay.GetShownItemByIndex(0);
			ListItem7 component = shownItemByIndex.GetComponent<ListItem7>();
			int firstItemIndex = component.Value - 1;
			mLoopListViewDay.RefreshAllShownItemWithFirstIndex(firstItemIndex);
		}

		private void OnListViewSnapTargetChanged(LoopListView2 listView, int targetIndex, int num)
		{
			int shownItemCount = listView.ShownItemCount;
			for (int i = 0; i < shownItemCount; i++)
			{
				LoopListViewItem2 shownItemByIndex = listView.GetShownItemByIndex(i);
				ListItem7 component = shownItemByIndex.GetComponent<ListItem7>();
				if (i == targetIndex)
				{
					component.mText.color = Color.red;
					switch (num)
					{
					case 0:
						Year = component.mText.text;
						break;
					case 1:
						Month = component.mText.text;
						break;
					case 2:
						Day = component.mText.text;
						break;
					}
				}
				else
				{
					component.mText.color = Color.black;
				}
			}
		}
	}
}
