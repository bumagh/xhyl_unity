using UnityEngine;

public class BCBM_Script_PtcFireworks_multi1 : MonoBehaviour
{
	private ParticleSystem _pFireworks1;

	private ParticleSystem _pFireworks2;

	private float _fTime;

	private bool _isNextPlay;

	private bool _isActive;

	private void Start()
	{
		_pFireworks1 = base.gameObject.GetComponent<ParticleSystem>();
		_pFireworks2 = base.transform.Find("PtcFireworks_multi2").GetComponent<ParticleSystem>();
	}

	private void Update()
	{
		if (!_isActive)
		{
			return;
		}
		if (_pFireworks1.isStopped && !_isNextPlay)
		{
			_fTime = UnityEngine.Random.Range(0f, 0.5f);
			_isNextPlay = true;
		}
		if (!_isNextPlay)
		{
			return;
		}
		_fTime -= Time.deltaTime;
		if (_fTime <= 0f)
		{
			_fTime = 0f;
			_isNextPlay = false;
			Color color;
			switch (UnityEngine.Random.Range(0, 10))
			{
			case 0:
				color = new Color(1f, 0f, 0f);
				break;
			case 2:
				color = new Color(0f, 1f, 0f);
				break;
			case 3:
				color = new Color(0f, 0f, 1f);
				break;
			case 4:
				color = new Color(UnityEngine.Random.Range(1f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
				break;
			case 5:
				color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1f);
				break;
			case 6:
				color = new Color(UnityEngine.Random.Range(0f, 1f), 1f, UnityEngine.Random.Range(0f, 1f));
				break;
			case 7:
				color = new Color(1f, UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
				break;
			default:
				color = new Color(1f, 0.5f, 0f);
				break;
			}
			SetColor(color);
			PlayPtc();
		}
	}

	public void SetEffectActive(bool isActive)
	{
		_isActive = isActive;
		_pFireworks1.GetComponent<Renderer>().enabled = _isActive;
		_pFireworks2.GetComponent<Renderer>().enabled = _isActive;
	}

	public void SetColor(Color c)
	{
		_pFireworks1.startColor = c;
		_pFireworks2.startColor = c;
	}

	public void PlayPtc()
	{
		base.transform.localPosition = new Vector3(UnityEngine.Random.Range(-18f, 18f), UnityEngine.Random.Range(-4f, 4f), 0f);
		_pFireworks1.Play();
		_pFireworks2.Play();
	}
}
