using UnityEngine;

public static class DrawArrow
{
	public static void ForGizmo(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20f)
	{
		Gizmos.DrawRay(pos, direction);
		DrawArrowEnd(gizmos: true, pos, direction, Gizmos.color, arrowHeadLength, arrowHeadAngle);
	}

	public static void ForGizmo(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20f)
	{
		Gizmos.DrawRay(pos, direction);
		DrawArrowEnd(gizmos: true, pos, direction, color, arrowHeadLength, arrowHeadAngle);
	}

	public static void ForDebug(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20f)
	{
		UnityEngine.Debug.DrawRay(pos, direction);
		DrawArrowEnd(gizmos: false, pos, direction, Gizmos.color, arrowHeadLength, arrowHeadAngle);
	}

	public static void ForDebug(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20f)
	{
		UnityEngine.Debug.DrawRay(pos, direction, color);
		DrawArrowEnd(gizmos: false, pos, direction, color, arrowHeadLength, arrowHeadAngle);
	}

	private static void DrawArrowEnd(bool gizmos, Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20f)
	{
		Vector3 a = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0f, 0f) * Vector3.back;
		Vector3 a2 = Quaternion.LookRotation(direction) * Quaternion.Euler(0f - arrowHeadAngle, 0f, 0f) * Vector3.back;
		Vector3 a3 = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, arrowHeadAngle, 0f) * Vector3.back;
		Vector3 a4 = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, 0f - arrowHeadAngle, 0f) * Vector3.back;
		if (gizmos)
		{
			Gizmos.color = color;
			Gizmos.DrawRay(pos + direction, a * arrowHeadLength);
			Gizmos.DrawRay(pos + direction, a2 * arrowHeadLength);
			Gizmos.DrawRay(pos + direction, a3 * arrowHeadLength);
			Gizmos.DrawRay(pos + direction, a4 * arrowHeadLength);
		}
		else
		{
			UnityEngine.Debug.DrawRay(pos + direction, a * arrowHeadLength, color);
			UnityEngine.Debug.DrawRay(pos + direction, a2 * arrowHeadLength, color);
			UnityEngine.Debug.DrawRay(pos + direction, a3 * arrowHeadLength, color);
			UnityEngine.Debug.DrawRay(pos + direction, a4 * arrowHeadLength, color);
		}
	}
}
