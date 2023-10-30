using System;
using UnityEngine;

public abstract class CircleScrollRectItemBase : MonoBehaviour
{
	[NonSerialized]
	public CircleScrollRectItemBase prevItem;

	[NonSerialized]
	public CircleScrollRectItemBase nextItem;

	[NonSerialized]
	public int currPosIndex;

	[NonSerialized]
	public int contentListIndex;

	private int itemListIndex;

	private int contentListLength;

	public abstract void UpdateContent(object data);

	public abstract object GetContentListItem(int index);

	public abstract int GetContentListLength();

	public void SetItemConfig(int id, CircleScrollRectItemBase nItem, CircleScrollRectItemBase pItem)
	{
		itemListIndex = id;
		prevItem = pItem;
		nextItem = nItem;
	}

	public void RefreshContentListLength(int itemListLen)
	{
		contentListLength = GetContentListLength();
		if (contentListLength <= 0)
		{
			contentListIndex = 0;
			return;
		}
		int num = itemListLen / 2;
		if (itemListIndex == num)
		{
			contentListIndex = 0;
		}
		else if (itemListIndex < num)
		{
			contentListIndex = contentListLength - (num - itemListIndex);
		}
		else
		{
			contentListIndex = itemListIndex - num;
		}
		while (contentListIndex < 0)
		{
			contentListIndex += contentListLength;
		}
		contentListIndex %= contentListLength;
		UpdateToCurrentContent();
	}

	public void UpdateToCurrentContent()
	{
		UpdateContent(GetContentListItem(contentListIndex));
	}

	public void UpdateToPrevContent()
	{
		contentListIndex = nextItem.contentListIndex - 1;
		contentListIndex = ((contentListIndex >= 0) ? contentListIndex : (contentListLength - 1));
		UpdateContent(GetContentListItem(contentListIndex));
	}

	public void UpdateToNextContent()
	{
		contentListIndex = prevItem.contentListIndex + 1;
		contentListIndex = ((contentListIndex != contentListLength) ? contentListIndex : 0);
		UpdateContent(GetContentListItem(contentListIndex));
	}
}
