using UnityEngine;

public class HTExplosion : MonoBehaviour
{
	public enum CameraFacingMode
	{
		BillBoard,
		Horizontal,
		Vertical,
		Never
	}

	public Material spriteSheetMaterial;

	public int spriteCount;

	public int uvAnimationTileX;

	public int uvAnimationTileY;

	public int framesPerSecond;

	public Vector3 size = new Vector3(1f, 1f, 1f);

	public float speedGrowing;

	public bool randomRotation;

	public bool isOneShot = true;

	public CameraFacingMode billboarding;

	public bool addLightEffect;

	public float lightRange;

	public Color lightColor;

	public float lightFadeSpeed = 1f;

	private Material mat;

	private Mesh mesh;

	private MeshRenderer meshRender;

	private AudioSource soundEffect;

	private float startTime;

	private Camera mainCam;

	private bool effectEnd;

	private float randomZAngle;

	private void Awake()
	{
		CreateParticle();
		mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		soundEffect = (GetComponent("AudioSource") as AudioSource);
		if (addLightEffect)
		{
			base.gameObject.AddComponent<Light>();
			base.gameObject.GetComponent<Light>().color = lightColor;
			base.gameObject.GetComponent<Light>().range = lightRange;
		}
		GetComponent<Renderer>().enabled = false;
	}

	private void Start()
	{
		startTime = Time.time;
		base.transform.localScale = size;
		if (randomRotation)
		{
			randomZAngle = UnityEngine.Random.Range(-180f, 180f);
		}
		else
		{
			randomZAngle = 0f;
		}
	}

	private void Update()
	{
		bool flag = false;
		Camera_BillboardingMode();
		float num = (Time.time - startTime) * (float)framesPerSecond;
		if ((num <= (float)spriteCount || !isOneShot) && !effectEnd)
		{
			num %= (float)(uvAnimationTileX * uvAnimationTileY);
			if (num == (float)spriteCount)
			{
				startTime = Time.time;
				num = 0f;
			}
			Vector2 value = new Vector2(1f / (float)uvAnimationTileX, 1f / (float)uvAnimationTileY);
			float num2 = Mathf.Floor(num % (float)uvAnimationTileX);
			float num3 = Mathf.Floor(num / (float)uvAnimationTileX);
			Vector2 value2 = new Vector2(num2 * value.x, 1f - value.y - num3 * value.y);
			GetComponent<Renderer>().material.SetTextureOffset("_MainTex", value2);
			GetComponent<Renderer>().material.SetTextureScale("_MainTex", value);
			base.transform.localScale += new Vector3(speedGrowing, speedGrowing, speedGrowing) * Time.deltaTime;
			GetComponent<Renderer>().enabled = true;
		}
		else
		{
			effectEnd = true;
			GetComponent<Renderer>().enabled = false;
			flag = true;
			if ((bool)soundEffect && soundEffect.isPlaying)
			{
				flag = false;
			}
			if (addLightEffect && flag && base.gameObject.GetComponent<Light>().intensity > 0f)
			{
				flag = false;
			}
			if (flag)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		if (addLightEffect && lightFadeSpeed != 0f)
		{
			base.gameObject.GetComponent<Light>().intensity -= lightFadeSpeed * Time.deltaTime;
		}
	}

	private void CreateParticle()
	{
		mesh = base.gameObject.AddComponent<MeshFilter>().mesh;
		meshRender = base.gameObject.AddComponent<MeshRenderer>();
		mesh.vertices = new Vector3[4]
		{
			new Vector3(-0.5f, -0.5f, 0f),
			new Vector3(-0.5f, 0.5f, 0f),
			new Vector3(0.5f, 0.5f, 0f),
			new Vector3(0.5f, -0.5f, 0f)
		};
		mesh.triangles = new int[6]
		{
			0,
			1,
			2,
			2,
			3,
			0
		};
		mesh.uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};
		meshRender.castShadows = false;
		meshRender.receiveShadows = false;
		mesh.RecalculateNormals();
		GetComponent<Renderer>().material = spriteSheetMaterial;
	}

	private void Camera_BillboardingMode()
	{
		Vector3 vector = mainCam.transform.position - base.transform.position;
		switch (billboarding)
		{
		case CameraFacingMode.BillBoard:
			base.transform.LookAt(vector);
			break;
		case CameraFacingMode.Horizontal:
			vector.x = (vector.z = 0f);
			base.transform.LookAt(mainCam.transform.position - vector);
			break;
		case CameraFacingMode.Vertical:
			vector.y = (vector.z = 0f);
			base.transform.LookAt(mainCam.transform.position - vector);
			break;
		}
		Transform transform = base.transform;
		Vector3 eulerAngles = base.transform.eulerAngles;
		float x = eulerAngles.x;
		Vector3 eulerAngles2 = base.transform.eulerAngles;
		transform.eulerAngles = new Vector3(x, eulerAngles2.y, randomZAngle);
	}
}
