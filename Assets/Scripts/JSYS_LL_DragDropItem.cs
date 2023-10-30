using UnityEngine;

[AddComponentMenu("NGUI/Examples/Drag and Drop Item")]
public class JSYS_LL_DragDropItem : MonoBehaviour
{
	public GameObject prefab;

	private Transform mTrans;

	private bool mIsDragging;

	private bool mSticky;

	private Transform mParent;

	private void UpdateTable()
	{
		UITable uITable = NGUITools.FindInParents<UITable>(base.gameObject);
		if (uITable != null)
		{
			uITable.repositionNow = true;
		}
	}

	private void Drop()
	{
		Collider collider = UICamera.lastHit.collider;
		JSYS_LL_DragDropContainer jSYS_LL_DragDropContainer = (collider != null) ? collider.gameObject.GetComponent<JSYS_LL_DragDropContainer>() : null;
		if (jSYS_LL_DragDropContainer != null)
		{
			mTrans.parent = jSYS_LL_DragDropContainer.transform;
			Vector3 localPosition = mTrans.localPosition;
			localPosition.z = 0f;
			mTrans.localPosition = localPosition;
		}
		else
		{
			mTrans.parent = mParent;
		}
		UpdateTable();
		NGUITools.MarkParentAsChanged(base.gameObject);
	}

	private void Awake()
	{
		mTrans = base.transform;
	}

	private void OnDrag(Vector3 delta)
	{
		if (base.enabled && UICamera.currentTouchID > -2)
		{
			if (!mIsDragging)
			{
				mIsDragging = true;
				mParent = mTrans.parent;
				mTrans.parent = JSYS_LL_DragDropRoot.root;
				Vector3 localPosition = mTrans.localPosition;
				localPosition.z = 0f;
				mTrans.localPosition = localPosition;
				NGUITools.MarkParentAsChanged(base.gameObject);
			}
			else
			{
				mTrans.localPosition += delta;
			}
		}
	}

	private void OnPress(bool isPressed)
	{
		if (!base.enabled)
		{
			return;
		}
		if (isPressed)
		{
			if (!UICamera.current.stickyPress)
			{
				mSticky = true;
			}
		}
		else if (mSticky)
		{
			mSticky = false;
		}
		mIsDragging = false;
		Collider component = GetComponent<Collider>();
		if (component != null)
		{
			component.enabled = !isPressed;
		}
		if (!isPressed)
		{
			Drop();
		}
	}
}
