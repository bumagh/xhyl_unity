using UnityEngine;
using UnityEngine.Rendering;

public class FK3_DebugLine : MonoBehaviour
{
	public class LinePoint
	{
		private Transform _transform;

		public Vector3 position = Vector3.zero;

		public bool isTransform;

		public Transform transform
		{
			get
			{
				return _transform;
			}
			set
			{
				_transform = value;
				isTransform = (_transform != null);
			}
		}

		public Vector3 GetPoint()
		{
			return isTransform ? transform.position : position;
		}

		public LinePoint SetPoint(Vector3 pos)
		{
			position = pos;
			isTransform = false;
			return this;
		}
	}

	public Color color;

	public int colorIndex;

	public bool useColor;

	[FK3_SortingLayer]
	[SerializeField]
	private string _layerName = "Default";

	[Range(-20000f, 20000f)]
	[SerializeField]
	private int _orderInLayer;

	private LineRenderer _line;

	private Vector3[] _points = new Vector3[2]
	{
		Vector3.zero,
		Vector3.one
	};

	private LinePoint _pointStart = new LinePoint();

	private LinePoint _pointEnd = new LinePoint();

	private void Awake()
	{
		Init_LineRenderer();
	}

	private void Start()
	{
	}

	private void Update()
	{
		_line.SetPositions(GetLinePoints());
	}

	private void Init_LineRenderer()
	{
		if (_line == null)
		{
			LineRenderer lineRenderer = base.gameObject.AddComponent<LineRenderer>();
			lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
			lineRenderer.widthMultiplier = 0.05f;
			lineRenderer.positionCount = 2;
			float alpha = 1f;
			Gradient gradient = new Gradient();
			Color yellow = Color.yellow;
			Color red = Color.red;
			red = UnityEngine.Random.ColorHSV();
			if (useColor)
			{
				lineRenderer.startColor = color;
				lineRenderer.endColor = color;
			}
			else
			{
				red = GetColorByIndex(colorIndex);
				gradient.SetKeys(new GradientColorKey[2]
				{
					new GradientColorKey(yellow, 0f),
					new GradientColorKey(red, 1f)
				}, new GradientAlphaKey[2]
				{
					new GradientAlphaKey(alpha, 0f),
					new GradientAlphaKey(alpha, 1f)
				});
				lineRenderer.colorGradient = gradient;
			}
			lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
			lineRenderer.receiveShadows = false;
			lineRenderer.useWorldSpace = true;
			_line = lineRenderer;
			_line.sortingLayerName = _layerName;
			_line.sortingOrder = _orderInLayer;
		}
	}

	public void SetLayer(string layerName, int orderInLayer)
	{
		_layerName = layerName;
		_orderInLayer = orderInLayer;
		if (_line != null)
		{
			_line.sortingLayerName = _layerName;
			_line.sortingOrder = _orderInLayer;
		}
	}

	private Color GetColorByIndex(int idx)
	{
		Color[] array = new Color[3]
		{
			Color.red,
			Color.green,
			Color.blue
		};
		return array[idx % array.Length];
	}

	private Vector3[] GetLinePoints()
	{
		_points[0] = _pointStart.GetPoint();
		_points[1] = _pointEnd.GetPoint();
		return _points;
	}

	public void SetLine(Transform begin, Transform end)
	{
		_pointStart.transform = begin;
		_pointEnd.transform = end;
		_line.SetPositions(GetLinePoints());
	}

	public void SetLine(Transform begin, Vector3 end)
	{
		_pointStart.transform = begin;
		_pointEnd.SetPoint(end);
		_line.SetPositions(GetLinePoints());
	}

	public void SetLine(Vector3 begin, Vector3 end)
	{
		_pointStart.SetPoint(begin);
		_pointEnd.SetPoint(end);
		_line.SetPositions(GetLinePoints());
	}

	public void SetLine(Vector3 begin, Transform end)
	{
		_pointStart.SetPoint(begin);
		_pointEnd.transform = end;
		_line.SetPositions(GetLinePoints());
	}
}
