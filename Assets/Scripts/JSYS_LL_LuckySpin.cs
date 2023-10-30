using JSYS_LL_GameCommon;
using UnityEngine;

public class JSYS_LL_LuckySpin : MonoBehaviour
{
	public enum LuckyType
	{
		LUCKY_NONE,
		LUCKY_SONGDENG,
		LUCKY_LIGHTING,
		LUCKY_BONUS
	}

	private float[] _TaiHaoArray = new float[10]
	{
		0.1f,
		0.1f,
		0.1f,
		0.1f,
		0.1f,
		0.1f,
		0.1f,
		0.1f,
		0.1f,
		0.1f
	};

	private float[] _LuckyTypArray = new float[3]
	{
		0.1f,
		0.1f,
		0.1f
	};

	private Vector3 _IniPos;

	private Quaternion _IniRotation;

	private int _nLuckyTaihao;

	private LuckyType _LuckyTyp;

	private Material _TaiHaoMat;

	private Material _LuckyPrizeMat;

	private float _TaiHao_Material_Ini_Offset = 0.07f;

	private float _LuckyType_Material_Ini_Offset = -0.01f;

	private float _Cur_TaiHao_MaterialOffset = 0.07f;

	private float _Cur_LuckyType_MaterialOffset = -0.01f;

	private bool _IsTaihao_Spin;

	private bool _IsLuckyTyp_Spin;

	private bool _Is_TaiHao_GoToStop;

	private bool _Is_LuckyPrize_GoToStop;

	private float _fTotalTaiHaoSpin_Time;

	private float _fTotalLuckyTypSpin_Time;

	private float _fCurTaiHao_SpinTime;

	private float _fCurLuckyTyp_SpinTime;

	private float _TaiHao_GoToStopTimeMax = 5f;

	private float _LuckyPrize_GoToStopTimeMax = 5f;

	private float m_TaiHao_Dis;

	private float m_LuckyPrize_Dis;

	private float _fTaiHaoStopRound = 2f;

	private float _fLuckyPrizeStopRound = 2f;

	private float _fTaiHaoBeginStopTime;

	private float _fLuckyPrizeBeginStopTime;

	public float mfTaiHao_MaxSpeed = 1f;

	public float mfLuckyPrize_MaxSpeed = 3f;

	private float _fTaiHaoSpeed;

	private float _fLuckyPrizeSpeed;

	public int no;

	private bool _isShineTaihao;

	private bool _isShinePrizeTyp;

	private bool _bTaihaoShineFlag;

	private bool _bPrizeShineFlag;

	private float _fShineTaihaoTime;

	private float _fShinePrizeTypTime;

	public Texture[] mTaihaoShineTex;

	public Texture[] mPrizeTypeShineTex;

	private void Awake()
	{
		_IniPos = base.transform.localPosition;
		_IniRotation = base.transform.localRotation;
		_TaiHaoMat = base.transform.Find("Spin").GetComponent<Renderer>().sharedMaterials[0];
		_LuckyPrizeMat = base.transform.Find("Spin").GetComponent<Renderer>().sharedMaterials[1];
		for (int i = 0; i < 10; i++)
		{
			int num = 9 - i;
			if (i <= 4)
			{
				_TaiHaoArray[num] = (_TaiHao_Material_Ini_Offset + (float)i * 1f / 10f) % 1f;
			}
			else if (i > 4)
			{
				_TaiHaoArray[num] = (_TaiHao_Material_Ini_Offset + (float)(i + 1) * 1f / 10f) % 1f;
			}
		}
		_TaiHaoArray[9] = _TaiHao_Material_Ini_Offset + 0.5f;
		for (int j = 0; j < 3; j++)
		{
			_LuckyTypArray[j] = _LuckyType_Material_Ini_Offset + (float)j * 1f / 3f;
		}
		_Cur_LuckyType_MaterialOffset = _LuckyTypArray[0];
		_TaiHaoMat.SetTextureOffset("_MainTex", new Vector2(0f, _Cur_TaiHao_MaterialOffset));
		_LuckyPrizeMat.SetTextureOffset("_MainTex", new Vector2(0f, _Cur_LuckyType_MaterialOffset));
	}

