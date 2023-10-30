using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGSpline.Curve
{
	[RequireComponent(typeof(BGCurve))]
	public abstract class BGCc : MonoBehaviour
	{
		[AttributeUsage(AttributeTargets.Class)]
		public class CcDescriptor : Attribute
		{
			public string Name
			{
				get;
				set;
			}

			public string Description
			{
				get;
				set;
			}

			public string Image
			{
				get;
				set;
			}

			public string Icon
			{
				get;
				set;
			}
		}

		[AttributeUsage(AttributeTargets.Class)]
		public class CcExcludeFromMenu : Attribute
		{
		}

		public class CcException : Exception
		{
			public CcException(string message)
				: base(message)
			{
			}
		}

		public GameObject curveContainer;

		protected BGCurve curve;

		[SerializeField]
		private BGCc parent;

		[SerializeField]
		private string ccName;

		private int transactionLevel;

		private CcDescriptor descriptor;

		public virtual string Info => null;

		public virtual string Warning => null;

		public virtual string Error => null;

		public virtual bool SupportHandles => false;

		public virtual bool SupportHandlesSettings => false;

		public virtual bool HideHandlesInInspector => false;

		public BGCurve Curve
		{
			get
			{
				if (curve == null)
				{
					curve = GetComponent<BGCurve>();
				}
				if (curve == null)
				{
					curve = curveContainer.GetComponent<BGCurve>();
				}
				return curve;
			}
		}

		public string CcName
		{
			get
			{
				return (!string.IsNullOrEmpty(ccName)) ? ccName : (string.Empty + GetInstanceID());
			}
			set
			{
				ParamChanged(ref ccName, value);
			}
		}

		public CcDescriptor Descriptor
		{
			get
			{
				if (descriptor == null)
				{
					descriptor = GetDescriptor(GetType());
				}
				return descriptor;
			}
		}

		public virtual string HelpURL => GetHelpUrl(GetType())?.URL;

		public event EventHandler ChangedParams;

		public void SetParent(BGCc parent)
		{
			this.parent = parent;
		}

		public T GetParent<T>() where T : BGCc
		{
			return (T)GetParent(typeof(T));
		}

		public BGCc GetParent(Type type)
		{
			if (parent != null)
			{
				return parent;
			}
			parent = (BGCc)GetComponent(type);
			return parent;
		}

		public virtual void Start()
		{
		}

		public virtual void OnDestroy()
		{
		}

		protected bool ParamChanged<T>(ref T oldValue, T newValue)
		{
			bool flag = oldValue == null;
			bool flag2 = newValue == null;
			if (flag && flag2)
			{
				return false;
			}
			if (flag == flag2 && oldValue.Equals(newValue))
			{
				return false;
			}
			oldValue = newValue;
			FireChangedParams();
			return true;
		}

		public bool HasError()
		{
			return !string.IsNullOrEmpty(Error);
		}

		public bool HasWarning()
		{
			return !string.IsNullOrEmpty(Warning);
		}

		protected string ChoseMessage(string baseError, Func<string> childError)
		{
			return string.IsNullOrEmpty(baseError) ? childError() : baseError;
		}

		public void FireChangedParams()
		{
			if (this.ChangedParams != null && transactionLevel == 0)
			{
				this.ChangedParams(this, null);
			}
		}

		public virtual void AddedInEditor()
		{
		}

		public Type GetParentClass()
		{
			return GetParentClass(GetType());
		}

		public static Type GetParentClass(Type ccType)
		{
			object[] customAttributes = BGReflectionAdapter.GetCustomAttributes(ccType, typeof(RequireComponent), true);
			if (customAttributes.Length == 0)
			{
				return null;
			}
			List<Type> list = new List<Type>();
			foreach (object obj in customAttributes)
			{
				RequireComponent requireComponent = (RequireComponent)obj;
				BGCc.CheckRequired(requireComponent.m_Type0, list);
				BGCc.CheckRequired(requireComponent.m_Type1, list);
				BGCc.CheckRequired(requireComponent.m_Type2, list);
			}
			if (list.Count == 0)
			{
				return null;
			}
			if (list.Count > 1)
			{
				throw new BGCc.CcException(ccType + " has more than one parent (extended from BGCc class), calculated by RequireComponent attribute");
			}
			return list[0];
		}

		private static void CheckRequired(Type type, List<Type> result)
		{
			if (type == null || BGReflectionAdapter.IsAbstract(type) || !BGReflectionAdapter.IsClass(type) || !BGReflectionAdapter.IsSubclassOf(type, typeof(BGCc)))
			{
				return;
			}
			result.Add(type);
		}

		public static bool IsSingle(Type ccType)
		{
			return BGReflectionAdapter.GetCustomAttributes(ccType, typeof(DisallowMultipleComponent), inherit: true).Length > 0;
		}

		public void Transaction(Action action)
		{
			transactionLevel++;
			try
			{
				action();
			}
			finally
			{
				transactionLevel--;
				if (transactionLevel == 0 && this.ChangedParams != null)
				{
					this.ChangedParams(this, null);
				}
			}
		}

		public static CcDescriptor GetDescriptor(Type type)
		{
			object[] customAttributes = BGReflectionAdapter.GetCustomAttributes(type, typeof(CcDescriptor), inherit: false);
			if (customAttributes.Length > 0)
			{
				return (CcDescriptor)customAttributes[0];
			}
			return null;
		}

		private static HelpURLAttribute GetHelpUrl(Type type)
		{
			object[] customAttributes = BGReflectionAdapter.GetCustomAttributes(type, typeof(HelpURLAttribute), inherit: false);
			if (customAttributes.Length > 0)
			{
				return (HelpURLAttribute)customAttributes[0];
			}
			return null;
		}

		[ContextMenu("Remove Component", true)]
		[ContextMenu("Paste Component Values", true)]
		[ContextMenu("Copy Component", true)]
		[ContextMenu("Reset", true)]
		private bool ContextMenuItems()
		{
			return false;
		}

		[ContextMenu("Remove Component")]
		[ContextMenu("Paste Component Values")]
		[ContextMenu("Copy Component")]
		[ContextMenu("Reset")]
		private void ContextMenuValidate()
		{
			ShowError("BGCurve components do not support this function");
		}

		[ContextMenu("BGCurve: Why menu items are disabled?")]
		private void WhyDisabled()
		{
			ShowError("BGCurve components do not support Resetting, Copy/Pasting and Removing components from standard Unity menu. Use colored tree view to remove components");
		}

		private static void ShowError(string message)
		{
			UnityEngine.Debug.Log(message);
		}
	}
}
