using FullInspector;
using HW3L;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace M__M.HaiWang.Player.Gun
{
	public class FK3_GunMgr : FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>
	{
		[SerializeField]
		private FK3_GunBehaviour m_prefab;

		[SerializeField]
		public List<FK3_GunBehaviour> m_guns;

		public FK3_GunSettings settings;

		protected override void Awake()
		{
			FK3_fiSimpleSingletonBehaviour<FK3_GunMgr>.s_instance = this;
			ResetAllGuns(force: true);
		}

		private void Start()
		{
			m_guns.ForEach(delegate(FK3_GunBehaviour gun)
			{
				if (!(gun == null))
				{
					gun.allowShoot = true;
				}
			});
		}

		[InspectorButton]
		private void ResetAllGuns()
		{
			ResetAllGuns(force: false);
		}

		private void ResetAllGuns(bool force)
		{
			if (null == settings || null == m_prefab)
			{
				return;
			}
			if (m_guns == null)
			{
				m_guns = new List<FK3_GunBehaviour>();
			}
			Assert.raiseExceptions = true;
			if (force)
			{
				List<Transform> list = new List<Transform>();
				IEnumerator enumerator = base.transform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object current = enumerator.Current;
						Transform transform = (Transform)current;
						if (transform.name.StartsWith("Gun_"))
						{
							list.Add(transform);
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
				list.ForEach(delegate(Transform _child)
				{
					UnityEngine.Object.DestroyImmediate(_child.gameObject);
				});
				m_guns.Clear();
			}
			while (m_guns.Count < 4)
			{
				m_guns.Add(null);
			}
			for (int i = 0; i < 4; i++)
			{
				FK3_GunBehaviour fK3_GunBehaviour = m_guns[i];
				if (null == fK3_GunBehaviour)
				{
					try
					{
						fK3_GunBehaviour = UnityEngine.Object.Instantiate(m_prefab);
						fK3_GunBehaviour.transform.SetParent(base.transform);
						fK3_GunBehaviour.transform.localScale = Vector3.one;
						m_guns[i] = fK3_GunBehaviour;
						fK3_GunBehaviour.gameObject.SetActive(value: true);
						FK3_GunConfig config = settings.GetConfig(i + 1);
						fK3_GunBehaviour.name = "Gun_" + config.id;
						fK3_GunBehaviour.SetConfig(config);
						fK3_GunBehaviour.DoReset(assert: true);
					}
					catch (Exception arg)
					{
						UnityEngine.Debug.LogError("错误: " + arg);
					}
				}
				else
				{
					UnityEngine.Debug.LogError("获取本地的: " + fK3_GunBehaviour.name);
				}
			}
		}

		public FK3_GunBehaviour GetGunById(int id)
		{
			foreach (FK3_GunBehaviour gun in m_guns)
			{
				if (gun.GetId() == id)
				{
					return gun;
				}
			}
			return null;
		}

		public FK3_GunBehaviour GetNativeGun()
		{
			foreach (FK3_GunBehaviour gun in m_guns)
			{
				if (gun.HasUser() && gun.IsNative())
				{
					return gun;
				}
			}
			return null;
		}

		public void ForeachGun(Action<FK3_GunBehaviour> action)
		{
			m_guns.ForEach(action);
		}
	}
}