	private void Update()
	{
		if (_IsTaihao_Spin)
		{
			_TaiHaoSpin_Update();
			_fCurTaiHao_SpinTime += Time.deltaTime;
			if (_fCurTaiHao_SpinTime >= _fTaiHaoBeginStopTime)
			{
				StopScroll_MachineNo(_fTotalTaiHaoSpin_Time - _fCurTaiHao_SpinTime);
			}
		}
		if (_IsLuckyTyp_Spin)
		{
			_LuckyTypSpin_Update();
			_fCurLuckyTyp_SpinTime += Time.deltaTime;
			if (_fCurLuckyTyp_SpinTime > _fLuckyPrizeBeginStopTime)
			{
				StopScroll_LuckyTyp(_fTotalLuckyTypSpin_Time - _fCurLuckyTyp_SpinTime);
			}
		}
		if (_Is_TaiHao_GoToStop)
		{
			_TaiHao_StopUpdate();
		}
		if (_Is_LuckyPrize_GoToStop)
		{
			_LuckyTyp_StopUpdate();
		}
		if (_isShineTaihao)
		{
			ShineTaiHao();
		}
		if (_isShinePrizeTyp)
		{
			ShinePrizeTyp();
		}
	}

	public void SpinTo(int nTaihao, LuckyType luckyTyp, float fTaiHao_Time, float fLuckyTyp_Time)
	{
		_nLuckyTaihao = nTaihao;
		_LuckyTyp = luckyTyp;
		_IsTaihao_Spin = true;
		_IsLuckyTyp_Spin = true;
		_fTotalTaiHaoSpin_Time = fTaiHao_Time;
		_fTotalLuckyTypSpin_Time = fLuckyTyp_Time;
		_fCurLuckyTyp_SpinTime = 0f;
		_fCurTaiHao_SpinTime = 0f;
		_fTaiHaoBeginStopTime = _fTotalTaiHaoSpin_Time / 3f;
		_fLuckyPrizeBeginStopTime = _fTotalLuckyTypSpin_Time / 3f;
	}

	public void StopScroll_MachineNo(float timeLen)
	{
		_IsTaihao_Spin = false;
		_Is_TaiHao_GoToStop = true;
		_TaiHao_GoToStopTimeMax = timeLen;
		float num = (!(timeLen * 0.2f <= 2f)) ? (timeLen * 0.2f) : 2f;
		m_TaiHao_Dis = _TaiHaoArray[_nLuckyTaihao] + num * 1f - _Cur_TaiHao_MaterialOffset;
	}

	public void StopScroll_LuckyTyp(float timeLen)
	{
		if (_LuckyTyp != 0)
		{
			_IsLuckyTyp_Spin = false;
			_Is_LuckyPrize_GoToStop = true;
			_LuckyPrize_GoToStopTimeMax = timeLen;
			int num = (int)(_LuckyTyp - 1);
			float num2 = timeLen * 1f;
			m_LuckyPrize_Dis = _Cur_LuckyType_MaterialOffset + num2 * 1f - _LuckyTypArray[num];
		}
	}

	public void Close()
	{
	}

	public void Reset()
	{
		base.transform.localPosition = _IniPos;
		base.transform.localRotation = _IniRotation;
		_fLuckyPrizeSpeed = 0f;
		_fTaiHaoSpeed = 0f;
		_IsTaihao_Spin = false;
		_Is_TaiHao_GoToStop = false;
		_IsLuckyTyp_Spin = false;
		_Is_LuckyPrize_GoToStop = false;
		_fCurTaiHao_SpinTime = 0f;
		_fTotalTaiHaoSpin_Time = 0f;
		_fCurLuckyTyp_SpinTime = 0f;
		_fTotalLuckyTypSpin_Time = 0f;
		_isShineTaihao = false;
		_isShinePrizeTyp = false;
		_bTaihaoShineFlag = false;
		_bPrizeShineFlag = false;
		_fShineTaihaoTime = 0f;
		_fShinePrizeTypTime = 0f;
		_LuckyPrizeMat.mainTexture = mPrizeTypeShineTex[0];
		_TaiHaoMat.mainTexture = mTaihaoShineTex[0];
	}

