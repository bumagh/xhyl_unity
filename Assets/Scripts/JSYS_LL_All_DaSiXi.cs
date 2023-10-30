using JSYS_LL_GameCommon;
using UnityEngine;

public class JSYS_LL_All_DaSiXi : MonoBehaviour
{
	public enum DASIXI_COLOR_TYPE
	{
		DASIXI_RED,
		DASIXI_GREEN,
		DASIXI_YELLOW,
		DASIXI_ALL_SHINE,
		DASIXI_BLUE
	}

	public Texture[] mRingColorTx;

	public Texture mRingBaseColorTx;

	public Texture[] mLightColorTx;

	private bool _isShine = true;

	private GameObject _Rings;

	private float _fCurOffSet;

	private GameObject[] _LightCylinderArr = new GameObject[8];

	private float fAnimTime = 0.5f;

	private float fCurTime;

	private int _ncolorIndex;

	private void Awake()
	{
		_Rings = base.transform.Find("Rings").gameObject;
		for (int i = 0; i < 8; i++)
		{
			string empty = string.Empty;
			empty = ((i != 0) ? ("pCylinder" + (i + 6).ToString()) : ("pCylinder" + (i + 1).ToString()));
			_LightCylinderArr[i] = base.transform.Find("dasixi_gaungshu").Find("cylinder").Find(empty)
				.gameObject;
			}
		}

		private void Start()
		{
		}

		private void Update()
		{
			_updateUvAnim();
			_updadteColor();
		}

		private void _updateUvAnim()
		{
			_fCurOffSet -= Time.deltaTime * 1f;
			_Rings.GetComponent<Renderer>().sharedMaterials[1].SetTextureOffset("_MainTex", new Vector2(_fCurOffSet, 0f));
			if (_fCurOffSet < 0f)
			{
				_fCurOffSet += 1f;
			}
		}

		private void _setAllColor(AnimalColor color)
		{
			_Rings.GetComponent<Renderer>().sharedMaterials[1].mainTexture = mRingColorTx[(int)color];
			for (int i = 0; i < 8; i++)
			{
				_LightCylinderArr[i].GetComponent<Renderer>().sharedMaterial.mainTexture = mLightColorTx[(int)color];
			}
		}

		private void _updadteColor()
		{
			if (_isShine)
			{
				fCurTime += Time.deltaTime;
				if (fCurTime > fAnimTime)
				{
					fCurTime = 0f;
					_ncolorIndex = (_ncolorIndex + 1) % 3;
					_setAllColor((AnimalColor)_ncolorIndex);
				}
			}
		}

		public void Show()
		{
			Reset();
			iTween.ScaleFrom(base.gameObject, Vector3.zero, 1.5f);
		}

		public void SetColor(AnimalColor color)
		{
			_isShine = false;
			_setAllColor(color);
		}

		public void Reset()
		{
			_isShine = true;
			iTween.Stop(base.gameObject);
			base.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}
