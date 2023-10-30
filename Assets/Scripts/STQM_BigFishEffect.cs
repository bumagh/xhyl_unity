using UnityEngine;

public class STQM_BigFishEffect : MonoBehaviour
{
	private float _fLife;

	private float _fMaxLife = 2f;

	private int childCount;

	private ParticleSystem[] ps;

	[HideInInspector]
	public bool bPlaying;

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

	private void Start()
	{
		childCount = base.transform.childCount;
		ps = new ParticleSystem[childCount];
		for (int i = 0; i < childCount; i++)
		{
			ps[i] = base.transform.GetChild(i).GetComponent<ParticleSystem>();
		}
	}

	public void PlayEffect(Vector3 pos)
	{
		base.transform.position = pos;
		for (int i = 0; i < childCount; i++)
		{
			ps[i].Stop();
			ps[i].Clear();
			ps[i].Play();
		}
		_fLife = _fMaxLife;
		bPlaying = true;
	}

	private void Update()
	{
		if (bPlaying)
		{
			_fLife -= Time.deltaTime;
			if (_fLife < 0f)
			{
				_fLife = 0f;
				bPlaying = false;
			}
		}
	}
}