	public void ResetOffset()
	{
		_Cur_LuckyType_MaterialOffset = _LuckyTypArray[0];
		_Cur_TaiHao_MaterialOffset = _TaiHaoArray[0];
		_TaiHaoMat.SetTextureOffset("_MainTex", new Vector2(0f, _Cur_TaiHao_MaterialOffset));
		_LuckyPrizeMat.SetTextureOffset("_MainTex", new Vector2(0f, _Cur_LuckyType_MaterialOffset));
	}

	private void _TaiHaoSpin_Update()
	{
		if (_fTaiHaoSpeed < mfTaiHao_MaxSpeed)
		{
			_fTaiHaoSpeed += LL_GameTool.GetDeltaTime() * 0.2f;
		}
		else
		{
			_fTaiHaoSpeed = mfTaiHao_MaxSpeed;
		}
		_Cur_TaiHao_MaterialOffset += LL_GameTool.GetDeltaTime() * _fTaiHaoSpeed;
		if (_Cur_TaiHao_MaterialOffset > 1f + _TaiHao_Material_Ini_Offset)
		{
			_Cur_TaiHao_MaterialOffset = _TaiHao_Material_Ini_Offset;
		}
		_TaiHaoMat.SetTextureOffset("_MainTex", new Vector2(0f, _Cur_TaiHao_MaterialOffset));
	}

	private void _LuckyTypSpin_Update()
	{
		if (_fLuckyPrizeSpeed < mfLuckyPrize_MaxSpeed)
		{
			_fLuckyPrizeSpeed += LL_GameTool.GetDeltaTime();
		}
		else
		{
			_fLuckyPrizeSpeed = mfLuckyPrize_MaxSpeed;
		}
		_Cur_LuckyType_MaterialOffset -= LL_GameTool.GetDeltaTime() * _fLuckyPrizeSpeed;
		if (_Cur_LuckyType_MaterialOffset < _LuckyType_Material_Ini_Offset)
		{
			_Cur_LuckyType_MaterialOffset = 1f + _LuckyType_Material_Ini_Offset;
		}
		_LuckyPrizeMat.SetTextureOffset("_MainTex", new Vector2(0f, _Cur_LuckyType_MaterialOffset));
	}

	private void _TaiHao_StopUpdate()
	{
		if (_TaiHao_GoToStopTimeMax > 0f)
		{
			float num = 2f * m_TaiHao_Dis / (_TaiHao_GoToStopTimeMax * _TaiHao_GoToStopTimeMax);
			_fTaiHaoSpeed = num * _TaiHao_GoToStopTimeMax;
			_Cur_TaiHao_MaterialOffset += _fTaiHaoSpeed * Time.deltaTime;
			_Cur_TaiHao_MaterialOffset %= 1f;
			_TaiHaoMat.SetTextureOffset("_MainTex", new Vector2(0f, _Cur_TaiHao_MaterialOffset));
			m_TaiHao_Dis -= _fTaiHaoSpeed * Time.deltaTime;
			if (m_TaiHao_Dis < 0.8f && m_TaiHao_Dis > 0f)
			{
				m_TaiHao_Dis = (_TaiHaoArray[_nLuckyTaihao] - _Cur_TaiHao_MaterialOffset + 2f) % 1f;
			}
			_TaiHao_GoToStopTimeMax -= Time.deltaTime;
		}
		else
		{
			_Is_TaiHao_GoToStop = false;
			_fTaiHaoSpeed = 0f;
			m_TaiHao_Dis = 0f;
			_TaiHao_GoToStopTimeMax = 0f;
			_Cur_TaiHao_MaterialOffset = _TaiHaoArray[_nLuckyTaihao];
			_TaiHaoMat.SetTextureOffset("_MainTex", new Vector2(0f, _Cur_TaiHao_MaterialOffset));
			_isShineTaihao = true;
			if (_nLuckyTaihao <= 8 && _nLuckyTaihao >= 1)
			{
				JSYS_LL_MusicMngr.GetSingleton().PlayGameMusic(JSYS_LL_MusicMngr.MUSIC_GAME_MUSIC.GAME_LUCKYPRIZE_BEGIN);
			}
		}
	}

