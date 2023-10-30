using System;
using UnityEngine;

namespace BansheeGz.BGSpline.Curve
{
	public interface BGCurvePointI
	{
		BGCurve Curve
		{
			get;
		}

		Vector3 PositionLocal
		{
			get;
			set;
		}

		Vector3 PositionLocalTransformed
		{
			get;
			set;
		}

		Vector3 PositionWorld
		{
			get;
			set;
		}

		Vector3 ControlFirstLocal
		{
			get;
			set;
		}

		Vector3 ControlFirstLocalTransformed
		{
			get;
			set;
		}

		Vector3 ControlFirstWorld
		{
			get;
			set;
		}

		Vector3 ControlSecondLocal
		{
			get;
			set;
		}

		Vector3 ControlSecondLocalTransformed
		{
			get;
			set;
		}

		Vector3 ControlSecondWorld
		{
			get;
			set;
		}

		BGCurvePoint.ControlTypeEnum ControlType
		{
			get;
			set;
		}

		Transform PointTransform
		{
			get;
			set;
		}

		T GetField<T>(string name);

		object GetField(string name, Type type);

		float GetFloat(string name);

		bool GetBool(string name);

		int GetInt(string name);

		Vector3 GetVector3(string name);

		Quaternion GetQuaternion(string name);

		Bounds GetBounds(string name);

		Color GetColor(string name);

		void SetField<T>(string name, T value);

		void SetField(string name, object value, Type type);

		void SetFloat(string name, float value);

		void SetBool(string name, bool value);

		void SetInt(string name, int value);

		void SetVector3(string name, Vector3 value);

		void SetQuaternion(string name, Quaternion value);

		void SetBounds(string name, Bounds value);

		void SetColor(string name, Color value);
	}
}
