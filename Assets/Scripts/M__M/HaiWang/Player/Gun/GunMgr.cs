using FullInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace M__M.HaiWang.Player.Gun
{
	public class GunMgr : fiSimpleSingletonBehaviour<GunMgr>
	{
		[SerializeField]
		private GunBehaviour m_prefab;

		[SerializeField]
		private List<GunBehaviour> m_guns;

		public GunSettings settings;

		protected override void Awake()
		{
			fiSimpleSingletonBehaviour<GunMgr>.s_instance = this;
			ResetAllGuns(force: true);
		}

		private void Start()
		{
			m_guns.ForEach(delegate(GunBehaviour gun)
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
				m_guns = new List<GunBehaviour>();
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
				GunBehaviour gunBehaviour = m_guns[i];
				if (null == gunBehaviour)
				{
					try
					{
						gunBehaviour = UnityEngine.Object.Instantiate(m_prefab);
						gunBehaviour.transform.SetParent(base.transform);
						gunBehaviour.transform.localScale = Vector3.one;
						m_guns[i] = gunBehaviour;
						gunBehaviour.gameObject.SetActive(value: true);
						GunConfig config = settings.GetConfig(i + 1);
						gunBehaviour.name = "Gun_" + config.id;
						gunBehaviour.SetConfig(config);
						gunBehaviour.DoReset(assert: true);
					}
					catch (Exception arg)
					{
						UnityEngine.Debug.LogError("错误: " + arg);
					}
				}
				else
				{
					UnityEngine.Debug.LogError("获取本地的: " + gunBehaviour.name);
				}
			}
		}

		public GunBehaviour GetGunById(int id)
		{
			foreach (GunBehaviour gun in m_guns)
			{
				if (gun.GetId() == id)
				{
					return gun;
				}
			}
			return null;
		}

		public GunBehaviour GetNativeGun()
		{
			foreach (GunBehaviour gun in m_guns)
			{
				if (gun.HasUser() && gun.IsNative())
				{
					return gun;
				}
			}
			return null;
		}

		public void ForeachGun(Action<GunBehaviour> action)
		{
			m_guns.ForEach(action);
		}
	}
}
