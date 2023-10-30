using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[SerializeField]
public class CircleScrollRect : UIBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IEventSystemHandler
{
	public enum Speed
	{
		Quick,
		Ordinary,
		Slow
	}

	[SerializeField]
	public CircleScrollRectItemBase[] listItems;

	public Text content;

	[SerializeField]
	public float oneShiftThreshold = 90f;

	[SerializeField]
	public bool horizontal;

	private Vector3[] itemPostions;

	private Vector2 dragStartPostion;

	private bool dragging;

	private bool needAdjust;

	private float Interval_Y;

	private Vector3 InitPos;

	public AudioClip[] audioClips;

	private AudioSource audioSource;

	[SerializeField]
	private Speed speed = Speed.Slow;

	private Comparison<CircleScrollRectItemBase> ComparisionY = delegate(CircleScrollRectItemBase itemA, CircleScrollRectItemBase itemB)
	{
		Vector3 localPosition5 = itemA.transform.localPosition;
		float y = localPosition5.y;
		Vector3 localPosition6 = itemB.transform.localPosition;
		if (y == localPosition6.y)
		{
			return 0;
		}
		Vector3 localPosition7 = itemA.transform.localPosition;
		float y2 = localPosition7.y;
		Vector3 localPosition8 = itemB.transform.localPosition;
		return (!(y2 > localPosition8.y)) ? 1 : (-1);
	};

	private Comparison<CircleScrollRectItemBase> ComparisionX = delegate(CircleScrollRectItemBase itemA, CircleScrollRectItemBase itemB)
	{
		Vector3 localPosition = itemA.transform.localPosition;
		float x = localPosition.x;
		Vector3 localPosition2 = itemB.transform.localPosition;
		if (x == localPosition2.x)
		{
			return 0;
		}
		Vector3 localPosition3 = itemA.transform.localPosition;
		float x2 = localPosition3.x;
		Vector3 localPosition4 = itemB.transform.localPosition;
		return (!(x2 > localPosition4.x)) ? 1 : (-1);
	};

	private Coroutine waitPaly;

	private int dragNum;

