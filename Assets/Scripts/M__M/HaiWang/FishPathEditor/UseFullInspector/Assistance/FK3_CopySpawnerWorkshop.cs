using BansheeGz.BGSpline.Curve;
using FullInspector;
using M__M.HaiWang.Fish;
using M__M.HaiWang.FishPathEditor.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace M__M.HaiWang.FishPathEditor.UseFullInspector.Assistance
{
	public class FK3_CopySpawnerWorkshop
	{
		public FK3_FishSpawnerDesign desgin;

		public bool printLog;

		public Transform root;

		public List<FK3_SpawnerCurveItem> items = new List<FK3_SpawnerCurveItem>();

		public void Awake()
		{
			if (items == null)
			{
				items = new List<FK3_SpawnerCurveItem>();
			}
			SetListAction();
		}

		[InspectorName("设置List元素的按钮action")]
		[InspectorButton]
		private void SetListAction()
		{
			int count = 0;
			items.ForEach(delegate(FK3_SpawnerCurveItem _item)
			{
				if (_item != null)
				{
					_item.addSpawnerAction = AddSpawnerAction;
					_item.removeSpawnerAction = RemoveSpawnerAction;
					count++;
				}
			});
			if (printLog)
			{
				UnityEngine.Debug.Log($"items set action> count: {FK3_LogHelper.Cyan(count.ToString())}");
			}
		}

		private void AddSpawnerAction(FK3_SpawnerCurveItem item)
		{
			if (item.spawner != null)
			{
				RemoveSpawnerAction(item);
			}
			item.spawner = item.curve.gameObject.AddComponent<FK3_FishSpawnerBehaviour>();
			item.spawner.GainCurve();
		}

		private void RemoveSpawnerAction(FK3_SpawnerCurveItem item)
		{
			if (Application.isEditor)
			{
				UnityEngine.Object.DestroyImmediate(item.spawner);
			}
			else
			{
				UnityEngine.Object.Destroy(item.spawner);
			}
			item.spawner = null;
		}

		[InspectorName("生成List列表")]
		[InspectorButton]
		private void RecoverList()
		{
			if (!(root == null))
			{
				if (items == null)
				{
					items = new List<FK3_SpawnerCurveItem>();
				}
				ClearList();
				IEnumerator enumerator = root.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object current = enumerator.Current;
						Transform transform = (Transform)current;
						if (!(transform == root))
						{
							BGCurve component = transform.GetComponent<BGCurve>();
							if (component != null)
							{
								FK3_SpawnerCurveItem fK3_SpawnerCurveItem = new FK3_SpawnerCurveItem();
								fK3_SpawnerCurveItem.curve = component;
								FK3_SpawnerCurveItem fK3_SpawnerCurveItem2 = fK3_SpawnerCurveItem;
								FK3_FishSpawnerBehaviour fK3_FishSpawnerBehaviour = fK3_SpawnerCurveItem2.spawner = transform.GetComponent<FK3_FishSpawnerBehaviour>();
								items.Add(fK3_SpawnerCurveItem2);
							}
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				SetListAction();
			}
		}

		private void ClearListItem()
		{
			if (items != null)
			{
				items.ForEach(delegate(FK3_SpawnerCurveItem _item)
				{
					if (_item != null && !(_item.spawner == null))
					{
						RemoveSpawnerAction(_item);
					}
				});
				items.Clear();
			}
		}

		[InspectorName("清除List列表")]
		[InspectorButton]
		private void ClearList()
		{
			if (items != null)
			{
				items.Clear();
			}
		}

		[InspectorName("清除List列表和元素spawner")]
		[InspectorButton]
		private void ClearListAndSpawner()
		{
			if (items != null)
			{
				items.ForEach(delegate(FK3_SpawnerCurveItem _item)
				{
					if (_item != null && !(_item.spawner == null))
					{
						RemoveSpawnerAction(_item);
					}
				});
				items.Clear();
			}
		}

		[InspectorButton]
		[InspectorName("填充List元素spawner")]
		private void FillListSpawner()
		{
			if (!(root == null) && items != null)
			{
				int i = 0;
				for (int count = items.Count; i < count; i++)
				{
					FK3_SpawnerCurveItem item = items[i];
					AddSpawnerAction(item);
				}
			}
		}

		[InspectorName("移除List元素spawner")]
		[InspectorButton]
		private void RemoveListSpawner()
		{
			if (root == null)
			{
				return;
			}
			FK3_FishSpawnerBehaviour[] componentsInChildren = root.GetComponentsInChildren<FK3_FishSpawnerBehaviour>();
			FK3_FishSpawnerBehaviour[] array = componentsInChildren;
			foreach (FK3_FishSpawnerBehaviour obj in array)
			{
				if (!Application.isPlaying)
				{
					UnityEngine.Object.DestroyImmediate(obj);
				}
				else
				{
					UnityEngine.Object.Destroy(obj);
				}
			}
		}

		[InspectorButton]
		[InspectorName("应用desgin至List元素")]
		private void ApplyDesginToList()
		{
			if (items != null && desgin != null && desgin.IsValid)
			{
				int i = 0;
				for (int count = items.Count; i < count; i++)
				{
					FK3_SpawnerCurveItem item = items[i];
					ApplyDesignToItem(item, i);
				}
			}
		}

		private bool ApplyDesignToItem(FK3_SpawnerCurveItem item, int index)
		{
			if (item.spawner == null)
			{
				AddSpawnerAction(item);
			}
			item.spawner.SetSpawnerData(desgin.spawnerData.Clone());
			item.spawner.SetGenerator(desgin.generator.Clone() as FK3_GeneratorBase<FK3_FishType>);
			item.spawner.id = index.ToString();
			item.spawner.moveSpeed = desgin.moveSpeed;
			item.spawner.moveStartDelay = desgin.moveStartDelay;
			return true;
		}
	}
}
