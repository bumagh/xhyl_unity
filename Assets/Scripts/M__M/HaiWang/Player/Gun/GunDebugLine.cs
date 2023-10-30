using UnityEngine;
using UnityEngine.Rendering;

namespace M__M.HaiWang.Player.Gun
{
	public class GunDebugLine : MonoBehaviour
	{
		private GunController m_gun;

		private LineRenderer m_line;

		private static int idCount;

		private int m_id;

		private void Awake()
		{
			m_gun = GetComponent<GunController>();
			m_line = GetComponent<LineRenderer>();
			m_id = idCount;
			idCount++;
			Init_LineRenderer();
		}

		private void Start()
		{
			Init();
		}

		private void Update()
		{
		}

		private void Init()
		{
			m_gun.Event_RotateByInput_Handler += Handle_RotateByInput;
		}

		private void Init_LineRenderer()
		{
			if (m_line == null)
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
				Color[] array = new Color[3]
				{
					Color.red,
					Color.green,
					Color.blue
				};
				red = array[m_id % array.Length];
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
				lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
				lineRenderer.receiveShadows = false;
				m_line = lineRenderer;
			}
		}

		private void Handle_RotateByInput(GunController gun, Vector3 begin, Vector3 end, float angle)
		{
			if (!(m_line == null))
			{
				Vector3[] positions = new Vector3[2]
				{
					begin,
					end
				};
				m_line.SetPositions(positions);
			}
		}
	}
}
