using FullInspector;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector
{
	public class FK3_CopyWorkshop
	{
		public struct RotateData
		{
			public float angle;

			public Vector3 offset;

			public RotateData(float angle, Vector3 offset)
			{
				this.angle = angle;
				this.offset = offset;
			}
		}

		public delegate Transform CreateFunCallback(Transform master, Transform root, int index);

		public delegate void SyncActionCallback(Transform master, Transform slave);

		public Color color = Color.red;

		public Transform master;

		public Transform root;

		public int count;

		public bool rotate;

		public Vector3 offset;

		[InspectorShowIf("rotate")]
		public RotateData rotateData = new RotateData(30f, Vector3.zero);

		public bool formatName;

		public string nameFormat = "{0}_slave_{1}";

		public List<Transform> childs = new List<Transform>();

		public CreateFunCallback createFun;

		public SyncActionCallback syncAction;

		[InspectorButton]
		public void Adjust()
		{
			Refresh();
		}

		[InspectorButton]
		public void ForceAdjust()
		{
			Refresh(force: true);
		}

		public void Refresh(bool force = false)
		{
			if (!Check())
			{
				return;
			}
			if (childs == null)
			{
				childs = new List<Transform>();
			}
			int num = childs.Count;
			int num2 = Mathf.Max(count, num);
			childs.Capacity = num2;
			Vector3 b = master.position - master.rotation * rotateData.offset;
			for (int i = 0; i < num2; i++)
			{
				if (i < count)
				{
					if (childs.Count <= i)
					{
						childs.Add(null);
					}
					Transform transform = childs[i];
					if (transform == null || force)
					{
						if (transform != null)
						{
							DestroyOne(transform);
						}
						transform = CreateOne(i);
						childs[i] = transform;
					}
					if (!rotate)
					{
						transform.position = master.position + offset * (i + 1);
						transform.rotation = master.rotation;
					}
					else
					{
						transform.rotation = master.rotation * Quaternion.Euler(0f, 0f, rotateData.angle * (float)(i + 1));
						transform.position = offset * (i + 1) + b + Quaternion.Euler(0f, 0f, rotateData.angle * (float)(i + 1)) * rotateData.offset;
					}
					if (formatName)
					{
						transform.gameObject.name = string.Format(nameFormat, master.name, i);
					}
				}
				else
				{
					Transform transform2 = childs[i];
					if (transform2 != null)
					{
						DestroyOne(transform2);
					}
				}
			}
			if (count < num)
			{
				childs.RemoveRange(count, num - count);
			}
		}

		private Transform CreateOne(int index)
		{
			Transform transform = (createFun == null) ? Object.Instantiate(master, root) : createFun(master, root, index);
			transform.name = string.Format(nameFormat, master.name, index);
			return transform;
		}

		private void SyncOne(Transform child)
		{
			if (!(child == null) && !(master == null) && syncAction != null)
			{
				syncAction(master, child);
			}
		}

		private void DestroyOne(Transform child)
		{
			UnityEngine.Object.DestroyImmediate(child.gameObject);
		}

		public void Clear()
		{
			if (childs != null)
			{
				foreach (Transform child in childs)
				{
					if (!(child == null))
					{
						DestroyOne(child);
					}
				}
				childs.Clear();
			}
		}

		public bool Check()
		{
			return !(master == null) && !(root == null);
		}
	}
}