	private void _LuckyTyp_StopUpdate()
	{
		if (_LuckyPrize_GoToStopTimeMax > 0f)
		{
			float num = 2f * m_LuckyPrize_Dis / (_LuckyPrize_GoToStopTimeMax * _LuckyPrize_GoToStopTimeMax);
			_fLuckyPrizeSpeed = num * _LuckyPrize_GoToStopTimeMax;
			_Cur_LuckyType_MaterialOffset -= _fLuckyPrizeSpeed * Time.deltaTime;
			if (_Cur_LuckyType_MaterialOffset < _LuckyType_Material_Ini_Offset)
			{
				_Cur_LuckyType_MaterialOffset = 1f + _Cur_LuckyType_MaterialOffset;
			}
			_LuckyPrizeMat.SetTextureOffset("_MainTex", new Vector2(0f, _Cur_LuckyType_MaterialOffset));
			m_LuckyPrize_Dis -= _fLuckyPrizeSpeed * Time.deltaTime;
			_LuckyPrize_GoToStopTimeMax -= Time.deltaTime;
			if (m_LuckyPrize_Dis < 0.8f && m_LuckyPrize_Dis > 0f)
			{
				int num2 = (int)(_LuckyTyp - 1);
				m_LuckyPrize_Dis = (_Cur_LuckyType_MaterialOffset - _LuckyTypArray[num2] + 2f) % 1f;
			}
		}
		else
		{
			_IsLuckyTyp_Spin = false;
			_Is_LuckyPrize_GoToStop = false;
			_fLuckyPrizeSpeed = 0f;
			_LuckyPrize_GoToStopTimeMax = 0f;
			m_LuckyPrize_Dis = 0f;
			int num3 = (int)(_LuckyTyp - 1);
			_Cur_LuckyType_MaterialOffset = _LuckyTypArray[num3];
			_LuckyPrizeMat.SetTextureOffset("_MainTex", new Vector2(0f, _Cur_LuckyType_MaterialOffset));
			_isShinePrizeTyp = true;
		}
	}

	private void ShineTaiHao()
	{
		_fShineTaihaoTime += Time.deltaTime;
		if (_fShineTaihaoTime > 0.5f)
		{
			_fShineTaihaoTime = 0f;
			_bTaihaoShineFlag = !_bTaihaoShineFlag;
			if (_bTaihaoShineFlag)
			{
				_TaiHaoMat.mainTexture = mTaihaoShineTex[0];
			}
			else
			{
				_TaiHaoMat.mainTexture = mTaihaoShineTex[1];
			}
		}
	}

	private void ShinePrizeTyp()
	{
		_fShinePrizeTypTime += Time.deltaTime;
		if (_fShinePrizeTypTime > 0.5f)
		{
			_fShinePrizeTypTime = 0f;
			_bPrizeShineFlag = !_bPrizeShineFlag;
			if (_bPrizeShineFlag)
			{
				_LuckyPrizeMat.mainTexture = mPrizeTypeShineTex[0];
			}
			else
			{
				_LuckyPrizeMat.mainTexture = mPrizeTypeShineTex[1];
			}
		}
	}
}
