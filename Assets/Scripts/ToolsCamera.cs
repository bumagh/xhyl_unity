using DG.Tweening;
using System;
using UnityEngine;

public class ToolsCamera : MonoBehaviour
{
	public enum Edge
	{
		None,
		Up,
		Down,
		Left,
		Right
	}

	[SerializeField]
	public float aspect;

	protected Vector3[] corners;

	protected Camera mCamera;

	public Camera camera => mCamera;

	protected virtual void Start()
	{
		mCamera = GetComponent<Camera>();
		mCamera.aspect = aspect;
	}

	public bool IsVisible(Transform tran, float outline)
	{
		float f = mCamera.fieldOfView / 2f * ((float)Math.PI / 180f);
		Vector3 position = mCamera.transform.position;
		float y = position.y;
		Vector3 position2 = tran.position;
		float num = (y - position2.y) * Mathf.Tan(f);
		float num2 = num * mCamera.aspect;
		float num3 = outline / (num2 * 2f);
		float num4 = outline / (num * 2f);
		bool flag = false;
		Vector3 vector = mCamera.WorldToViewportPoint(tran.position);
		if (vector.x >= 0f - num3 && vector.x <= 1f + num3 && vector.y >= 0f - num4 && vector.y <= 1f + num4)
		{
			flag = true;
		}
		float y2 = vector.y;
		Vector3 position3 = base.transform.position;
		int num5;
		if (y2 > position3.y - mCamera.farClipPlane)
		{
			float y3 = vector.y;
			Vector3 position4 = base.transform.position;
			num5 = ((y3 < position4.y - mCamera.nearClipPlane) ? 1 : 0);
		}
		else
		{
			num5 = 0;
		}
		bool flag2 = (byte)num5 != 0;
		return flag && flag2;
	}

	public Edge IsEdge(Vector3 pos)
	{
		Vector2 vector = mCamera.WorldToViewportPoint(pos);
		if (vector.x <= 0f)
		{
			return Edge.Left;
		}
		if (vector.x >= 1f)
		{
			return Edge.Right;
		}
		if (vector.y <= 0f)
		{
			return Edge.Down;
		}
		if (vector.y >= 1f)
		{
			return Edge.Up;
		}
		return Edge.None;
	}

	public bool InViewport(Vector3 pos)
	{
		pos = mCamera.WorldToViewportPoint(pos);
		if (pos.x >= 0f && pos.x <= 1f && pos.y >= 0f && pos.y <= 1f)
		{
			return true;
		}
		return false;
	}

	public Vector3 MoveInViewport(Vector3 current, Vector3 direction, float speed)
	{
		return MoveInViewport(current, direction, speed, new Vector4(1f, 0f, 0f, 1f));
	}

	public Vector3 MoveInViewport(Vector3 current, Vector3 direction, float speed, Vector4 edge)
	{
		Vector3 vector = current + direction * Time.deltaTime * speed;
		Vector3 vector2 = mCamera.WorldToViewportPoint(vector);
		if (direction == Vector3.up && vector2.y <= edge[0])
		{
			return vector;
		}
		if (direction == Vector3.down && vector2.y >= edge[1])
		{
			return vector;
		}
		if (direction == Vector3.left && vector2.x >= edge[2])
		{
			return vector;
		}
		if (direction == Vector3.right && vector2.x <= edge[3])
		{
			return vector;
		}
		return current;
	}

	public Vector2 MouseViewportPoint()
	{
		return mCamera.ScreenToViewportPoint(UnityEngine.Input.mousePosition);
	}

	public Vector3 WorldToView(Vector3 position)
	{
		return mCamera.WorldToViewportPoint(position);
	}

	public Vector3 WorldToView(Vector3 position, float z)
	{
		position = mCamera.WorldToViewportPoint(position);
		position.z = z;
		return position;
	}

	public Vector3 ViewToWorld(Vector3 position)
	{
		return ViewToWorld(position, position.z);
	}

	public Vector3 ViewToWorld(Vector3 position, float farFromCamera)
	{
		position.z = farFromCamera;
		return mCamera.ViewportToWorldPoint(position);
	}

