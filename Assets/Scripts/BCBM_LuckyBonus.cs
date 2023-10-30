using UnityEngine;

public class BCBM_LuckyBonus : MonoBehaviour
{
	public Texture[] mBonusTexue;

	public GameObject mBonusNum;

	private float scrollSpeed = -5f;

	private float offset;

	private void Start()
	{
	}

	private void Update()
	{
		_updateUVAnim();
	}

	public void SetBonusNum(int num)
	{
		if (num > 99999)
		{
			num = 99999;
		}
		int num2 = num / 10000 % 10;
		int num3 = num / 1000 % 10;
		int num4 = num / 100 % 10;
		int num5 = num / 10 % 10;
		int num6 = num % 10;
		mBonusNum.GetComponent<Renderer>().sharedMaterials[4].mainTexture = mBonusTexue[num2];
		mBonusNum.GetComponent<Renderer>().sharedMaterials[5].mainTexture = mBonusTexue[num3];
		mBonusNum.GetComponent<Renderer>().sharedMaterials[3].mainTexture = mBonusTexue[num4];
		mBonusNum.GetComponent<Renderer>().sharedMaterials[7].mainTexture = mBonusTexue[num5];
		mBonusNum.GetComponent<Renderer>().sharedMaterials[6].mainTexture = mBonusTexue[num6];
	}

	private void _updateUVAnim()
	{
		offset -= Time.deltaTime * scrollSpeed;
		if (offset < 0f)
		{
			offset = 1f;
		}
		mBonusNum.GetComponent<Renderer>().sharedMaterials[1].SetTextureOffset("_MainTex", new Vector2(offset, 0f));
	}
}
