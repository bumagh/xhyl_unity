using UnityEngine;

public class LL_All_SongDeng : MonoBehaviour
{
	private Vector3 Ini_localPosition;

	private bool isUp = true;

	public Texture[] mNumTexture;

	private int _nSongDengNum;

	private GameObject _NumberObj;

	private float fAnimTime = 0.0833333358f;

	private float fCurTime;

	private bool _isShow;

	private void Start()
	{
		_NumberObj = base.transform.Find("SongDengNumber").gameObject;
	}

	private void Update()
	{
		if (_isShow && _nSongDengNum < 0)
		{
			fCurTime += Time.deltaTime;
			if (fCurTime > fAnimTime)
			{
				fCurTime = 0f;
				int num = UnityEngine.Random.Range(0, 12);
				_NumberObj.GetComponent<Renderer>().sharedMaterial.mainTexture = mNumTexture[num];
			}
		}
		if (!_isShow)
		{
			return;
		}
		if (isUp)
		{
			base.transform.localPosition += Time.deltaTime * Vector3.up * 0.5f;
			Vector3 localPosition = base.transform.localPosition;
			if (localPosition.y > 6.2f)
			{
				isUp = false;
			}
		}
		else
		{
			base.transform.localPosition += Time.deltaTime * Vector3.down * 0.5f;
			Vector3 localPosition2 = base.transform.localPosition;
			if (localPosition2.y < 6f)
			{
				isUp = true;
			}
		}
	}

	public void ShowSongDeng(bool isShow, int number, bool withAnim = false)
	{
		Ini_localPosition = base.transform.localPosition;
		_isShow = isShow;
		_nSongDengNum = number;
		if (!_isShow)
		{
			return;
		}
		if (_nSongDengNum >= 12)
		{
			UnityEngine.Debug.Log("ShowSongDeng number error with the value:" + number);
			return;
		}
		if (_nSongDengNum >= 0)
		{
			_NumberObj.GetComponent<Renderer>().sharedMaterial.mainTexture = mNumTexture[_nSongDengNum];
		}
		if (withAnim)
		{
			iTween.ScaleFrom(base.gameObject, Vector3.zero, 1f);
		}
		else
		{
			base.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	public void Reset()
	{
		base.transform.localPosition = Ini_localPosition;
		iTween.Stop(base.gameObject);
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		fCurTime = 0f;
		_nSongDengNum = 0;
		ShowSongDeng(isShow: false, 0);
	}
}
