using System;
using UnityEngine;

namespace BansheeGz.BGSpline.Curve
{
	[Serializable]
	public class BGCurvePointField : MonoBehaviour
	{
		public enum TypeEnum
		{
			Bool = 0,
			Int = 1,
			Float = 2,
			String = 3,
			Vector3 = 100,
			Bounds = 101,
			Color = 102,
			Quaternion = 103,
			AnimationCurve = 200,
			GameObject = 201,
			Component = 202,
			BGCurve = 300,
			BGCurvePointComponent = 301,
			BGCurvePointGO = 302
		}

		[SerializeField]
		private BGCurve curve;

		[SerializeField]
		private string fieldName;

		[SerializeField]
		private TypeEnum type;

		public string FieldName
		{
			get
			{
				return fieldName;
			}
			set
			{
				if (!string.Equals(FieldName, value))
				{
					CheckName(curve, value, throwException: true);
					curve.FireBeforeChange("field name is changed");
					fieldName = value;
					curve.PrivateUpdateFieldsValuesIndexes();
					curve.FireChange(BGCurveChangedArgs.GetInstance(curve, BGCurveChangedArgs.ChangeTypeEnum.Fields, "field name is changed"), ignoreEventsGrouping: false, this);
				}
			}
		}

		public TypeEnum Type => type;

		public BGCurve Curve => curve;

		public void Init(BGCurve curve, string fieldName, TypeEnum type)
		{
			if (!string.IsNullOrEmpty(this.fieldName))
			{
				throw new UnityException("You can not init twice.");
			}
			CheckName(curve, fieldName, throwException: true);
			this.curve = curve;
			this.fieldName = fieldName;
			this.type = type;
		}

		public static string CheckName(BGCurve curve, string name, bool throwException = false)
		{
			string text = null;
			if (string.IsNullOrEmpty(name))
			{
				text = "Field's name can not be null";
			}
			else if (name.Length > 16)
			{
				text = "Name should be 16 chars max. Current name has " + name.Length + " chars.";
			}
			else
			{
				char c = name[0];
				if ((c < 'A' || c > 'Z') && (c < 'a' || c > 'z'))
				{
					text = "Name should start with a English letter.";
				}
				else
				{
					for (int i = 1; i < name.Length; i++)
					{
						char c2 = name[i];
						if ((c2 < 'A' || c2 > 'Z') && (c2 < 'a' || c2 > 'z') && (c2 < '0' || c2 > '9'))
						{
							text = "Name should contain English letters or numbers only.";
							break;
						}
					}
					if (text == null && curve.HasField(name))
					{
						text = "Field with name '" + name + "' already exists.";
					}
				}
			}
			if (throwException && text != null)
			{
				throw new UnityException(text);
			}
			return text;
		}

		protected bool Equals(BGCurvePointField other)
		{
			return object.Equals(curve, other.curve) && string.Equals(fieldName, other.fieldName);
		}

		public bool Equals(object obj)
		{
			if (object.ReferenceEquals(null, obj))
			{
				return false;
			}
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((BGCurvePointField)obj);
		}

		public int GetHashCode()
		{
			return ((curve != null) ? curve.GetHashCode() : 0) ^ ((fieldName != null) ? fieldName.GetHashCode() : 0);
		}

		public string ToString()
		{
			return fieldName;
		}
	}
}
