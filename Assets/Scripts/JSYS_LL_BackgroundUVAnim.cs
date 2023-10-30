using UnityEngine;

public class JSYS_LL_BackgroundUVAnim : MonoBehaviour
{
	private float scrollSpeed = 0.5f;

	private int countX = 4;

	private int countY = 4;

	private float offsetX;

	private float offsetY;

	private Vector2 singleTexSize;

	private float offset;

	private void Start()
	{
		singleTexSize = new Vector2(1f / (float)countX, 1f / (float)countY);
	}

	private void Update()
	{
		offset -= Time.deltaTime * scrollSpeed;
		if (offset < 0f)
		{
			offset = 1f;
		}
		GetComponent<Renderer>().sharedMaterials[1].SetTextureOffset("_MainTex", new Vector2(offset, 0f));
	}
}
