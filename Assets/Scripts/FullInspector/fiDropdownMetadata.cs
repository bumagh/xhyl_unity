using FullInspector.Internal;
using System;
using UnityEngine;

namespace FullInspector
{
	[Serializable]
	public class fiDropdownMetadata : IGraphMetadataItemPersistent, ISerializationCallbackReceiver
	{
		private fiAnimBool _isActive = new fiAnimBool(value: true);

		[SerializeField]
		private bool _showDropdown;

		private bool _invertedDefaultState;

		private bool _forceDisable;

		[SerializeField]
		private bool _serializedIsActive;

		public bool IsActive
		{
			get
			{
				return _isActive.value;
			}
			set
			{
				if (value != _isActive.target)
				{
					if (fiSettings.EnableAnimation)
					{
						_isActive.target = value;
					}
					else
					{
						_isActive = new fiAnimBool(value);
					}
				}
			}
		}

		public float AnimPercentage => _isActive.faded;

		public bool IsAnimating => _isActive.isAnimating;

		public bool ShouldDisplayDropdownArrow
		{
			get
			{
				return !_forceDisable && _showDropdown;
			}
			set
			{
				if (!_forceDisable || !value)
				{
					_showDropdown = value;
				}
			}
		}

		public void InvertDefaultState()
		{
			_invertedDefaultState = true;
		}

		public void ForceHideWithoutAnimation()
		{
			_forceDisable = false;
			_showDropdown = true;
			_isActive = new fiAnimBool(value: false);
		}

		public void ForceDisable()
		{
			_forceDisable = true;
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			_serializedIsActive = IsActive;
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			_isActive = new fiAnimBool(_serializedIsActive);
		}

		bool IGraphMetadataItemPersistent.ShouldSerialize()
		{
			if (_invertedDefaultState)
			{
				return IsActive;
			}
			return !IsActive;
		}
	}
}