	private int num;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
		Init();
	}

	public void PlaySource(bool isEnd)
	{
		if (audioSource == null)
		{
			audioSource = GetComponent<AudioSource>();
		}
		if (isEnd)
		{
			audioSource.PlayOneShot(audioClips[0]);
		}
		else
		{
			audioSource.PlayOneShot(audioClips[1]);
		}
	}

	public void Init()
	{
		if (listItems == null || listItems.Length == 0)
		{
			return;
		}
		if (itemPostions != null && itemPostions.Length > 0)
		{
			for (int i = 0; i < itemPostions.Length; i++)
			{
				listItems[i].transform.localPosition = itemPostions[i];
			}
		}
		if (horizontal)
		{
			Array.Sort(listItems, ComparisionX);
		}
		else
		{
			Array.Sort(listItems, ComparisionY);
		}
		itemPostions = new Vector3[listItems.Length];
		for (int j = 0; j < listItems.Length; j++)
		{
			listItems[j].SetItemConfig(j, listItems[(j + 1) % listItems.Length], listItems[(j - 1 + listItems.Length) % listItems.Length]);
			ref Vector3 reference = ref itemPostions[j];
			Vector3 localPosition = listItems[j].transform.localPosition;
			float x = localPosition.x;
			Vector3 localPosition2 = listItems[j].transform.localPosition;
			float y = localPosition2.y;
			Vector3 localPosition3 = listItems[j].transform.localPosition;
			reference = new Vector3(x, y, localPosition3.z);
			listItems[j].currPosIndex = j;
		}
		RefreshContentListLength();
		GetCurrPointItem();
		InitValue();
	}

	private void InitValue()
	{
		Interval_Y = 18f;
		int num = listItems.Length / 2;
		InitPos = listItems[num].transform.localPosition;
		for (int i = 0; i < listItems.Length; i++)
		{
			GameObject go = listItems[i].gameObject;
			listItems[i].GetComponent<Button>().onClick.AddListener(delegate
			{
				AutoMoveAllItem(go);
			});
		}
	}

	private void AutoMoveAllItem(GameObject go)
	{
		if (dragging)
		{
			return;
		}
		Vector3 localPosition = go.transform.localPosition;
		if (localPosition.y != 0f)
		{
			Vector3 localPosition2 = go.transform.localPosition;
			if (localPosition2.y > 0f)
			{
				UpdateItemMovePos(2);
			}
			else
			{
				UpdateItemMovePos(1);
			}
		}
	}

	private void UpdateItemMovePos(int status)
	{
		if (waitPaly != null)
		{
			StopCoroutine(waitPaly);
		}
		waitPaly = StartCoroutine(WaitPaly());
		float endValue = Interval_Y;
		if (status == 2)
		{
			endValue = 0f - Interval_Y;
		}
		float move_Y = 0f;
		Tween tween = DOTween.To(() => move_Y, delegate(float x)
		{
			move_Y = x;
		}, endValue, 0.3f);
		tween.onUpdate = delegate
		{
			ShiftListVertical(new Vector2(InitPos.x, move_Y));
		};
		tween.onComplete = delegate
		{
			AdjustLocationY();
			GetCurrPointItem();
		};
	}

	private IEnumerator WaitPaly()
	{
		for (int i = 0; i < 5; i++)
		{
			PlaySource(isEnd: false);
			yield return null;
		}
	}

	public void RefreshContentListLength()
	{
		for (int i = 0; i < listItems.Length; i++)
		{
			listItems[i].RefreshContentListLength(listItems.Length);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public virtual void OnBeginDrag(PointerEventData eventData)
	{
		BeginDrag(eventData);
	}

	public void BeginDrag(PointerEventData eventData)
	{
		dragging = true;
		needAdjust = false;
		dragStartPostion = eventData.position;
	}

	public virtual void OnDrag(PointerEventData eventData)
	{
		dragNum++;
		if (speed == Speed.Ordinary)
		{
			if (dragNum % 2 == 0)
			{
				return;
			}
		}
		else if (speed == Speed.Slow && (dragNum % 2 == 0 || dragNum % 3 == 0 || dragNum % 5 == 0 || dragNum % 7 == 0 || dragNum % 9 == 0))
		{
			return;
		}
		Draging(eventData);
		DragingChange();
	}

	public void Draging(PointerEventData eventData)
	{
		if (num < 1)
		{
			num++;
			PlaySource(isEnd: false);
		}
		else
		{
			num = 0;
		}
		if (horizontal)
		{
			if (ShiftListHorizontal(eventData.position - dragStartPostion))
			{
				dragStartPostion = eventData.position;
			}
		}
		else if (ShiftListVertical(eventData.position - dragStartPostion))
		{
			dragStartPostion = eventData.position;
		}
	}

	public void DragingChange()
	{
	}

	public virtual void OnEndDrag(PointerEventData eventData)
	{
		EndDrag(eventData);
	}

	public void EndDrag(PointerEventData eventData)
	{
		dragging = false;
		if (needAdjust)
		{
			if (horizontal)
			{
				AdjustLocationX();
			}
			else
			{
				AdjustLocationY();
			}
		}
		GetCurrPointItem();
	}

	private void GetCurrPointItem()
	{
		for (int i = 0; i < listItems.Length; i++)
		{
			Vector3 localPosition = listItems[i].transform.localPosition;
			if (localPosition.y >= 0f)
			{
				Vector3 localPosition2 = listItems[i].transform.localPosition;
				if (localPosition2.y <= 0.15f)
				{
					goto IL_0098;
				}
			}
			Vector3 localPosition3 = listItems[i].transform.localPosition;
			if (localPosition3.y <= 0f)
			{
				Vector3 localPosition4 = listItems[i].transform.localPosition;
				if (localPosition4.y >= -0.15f)
				{
					goto IL_0098;
				}
			}
			goto IL_00ec;
			IL_00ec:
			listItems[i].GetComponent<FormationLeftItem>().UpdateShow(isShow: false);
			continue;
			IL_0098:
			if ((bool)listItems[i].GetComponent<FormationLeftItem>())
			{
				listItems[i].GetComponent<FormationLeftItem>().UpdateShow(isShow: true);
				int num = i + 1;
				if (num >= listItems.Length)
				{
					num = 0;
				}
				ZH2_GVars.selectRoomId = num;
				PlaySource(isEnd: true);
				continue;
			}
			goto IL_00ec;
		}
	}

	public bool ShiftListVertical(Vector2 delta)
	{
		if (listItems == null || listItems.Length < 2 || delta.y == 0f)
		{
			return false;
		}
		needAdjust = true;
		bool flag = delta.y < 0f;
		for (int i = 0; i < listItems.Length; i++)
		{
			int currPosIndex = listItems[i].currPosIndex;
			int num = (!flag) ? (currPosIndex - 1) : (currPosIndex + 1);
			if (num < 0)
			{
				Transform transform = listItems[i].transform;
				Vector3 localPosition = listItems[i].transform.localPosition;
				transform.localPosition = new Vector3(65f, localPosition.y, 0f);
			}
			if (num < 0 || num >= listItems.Length)
			{
				continue;
			}
			float num2 = Mathf.Abs(delta.y) / oneShiftThreshold;
			if (num2 <= 1f)
			{
				listItems[i].transform.localPosition = Vector3.Lerp(listItems[i].transform.localPosition, itemPostions[num], num2);
			}
			if (!(num2 >= 1f))
			{
				Vector3 localPosition2 = listItems[i].transform.localPosition;
				if (!(Mathf.Abs(localPosition2.y - itemPostions[num].y) <= 1f))
				{
					continue;
				}
			}
			ShiftOneTime(!flag);
			return true;
		}
		return false;
	}

	public bool ShiftListHorizontal(Vector2 delta)
	{
		if (listItems == null || listItems.Length < 2 || delta.x == 0f)
		{
			return false;
		}
		needAdjust = true;
		bool flag = delta.x < 0f;
		for (int i = 0; i < listItems.Length; i++)
		{
			int currPosIndex = listItems[i].currPosIndex;
			int num = (!flag) ? (currPosIndex - 1) : (currPosIndex + 1);
			if (num < 0 || num >= listItems.Length)
			{
				continue;
			}
			float num2 = Mathf.Abs(delta.x) / oneShiftThreshold;
			if (num2 <= 1f)
			{
				listItems[i].transform.localPosition = Vector3.Lerp(listItems[i].transform.localPosition, itemPostions[num], num2);
			}
			if (!(num2 >= 1f))
			{
				Vector3 localPosition = listItems[i].transform.localPosition;
				if (!(Mathf.Abs(localPosition.x - itemPostions[num].x) <= 1f))
				{
					continue;
				}
			}
			ShiftOneTime(!flag);
			return true;
		}
		return false;
	}

	private void ShiftOneTime(bool reverse)
	{
		needAdjust = false;
		for (int i = 0; i < listItems.Length; i++)
		{
			int currPosIndex = listItems[i].currPosIndex;
			if (reverse)
			{
				listItems[i].transform.localPosition = itemPostions[(currPosIndex - 1 + listItems.Length) % listItems.Length];
				listItems[i].currPosIndex = (currPosIndex - 1 + listItems.Length) % listItems.Length;
				if (currPosIndex - 1 < 0)
				{
					listItems[i].UpdateToNextContent();
				}
			}
			else
			{
				listItems[i].transform.localPosition = itemPostions[(currPosIndex + 1) % itemPostions.Length];
				listItems[i].currPosIndex = (currPosIndex + 1) % itemPostions.Length;
				if (currPosIndex + 1 >= itemPostions.Length)
				{
					listItems[i].UpdateToPrevContent();
				}
			}
		}
	}

	public void AdjustLocationY()
	{
		int num = 0;
		int num2 = itemPostions.Length / 2;
		for (int i = 0; i < listItems.Length; i++)
		{
			if (listItems[i].currPosIndex == num2)
			{
				num = i;
			}
		}
		bool reverse = false;
		if (num2 - 1 >= 0 && num2 + 1 < itemPostions.Length)
		{
			Vector3 localPosition = listItems[num].transform.localPosition;
			float f = localPosition.y - itemPostions[num2 - 1].y;
			Vector3 localPosition2 = listItems[num].transform.localPosition;
			float f2 = localPosition2.y - itemPostions[num2 + 1].y;
			if (Mathf.Abs(f) < Mathf.Abs(f2))
			{
				reverse = true;
			}
		}
		else if (num2 - 1 < 0)
		{
			reverse = true;
		}
		ShiftOneTime(reverse);
	}

	public void AdjustLocationX()
	{
		int num = 0;
		int num2 = itemPostions.Length / 2;
		for (int i = 0; i < listItems.Length; i++)
		{
			if (listItems[i].currPosIndex == num2)
			{
				num = i;
			}
		}
		bool reverse = false;
		if (num2 - 1 >= 0 && num2 + 1 < itemPostions.Length)
		{
			Vector3 localPosition = listItems[num].transform.localPosition;
			float f = localPosition.x - itemPostions[num2 - 1].x;
			Vector3 localPosition2 = listItems[num].transform.localPosition;
			float f2 = localPosition2.x - itemPostions[num2 + 1].x;
			if (Mathf.Abs(f) < Mathf.Abs(f2))
			{
				reverse = true;
			}
		}
		else if (num2 - 1 < 0)
		{
			reverse = true;
		}
		ShiftOneTime(reverse);
	}
}
