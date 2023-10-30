using FullSerializer.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace FullInspector
{
	public abstract class tkControl<T, TContext> : tkIControl
	{
		private int _uniqueId;

		private List<tkStyle<T, TContext>> _styles;

		public Type ContextType => typeof(TContext);

		public tkStyle<T, TContext> Style
		{
			set
			{
				Styles = new List<tkStyle<T, TContext>>
				{
					value
				};
			}
		}

		public List<tkStyle<T, TContext>> Styles
		{
			get
			{
				if (_styles == null)
				{
					_styles = new List<tkStyle<T, TContext>>();
				}
				return _styles;
			}
			set
			{
				_styles = value;
			}
		}

		protected virtual IEnumerable<tkIControl> NonMemberChildControls
		{
			get
			{
				yield break;
			}
		}

		protected fiGraphMetadata GetInstanceMetadata(fiGraphMetadata metadata)
		{
			fiGraphMetadataChild fiGraphMetadataChild = metadata.Enter(_uniqueId, metadata.Context);
			return fiGraphMetadataChild.Metadata;
		}

		protected abstract T DoEdit(Rect rect, T obj, TContext context, fiGraphMetadata metadata);

		protected abstract float DoGetHeight(T obj, TContext context, fiGraphMetadata metadata);

		public virtual bool ShouldShow(T obj, TContext context, fiGraphMetadata metadata)
		{
			return true;
		}

		public T Edit(Rect rect, T obj, TContext context, fiGraphMetadata metadata)
		{
			if (Styles == null)
			{
				return DoEdit(rect, obj, context, metadata);
			}
			for (int i = 0; i < Styles.Count; i++)
			{
				Styles[i].Activate(obj, context);
			}
			T result = DoEdit(rect, obj, context, metadata);
			for (int j = 0; j < Styles.Count; j++)
			{
				Styles[j].Deactivate(obj, context);
			}
			return result;
		}

		public object Edit(Rect rect, object obj, object context, fiGraphMetadata metadata)
		{
			return Edit(rect, (T)obj, (TContext)context, metadata);
		}

		public float GetHeight(T obj, TContext context, fiGraphMetadata metadata)
		{
			if (Styles == null)
			{
				return DoGetHeight(obj, context, metadata);
			}
			for (int i = 0; i < Styles.Count; i++)
			{
				Styles[i].Activate(obj, context);
			}
			float result = DoGetHeight(obj, context, metadata);
			for (int j = 0; j < Styles.Count; j++)
			{
				Styles[j].Deactivate(obj, context);
			}
			return result;
		}

		public float GetHeight(object obj, object context, fiGraphMetadata metadata)
		{
			return GetHeight((T)obj, (TContext)context, metadata);
		}

		void tkIControl.InitializeId(ref int nextId)
		{
			_uniqueId = nextId++;
			foreach (tkIControl nonMemberChildControl in NonMemberChildControls)
			{
				nonMemberChildControl.InitializeId(ref nextId);
			}
			Type type = GetType();
			while (type != null)
			{
				List<InspectedMember> members = InspectedType.Get(type).GetMembers(InspectedMemberFilters.TkControlMembers);
				foreach (InspectedMember item in members)
				{
					Type storageType = item.Property.StorageType;
					IEnumerable<tkIControl> value2;
					if (typeof(tkIControl).IsAssignableFrom(storageType))
					{
						if (TryReadValue(item, this, out tkIControl value))
						{
							value?.InitializeId(ref nextId);
						}
					}
					else if (typeof(IEnumerable<tkIControl>).IsAssignableFrom(storageType) && TryReadValue(item, this, out value2) && value2 != null)
					{
						foreach (tkIControl item2 in value2)
						{
							item2.InitializeId(ref nextId);
						}
					}
				}
				type = type.Resolve().BaseType;
			}
		}

		private static bool TryReadValue<TValue>(InspectedMember inspectedMember, object context, out TValue value)
		{
			PropertyInfo propertyInfo = inspectedMember.MemberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				value = (TValue)propertyInfo.GetValue(context, null);
				return true;
			}
			FieldInfo fieldInfo = inspectedMember.MemberInfo as FieldInfo;
			if (fieldInfo != null)
			{
				value = (TValue)fieldInfo.GetValue(context);
				return true;
			}
			value = default(TValue);
			return false;
		}
	}
}
