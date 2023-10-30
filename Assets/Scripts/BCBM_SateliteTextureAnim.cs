using UnityEngine;

public class BCBM_SateliteTextureAnim : MonoBehaviour
{
	public Texture[] mAnimTexture;

	private float fAnimTime = 0.05f;

	private float fCurTime;

	private int nTextureIndex;

	private void Start()
	{
	}

	private void Update()
	{
		fCurTime += Time.deltaTime;
		if (fCurTime > fAnimTime)
		{
			fCurTime = 0f;
			nTextureIndex = (nTextureIndex + 1) % mAnimTexture.Length;
			GetComponent<Renderer>().sharedMaterials[1].mainTexture = mAnimTexture[nTextureIndex];
		}
	}
}
