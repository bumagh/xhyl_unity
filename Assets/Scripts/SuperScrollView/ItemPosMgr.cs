using System.Collections.Generic;

namespace SuperScrollView
{
	public class ItemPosMgr
	{
		public const int mItemMaxCountPerGroup = 100;

		private List<ItemSizeGroup> mItemSizeGroupList = new List<ItemSizeGroup>();

		private int mDirtyBeginIndex = int.MaxValue;

		public float mTotalSize;

		public float mItemDefaultSize = 20f;

		public ItemPosMgr(float itemDefaultSize)
		{
			mItemDefaultSize = itemDefaultSize;
		}

		public void SetItemMaxCount(int maxCount)
		{
			mDirtyBeginIndex = 0;
			mTotalSize = 0f;
			int num = maxCount % 100;
			int itemCount = num;
			int num2 = maxCount / 100;
			if (num > 0)
			{
				num2++;
			}
			else
			{
				itemCount = 100;
			}
			int count = mItemSizeGroupList.Count;
			if (count > num2)
			{
				int count2 = count - num2;
				mItemSizeGroupList.RemoveRange(num2, count2);
			}
			else if (count < num2)
			{
				int num3 = num2 - count;
				for (int i = 0; i < num3; i++)
				{
					ItemSizeGroup item = new ItemSizeGroup(count + i, mItemDefaultSize);
					mItemSizeGroupList.Add(item);
				}
			}
			count = mItemSizeGroupList.Count;
			if (count != 0)
			{
				for (int j = 0; j < count - 1; j++)
				{
					mItemSizeGroupList[j].SetItemCount(100);
				}
				mItemSizeGroupList[count - 1].SetItemCount(itemCount);
				for (int k = 0; k < count; k++)
				{
					mTotalSize += mItemSizeGroupList[k].mGroupSize;
				}
			}
		}

		public void SetItemSize(int itemIndex, float size)
		{
			int num = itemIndex / 100;
			int index = itemIndex % 100;
			ItemSizeGroup itemSizeGroup = mItemSizeGroupList[num];
			float num2 = itemSizeGroup.SetItemSize(index, size);
			if (num2 != 0f && num < mDirtyBeginIndex)
			{
				mDirtyBeginIndex = num;
			}
			mTotalSize += num2;
		}

		public float GetItemPos(int itemIndex)
		{
			Update(updateAll: true);
			int index = itemIndex / 100;
			int index2 = itemIndex % 100;
			return mItemSizeGroupList[index].GetItemStartPos(index2);
		}

		public void GetItemIndexAndPosAtGivenPos(float pos, ref int index, ref float itemPos)
		{
			Update(updateAll: true);
			index = 0;
			itemPos = 0f;
			int count = mItemSizeGroupList.Count;
			if (count == 0)
			{
				return;
			}
			ItemSizeGroup itemSizeGroup = null;
			int num = 0;
			int num2 = count - 1;
			while (num <= num2)
			{
				int num3 = (num + num2) / 2;
				ItemSizeGroup itemSizeGroup2 = mItemSizeGroupList[num3];
				if (itemSizeGroup2.mGroupStartPos <= pos && itemSizeGroup2.mGroupEndPos >= pos)
				{
					itemSizeGroup = itemSizeGroup2;
					break;
				}
				if (pos > itemSizeGroup2.mGroupEndPos)
				{
					num = num3 + 1;
				}
				else
				{
					num2 = num3 - 1;
				}
			}
			int num4 = -1;
			if (itemSizeGroup != null)
			{
				num4 = itemSizeGroup.GetItemIndexByPos(pos - itemSizeGroup.mGroupStartPos);
				if (num4 >= 0)
				{
					index = num4 + itemSizeGroup.mGroupIndex * 100;
					itemPos = itemSizeGroup.GetItemStartPos(num4);
				}
			}
		}

		public void Update(bool updateAll)
		{
			int count = mItemSizeGroupList.Count;
			if (count == 0 || mDirtyBeginIndex >= count)
			{
				return;
			}
			int num = 0;
			for (int i = mDirtyBeginIndex; i < count; i++)
			{
				num++;
				ItemSizeGroup itemSizeGroup = mItemSizeGroupList[i];
				mDirtyBeginIndex++;
				itemSizeGroup.UpdateAllItemStartPos();
				if (i == 0)
				{
					itemSizeGroup.mGroupStartPos = 0f;
					itemSizeGroup.mGroupEndPos = itemSizeGroup.mGroupSize;
				}
				else
				{
					itemSizeGroup.mGroupStartPos = mItemSizeGroupList[i - 1].mGroupEndPos;
					itemSizeGroup.mGroupEndPos = itemSizeGroup.mGroupStartPos + itemSizeGroup.mGroupSize;
				}
				if (!updateAll && num > 1)
				{
					break;
				}
			}
		}
	}
}