	public Vector3 WorldToViewToWorld(Vector3 position, float farFromCamera)
	{
		position = mCamera.WorldToViewportPoint(position);
		position.z = farFromCamera;
		return mCamera.ViewportToWorldPoint(position);
	}

	public Vector3 WorldToCameraWorld(Vector3 position, ToolsCamera cam, float farFromCamera)
	{
		position = WorldToView(position);
		return cam.ViewToWorld(position, farFromCamera);
	}

	public void ShakePosition(float duration)
	{
		base.transform.DOShakePosition(duration, 2f, 30).OnComplete(delegate
		{
			base.transform.localPosition = Vector3.zero;
		});
	}

	protected virtual void OnDrawGizmos()
	{
		if (GetComponent<Camera>().aspect != 1f)
		{
			mCamera = GetComponent<Camera>();
			aspect = GetComponent<Camera>().aspect;
			corners = GetComponent<Camera>().GetWorldCorners(GetComponent<Camera>().farClipPlane);
		}
	}

	protected void DrawCameraView(Camera camera, Color color, float aspect, float scale)
	{
		Matrix4x4 matrix = Gizmos.matrix;
		Gizmos.color = color;
		if (camera.orthographic)
		{
			Gizmos.matrix = Matrix4x4.TRS(camera.transform.position, camera.transform.rotation, new Vector3(1f * scale, 1f * scale, 1f));
			float z = camera.farClipPlane - camera.nearClipPlane;
			float z2 = (camera.farClipPlane + camera.nearClipPlane) * 0.5f;
			Gizmos.DrawWireCube(new Vector3(0f, 0f, z2), new Vector3(camera.orthographicSize * 2f * camera.aspect, camera.orthographicSize * 2f, z));
		}
		else
		{
			Gizmos.matrix = Matrix4x4.TRS(camera.transform.position, camera.transform.rotation, new Vector3(scale, scale * aspect, 1f));
			Gizmos.DrawFrustum(Vector3.zero, camera.fieldOfView, camera.farClipPlane, camera.nearClipPlane, aspect);
		}
		Gizmos.matrix = matrix;
	}

	protected void DrawAreaPlane(Camera camera, Color color, float ypos, Vector3[] corners)
	{
		if (camera.orthographic)
		{
			Vector3 center = Vector3.Lerp(corners[0], corners[2], 0.5f);
			center.y = ypos;
			Vector3 size = corners[2] - corners[0];
			Gizmos.color = color;
			Gizmos.DrawCube(center, size);
			return;
		}
		Vector3 position = camera.transform.position;
		float t = Mathf.Abs(ypos - position.y) / camera.farClipPlane;
		Vector3[] array = new Vector3[4];
		for (int i = 0; i < corners.Length; i++)
		{
			array[i] = Vector3.Lerp(camera.transform.position, corners[i], t);
		}
		Vector3 center2 = Vector3.Lerp(array[0], array[2], 0.5f);
		Vector3 size2 = array[2] - array[0];
		Gizmos.color = color;
		Gizmos.DrawCube(center2, size2);
	}

	protected void DrawAreaPlane(Camera camera, Color color, float ypos, Vector3[] corners, float outline)
	{
		if (camera.orthographic)
		{
			Vector3 center = Vector3.Lerp(corners[0], corners[2], 0.5f);
			center.y = ypos;
			Vector3 a = corners[2] - corners[0];
			Gizmos.color = color;
			Gizmos.DrawCube(center, a + new Vector3(outline, 0f, outline) * 2.828f);
			return;
		}
		Vector3 position = camera.transform.position;
		float t = Mathf.Abs(ypos - position.y) / camera.farClipPlane;
		Vector3[] array = new Vector3[4];
		for (int i = 0; i < corners.Length; i++)
		{
			array[i] = Vector3.Lerp(camera.transform.position, corners[i], t);
		}
		Vector3 center2 = Vector3.Lerp(array[0], array[2], 0.5f);
		Vector3 a2 = array[2] - array[0];
		Gizmos.color = color;
		Gizmos.DrawCube(center2, a2 + new Vector3(outline, 0f, outline) * 2f);
	}
}
