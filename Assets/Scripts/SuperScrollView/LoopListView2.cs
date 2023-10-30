using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SuperScrollView
{
	public class LoopListView2 : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IEventSystemHandler
	{
		private class SnapData
		{
			public SnapStatus mSnapStatus;

			public int mSnapTargetIndex;

			public float mTargetSnapVal;

			public float mCurSnapVal;

			public bool mIsForceSnapTo;

			public void Clear()
			{
				mSnapStatus = SnapStatus.NoTargetSet;
				mIsForceSnapTo = false;
			}
		}

		private Dictionary<string, ItemPool> mItemPoolDict = new Dictionary<string, ItemPool>();

		private List<ItemPool> mItemPoolList = new List<ItemPool>();

		[SerializeField]
		private List<ItemPrefabConfData> mItemPrefabDataList = new List<ItemPrefabConfData>();

		[SerializeField]
		private ListItemArrangeType mArrangeType;

		private List<LoopListViewItem2> mItemList = new List<LoopListViewItem2>();

		private RectTransform mContainerTrans;

		private ScrollRect mScrollRect;

		private RectTransform mScrollRectTransform;

		private RectTransform mViewPortRectTransform;

		private float mItemDefaultWithPaddingSize = 20f;

		private int mItemTotalCount;

		private bool mIsVertList;

		private Func<LoopListView2, int, LoopListViewItem2> mOnGetItemByIndex;

		private Vector3[] mItemWorldCorners = new Vector3[4];

		private Vector3[] mViewPortRectLocalCorners = new Vector3[4];

		private int mCurReadyMinItemIndex;

		private int mCurReadyMaxItemIndex;

		private bool mNeedCheckNextMinItem = true;

		private bool mNeedCheckNextMaxItem = true;

		private ItemPosMgr mItemPosMgr;

		private float mDistanceForRecycle0 = 300f;

		private float mDistanceForNew0 = 200f;

		private float mDistanceForRecycle1 = 300f;

		private float mDistanceForNew1 = 200f;

		[SerializeField]
		private bool mSupportScrollBar = true;

		private bool mIsDraging;

		private PointerEventData mPointerEventData;

		public Action mOnBeginDragAction;

		public Action mOnDragingAction;

		public Action mOnEndDragAction;

		private int mLastItemIndex;

		private float mLastItemPadding;

		private float mSmoothDumpVel;

		private float mSmoothDumpRate = 0.3f;

		private float mSnapFinishThreshold = 0.1f;

		private float mSnapVecThreshold = 145f;

		[SerializeField]
		private bool mItemSnapEnable;

		private Vector3 mLastFrameContainerPos = Vector3.zero;

		public Action<LoopListView2, LoopListViewItem2> mOnSnapItemFinished;

		public Action<LoopListView2, LoopListViewItem2> mOnSnapNearestChanged;

		private int mCurSnapNearestItemIndex = -1;

		private Vector2 mAdjustedVec;

		private bool mNeedAdjustVec;

		private int mLeftSnapUpdateExtraCount = 1;

		[SerializeField]
		private Vector2 mViewPortSnapPivot = Vector2.zero;

		[SerializeField]
		private Vector2 mItemSnapPivot = Vector2.zero;

		private ClickEventListener mScrollBarClickEventListener;

		private SnapData mCurSnapData = new SnapData();

		private Vector3 mLastSnapCheckPos = Vector3.zero;

		private bool mListViewInited;

		private int mListUpdateCheckFrameCount;

		public ListItemArrangeType ArrangeType
		{
			get
			{
				return mArrangeType;
			}
			set
			{
				mArrangeType = value;
			}
		}

		public bool IsVertList => mIsVertList;

		public int ItemTotalCount => mItemTotalCount;

		public RectTransform ContainerTrans => mContainerTrans;

		public ScrollRect ScrollRect => mScrollRect;

		public bool IsDraging => mIsDraging;

		public bool ItemSnapEnable
		{
			get
			{
				return mItemSnapEnable;
			}
			set
			{
				mItemSnapEnable = value;
			}
		}

		public bool SupportScrollBar
		{
			get
			{
				return mSupportScrollBar;
			}
			set
			{
				mSupportScrollBar = value;
			}
		}

		public int ShownItemCount => mItemList.Count;

		public float ViewPortSize
		{
			get
			{
				if (mIsVertList)
				{
					return mViewPortRectTransform.rect.height;
				}
				return mViewPortRectTransform.rect.width;
			}
		}

		public float ViewPortWidth => mViewPortRectTransform.rect.width;

		public float ViewPortHeight => mViewPortRectTransform.rect.height;

		public int CurSnapNearestItemIndex => mCurSnapNearestItemIndex;

		public ItemPrefabConfData GetItemPrefabConfData(string prefabName)
		{
			foreach (ItemPrefabConfData mItemPrefabData in mItemPrefabDataList)
			{
				if (mItemPrefabData.mItemPrefab == null)
				{
					UnityEngine.Debug.LogError("A item prefab is null ");
				}
				else if (prefabName == mItemPrefabData.mItemPrefab.name)
				{
					return mItemPrefabData;
				}
			}
			return null;
		}

		public void OnItemPrefabChanged(string prefabName)
		{
			ItemPrefabConfData itemPrefabConfData = GetItemPrefabConfData(prefabName);
			if (itemPrefabConfData == null)
			{
				return;
			}
			ItemPool value = null;
			if (mItemPoolDict.TryGetValue(prefabName, out value))
			{
				int num = -1;
				Vector3 pos = Vector3.zero;
				if (mItemList.Count > 0)
				{
					num = mItemList[0].ItemIndex;
					pos = mItemList[0].CachedRectTransform.localPosition;
				}
				RecycleAllItem();
				ClearAllTmpRecycledItem();
				value.DestroyAllItem();
				value.Init(itemPrefabConfData.mItemPrefab, itemPrefabConfData.mPadding, itemPrefabConfData.mStartPosOffset, itemPrefabConfData.mInitCreateCount, mContainerTrans);
				if (num >= 0)
				{
					RefreshAllShownItemWithFirstIndexAndPos(num, pos);
				}
			}
		}

		public void InitListView(int itemTotalCount, Func<LoopListView2, int, LoopListViewItem2> onGetItemByIndex, LoopListViewInitParam initParam = null)
		{
			if (initParam != null)
			{
				mDistanceForRecycle0 = initParam.mDistanceForRecycle0;
				mDistanceForNew0 = initParam.mDistanceForNew0;
				mDistanceForRecycle1 = initParam.mDistanceForRecycle1;
				mDistanceForNew1 = initParam.mDistanceForNew1;
				mSmoothDumpRate = initParam.mSmoothDumpRate;
				mSnapFinishThreshold = initParam.mSnapFinishThreshold;
				mSnapVecThreshold = initParam.mSnapVecThreshold;
				mItemDefaultWithPaddingSize = initParam.mItemDefaultWithPaddingSize;
			}
			mScrollRect = base.gameObject.GetComponent<ScrollRect>();
			if (mScrollRect == null)
			{
				UnityEngine.Debug.LogError("ListView Init Failed! ScrollRect component not found!");
				return;
			}
			if (mDistanceForRecycle0 <= mDistanceForNew0)
			{
				UnityEngine.Debug.LogError("mDistanceForRecycle0 should be bigger than mDistanceForNew0");
			}
			if (mDistanceForRecycle1 <= mDistanceForNew1)
			{
				UnityEngine.Debug.LogError("mDistanceForRecycle1 should be bigger than mDistanceForNew1");
			}
			mCurSnapData.Clear();
			mItemPosMgr = new ItemPosMgr(mItemDefaultWithPaddingSize);
			mScrollRectTransform = mScrollRect.GetComponent<RectTransform>();
			mContainerTrans = mScrollRect.content;
			mViewPortRectTransform = mScrollRect.viewport;
			if (mViewPortRectTransform == null)
			{
				mViewPortRectTransform = mScrollRectTransform;
			}
			if (mScrollRect.horizontalScrollbarVisibility == ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport && mScrollRect.horizontalScrollbar != null)
			{
				UnityEngine.Debug.LogError("ScrollRect.horizontalScrollbarVisibility cannot be set to AutoHideAndExpandViewport");
			}
			if (mScrollRect.verticalScrollbarVisibility == ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport && mScrollRect.verticalScrollbar != null)
			{
				UnityEngine.Debug.LogError("ScrollRect.verticalScrollbarVisibility cannot be set to AutoHideAndExpandViewport");
			}
			mIsVertList = (mArrangeType == ListItemArrangeType.TopToBottom || mArrangeType == ListItemArrangeType.BottomToTop);
			mScrollRect.horizontal = !mIsVertList;
			mScrollRect.vertical = mIsVertList;
			SetScrollbarListener();
			AdjustPivot(mViewPortRectTransform);
			AdjustAnchor(mContainerTrans);
			AdjustContainerPivot(mContainerTrans);
			InitItemPool();
			mOnGetItemByIndex = onGetItemByIndex;
			if (mListViewInited)
			{
				UnityEngine.Debug.LogError("LoopListView2.InitListView method can be called only once.");
			}
			mListViewInited = true;
			ResetListView();
			SetListItemCount(itemTotalCount);
		}

		private void SetScrollbarListener()
		{
			mScrollBarClickEventListener = null;
			Scrollbar scrollbar = null;
			if (mIsVertList && mScrollRect.verticalScrollbar != null)
			{
				scrollbar = mScrollRect.verticalScrollbar;
			}
			if (!mIsVertList && mScrollRect.horizontalScrollbar != null)
			{
				scrollbar = mScrollRect.horizontalScrollbar;
			}
			if (!(scrollbar == null))
			{
				ClickEventListener clickEventListener = mScrollBarClickEventListener = ClickEventListener.Get(scrollbar.gameObject);
				clickEventListener.SetPointerUpHandler(OnPointerUpInScrollBar);
				clickEventListener.SetPointerDownHandler(OnPointerDownInScrollBar);
			}
		}

		private void OnPointerDownInScrollBar(GameObject obj)
		{
			mCurSnapData.Clear();
		}

		private void OnPointerUpInScrollBar(GameObject obj)
		{
			ForceSnapUpdateCheck();
		}

		public void ResetListView()
		{
			mViewPortRectTransform.GetLocalCorners(mViewPortRectLocalCorners);
			mContainerTrans.localPosition = Vector3.zero;
			ForceSnapUpdateCheck();
		}

		public void SetListItemCount(int itemCount, bool resetPos = true)
		{
			if (itemCount == mItemTotalCount)
			{
				return;
			}
			mCurSnapData.Clear();
			mItemTotalCount = itemCount;
			if (mItemTotalCount < 0)
			{
				mSupportScrollBar = false;
			}
			if (mSupportScrollBar)
			{
				mItemPosMgr.SetItemMaxCount(mItemTotalCount);
			}
			else
			{
				mItemPosMgr.SetItemMaxCount(0);
			}
			if (mItemTotalCount == 0)
			{
				mCurReadyMaxItemIndex = 0;
				mCurReadyMinItemIndex = 0;
				mNeedCheckNextMaxItem = false;
				mNeedCheckNextMinItem = false;
				RecycleAllItem();
				ClearAllTmpRecycledItem();
				UpdateContentSize();
				return;
			}
			mLeftSnapUpdateExtraCount = 1;
			mNeedCheckNextMaxItem = true;
			mNeedCheckNextMinItem = true;
			if (resetPos)
			{
				MovePanelToItemIndex(0, 0f);
				return;
			}
			if (mItemList.Count == 0)
			{
				MovePanelToItemIndex(0, 0f);
				return;
			}
			int num = mItemTotalCount - 1;
			int itemIndex = mItemList[mItemList.Count - 1].ItemIndex;
			if (itemIndex <= num)
			{
				UpdateContentSize();
				UpdateAllShownItemsPos();
			}
			else
			{
				MovePanelToItemIndex(num, 0f);
			}
		}

		public LoopListViewItem2 GetShownItemByItemIndex(int itemIndex)
		{
			int count = mItemList.Count;
			if (count == 0)
			{
				return null;
			}
			if (itemIndex < mItemList[0].ItemIndex || itemIndex > mItemList[count - 1].ItemIndex)
			{
				return null;
			}
			int index = itemIndex - mItemList[0].ItemIndex;
			return mItemList[index];
		}

		public LoopListViewItem2 GetShownItemByIndex(int index)
		{
			int count = mItemList.Count;
			if (index < 0 || index >= count)
			{
				return null;
			}
			return mItemList[index];
		}

		public LoopListViewItem2 GetShownItemByIndexWithoutCheck(int index)
		{
			return mItemList[index];
		}

		public int GetIndexInShownItemList(LoopListViewItem2 item)
		{
			if (item == null)
			{
				return -1;
			}
			int count = mItemList.Count;
			if (count == 0)
			{
				return -1;
			}
			for (int i = 0; i < count; i++)
			{
				if (mItemList[i] == item)
				{
					return i;
				}
			}
			return -1;
		}

		public void DoActionForEachShownItem(Action<LoopListViewItem2, object> action, object param)
		{
			if (action == null)
			{
				return;
			}
			int count = mItemList.Count;
			if (count != 0)
			{
				for (int i = 0; i < count; i++)
				{
					action(mItemList[i], param);
				}
			}
		}

		public LoopListViewItem2 NewListViewItem(string itemPrefabName)
		{
			ItemPool value = null;
			if (!mItemPoolDict.TryGetValue(itemPrefabName, out value))
			{
				return null;
			}
			LoopListViewItem2 item = value.GetItem();
			RectTransform component = item.GetComponent<RectTransform>();
			component.SetParent(mContainerTrans);
			component.localScale = Vector3.one;
			component.localPosition = Vector3.zero;
			component.localEulerAngles = Vector3.zero;
			item.ParentListView = this;
			return item;
		}

		public void OnItemSizeChanged(int itemIndex)
		{
			LoopListViewItem2 shownItemByItemIndex = GetShownItemByItemIndex(itemIndex);
			if (shownItemByItemIndex == null)
			{
				return;
			}
			if (mSupportScrollBar)
			{
				if (mIsVertList)
				{
					SetItemSize(itemIndex, shownItemByItemIndex.CachedRectTransform.rect.height, shownItemByItemIndex.Padding);
				}
				else
				{
					SetItemSize(itemIndex, shownItemByItemIndex.CachedRectTransform.rect.width, shownItemByItemIndex.Padding);
				}
			}
			UpdateContentSize();
			UpdateAllShownItemsPos();
		}

		public void RefreshItemByItemIndex(int itemIndex)
		{
			int count = mItemList.Count;
			if (count == 0 || itemIndex < mItemList[0].ItemIndex || itemIndex > mItemList[count - 1].ItemIndex)
			{
				return;
			}
			int itemIndex2 = mItemList[0].ItemIndex;
			int index = itemIndex - itemIndex2;
			LoopListViewItem2 loopListViewItem = mItemList[index];
			Vector3 localPosition = loopListViewItem.CachedRectTransform.localPosition;
			RecycleItemTmp(loopListViewItem);
			LoopListViewItem2 newItemByIndex = GetNewItemByIndex(itemIndex);
			if (newItemByIndex == null)
			{
				RefreshAllShownItemWithFirstIndex(itemIndex2);
				return;
			}
			mItemList[index] = newItemByIndex;
			if (mIsVertList)
			{
				localPosition.x = newItemByIndex.StartPosOffset;
			}
			else
			{
				localPosition.y = newItemByIndex.StartPosOffset;
			}
			newItemByIndex.CachedRectTransform.localPosition = localPosition;
			OnItemSizeChanged(itemIndex);
			ClearAllTmpRecycledItem();
		}

		public void FinishSnapImmediately()
		{
			UpdateSnapMove(immediate: true);
		}

		public void MovePanelToItemIndex(int itemIndex, float offset)
		{
			mScrollRect.StopMovement();
			mCurSnapData.Clear();
			if (itemIndex < 0 || mItemTotalCount == 0)
			{
				return;
			}
			if (mItemTotalCount > 0 && itemIndex >= mItemTotalCount)
			{
				itemIndex = mItemTotalCount - 1;
			}
			if (offset < 0f)
			{
				offset = 0f;
			}
			Vector3 zero = Vector3.zero;
			float viewPortSize = ViewPortSize;
			if (offset > viewPortSize)
			{
				offset = viewPortSize;
			}
			if (mArrangeType == ListItemArrangeType.TopToBottom)
			{
				Vector3 localPosition = mContainerTrans.localPosition;
				float num = localPosition.y;
				if (num < 0f)
				{
					num = 0f;
				}
				zero.y = 0f - num - offset;
			}
			else if (mArrangeType == ListItemArrangeType.BottomToTop)
			{
				Vector3 localPosition2 = mContainerTrans.localPosition;
				float num2 = localPosition2.y;
				if (num2 > 0f)
				{
					num2 = 0f;
				}
				zero.y = 0f - num2 + offset;
			}
			else if (mArrangeType == ListItemArrangeType.LeftToRight)
			{
				Vector3 localPosition3 = mContainerTrans.localPosition;
				float num3 = localPosition3.x;
				if (num3 > 0f)
				{
					num3 = 0f;
				}
				zero.x = 0f - num3 + offset;
			}
			else if (mArrangeType == ListItemArrangeType.RightToLeft)
			{
				Vector3 localPosition4 = mContainerTrans.localPosition;
				float num4 = localPosition4.x;
				if (num4 < 0f)
				{
					num4 = 0f;
				}
				zero.x = 0f - num4 - offset;
			}
			RecycleAllItem();
			LoopListViewItem2 newItemByIndex = GetNewItemByIndex(itemIndex);
			if (newItemByIndex == null)
			{
				ClearAllTmpRecycledItem();
				return;
			}
			if (mIsVertList)
			{
				zero.x = newItemByIndex.StartPosOffset;
			}
			else
			{
				zero.y = newItemByIndex.StartPosOffset;
			}
			newItemByIndex.CachedRectTransform.localPosition = zero;
			if (mSupportScrollBar)
			{
				if (mIsVertList)
				{
					SetItemSize(itemIndex, newItemByIndex.CachedRectTransform.rect.height, newItemByIndex.Padding);
				}
				else
				{
					SetItemSize(itemIndex, newItemByIndex.CachedRectTransform.rect.width, newItemByIndex.Padding);
				}
			}
			mItemList.Add(newItemByIndex);
			UpdateContentSize();
			UpdateListView(viewPortSize + 100f, viewPortSize + 100f, viewPortSize, viewPortSize);
			AdjustPanelPos();
			ClearAllTmpRecycledItem();
		}

		public void RefreshAllShownItem()
		{
			if (mItemList.Count != 0)
			{
				RefreshAllShownItemWithFirstIndex(mItemList[0].ItemIndex);
			}
		}

		public void RefreshAllShownItemWithFirstIndex(int firstItemIndex)
		{
			int count = mItemList.Count;
			if (count == 0)
			{
				return;
			}
			LoopListViewItem2 loopListViewItem = mItemList[0];
			Vector3 localPosition = loopListViewItem.CachedRectTransform.localPosition;
			RecycleAllItem();
			for (int i = 0; i < count; i++)
			{
				int num = firstItemIndex + i;
				LoopListViewItem2 newItemByIndex = GetNewItemByIndex(num);
				if (newItemByIndex == null)
				{
					break;
				}
				if (mIsVertList)
				{
					localPosition.x = newItemByIndex.StartPosOffset;
				}
				else
				{
					localPosition.y = newItemByIndex.StartPosOffset;
				}
				newItemByIndex.CachedRectTransform.localPosition = localPosition;
				if (mSupportScrollBar)
				{
					if (mIsVertList)
					{
						SetItemSize(num, newItemByIndex.CachedRectTransform.rect.height, newItemByIndex.Padding);
					}
					else
					{
						SetItemSize(num, newItemByIndex.CachedRectTransform.rect.width, newItemByIndex.Padding);
					}
				}
				mItemList.Add(newItemByIndex);
			}
			UpdateContentSize();
			UpdateAllShownItemsPos();
			ClearAllTmpRecycledItem();
		}

		public void RefreshAllShownItemWithFirstIndexAndPos(int firstItemIndex, Vector3 pos)
		{
			RecycleAllItem();
			LoopListViewItem2 newItemByIndex = GetNewItemByIndex(firstItemIndex);
			if (newItemByIndex == null)
			{
				return;
			}
			if (mIsVertList)
			{
				pos.x = newItemByIndex.StartPosOffset;
			}
			else
			{
				pos.y = newItemByIndex.StartPosOffset;
			}
			newItemByIndex.CachedRectTransform.localPosition = pos;
			if (mSupportScrollBar)
			{
				if (mIsVertList)
				{
					SetItemSize(firstItemIndex, newItemByIndex.CachedRectTransform.rect.height, newItemByIndex.Padding);
				}
				else
				{
					SetItemSize(firstItemIndex, newItemByIndex.CachedRectTransform.rect.width, newItemByIndex.Padding);
				}
			}
			mItemList.Add(newItemByIndex);
			UpdateContentSize();
			UpdateAllShownItemsPos();
			UpdateListView(mDistanceForRecycle0, mDistanceForRecycle1, mDistanceForNew0, mDistanceForNew1);
			ClearAllTmpRecycledItem();
		}

		private void RecycleItemTmp(LoopListViewItem2 item)
		{
			if (!(item == null) && !string.IsNullOrEmpty(item.ItemPrefabName))
			{
				ItemPool value = null;
				if (mItemPoolDict.TryGetValue(item.ItemPrefabName, out value))
				{
					value.RecycleItem(item);
				}
			}
		}

		private void ClearAllTmpRecycledItem()
		{
			int count = mItemPoolList.Count;
			for (int i = 0; i < count; i++)
			{
				mItemPoolList[i].ClearTmpRecycledItem();
			}
		}

		private void RecycleAllItem()
		{
			foreach (LoopListViewItem2 mItem in mItemList)
			{
				RecycleItemTmp(mItem);
			}
			mItemList.Clear();
		}

		private void AdjustContainerPivot(RectTransform rtf)
		{
			Vector2 pivot = rtf.pivot;
			if (mArrangeType == ListItemArrangeType.BottomToTop)
			{
				pivot.y = 0f;
			}
			else if (mArrangeType == ListItemArrangeType.TopToBottom)
			{
				pivot.y = 1f;
			}
			else if (mArrangeType == ListItemArrangeType.LeftToRight)
			{
				pivot.x = 0f;
			}
			else if (mArrangeType == ListItemArrangeType.RightToLeft)
			{
				pivot.x = 1f;
			}
			rtf.pivot = pivot;
		}

		private void AdjustPivot(RectTransform rtf)
		{
			Vector2 pivot = rtf.pivot;
			if (mArrangeType == ListItemArrangeType.BottomToTop)
			{
				pivot.y = 0f;
			}
			else if (mArrangeType == ListItemArrangeType.TopToBottom)
			{
				pivot.y = 1f;
			}
			else if (mArrangeType == ListItemArrangeType.LeftToRight)
			{
				pivot.x = 0f;
			}
			else if (mArrangeType == ListItemArrangeType.RightToLeft)
			{
				pivot.x = 1f;
			}
			rtf.pivot = pivot;
		}

		private void AdjustContainerAnchor(RectTransform rtf)
		{
			Vector2 anchorMin = rtf.anchorMin;
			Vector2 anchorMax = rtf.anchorMax;
			if (mArrangeType == ListItemArrangeType.BottomToTop)
			{
				anchorMin.y = 0f;
				anchorMax.y = 0f;
			}
			else if (mArrangeType == ListItemArrangeType.TopToBottom)
			{
				anchorMin.y = 1f;
				anchorMax.y = 1f;
			}
			else if (mArrangeType == ListItemArrangeType.LeftToRight)
			{
				anchorMin.x = 0f;
				anchorMax.x = 0f;
			}
			else if (mArrangeType == ListItemArrangeType.RightToLeft)
			{
				anchorMin.x = 1f;
				anchorMax.x = 1f;
			}
			rtf.anchorMin = anchorMin;
			rtf.anchorMax = anchorMax;
		}

		private void AdjustAnchor(RectTransform rtf)
		{
			Vector2 anchorMin = rtf.anchorMin;
			Vector2 anchorMax = rtf.anchorMax;
			if (mArrangeType == ListItemArrangeType.BottomToTop)
			{
				anchorMin.y = 0f;
				anchorMax.y = 0f;
			}
			else if (mArrangeType == ListItemArrangeType.TopToBottom)
			{
				anchorMin.y = 1f;
				anchorMax.y = 1f;
			}
			else if (mArrangeType == ListItemArrangeType.LeftToRight)
			{
				anchorMin.x = 0f;
				anchorMax.x = 0f;
			}
			else if (mArrangeType == ListItemArrangeType.RightToLeft)
			{
				anchorMin.x = 1f;
				anchorMax.x = 1f;
			}
			rtf.anchorMin = anchorMin;
			rtf.anchorMax = anchorMax;
		}

		private void InitItemPool()
		{
			foreach (ItemPrefabConfData mItemPrefabData in mItemPrefabDataList)
			{
				if (mItemPrefabData.mItemPrefab == null)
				{
					UnityEngine.Debug.LogError("A item prefab is null ");
				}
				else
				{
					string name = mItemPrefabData.mItemPrefab.name;
					if (mItemPoolDict.ContainsKey(name))
					{
						UnityEngine.Debug.LogError("A item prefab with name " + name + " has existed!");
					}
					else
					{
						RectTransform component = mItemPrefabData.mItemPrefab.GetComponent<RectTransform>();
						if (component == null)
						{
							UnityEngine.Debug.LogError("RectTransform component is not found in the prefab " + name);
						}
						else
						{
							AdjustAnchor(component);
							AdjustPivot(component);
							LoopListViewItem2 component2 = mItemPrefabData.mItemPrefab.GetComponent<LoopListViewItem2>();
							if (component2 == null)
							{
								mItemPrefabData.mItemPrefab.AddComponent<LoopListViewItem2>();
							}
							ItemPool itemPool = new ItemPool();
							itemPool.Init(mItemPrefabData.mItemPrefab, mItemPrefabData.mPadding, mItemPrefabData.mStartPosOffset, mItemPrefabData.mInitCreateCount, mContainerTrans);
							mItemPoolDict.Add(name, itemPool);
							mItemPoolList.Add(itemPool);
						}
					}
				}
			}
		}

		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				mIsDraging = true;
				CacheDragPointerEventData(eventData);
				mCurSnapData.Clear();
				if (mOnBeginDragAction != null)
				{
					mOnBeginDragAction();
				}
			}
		}

		public virtual void OnEndDrag(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				mIsDraging = false;
				mPointerEventData = null;
				if (mOnEndDragAction != null)
				{
					mOnEndDragAction();
				}
				ForceSnapUpdateCheck();
			}
		}

		public virtual void OnDrag(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				CacheDragPointerEventData(eventData);
				if (mOnDragingAction != null)
				{
					mOnDragingAction();
				}
			}
		}

		private void CacheDragPointerEventData(PointerEventData eventData)
		{
			if (mPointerEventData == null)
			{
				mPointerEventData = new PointerEventData(EventSystem.current);
			}
			mPointerEventData.button = eventData.button;
			mPointerEventData.position = eventData.position;
			mPointerEventData.pointerPressRaycast = eventData.pointerPressRaycast;
			mPointerEventData.pointerCurrentRaycast = eventData.pointerCurrentRaycast;
		}

		private LoopListViewItem2 GetNewItemByIndex(int index)
		{
			if (mSupportScrollBar && index < 0)
			{
				return null;
			}
			if (mItemTotalCount > 0 && index >= mItemTotalCount)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = mOnGetItemByIndex(this, index);
			if (loopListViewItem == null)
			{
				return null;
			}
			loopListViewItem.ItemIndex = index;
			loopListViewItem.ItemCreatedCheckFrameCount = mListUpdateCheckFrameCount;
			return loopListViewItem;
		}

		private void SetItemSize(int itemIndex, float itemSize, float padding)
		{
			mItemPosMgr.SetItemSize(itemIndex, itemSize + padding);
			if (itemIndex >= mLastItemIndex)
			{
				mLastItemIndex = itemIndex;
				mLastItemPadding = padding;
			}
		}

		private void GetPlusItemIndexAndPosAtGivenPos(float pos, ref int index, ref float itemPos)
		{
			mItemPosMgr.GetItemIndexAndPosAtGivenPos(pos, ref index, ref itemPos);
		}

		private float GetItemPos(int itemIndex)
		{
			return mItemPosMgr.GetItemPos(itemIndex);
		}

		public Vector3 GetItemCornerPosInViewPort(LoopListViewItem2 item, ItemCornerEnum corner = ItemCornerEnum.LeftBottom)
		{
			item.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
			return mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[(int)corner]);
		}

		private void AdjustPanelPos()
		{
			if (mItemList.Count == 0)
			{
				return;
			}
			UpdateAllShownItemsPos();
			float viewPortSize = ViewPortSize;
			float contentPanelSize = GetContentPanelSize();
			if (mArrangeType == ListItemArrangeType.TopToBottom)
			{
				if (contentPanelSize <= viewPortSize)
				{
					Vector3 localPosition = mContainerTrans.localPosition;
					localPosition.y = 0f;
					mContainerTrans.localPosition = localPosition;
					mItemList[0].CachedRectTransform.localPosition = new Vector3(mItemList[0].StartPosOffset, 0f, 0f);
					UpdateAllShownItemsPos();
					return;
				}
				LoopListViewItem2 loopListViewItem = mItemList[0];
				loopListViewItem.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
				Vector3 vector = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[1]);
				if (vector.y < mViewPortRectLocalCorners[1].y)
				{
					Vector3 localPosition2 = mContainerTrans.localPosition;
					localPosition2.y = 0f;
					mContainerTrans.localPosition = localPosition2;
					mItemList[0].CachedRectTransform.localPosition = new Vector3(mItemList[0].StartPosOffset, 0f, 0f);
					UpdateAllShownItemsPos();
					return;
				}
				LoopListViewItem2 loopListViewItem2 = mItemList[mItemList.Count - 1];
				loopListViewItem2.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
				Vector3 vector2 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[0]);
				float num = vector2.y - mViewPortRectLocalCorners[0].y;
				if (num > 0f)
				{
					Vector3 localPosition3 = mItemList[0].CachedRectTransform.localPosition;
					localPosition3.y -= num;
					mItemList[0].CachedRectTransform.localPosition = localPosition3;
					UpdateAllShownItemsPos();
				}
			}
			else if (mArrangeType == ListItemArrangeType.BottomToTop)
			{
				if (contentPanelSize <= viewPortSize)
				{
					Vector3 localPosition4 = mContainerTrans.localPosition;
					localPosition4.y = 0f;
					mContainerTrans.localPosition = localPosition4;
					mItemList[0].CachedRectTransform.localPosition = new Vector3(mItemList[0].StartPosOffset, 0f, 0f);
					UpdateAllShownItemsPos();
					return;
				}
				LoopListViewItem2 loopListViewItem3 = mItemList[0];
				loopListViewItem3.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
				Vector3 vector3 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[0]);
				if (vector3.y > mViewPortRectLocalCorners[0].y)
				{
					Vector3 localPosition5 = mContainerTrans.localPosition;
					localPosition5.y = 0f;
					mContainerTrans.localPosition = localPosition5;
					mItemList[0].CachedRectTransform.localPosition = new Vector3(mItemList[0].StartPosOffset, 0f, 0f);
					UpdateAllShownItemsPos();
					return;
				}
				LoopListViewItem2 loopListViewItem4 = mItemList[mItemList.Count - 1];
				loopListViewItem4.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
				Vector3 vector4 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[1]);
				float num2 = mViewPortRectLocalCorners[1].y - vector4.y;
				if (num2 > 0f)
				{
					Vector3 localPosition6 = mItemList[0].CachedRectTransform.localPosition;
					localPosition6.y += num2;
					mItemList[0].CachedRectTransform.localPosition = localPosition6;
					UpdateAllShownItemsPos();
				}
			}
			else if (mArrangeType == ListItemArrangeType.LeftToRight)
			{
				if (contentPanelSize <= viewPortSize)
				{
					Vector3 localPosition7 = mContainerTrans.localPosition;
					localPosition7.x = 0f;
					mContainerTrans.localPosition = localPosition7;
					mItemList[0].CachedRectTransform.localPosition = new Vector3(0f, mItemList[0].StartPosOffset, 0f);
					UpdateAllShownItemsPos();
					return;
				}
				LoopListViewItem2 loopListViewItem5 = mItemList[0];
				loopListViewItem5.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
				Vector3 vector5 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[1]);
				if (vector5.x > mViewPortRectLocalCorners[1].x)
				{
					Vector3 localPosition8 = mContainerTrans.localPosition;
					localPosition8.x = 0f;
					mContainerTrans.localPosition = localPosition8;
					mItemList[0].CachedRectTransform.localPosition = new Vector3(0f, mItemList[0].StartPosOffset, 0f);
					UpdateAllShownItemsPos();
					return;
				}
				LoopListViewItem2 loopListViewItem6 = mItemList[mItemList.Count - 1];
				loopListViewItem6.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
				Vector3 vector6 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[2]);
				float num3 = mViewPortRectLocalCorners[2].x - vector6.x;
				if (num3 > 0f)
				{
					Vector3 localPosition9 = mItemList[0].CachedRectTransform.localPosition;
					localPosition9.x += num3;
					mItemList[0].CachedRectTransform.localPosition = localPosition9;
					UpdateAllShownItemsPos();
				}
			}
			else
			{
				if (mArrangeType != ListItemArrangeType.RightToLeft)
				{
					return;
				}
				if (contentPanelSize <= viewPortSize)
				{
					Vector3 localPosition10 = mContainerTrans.localPosition;
					localPosition10.x = 0f;
					mContainerTrans.localPosition = localPosition10;
					mItemList[0].CachedRectTransform.localPosition = new Vector3(0f, mItemList[0].StartPosOffset, 0f);
					UpdateAllShownItemsPos();
					return;
				}
				LoopListViewItem2 loopListViewItem7 = mItemList[0];
				loopListViewItem7.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
				Vector3 vector7 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[2]);
				if (vector7.x < mViewPortRectLocalCorners[2].x)
				{
					Vector3 localPosition11 = mContainerTrans.localPosition;
					localPosition11.x = 0f;
					mContainerTrans.localPosition = localPosition11;
					mItemList[0].CachedRectTransform.localPosition = new Vector3(0f, mItemList[0].StartPosOffset, 0f);
					UpdateAllShownItemsPos();
					return;
				}
				LoopListViewItem2 loopListViewItem8 = mItemList[mItemList.Count - 1];
				loopListViewItem8.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
				Vector3 vector8 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[1]);
				float num4 = vector8.x - mViewPortRectLocalCorners[1].x;
				if (num4 > 0f)
				{
					Vector3 localPosition12 = mItemList[0].CachedRectTransform.localPosition;
					localPosition12.x -= num4;
					mItemList[0].CachedRectTransform.localPosition = localPosition12;
					UpdateAllShownItemsPos();
				}
			}
		}

		private void Update()
		{
			if (!mListViewInited)
			{
				return;
			}
			if (mNeedAdjustVec)
			{
				mNeedAdjustVec = false;
				if (mIsVertList)
				{
					Vector2 velocity = mScrollRect.velocity;
					if (velocity.y * mAdjustedVec.y > 0f)
					{
						mScrollRect.velocity = mAdjustedVec;
					}
				}
				else
				{
					Vector2 velocity2 = mScrollRect.velocity;
					if (velocity2.x * mAdjustedVec.x > 0f)
					{
						mScrollRect.velocity = mAdjustedVec;
					}
				}
			}
			if (mSupportScrollBar)
			{
				mItemPosMgr.Update(updateAll: false);
			}
			UpdateSnapMove();
			UpdateListView(mDistanceForRecycle0, mDistanceForRecycle1, mDistanceForNew0, mDistanceForNew1);
			ClearAllTmpRecycledItem();
			mLastFrameContainerPos = mContainerTrans.localPosition;
		}

		private void UpdateSnapMove(bool immediate = false)
		{
			if (mItemSnapEnable)
			{
				if (mIsVertList)
				{
					UpdateSnapVertical(immediate);
				}
				else
				{
					UpdateSnapHorizontal(immediate);
				}
			}
		}

		public void UpdateAllShownItemSnapData()
		{
			if (!mItemSnapEnable)
			{
				return;
			}
			int count = mItemList.Count;
			if (count == 0)
			{
				return;
			}
			Vector3 localPosition = mContainerTrans.localPosition;
			LoopListViewItem2 loopListViewItem = mItemList[0];
			loopListViewItem.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			if (mArrangeType == ListItemArrangeType.TopToBottom)
			{
				num4 = (0f - (1f - mViewPortSnapPivot.y)) * mViewPortRectTransform.rect.height;
				Vector3 vector = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[1]);
				num = vector.y;
				num2 = num - loopListViewItem.ItemSizeWithPadding;
				num3 = num - loopListViewItem.ItemSize * (1f - mItemSnapPivot.y);
				for (int i = 0; i < count; i++)
				{
					mItemList[i].DistanceWithViewPortSnapCenter = num4 - num3;
					if (i + 1 < count)
					{
						num = num2;
						num2 -= mItemList[i + 1].ItemSizeWithPadding;
						num3 = num - mItemList[i + 1].ItemSize * (1f - mItemSnapPivot.y);
					}
				}
			}
			else if (mArrangeType == ListItemArrangeType.BottomToTop)
			{
				num4 = mViewPortSnapPivot.y * mViewPortRectTransform.rect.height;
				Vector3 vector2 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[0]);
				num = vector2.y;
				num2 = num + loopListViewItem.ItemSizeWithPadding;
				num3 = num + loopListViewItem.ItemSize * mItemSnapPivot.y;
				for (int j = 0; j < count; j++)
				{
					mItemList[j].DistanceWithViewPortSnapCenter = num4 - num3;
					if (j + 1 < count)
					{
						num = num2;
						num2 += mItemList[j + 1].ItemSizeWithPadding;
						num3 = num + mItemList[j + 1].ItemSize * mItemSnapPivot.y;
					}
				}
			}
			else if (mArrangeType == ListItemArrangeType.RightToLeft)
			{
				num4 = (0f - (1f - mViewPortSnapPivot.x)) * mViewPortRectTransform.rect.width;
				Vector3 vector3 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[2]);
				num = vector3.x;
				num2 = num - loopListViewItem.ItemSizeWithPadding;
				num3 = num - loopListViewItem.ItemSize * (1f - mItemSnapPivot.x);
				for (int k = 0; k < count; k++)
				{
					mItemList[k].DistanceWithViewPortSnapCenter = num4 - num3;
					if (k + 1 < count)
					{
						num = num2;
						num2 -= mItemList[k + 1].ItemSizeWithPadding;
						num3 = num - mItemList[k + 1].ItemSize * (1f - mItemSnapPivot.x);
					}
				}
			}
			else
			{
				if (mArrangeType != ListItemArrangeType.LeftToRight)
				{
					return;
				}
				num4 = mViewPortSnapPivot.x * mViewPortRectTransform.rect.width;
				Vector3 vector4 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[1]);
				num = vector4.x;
				num2 = num + loopListViewItem.ItemSizeWithPadding;
				num3 = num + loopListViewItem.ItemSize * mItemSnapPivot.x;
				for (int l = 0; l < count; l++)
				{
					mItemList[l].DistanceWithViewPortSnapCenter = num4 - num3;
					if (l + 1 < count)
					{
						num = num2;
						num2 += mItemList[l + 1].ItemSizeWithPadding;
						num3 = num + mItemList[l + 1].ItemSize * mItemSnapPivot.x;
					}
				}
			}
		}

		private void UpdateSnapVertical(bool immediate = false)
		{
			if (!mItemSnapEnable)
			{
				return;
			}
			int count = mItemList.Count;
			if (count == 0)
			{
				return;
			}
			Vector3 localPosition = mContainerTrans.localPosition;
			bool flag = localPosition.y != mLastSnapCheckPos.y;
			mLastSnapCheckPos = localPosition;
			if (!flag && mLeftSnapUpdateExtraCount > 0)
			{
				mLeftSnapUpdateExtraCount--;
				flag = true;
			}
			if (flag)
			{
				LoopListViewItem2 loopListViewItem = mItemList[0];
				loopListViewItem.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
				int num = -1;
				float num2 = 0f;
				float num3 = 0f;
				float num4 = 0f;
				float num5 = float.MaxValue;
				float num6 = 0f;
				float num7 = 0f;
				float num8 = 0f;
				if (mArrangeType == ListItemArrangeType.TopToBottom)
				{
					num8 = (0f - (1f - mViewPortSnapPivot.y)) * mViewPortRectTransform.rect.height;
					Vector3 vector = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[1]);
					num2 = vector.y;
					num3 = num2 - loopListViewItem.ItemSizeWithPadding;
					num4 = num2 - loopListViewItem.ItemSize * (1f - mItemSnapPivot.y);
					for (int i = 0; i < count; i++)
					{
						num6 = num8 - num4;
						num7 = Mathf.Abs(num6);
						if (num7 < num5)
						{
							num5 = num7;
							num = i;
							if (i + 1 < count)
							{
								num2 = num3;
								num3 -= mItemList[i + 1].ItemSizeWithPadding;
								num4 = num2 - mItemList[i + 1].ItemSize * (1f - mItemSnapPivot.y);
							}
							continue;
						}
						break;
					}
				}
				else if (mArrangeType == ListItemArrangeType.BottomToTop)
				{
					num8 = mViewPortSnapPivot.y * mViewPortRectTransform.rect.height;
					Vector3 vector2 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[0]);
					num2 = vector2.y;
					num3 = num2 + loopListViewItem.ItemSizeWithPadding;
					num4 = num2 + loopListViewItem.ItemSize * mItemSnapPivot.y;
					for (int j = 0; j < count; j++)
					{
						num6 = num8 - num4;
						num7 = Mathf.Abs(num6);
						if (num7 < num5)
						{
							num5 = num7;
							num = j;
							if (j + 1 < count)
							{
								num2 = num3;
								num3 += mItemList[j + 1].ItemSizeWithPadding;
								num4 = num2 + mItemList[j + 1].ItemSize * mItemSnapPivot.y;
							}
							continue;
						}
						break;
					}
				}
				if (num >= 0)
				{
					int num9 = mCurSnapNearestItemIndex;
					mCurSnapNearestItemIndex = mItemList[num].ItemIndex;
					if (mItemList[num].ItemIndex != num9 && mOnSnapNearestChanged != null)
					{
						mOnSnapNearestChanged(this, mItemList[num]);
					}
				}
				else
				{
					mCurSnapNearestItemIndex = -1;
				}
			}
			if (!CanSnap())
			{
				ClearSnapData();
				return;
			}
			Vector2 velocity = mScrollRect.velocity;
			float num10 = Mathf.Abs(velocity.y);
			UpdateCurSnapData();
			if (mCurSnapData.mSnapStatus != SnapStatus.SnapMoving)
			{
				return;
			}
			if (num10 > 0f)
			{
				mScrollRect.StopMovement();
			}
			float mCurSnapVal = mCurSnapData.mCurSnapVal;
			mCurSnapData.mCurSnapVal = Mathf.SmoothDamp(mCurSnapData.mCurSnapVal, mCurSnapData.mTargetSnapVal, ref mSmoothDumpVel, mSmoothDumpRate);
			float num11 = mCurSnapData.mCurSnapVal - mCurSnapVal;
			if (immediate || Mathf.Abs(mCurSnapData.mTargetSnapVal - mCurSnapData.mCurSnapVal) < mSnapFinishThreshold)
			{
				localPosition.y = localPosition.y + mCurSnapData.mTargetSnapVal - mCurSnapVal;
				mCurSnapData.mSnapStatus = SnapStatus.SnapMoveFinish;
				if (mOnSnapItemFinished != null)
				{
					LoopListViewItem2 shownItemByItemIndex = GetShownItemByItemIndex(mCurSnapNearestItemIndex);
					if (shownItemByItemIndex != null)
					{
						mOnSnapItemFinished(this, shownItemByItemIndex);
					}
				}
			}
			else
			{
				localPosition.y += num11;
			}
			if (mArrangeType == ListItemArrangeType.TopToBottom)
			{
				float max = mViewPortRectLocalCorners[0].y + mContainerTrans.rect.height;
				localPosition.y = Mathf.Clamp(localPosition.y, 0f, max);
				mContainerTrans.localPosition = localPosition;
			}
			else if (mArrangeType == ListItemArrangeType.BottomToTop)
			{
				float min = mViewPortRectLocalCorners[1].y - mContainerTrans.rect.height;
				localPosition.y = Mathf.Clamp(localPosition.y, min, 0f);
				mContainerTrans.localPosition = localPosition;
			}
		}

		private void UpdateCurSnapData()
		{
			if (mItemList.Count == 0)
			{
				mCurSnapData.Clear();
				return;
			}
			if (mCurSnapData.mSnapStatus == SnapStatus.SnapMoveFinish)
			{
				if (mCurSnapData.mSnapTargetIndex == mCurSnapNearestItemIndex)
				{
					return;
				}
				mCurSnapData.mSnapStatus = SnapStatus.NoTargetSet;
			}
			if (mCurSnapData.mSnapStatus == SnapStatus.SnapMoving)
			{
				if (mCurSnapData.mSnapTargetIndex == mCurSnapNearestItemIndex || mCurSnapData.mIsForceSnapTo)
				{
					return;
				}
				mCurSnapData.mSnapStatus = SnapStatus.NoTargetSet;
			}
			if (mCurSnapData.mSnapStatus == SnapStatus.NoTargetSet)
			{
				LoopListViewItem2 shownItemByItemIndex = GetShownItemByItemIndex(mCurSnapNearestItemIndex);
				if (shownItemByItemIndex == null)
				{
					return;
				}
				mCurSnapData.mSnapTargetIndex = mCurSnapNearestItemIndex;
				mCurSnapData.mSnapStatus = SnapStatus.TargetHasSet;
				mCurSnapData.mIsForceSnapTo = false;
			}
			if (mCurSnapData.mSnapStatus == SnapStatus.TargetHasSet)
			{
				LoopListViewItem2 shownItemByItemIndex2 = GetShownItemByItemIndex(mCurSnapData.mSnapTargetIndex);
				if (shownItemByItemIndex2 == null)
				{
					mCurSnapData.Clear();
					return;
				}
				UpdateAllShownItemSnapData();
				mCurSnapData.mTargetSnapVal = shownItemByItemIndex2.DistanceWithViewPortSnapCenter;
				mCurSnapData.mCurSnapVal = 0f;
				mCurSnapData.mSnapStatus = SnapStatus.SnapMoving;
			}
		}

		public void ClearSnapData()
		{
			mCurSnapData.Clear();
		}

		public void SetSnapTargetItemIndex(int itemIndex)
		{
			mCurSnapData.mSnapTargetIndex = itemIndex;
			mCurSnapData.mSnapStatus = SnapStatus.TargetHasSet;
			mCurSnapData.mIsForceSnapTo = true;
		}

		public void ForceSnapUpdateCheck()
		{
			if (mLeftSnapUpdateExtraCount <= 0)
			{
				mLeftSnapUpdateExtraCount = 1;
			}
		}

		private void UpdateSnapHorizontal(bool immediate = false)
		{
			if (!mItemSnapEnable)
			{
				return;
			}
			int count = mItemList.Count;
			if (count == 0)
			{
				return;
			}
			Vector3 localPosition = mContainerTrans.localPosition;
			bool flag = localPosition.x != mLastSnapCheckPos.x;
			mLastSnapCheckPos = localPosition;
			if (!flag && mLeftSnapUpdateExtraCount > 0)
			{
				mLeftSnapUpdateExtraCount--;
				flag = true;
			}
			if (flag)
			{
				LoopListViewItem2 loopListViewItem = mItemList[0];
				loopListViewItem.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
				int num = -1;
				float num2 = 0f;
				float num3 = 0f;
				float num4 = 0f;
				float num5 = float.MaxValue;
				float num6 = 0f;
				float num7 = 0f;
				float num8 = 0f;
				if (mArrangeType == ListItemArrangeType.RightToLeft)
				{
					num8 = (0f - (1f - mViewPortSnapPivot.x)) * mViewPortRectTransform.rect.width;
					Vector3 vector = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[2]);
					num2 = vector.x;
					num3 = num2 - loopListViewItem.ItemSizeWithPadding;
					num4 = num2 - loopListViewItem.ItemSize * (1f - mItemSnapPivot.x);
					for (int i = 0; i < count; i++)
					{
						num6 = num8 - num4;
						num7 = Mathf.Abs(num6);
						if (num7 < num5)
						{
							num5 = num7;
							num = i;
							if (i + 1 < count)
							{
								num2 = num3;
								num3 -= mItemList[i + 1].ItemSizeWithPadding;
								num4 = num2 - mItemList[i + 1].ItemSize * (1f - mItemSnapPivot.x);
							}
							continue;
						}
						break;
					}
				}
				else if (mArrangeType == ListItemArrangeType.LeftToRight)
				{
					num8 = mViewPortSnapPivot.x * mViewPortRectTransform.rect.width;
					Vector3 vector2 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[1]);
					num2 = vector2.x;
					num3 = num2 + loopListViewItem.ItemSizeWithPadding;
					num4 = num2 + loopListViewItem.ItemSize * mItemSnapPivot.x;
					for (int j = 0; j < count; j++)
					{
						num6 = num8 - num4;
						num7 = Mathf.Abs(num6);
						if (num7 < num5)
						{
							num5 = num7;
							num = j;
							if (j + 1 < count)
							{
								num2 = num3;
								num3 += mItemList[j + 1].ItemSizeWithPadding;
								num4 = num2 + mItemList[j + 1].ItemSize * mItemSnapPivot.x;
							}
							continue;
						}
						break;
					}
				}
				if (num >= 0)
				{
					int num9 = mCurSnapNearestItemIndex;
					mCurSnapNearestItemIndex = mItemList[num].ItemIndex;
					if (mItemList[num].ItemIndex != num9 && mOnSnapNearestChanged != null)
					{
						mOnSnapNearestChanged(this, mItemList[num]);
					}
				}
				else
				{
					mCurSnapNearestItemIndex = -1;
				}
			}
			if (!CanSnap())
			{
				ClearSnapData();
				return;
			}
			Vector2 velocity = mScrollRect.velocity;
			float num10 = Mathf.Abs(velocity.x);
			UpdateCurSnapData();
			if (mCurSnapData.mSnapStatus != SnapStatus.SnapMoving)
			{
				return;
			}
			if (num10 > 0f)
			{
				mScrollRect.StopMovement();
			}
			float mCurSnapVal = mCurSnapData.mCurSnapVal;
			mCurSnapData.mCurSnapVal = Mathf.SmoothDamp(mCurSnapData.mCurSnapVal, mCurSnapData.mTargetSnapVal, ref mSmoothDumpVel, mSmoothDumpRate);
			float num11 = mCurSnapData.mCurSnapVal - mCurSnapVal;
			if (immediate || Mathf.Abs(mCurSnapData.mTargetSnapVal - mCurSnapData.mCurSnapVal) < mSnapFinishThreshold)
			{
				localPosition.x = localPosition.x + mCurSnapData.mTargetSnapVal - mCurSnapVal;
				mCurSnapData.mSnapStatus = SnapStatus.SnapMoveFinish;
				if (mOnSnapItemFinished != null)
				{
					LoopListViewItem2 shownItemByItemIndex = GetShownItemByItemIndex(mCurSnapNearestItemIndex);
					if (shownItemByItemIndex != null)
					{
						mOnSnapItemFinished(this, shownItemByItemIndex);
					}
				}
			}
			else
			{
				localPosition.x += num11;
			}
			if (mArrangeType == ListItemArrangeType.LeftToRight)
			{
				float min = mViewPortRectLocalCorners[2].x - mContainerTrans.rect.width;
				localPosition.x = Mathf.Clamp(localPosition.x, min, 0f);
				mContainerTrans.localPosition = localPosition;
			}
			else if (mArrangeType == ListItemArrangeType.RightToLeft)
			{
				float max = mViewPortRectLocalCorners[1].x + mContainerTrans.rect.width;
				localPosition.x = Mathf.Clamp(localPosition.x, 0f, max);
				mContainerTrans.localPosition = localPosition;
			}
		}

		private bool CanSnap()
		{
			if (mIsDraging)
			{
				return false;
			}
			if (mScrollBarClickEventListener != null && mScrollBarClickEventListener.IsPressd)
			{
				return false;
			}
			if (mIsVertList)
			{
				if (mContainerTrans.rect.height <= ViewPortHeight)
				{
					return false;
				}
			}
			else if (mContainerTrans.rect.width <= ViewPortWidth)
			{
				return false;
			}
			float num = 0f;
			if (mIsVertList)
			{
				Vector2 velocity = mScrollRect.velocity;
				num = Mathf.Abs(velocity.y);
			}
			else
			{
				Vector2 velocity2 = mScrollRect.velocity;
				num = Mathf.Abs(velocity2.x);
			}
			if (num > mSnapVecThreshold)
			{
				return false;
			}
			if (num < 2f)
			{
				return true;
			}
			float num2 = 3f;
			Vector3 localPosition = mContainerTrans.localPosition;
			if (mArrangeType == ListItemArrangeType.LeftToRight)
			{
				float num3 = mViewPortRectLocalCorners[2].x - mContainerTrans.rect.width;
				if (localPosition.x < num3 - num2 || localPosition.x > num2)
				{
					return false;
				}
			}
			else if (mArrangeType == ListItemArrangeType.RightToLeft)
			{
				float num4 = mViewPortRectLocalCorners[1].x + mContainerTrans.rect.width;
				if (localPosition.x > num4 + num2 || localPosition.x < 0f - num2)
				{
					return false;
				}
			}
			else if (mArrangeType == ListItemArrangeType.TopToBottom)
			{
				float num5 = mViewPortRectLocalCorners[0].y + mContainerTrans.rect.height;
				if (localPosition.y > num5 + num2 || localPosition.y < 0f - num2)
				{
					return false;
				}
			}
			else if (mArrangeType == ListItemArrangeType.BottomToTop)
			{
				float num6 = mViewPortRectLocalCorners[1].y - mContainerTrans.rect.height;
				if (localPosition.y < num6 - num2 || localPosition.y > num2)
				{
					return false;
				}
			}
			return true;
		}

		public void UpdateListView(float distanceForRecycle0, float distanceForRecycle1, float distanceForNew0, float distanceForNew1)
		{
			mListUpdateCheckFrameCount++;
			if (mIsVertList)
			{
				bool flag = true;
				int num = 0;
				int num2 = 9999;
				while (true)
				{
					if (flag)
					{
						num++;
						if (num >= num2)
						{
							break;
						}
						flag = UpdateForVertList(distanceForRecycle0, distanceForRecycle1, distanceForNew0, distanceForNew1);
						continue;
					}
					return;
				}
				UnityEngine.Debug.LogError("UpdateListView Vertical while loop " + num + " times! something is wrong!");
				return;
			}
			bool flag2 = true;
			int num3 = 0;
			int num4 = 9999;
			while (true)
			{
				if (flag2)
				{
					num3++;
					if (num3 >= num4)
					{
						break;
					}
					flag2 = UpdateForHorizontalList(distanceForRecycle0, distanceForRecycle1, distanceForNew0, distanceForNew1);
					continue;
				}
				return;
			}
			UnityEngine.Debug.LogError("UpdateListView  Horizontal while loop " + num3 + " times! something is wrong!");
		}

		private bool UpdateForVertList(float distanceForRecycle0, float distanceForRecycle1, float distanceForNew0, float distanceForNew1)
		{
			if (mItemTotalCount == 0)
			{
				if (mItemList.Count > 0)
				{
					RecycleAllItem();
				}
				return false;
			}
			if (mArrangeType == ListItemArrangeType.TopToBottom)
			{
				if (mItemList.Count == 0)
				{
					Vector3 localPosition = mContainerTrans.localPosition;
					float num = localPosition.y;
					if (num < 0f)
					{
						num = 0f;
					}
					int index = 0;
					float itemPos = 0f - num;
					if (mSupportScrollBar)
					{
						GetPlusItemIndexAndPosAtGivenPos(num, ref index, ref itemPos);
						itemPos = 0f - itemPos;
					}
					LoopListViewItem2 newItemByIndex = GetNewItemByIndex(index);
					if (newItemByIndex == null)
					{
						return false;
					}
					if (mSupportScrollBar)
					{
						SetItemSize(index, newItemByIndex.CachedRectTransform.rect.height, newItemByIndex.Padding);
					}
					mItemList.Add(newItemByIndex);
					newItemByIndex.CachedRectTransform.localPosition = new Vector3(newItemByIndex.StartPosOffset, itemPos, 0f);
					UpdateContentSize();
					return true;
				}
				LoopListViewItem2 loopListViewItem = mItemList[0];
				loopListViewItem.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
				Vector3 vector = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[1]);
				Vector3 vector2 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[0]);
				if (!mIsDraging && loopListViewItem.ItemCreatedCheckFrameCount != mListUpdateCheckFrameCount && vector2.y - mViewPortRectLocalCorners[1].y > distanceForRecycle0)
				{
					mItemList.RemoveAt(0);
					RecycleItemTmp(loopListViewItem);
					if (!mSupportScrollBar)
					{
						UpdateContentSize();
						CheckIfNeedUpdataItemPos();
					}
					return true;
				}
				LoopListViewItem2 loopListViewItem2 = mItemList[mItemList.Count - 1];
				loopListViewItem2.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
				Vector3 vector3 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[1]);
				Vector3 vector4 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[0]);
				if (!mIsDraging && loopListViewItem2.ItemCreatedCheckFrameCount != mListUpdateCheckFrameCount && mViewPortRectLocalCorners[0].y - vector3.y > distanceForRecycle1)
				{
					mItemList.RemoveAt(mItemList.Count - 1);
					RecycleItemTmp(loopListViewItem2);
					if (!mSupportScrollBar)
					{
						UpdateContentSize();
						CheckIfNeedUpdataItemPos();
					}
					return true;
				}
				if (mViewPortRectLocalCorners[0].y - vector4.y < distanceForNew1)
				{
					if (loopListViewItem2.ItemIndex > mCurReadyMaxItemIndex)
					{
						mCurReadyMaxItemIndex = loopListViewItem2.ItemIndex;
						mNeedCheckNextMaxItem = true;
					}
					int num2 = loopListViewItem2.ItemIndex + 1;
					if (num2 <= mCurReadyMaxItemIndex || mNeedCheckNextMaxItem)
					{
						LoopListViewItem2 newItemByIndex2 = GetNewItemByIndex(num2);
						if (!(newItemByIndex2 == null))
						{
							if (mSupportScrollBar)
							{
								SetItemSize(num2, newItemByIndex2.CachedRectTransform.rect.height, newItemByIndex2.Padding);
							}
							mItemList.Add(newItemByIndex2);
							Vector3 localPosition2 = loopListViewItem2.CachedRectTransform.localPosition;
							float y = localPosition2.y - loopListViewItem2.CachedRectTransform.rect.height - loopListViewItem2.Padding;
							newItemByIndex2.CachedRectTransform.localPosition = new Vector3(newItemByIndex2.StartPosOffset, y, 0f);
							UpdateContentSize();
							CheckIfNeedUpdataItemPos();
							if (num2 > mCurReadyMaxItemIndex)
							{
								mCurReadyMaxItemIndex = num2;
							}
							return true;
						}
						mCurReadyMaxItemIndex = loopListViewItem2.ItemIndex;
						mNeedCheckNextMaxItem = false;
						CheckIfNeedUpdataItemPos();
					}
				}
				if (vector.y - mViewPortRectLocalCorners[1].y < distanceForNew0)
				{
					if (loopListViewItem.ItemIndex < mCurReadyMinItemIndex)
					{
						mCurReadyMinItemIndex = loopListViewItem.ItemIndex;
						mNeedCheckNextMinItem = true;
					}
					int num3 = loopListViewItem.ItemIndex - 1;
					if (num3 >= mCurReadyMinItemIndex || mNeedCheckNextMinItem)
					{
						LoopListViewItem2 newItemByIndex3 = GetNewItemByIndex(num3);
						if (!(newItemByIndex3 == null))
						{
							if (mSupportScrollBar)
							{
								SetItemSize(num3, newItemByIndex3.CachedRectTransform.rect.height, newItemByIndex3.Padding);
							}
							mItemList.Insert(0, newItemByIndex3);
							Vector3 localPosition3 = loopListViewItem.CachedRectTransform.localPosition;
							float y2 = localPosition3.y + newItemByIndex3.CachedRectTransform.rect.height + newItemByIndex3.Padding;
							newItemByIndex3.CachedRectTransform.localPosition = new Vector3(newItemByIndex3.StartPosOffset, y2, 0f);
							UpdateContentSize();
							CheckIfNeedUpdataItemPos();
							if (num3 < mCurReadyMinItemIndex)
							{
								mCurReadyMinItemIndex = num3;
							}
							return true;
						}
						mCurReadyMinItemIndex = loopListViewItem.ItemIndex;
						mNeedCheckNextMinItem = false;
					}
				}
			}
			else
			{
				if (mItemList.Count == 0)
				{
					Vector3 localPosition4 = mContainerTrans.localPosition;
					float num4 = localPosition4.y;
					if (num4 > 0f)
					{
						num4 = 0f;
					}
					int index2 = 0;
					float itemPos2 = 0f - num4;
					if (mSupportScrollBar)
					{
						GetPlusItemIndexAndPosAtGivenPos(0f - num4, ref index2, ref itemPos2);
					}
					LoopListViewItem2 newItemByIndex4 = GetNewItemByIndex(index2);
					if (newItemByIndex4 == null)
					{
						return false;
					}
					if (mSupportScrollBar)
					{
						SetItemSize(index2, newItemByIndex4.CachedRectTransform.rect.height, newItemByIndex4.Padding);
					}
					mItemList.Add(newItemByIndex4);
					newItemByIndex4.CachedRectTransform.localPosition = new Vector3(newItemByIndex4.StartPosOffset, itemPos2, 0f);
					UpdateContentSize();
					return true;
				}
				LoopListViewItem2 loopListViewItem3 = mItemList[0];
				loopListViewItem3.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
				Vector3 vector5 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[1]);
				Vector3 vector6 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[0]);
				if (!mIsDraging && loopListViewItem3.ItemCreatedCheckFrameCount != mListUpdateCheckFrameCount && mViewPortRectLocalCorners[0].y - vector5.y > distanceForRecycle0)
				{
					mItemList.RemoveAt(0);
					RecycleItemTmp(loopListViewItem3);
					if (!mSupportScrollBar)
					{
						UpdateContentSize();
						CheckIfNeedUpdataItemPos();
					}
					return true;
				}
				LoopListViewItem2 loopListViewItem4 = mItemList[mItemList.Count - 1];
				loopListViewItem4.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
				Vector3 vector7 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[1]);
				Vector3 vector8 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[0]);
				if (!mIsDraging && loopListViewItem4.ItemCreatedCheckFrameCount != mListUpdateCheckFrameCount && vector8.y - mViewPortRectLocalCorners[1].y > distanceForRecycle1)
				{
					mItemList.RemoveAt(mItemList.Count - 1);
					RecycleItemTmp(loopListViewItem4);
					if (!mSupportScrollBar)
					{
						UpdateContentSize();
						CheckIfNeedUpdataItemPos();
					}
					return true;
				}
				if (vector7.y - mViewPortRectLocalCorners[1].y < distanceForNew1)
				{
					if (loopListViewItem4.ItemIndex > mCurReadyMaxItemIndex)
					{
						mCurReadyMaxItemIndex = loopListViewItem4.ItemIndex;
						mNeedCheckNextMaxItem = true;
					}
					int num5 = loopListViewItem4.ItemIndex + 1;
					if (num5 <= mCurReadyMaxItemIndex || mNeedCheckNextMaxItem)
					{
						LoopListViewItem2 newItemByIndex5 = GetNewItemByIndex(num5);
						if (!(newItemByIndex5 == null))
						{
							if (mSupportScrollBar)
							{
								SetItemSize(num5, newItemByIndex5.CachedRectTransform.rect.height, newItemByIndex5.Padding);
							}
							mItemList.Add(newItemByIndex5);
							Vector3 localPosition5 = loopListViewItem4.CachedRectTransform.localPosition;
							float y3 = localPosition5.y + loopListViewItem4.CachedRectTransform.rect.height + loopListViewItem4.Padding;
							newItemByIndex5.CachedRectTransform.localPosition = new Vector3(newItemByIndex5.StartPosOffset, y3, 0f);
							UpdateContentSize();
							CheckIfNeedUpdataItemPos();
							if (num5 > mCurReadyMaxItemIndex)
							{
								mCurReadyMaxItemIndex = num5;
							}
							return true;
						}
						mNeedCheckNextMaxItem = false;
						CheckIfNeedUpdataItemPos();
					}
				}
				if (mViewPortRectLocalCorners[0].y - vector6.y < distanceForNew0)
				{
					if (loopListViewItem3.ItemIndex < mCurReadyMinItemIndex)
					{
						mCurReadyMinItemIndex = loopListViewItem3.ItemIndex;
						mNeedCheckNextMinItem = true;
					}
					int num6 = loopListViewItem3.ItemIndex - 1;
					if (num6 >= mCurReadyMinItemIndex || mNeedCheckNextMinItem)
					{
						LoopListViewItem2 newItemByIndex6 = GetNewItemByIndex(num6);
						if (newItemByIndex6 == null)
						{
							mNeedCheckNextMinItem = false;
							return false;
						}
						if (mSupportScrollBar)
						{
							SetItemSize(num6, newItemByIndex6.CachedRectTransform.rect.height, newItemByIndex6.Padding);
						}
						mItemList.Insert(0, newItemByIndex6);
						Vector3 localPosition6 = loopListViewItem3.CachedRectTransform.localPosition;
						float y4 = localPosition6.y - newItemByIndex6.CachedRectTransform.rect.height - newItemByIndex6.Padding;
						newItemByIndex6.CachedRectTransform.localPosition = new Vector3(newItemByIndex6.StartPosOffset, y4, 0f);
						UpdateContentSize();
						CheckIfNeedUpdataItemPos();
						if (num6 < mCurReadyMinItemIndex)
						{
							mCurReadyMinItemIndex = num6;
						}
						return true;
					}
				}
			}
			return false;
		}

		private bool UpdateForHorizontalList(float distanceForRecycle0, float distanceForRecycle1, float distanceForNew0, float distanceForNew1)
		{
			if (mItemTotalCount == 0)
			{
				if (mItemList.Count > 0)
				{
					RecycleAllItem();
				}
				return false;
			}
			if (mArrangeType == ListItemArrangeType.LeftToRight)
			{
				if (mItemList.Count == 0)
				{
					Vector3 localPosition = mContainerTrans.localPosition;
					float num = localPosition.x;
					if (num > 0f)
					{
						num = 0f;
					}
					int index = 0;
					float itemPos = 0f - num;
					if (mSupportScrollBar)
					{
						GetPlusItemIndexAndPosAtGivenPos(0f - num, ref index, ref itemPos);
					}
					LoopListViewItem2 newItemByIndex = GetNewItemByIndex(index);
					if (newItemByIndex == null)
					{
						return false;
					}
					if (mSupportScrollBar)
					{
						SetItemSize(index, newItemByIndex.CachedRectTransform.rect.width, newItemByIndex.Padding);
					}
					mItemList.Add(newItemByIndex);
					newItemByIndex.CachedRectTransform.localPosition = new Vector3(itemPos, newItemByIndex.StartPosOffset, 0f);
					UpdateContentSize();
					return true;
				}
				LoopListViewItem2 loopListViewItem = mItemList[0];
				loopListViewItem.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
				Vector3 vector = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[1]);
				Vector3 vector2 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[2]);
				if (!mIsDraging && loopListViewItem.ItemCreatedCheckFrameCount != mListUpdateCheckFrameCount && mViewPortRectLocalCorners[1].x - vector2.x > distanceForRecycle0)
				{
					mItemList.RemoveAt(0);
					RecycleItemTmp(loopListViewItem);
					if (!mSupportScrollBar)
					{
						UpdateContentSize();
						CheckIfNeedUpdataItemPos();
					}
					return true;
				}
				LoopListViewItem2 loopListViewItem2 = mItemList[mItemList.Count - 1];
				loopListViewItem2.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
				Vector3 vector3 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[1]);
				Vector3 vector4 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[2]);
				if (!mIsDraging && loopListViewItem2.ItemCreatedCheckFrameCount != mListUpdateCheckFrameCount && vector3.x - mViewPortRectLocalCorners[2].x > distanceForRecycle1)
				{
					mItemList.RemoveAt(mItemList.Count - 1);
					RecycleItemTmp(loopListViewItem2);
					if (!mSupportScrollBar)
					{
						UpdateContentSize();
						CheckIfNeedUpdataItemPos();
					}
					return true;
				}
				if (vector4.x - mViewPortRectLocalCorners[2].x < distanceForNew1)
				{
					if (loopListViewItem2.ItemIndex > mCurReadyMaxItemIndex)
					{
						mCurReadyMaxItemIndex = loopListViewItem2.ItemIndex;
						mNeedCheckNextMaxItem = true;
					}
					int num2 = loopListViewItem2.ItemIndex + 1;
					if (num2 <= mCurReadyMaxItemIndex || mNeedCheckNextMaxItem)
					{
						LoopListViewItem2 newItemByIndex2 = GetNewItemByIndex(num2);
						if (!(newItemByIndex2 == null))
						{
							if (mSupportScrollBar)
							{
								SetItemSize(num2, newItemByIndex2.CachedRectTransform.rect.width, newItemByIndex2.Padding);
							}
							mItemList.Add(newItemByIndex2);
							Vector3 localPosition2 = loopListViewItem2.CachedRectTransform.localPosition;
							float x = localPosition2.x + loopListViewItem2.CachedRectTransform.rect.width + loopListViewItem2.Padding;
							newItemByIndex2.CachedRectTransform.localPosition = new Vector3(x, newItemByIndex2.StartPosOffset, 0f);
							UpdateContentSize();
							CheckIfNeedUpdataItemPos();
							if (num2 > mCurReadyMaxItemIndex)
							{
								mCurReadyMaxItemIndex = num2;
							}
							return true;
						}
						mCurReadyMaxItemIndex = loopListViewItem2.ItemIndex;
						mNeedCheckNextMaxItem = false;
						CheckIfNeedUpdataItemPos();
					}
				}
				if (mViewPortRectLocalCorners[1].x - vector.x < distanceForNew0)
				{
					if (loopListViewItem.ItemIndex < mCurReadyMinItemIndex)
					{
						mCurReadyMinItemIndex = loopListViewItem.ItemIndex;
						mNeedCheckNextMinItem = true;
					}
					int num3 = loopListViewItem.ItemIndex - 1;
					if (num3 >= mCurReadyMinItemIndex || mNeedCheckNextMinItem)
					{
						LoopListViewItem2 newItemByIndex3 = GetNewItemByIndex(num3);
						if (!(newItemByIndex3 == null))
						{
							if (mSupportScrollBar)
							{
								SetItemSize(num3, newItemByIndex3.CachedRectTransform.rect.width, newItemByIndex3.Padding);
							}
							mItemList.Insert(0, newItemByIndex3);
							Vector3 localPosition3 = loopListViewItem.CachedRectTransform.localPosition;
							float x2 = localPosition3.x - newItemByIndex3.CachedRectTransform.rect.width - newItemByIndex3.Padding;
							newItemByIndex3.CachedRectTransform.localPosition = new Vector3(x2, newItemByIndex3.StartPosOffset, 0f);
							UpdateContentSize();
							CheckIfNeedUpdataItemPos();
							if (num3 < mCurReadyMinItemIndex)
							{
								mCurReadyMinItemIndex = num3;
							}
							return true;
						}
						mCurReadyMinItemIndex = loopListViewItem.ItemIndex;
						mNeedCheckNextMinItem = false;
					}
				}
			}
			else
			{
				if (mItemList.Count == 0)
				{
					Vector3 localPosition4 = mContainerTrans.localPosition;
					float num4 = localPosition4.x;
					if (num4 < 0f)
					{
						num4 = 0f;
					}
					int index2 = 0;
					float itemPos2 = 0f - num4;
					if (mSupportScrollBar)
					{
						GetPlusItemIndexAndPosAtGivenPos(num4, ref index2, ref itemPos2);
						itemPos2 = 0f - itemPos2;
					}
					LoopListViewItem2 newItemByIndex4 = GetNewItemByIndex(index2);
					if (newItemByIndex4 == null)
					{
						return false;
					}
					if (mSupportScrollBar)
					{
						SetItemSize(index2, newItemByIndex4.CachedRectTransform.rect.width, newItemByIndex4.Padding);
					}
					mItemList.Add(newItemByIndex4);
					newItemByIndex4.CachedRectTransform.localPosition = new Vector3(itemPos2, newItemByIndex4.StartPosOffset, 0f);
					UpdateContentSize();
					return true;
				}
				LoopListViewItem2 loopListViewItem3 = mItemList[0];
				loopListViewItem3.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
				Vector3 vector5 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[1]);
				Vector3 vector6 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[2]);
				if (!mIsDraging && loopListViewItem3.ItemCreatedCheckFrameCount != mListUpdateCheckFrameCount && vector5.x - mViewPortRectLocalCorners[2].x > distanceForRecycle0)
				{
					mItemList.RemoveAt(0);
					RecycleItemTmp(loopListViewItem3);
					if (!mSupportScrollBar)
					{
						UpdateContentSize();
						CheckIfNeedUpdataItemPos();
					}
					return true;
				}
				LoopListViewItem2 loopListViewItem4 = mItemList[mItemList.Count - 1];
				loopListViewItem4.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
				Vector3 vector7 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[1]);
				Vector3 vector8 = mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[2]);
				if (!mIsDraging && loopListViewItem4.ItemCreatedCheckFrameCount != mListUpdateCheckFrameCount && mViewPortRectLocalCorners[1].x - vector8.x > distanceForRecycle1)
				{
					mItemList.RemoveAt(mItemList.Count - 1);
					RecycleItemTmp(loopListViewItem4);
					if (!mSupportScrollBar)
					{
						UpdateContentSize();
						CheckIfNeedUpdataItemPos();
					}
					return true;
				}
				if (mViewPortRectLocalCorners[1].x - vector7.x < distanceForNew1)
				{
					if (loopListViewItem4.ItemIndex > mCurReadyMaxItemIndex)
					{
						mCurReadyMaxItemIndex = loopListViewItem4.ItemIndex;
						mNeedCheckNextMaxItem = true;
					}
					int num5 = loopListViewItem4.ItemIndex + 1;
					if (num5 <= mCurReadyMaxItemIndex || mNeedCheckNextMaxItem)
					{
						LoopListViewItem2 newItemByIndex5 = GetNewItemByIndex(num5);
						if (!(newItemByIndex5 == null))
						{
							if (mSupportScrollBar)
							{
								SetItemSize(num5, newItemByIndex5.CachedRectTransform.rect.width, newItemByIndex5.Padding);
							}
							mItemList.Add(newItemByIndex5);
							Vector3 localPosition5 = loopListViewItem4.CachedRectTransform.localPosition;
							float x3 = localPosition5.x - loopListViewItem4.CachedRectTransform.rect.width - loopListViewItem4.Padding;
							newItemByIndex5.CachedRectTransform.localPosition = new Vector3(x3, newItemByIndex5.StartPosOffset, 0f);
							UpdateContentSize();
							CheckIfNeedUpdataItemPos();
							if (num5 > mCurReadyMaxItemIndex)
							{
								mCurReadyMaxItemIndex = num5;
							}
							return true;
						}
						mCurReadyMaxItemIndex = loopListViewItem4.ItemIndex;
						mNeedCheckNextMaxItem = false;
						CheckIfNeedUpdataItemPos();
					}
				}
				if (vector6.x - mViewPortRectLocalCorners[2].x < distanceForNew0)
				{
					if (loopListViewItem3.ItemIndex < mCurReadyMinItemIndex)
					{
						mCurReadyMinItemIndex = loopListViewItem3.ItemIndex;
						mNeedCheckNextMinItem = true;
					}
					int num6 = loopListViewItem3.ItemIndex - 1;
					if (num6 >= mCurReadyMinItemIndex || mNeedCheckNextMinItem)
					{
						LoopListViewItem2 newItemByIndex6 = GetNewItemByIndex(num6);
						if (!(newItemByIndex6 == null))
						{
							if (mSupportScrollBar)
							{
								SetItemSize(num6, newItemByIndex6.CachedRectTransform.rect.width, newItemByIndex6.Padding);
							}
							mItemList.Insert(0, newItemByIndex6);
							Vector3 localPosition6 = loopListViewItem3.CachedRectTransform.localPosition;
							float x4 = localPosition6.x + newItemByIndex6.CachedRectTransform.rect.width + newItemByIndex6.Padding;
							newItemByIndex6.CachedRectTransform.localPosition = new Vector3(x4, newItemByIndex6.StartPosOffset, 0f);
							UpdateContentSize();
							CheckIfNeedUpdataItemPos();
							if (num6 < mCurReadyMinItemIndex)
							{
								mCurReadyMinItemIndex = num6;
							}
							return true;
						}
						mCurReadyMinItemIndex = loopListViewItem3.ItemIndex;
						mNeedCheckNextMinItem = false;
					}
				}
			}
			return false;
		}

		private float GetContentPanelSize()
		{
			if (mSupportScrollBar)
			{
				float num = (!(mItemPosMgr.mTotalSize > 0f)) ? 0f : (mItemPosMgr.mTotalSize - mLastItemPadding);
				if (num < 0f)
				{
					num = 0f;
				}
				return num;
			}
			int count = mItemList.Count;
			switch (count)
			{
			case 0:
				return 0f;
			case 1:
				return mItemList[0].ItemSize;
			case 2:
				return mItemList[0].ItemSizeWithPadding + mItemList[1].ItemSize;
			default:
			{
				float num2 = 0f;
				for (int i = 0; i < count - 1; i++)
				{
					num2 += mItemList[i].ItemSizeWithPadding;
				}
				return num2 + mItemList[count - 1].ItemSize;
			}
			}
		}

		private void CheckIfNeedUpdataItemPos()
		{
			if (mItemList.Count == 0)
			{
				return;
			}
			if (mArrangeType == ListItemArrangeType.TopToBottom)
			{
				LoopListViewItem2 loopListViewItem = mItemList[0];
				LoopListViewItem2 loopListViewItem2 = mItemList[mItemList.Count - 1];
				float contentPanelSize = GetContentPanelSize();
				if (loopListViewItem.TopY > 0f || (loopListViewItem.ItemIndex == mCurReadyMinItemIndex && loopListViewItem.TopY != 0f))
				{
					UpdateAllShownItemsPos();
				}
				else if (0f - loopListViewItem2.BottomY > contentPanelSize || (loopListViewItem2.ItemIndex == mCurReadyMaxItemIndex && 0f - loopListViewItem2.BottomY != contentPanelSize))
				{
					UpdateAllShownItemsPos();
				}
			}
			else if (mArrangeType == ListItemArrangeType.BottomToTop)
			{
				LoopListViewItem2 loopListViewItem3 = mItemList[0];
				LoopListViewItem2 loopListViewItem4 = mItemList[mItemList.Count - 1];
				float contentPanelSize2 = GetContentPanelSize();
				if (loopListViewItem3.BottomY < 0f || (loopListViewItem3.ItemIndex == mCurReadyMinItemIndex && loopListViewItem3.BottomY != 0f))
				{
					UpdateAllShownItemsPos();
				}
				else if (loopListViewItem4.TopY > contentPanelSize2 || (loopListViewItem4.ItemIndex == mCurReadyMaxItemIndex && loopListViewItem4.TopY != contentPanelSize2))
				{
					UpdateAllShownItemsPos();
				}
			}
			else if (mArrangeType == ListItemArrangeType.LeftToRight)
			{
				LoopListViewItem2 loopListViewItem5 = mItemList[0];
				LoopListViewItem2 loopListViewItem6 = mItemList[mItemList.Count - 1];
				float contentPanelSize3 = GetContentPanelSize();
				if (loopListViewItem5.LeftX < 0f || (loopListViewItem5.ItemIndex == mCurReadyMinItemIndex && loopListViewItem5.LeftX != 0f))
				{
					UpdateAllShownItemsPos();
				}
				else if (loopListViewItem6.RightX > contentPanelSize3 || (loopListViewItem6.ItemIndex == mCurReadyMaxItemIndex && loopListViewItem6.RightX != contentPanelSize3))
				{
					UpdateAllShownItemsPos();
				}
			}
			else if (mArrangeType == ListItemArrangeType.RightToLeft)
			{
				LoopListViewItem2 loopListViewItem7 = mItemList[0];
				LoopListViewItem2 loopListViewItem8 = mItemList[mItemList.Count - 1];
				float contentPanelSize4 = GetContentPanelSize();
				if (loopListViewItem7.RightX > 0f || (loopListViewItem7.ItemIndex == mCurReadyMinItemIndex && loopListViewItem7.RightX != 0f))
				{
					UpdateAllShownItemsPos();
				}
				else if (0f - loopListViewItem8.LeftX > contentPanelSize4 || (loopListViewItem8.ItemIndex == mCurReadyMaxItemIndex && 0f - loopListViewItem8.LeftX != contentPanelSize4))
				{
					UpdateAllShownItemsPos();
				}
			}
		}

		private void UpdateAllShownItemsPos()
		{
			int count = mItemList.Count;
			if (count == 0)
			{
				return;
			}
			mAdjustedVec = (mContainerTrans.localPosition - mLastFrameContainerPos) / Time.deltaTime;
			if (mArrangeType == ListItemArrangeType.TopToBottom)
			{
				float num = 0f;
				if (mSupportScrollBar)
				{
					num = 0f - GetItemPos(mItemList[0].ItemIndex);
				}
				Vector3 localPosition = mItemList[0].CachedRectTransform.localPosition;
				float y = localPosition.y;
				float num2 = num - y;
				float num3 = num;
				for (int i = 0; i < count; i++)
				{
					LoopListViewItem2 loopListViewItem = mItemList[i];
					loopListViewItem.CachedRectTransform.localPosition = new Vector3(loopListViewItem.StartPosOffset, num3, 0f);
					num3 = num3 - loopListViewItem.CachedRectTransform.rect.height - loopListViewItem.Padding;
				}
				if (num2 != 0f)
				{
					Vector2 v = mContainerTrans.localPosition;
					v.y -= num2;
					mContainerTrans.localPosition = v;
				}
			}
			else if (mArrangeType == ListItemArrangeType.BottomToTop)
			{
				float num4 = 0f;
				if (mSupportScrollBar)
				{
					num4 = GetItemPos(mItemList[0].ItemIndex);
				}
				Vector3 localPosition2 = mItemList[0].CachedRectTransform.localPosition;
				float y2 = localPosition2.y;
				float num5 = num4 - y2;
				float num6 = num4;
				for (int j = 0; j < count; j++)
				{
					LoopListViewItem2 loopListViewItem2 = mItemList[j];
					loopListViewItem2.CachedRectTransform.localPosition = new Vector3(loopListViewItem2.StartPosOffset, num6, 0f);
					num6 = num6 + loopListViewItem2.CachedRectTransform.rect.height + loopListViewItem2.Padding;
				}
				if (num5 != 0f)
				{
					Vector3 localPosition3 = mContainerTrans.localPosition;
					localPosition3.y -= num5;
					mContainerTrans.localPosition = localPosition3;
				}
			}
			else if (mArrangeType == ListItemArrangeType.LeftToRight)
			{
				float num7 = 0f;
				if (mSupportScrollBar)
				{
					num7 = GetItemPos(mItemList[0].ItemIndex);
				}
				Vector3 localPosition4 = mItemList[0].CachedRectTransform.localPosition;
				float x = localPosition4.x;
				float num8 = num7 - x;
				float num9 = num7;
				for (int k = 0; k < count; k++)
				{
					LoopListViewItem2 loopListViewItem3 = mItemList[k];
					loopListViewItem3.CachedRectTransform.localPosition = new Vector3(num9, loopListViewItem3.StartPosOffset, 0f);
					num9 = num9 + loopListViewItem3.CachedRectTransform.rect.width + loopListViewItem3.Padding;
				}
				if (num8 != 0f)
				{
					Vector3 localPosition5 = mContainerTrans.localPosition;
					localPosition5.x -= num8;
					mContainerTrans.localPosition = localPosition5;
				}
			}
			else if (mArrangeType == ListItemArrangeType.RightToLeft)
			{
				float num10 = 0f;
				if (mSupportScrollBar)
				{
					num10 = 0f - GetItemPos(mItemList[0].ItemIndex);
				}
				Vector3 localPosition6 = mItemList[0].CachedRectTransform.localPosition;
				float x2 = localPosition6.x;
				float num11 = num10 - x2;
				float num12 = num10;
				for (int l = 0; l < count; l++)
				{
					LoopListViewItem2 loopListViewItem4 = mItemList[l];
					loopListViewItem4.CachedRectTransform.localPosition = new Vector3(num12, loopListViewItem4.StartPosOffset, 0f);
					num12 = num12 - loopListViewItem4.CachedRectTransform.rect.width - loopListViewItem4.Padding;
				}
				if (num11 != 0f)
				{
					Vector3 localPosition7 = mContainerTrans.localPosition;
					localPosition7.x -= num11;
					mContainerTrans.localPosition = localPosition7;
				}
			}
			if (mIsDraging)
			{
				mScrollRect.OnBeginDrag(mPointerEventData);
				mScrollRect.Rebuild(CanvasUpdate.PostLayout);
				mScrollRect.velocity = mAdjustedVec;
				mNeedAdjustVec = true;
			}
		}

		private void UpdateContentSize()
		{
			float contentPanelSize = GetContentPanelSize();
			if (mIsVertList)
			{
				if (mContainerTrans.rect.height != contentPanelSize)
				{
					mContainerTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentPanelSize);
				}
			}
			else if (mContainerTrans.rect.width != contentPanelSize)
			{
				mContainerTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, contentPanelSize);
			}
		}
	}
}
