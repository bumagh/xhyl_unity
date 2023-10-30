using BCBM_GameCommon;
using System;
using UnityEngine;

public class BCBM_AnimalScript : BCBM_RoleAnimationScript
{
	private Animal_Action_State _Animal_Action;

	private Animal_Action_State _lastAnimal_Action;

	private Vector3 _centerPositon = new Vector3(0f, 0.5f, 0f);

	private Vector3 _iniPos;

	private Vector3 _parentIniPos;

	private Quaternion _iniRotation;

	public GameObject mAnimalRing;

	private Texture[] _colorTexture = new Texture[3];

	private GameObject _animal;

	private float _fShineTime;

	private bool _bShineFlag;

	public bool mbShineGold;

	private Material _iniMat;

	public Material mGoldMat;

	private Material testMat;

	public AnimalType mAnimalType
	{
		get
		{
			return mAnimalType;
		}
		set
		{
			mAnimalType = value;
		}
	}

	public AnimalColor mColor
	{
		get
		{
			return mColor;
		}
		set
		{
			mColor = value;
		}
	}

	private void Start()
	{
		if (base.gameObject.CompareTag("lion"))
		{
			base.transform.Find("Animal").GetComponent<Renderer>().material.SetFloat("_Cutoff", 0.1f);
		}
		_animal = base.transform.Find("Animal").gameObject;
		_Animal_Action = Animal_Action_State.Animal_Wait;
		SetAnimation(AnimationType.AT_WAIT);
		_iniPos = base.transform.localPosition;
		_iniRotation = base.transform.localRotation;
		_parentIniPos = base.transform.parent.localPosition;
		_centerPositon = new Vector3(0f, BCBM_Parameter.G_fCenterAnimalHeight, 0f);
		_optimize();
		_iniMat = _animal.GetComponent<Renderer>().sharedMaterial;
		ShowGoldShine(isShow: false);
		ShowGold(isShow: false);
		_loadAnimalRingTexture();
	}

	private void Update()
	{
		_logicCtrl();
		if (mbShineGold)
		{
			_shineJPGold();
		}
	}

	public void ShowGoldShine(bool isShow)
	{
		mbShineGold = isShow;
		if (!mbShineGold)
		{
			_fShineTime = 0f;
			_bShineFlag = false;
			_animal.GetComponent<Renderer>().sharedMaterial = _iniMat;
		}
	}

	public void ShowGold(bool isShow)
	{
		if (isShow)
		{
			_animal.GetComponent<Renderer>().sharedMaterial = mGoldMat;
		}
		else
		{
			_animal.GetComponent<Renderer>().sharedMaterial = _iniMat;
		}
	}

	public void SetAnimalActionState(Animal_Action_State state)
	{
		_Animal_Action = state;
		switch (state)
		{
		case Animal_Action_State.Animal_Bounce:
			break;
		case Animal_Action_State.Animal_Wait:
			Reset();
			break;
		case Animal_Action_State.Animal_Spin:
			SetAnimation(AnimationType.AT_STOP);
			break;
		case Animal_Action_State.Animal_Bounce_And_GoCenter:
			SetAnimation(AnimationType.AT_STOP);
			break;
		case Animal_Action_State.Animal_Play_Win:
		{
			BCBM_EffectMngr.GetSingleton().ShowAnimalParticle(isShow: true);
			int typ = 0;
			if (base.gameObject.CompareTag("lion"))
			{
				typ = 3;
			}
			else if (base.gameObject.CompareTag("rabbit"))
			{
				typ = 0;
			}
			else if (base.gameObject.CompareTag("panda"))
			{
				typ = 2;
			}
			else if (base.gameObject.CompareTag("monkey"))
			{
				typ = 1;
			}
			BCBM_MusicMngr.GetSingleton().PlayAnimalWin((AnimalType)typ);
			SetAnimation(AnimationType.AT_WIN);
			break;
		}
		}
	}

