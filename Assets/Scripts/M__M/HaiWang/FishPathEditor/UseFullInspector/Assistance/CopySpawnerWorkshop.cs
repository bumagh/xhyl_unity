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
	public class CopySpawnerWorkshop
	{
		public FishSpawnerDesign desgin;

		public bool printLog;

		public Transform root;

		public List<SpawnerCurveItem> items = new List<SpawnerCurveItem>();

		public void Awake()
		{
			if (items == null)
			{
				items = new List<SpawnerCurveItem>();
			}
			SetListAction();
		}

		[InspectorButton]
		[InspectorName("设置List元素的按钮action")]
		private void SetListAction()
		{
			int count = 0;
			items.ForEach(delegate(SpawnerCurveItem _item)
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
				UnityEngine.Debug.Log($"items set action> count: {HW2_LogHelper.Cyan(count.ToString())}");
			}
		}

		private void AddSpawnerAction(SpawnerCurveItem item)
		{
			if (item.spawner != null)
			{
				RemoveSpawnerAction(item);
			}
			item.spawner = item.curve.gameObject.AddComponent<FishSpawnerBehaviour>();
			item.spawner.GainCurve();
		}

		private void RemoveSpawnerAction(SpawnerCurveItem item)
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

		[InspectorButton]
		[InspectorName("生成List列表")]
		private void RecoverList()
		{
			if (!(root == null))
			{
				if (items == null)
				{
					items = new List<SpawnerCurveItem>();
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
								SpawnerCurveItem spawnerCurveItem = new SpawnerCurveItem();
								spawnerCurveItem.curve = component;
								SpawnerCurveItem spawnerCurveItem2 = spawnerCurveItem;
								FishSpawnerBehaviour fishSpawnerBehaviour = spawnerCurveItem2.spawner = transform.GetComponent<FishSpawnerBehaviour>();
								items.Add(spawnerCurveItem2);
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
				items.ForEach(delegate(SpawnerCurveItem _item)
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
		[InspectorName("清除List列表")]
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
				items.ForEach(delegate(SpawnerCurveItem _item)
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
					SpawnerCurveItem item = items[i];
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
			FishSpawnerBehaviour[] componentsInChildren = root.GetComponentsInChildren<FishSpawnerBehaviour>();
			FishSpawnerBehaviour[] array = componentsInChildren;
			foreach (FishSpawnerBehaviour obj in array)
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

		[InspectorName("应用desgin至List元素")]
		[InspectorButton]
		private void ApplyDesginToList()
		{
			if (items != null && desgin != null && desgin.IsValid)
			{
				int i = 0;
				for (int count = items.Count; i < count; i++)
				{
					SpawnerCurveItem item = items[i];
					ApplyDesignToItem(item, i);
				}
			}
		}

		private bool ApplyDesignToItem(SpawnerCurveItem item, int index)
		{
			if (item.spawner == null)
			{
				AddSpawnerAction(item);
			}
			item.spawner.SetSpawnerData(desgin.spawnerData.Clone());
			item.spawner.SetGenerator(desgin.generator.Clone() as GeneratorBase<FishType>);
			item.spawner.id = index.ToString();
			item.spawner.moveSpeed = desgin.moveSpeed;
			item.spawner.moveStartDelay = desgin.moveStartDelay;
			return true;
		}
	}
}
