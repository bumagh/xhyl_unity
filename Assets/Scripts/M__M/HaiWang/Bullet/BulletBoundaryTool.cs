using UnityEngine;
using UnityEngine.Rendering;

namespace M__M.HaiWang.Bullet
{
	[ExecuteInEditMode]
	public class BulletBoundaryTool : MonoBehaviour
	{
		public bool showBox;

		public Rect boundary;

		private LineRenderer m_line;

		private void Awake()
		{
			m_line = GetComponent<LineRenderer>();
			boundary = new Rect(-7.1f, -4f, 14.2f, 8f);
			UnityEngine.Object.Destroy(this);
		}

		private void Start()
		{
			OnValidate();
			Init_LineRenderer();
			if (showBox)
			{
				ShowBoundaryBox();
			}
		}

		private void OnValidate()
		{
			if (showBox)
			{
				ShowBoundaryBox();
			}
			else
			{
				HideBoundaryBox();
			}
		}

		private void ShowBoundaryBox()
		{
			if (!(m_line == null))
			{
				m_line.enabled = true;
				float z = 0f;
				Vector3[] array = new Vector3[5]
				{
					new Vector3(boundary.xMin, boundary.yMin, z),
					new Vector3(boundary.xMin, boundary.yMax, z),
					new Vector3(boundary.xMax, boundary.yMax, z),
					new Vector3(boundary.xMax, boundary.yMin, z),
					new Vector3(boundary.xMin, boundary.yMin, z)
				};
				m_line.positionCount = array.Length;
				m_line.SetPositions(array);
			}
		}

		private void HideBoundaryBox()
		{
			if (!(m_line == null))
			{
				m_line.enabled = false;
			}
		}

		private void Init_LineRenderer()
		{
			if (m_line == null)
			{
				LineRenderer lineRenderer = base.gameObject.AddComponent<LineRenderer>();
				lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
				lineRenderer.widthMultiplier = 0.02f;
				lineRenderer.positionCount = 2;
				Color32 c = Color.yellow;
				lineRenderer.startColor = c;
				lineRenderer.endColor = c;
				lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
				lineRenderer.receiveShadows = false;
				m_line = lineRenderer;
			}
		}
	}
}