	private void _logicCtrl()
	{
		float deltaTime = Time.deltaTime;
		if (deltaTime > 71f / (678f * (float)Math.PI))
		{
		}
		if (_Animal_Action == Animal_Action_State.Animal_Bounce_And_GoCenter)
		{
			if (_lastAnimal_Action != Animal_Action_State.Animal_Bounce_And_GoCenter)
			{
				mAnimalRing.SetActive(value: true);
				iTween.ScaleTo(base.gameObject, iTween.Hash("scale", BCBM_Parameter.G_AnimalMaxScale2, "delay", 0.3f, "time", 0.1f, "easetype", iTween.EaseType.easeInOutSine));
				iTween.ScaleTo(base.gameObject, iTween.Hash("scale", BCBM_Parameter.G_AnimalMinScale, "delay", 0.4f, "time", 0.1f, "easetype", iTween.EaseType.easeInOutSine));
				iTween.ScaleTo(base.gameObject, iTween.Hash("scale", BCBM_Parameter.G_AnimalMaxScale, "delay", 0.5f, "time", 0.1f, "easetype", iTween.EaseType.easeOutBack));
				iTween.MoveTo(base.transform.parent.gameObject, iTween.Hash("y", _centerPositon.y, "delay", 1.6f, "time", 0.8f, "easetype", iTween.EaseType.easeInOutSine));
				iTween.MoveTo(base.transform.parent.gameObject, iTween.Hash("position", _centerPositon, "time", 1.8f, "delay", 2.4f, "oncomplete", "SetAnimalActionState", "easetype", iTween.EaseType.easeInOutSine, "oncompleteparams", Animal_Action_State.Animal_Play_Win, "oncompletetarget", base.gameObject));
				Vector3 eulerAngles = base.transform.eulerAngles;
				float y = eulerAngles.y;
				iTween.RotateTo(base.gameObject, iTween.Hash("rotation", Vector3.up * 0f, "time", 1.6f, "delay", 2.4f, "easetype", iTween.EaseType.easeInOutSine));
				BCBM_MusicMngr.GetSingleton().PlayAnimalOut();
			}
		}
		else if (_Animal_Action == Animal_Action_State.Animal_Bounce && _lastAnimal_Action != Animal_Action_State.Animal_Bounce)
		{
			mAnimalRing.SetActive(value: true);
			iTween.ScaleTo(base.gameObject, iTween.Hash("scale", BCBM_Parameter.G_AnimalMaxScale2, "delay", 0.3f, "time", 0.1f, "easetype", iTween.EaseType.easeInOutSine));
			iTween.ScaleTo(base.gameObject, iTween.Hash("scale", BCBM_Parameter.G_AnimalMinScale, "delay", 0.4f, "time", 0.1f, "easetype", iTween.EaseType.easeInOutSine));
			iTween.ScaleTo(base.gameObject, iTween.Hash("scale", BCBM_Parameter.G_AnimalMaxScale, "delay", 0.5f, "time", 0.1f, "easetype", iTween.EaseType.easeOutBack));
			BCBM_MusicMngr.GetSingleton().PlayAnimalOut();
		}
		_lastAnimal_Action = _Animal_Action;
	}

	public void SetColorOfAnimalRing(AnimalColor color)
	{
		if (color < (AnimalColor)4 && color >= AnimalColor.Animal_Red)
		{
			mAnimalRing.GetComponent<Renderer>().material.mainTexture = _colorTexture[(int)color];
		}
		else
		{
			BCBM_ErrorManager.GetSingleton().AddError("---Error Client3D---:SetColorOfAnimalRing wrong color");
		}
	}

	private void _loadAnimalRingTexture()
	{
		AnimalType animalType = AnimalType.Lion;
		if (base.gameObject.CompareTag("lion"))
		{
			animalType = AnimalType.Lion;
		}
		else if (base.gameObject.CompareTag("panda"))
		{
			animalType = AnimalType.Panda;
		}
		else if (base.gameObject.CompareTag("monkey"))
		{
			animalType = AnimalType.Monkey;
		}
		else if (base.gameObject.CompareTag("rabbit"))
		{
			animalType = AnimalType.Rabbit;
		}
		switch (animalType)
		{
		case AnimalType.Rabbit:
			_colorTexture[0] = (Texture)Resources.Load("AnimalRingTextures/r_rabbit");
			_colorTexture[1] = (Texture)Resources.Load("AnimalRingTextures/g_rabbit");
			_colorTexture[2] = (Texture)Resources.Load("AnimalRingTextures/y_rabbit");
			break;
		case AnimalType.Monkey:
			_colorTexture[0] = (Texture)Resources.Load("AnimalRingTextures/r_monkey");
			_colorTexture[1] = (Texture)Resources.Load("AnimalRingTextures/g_monkey");
			_colorTexture[2] = (Texture)Resources.Load("AnimalRingTextures/y_monkey");
			break;
		case AnimalType.Panda:
			_colorTexture[0] = (Texture)Resources.Load("AnimalRingTextures/r_panda");
			_colorTexture[1] = (Texture)Resources.Load("AnimalRingTextures/g_panda");
			_colorTexture[2] = (Texture)Resources.Load("AnimalRingTextures/y_panda");
			break;
		case AnimalType.Lion:
			_colorTexture[0] = (Texture)Resources.Load("AnimalRingTextures/r_lion");
			_colorTexture[1] = (Texture)Resources.Load("AnimalRingTextures/g_lion");
			_colorTexture[2] = (Texture)Resources.Load("AnimalRingTextures/y_lion");
			break;
		}
	}

	private void _optimize()
	{
	}

	private void _shineJPGold()
	{
		_fShineTime += Time.deltaTime;
		if (_fShineTime > 0.5f)
		{
			_fShineTime = 0f;
			_bShineFlag = !_bShineFlag;
			if (_bShineFlag)
			{
				_animal.GetComponent<Renderer>().sharedMaterial = (Material)Resources.Load("Materials/GoldAnimal");
			}
			else
			{
				_animal.GetComponent<Renderer>().sharedMaterial = _iniMat;
			}
		}
	}

	public void Reset()
	{
		mAnimalRing.SetActive(value: false);
		iTween.Stop(base.gameObject);
		iTween.Stop(base.transform.parent.gameObject);
		base.transform.localPosition = _iniPos;
		base.transform.localRotation = _iniRotation;
		base.transform.parent.localPosition = _parentIniPos;
		base.transform.localScale = BCBM_Parameter.G_AnimalMinScale;
		SetAnimation(AnimationType.AT_WAIT);
		ShowGoldShine(isShow: false);
		ShowGold(isShow: false);
	}
}
