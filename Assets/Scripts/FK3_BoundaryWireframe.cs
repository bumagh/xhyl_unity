using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class FK3_BoundaryWireframe : MonoBehaviour
{
	public bool showBox;

	public bool destroyIfPlay;

	public bool destroyIfNotEditor = true;

	public Rect boundary;

	private LineRenderer m_line;

	protected virtual void Awake()
	{
		m_line = GetComponent<LineRenderer>();
		if ((Application.isPlaying && destroyIfPlay) || (!Application.isEditor && destroyIfNotEditor))
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	protected virtual void Start()
	{
		OnValidate();
		Init_LineRenderer();
		if (showBox)
		{
			ShowBoundaryBox();
		}
	}

	protected virtual void OnValidate()
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

	protected void ShowBoundaryBox()
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

	protected void HideBoundaryBox()
	{
		if (!(m_line == null))
		{
			m_line.enabled = false;
		}
	}

	protected void Init_LineRenderer()
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
