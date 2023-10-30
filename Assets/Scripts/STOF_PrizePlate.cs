using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class STOF_PrizePlate : MonoBehaviour
{
	private GameObject _numObj;

	private GameObject _plateObj;

	private Image imgPlate;

	private Text txt;

	private Color col;

	private bool _isDead;

	private float _fLife;

	private float _fMaxLife = 5f;

	private Material material;

	public float mLife
	{
		get
		{
			return _fLife;
		}
		set
		{
			_fLife = value;
		}
	}

	private void Awake()
	{
		_numObj = base.transform.Find("TxtScore").gameObject;
		_plateObj = base.transform.Find("ImgScoreBg").gameObject;
		imgPlate = _plateObj.GetComponent<Image>();
		txt = _numObj.GetComponent<Text>();
	}

	private void OnEnable()
	{
		SetMaterial();
	}

	private void SetMaterial()
	{
		if (material == null)
		{
			material = Resources.Load<Material>("FlowLightShader_ImgLine");
		}
		if (material != null)
		{
			txt.material = material;
			imgPlate.material = material;
		}
	}

	public void ShowScore(int nNum, Vector3 playerPos, int playerID)
	{
		SetMaterial();
		Vector3 vector = (playerID != 1 && playerID != 2) ? Vector3.down : Vector3.up;
		Quaternion quaternion = default(Quaternion);
		quaternion.SetFromToRotation(Vector3.up, vector);
		Transform transform = base.transform;
		Vector3 forward = Vector3.forward;
		Vector3 eulerAngles = quaternion.eulerAngles;
		transform.eulerAngles = forward * eulerAngles.z;
		base.transform.position = playerPos - vector * 1f;
		base.transform.DOKill();
		base.transform.DOMove(playerPos + vector * 2.3f, 0.5f);
		txt.text = nNum.ToString();
		_fLife = _fMaxLife;
	}

	private void Update()
	{
		if (!_isDead)
		{
			_fLife -= Time.deltaTime;
			if (_fLife < 0f)
			{
				_fLife = 0f;
				DestroyObj();
			}
		}
	}

	private void _onFadeEnd()
	{
		STOF_EffectMngr.GetSingleton().DestroyEffectObj(base.gameObject);
	}

	public void DestroyObj()
	{
		if (!_isDead)
		{
			txt.material = null;
			imgPlate.material = null;
			_fLife = 0f;
			_isDead = true;
			base.transform.DOKill();
			imgPlate.DOFade(0f, 0.5f).OnComplete(_onFadeEnd);
			txt.DOFade(0f, 0.5f);
		}
	}

	public void OnSpawned()
	{
		_isDead = false;
		col = txt.color;
		col.a = 1f;
		txt.color = col;
		imgPlate.color = col;
	}
}
