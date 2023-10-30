using UnityEngine;

public class FK3_SafeAreaFitter : MonoBehaviour
{
	[Header("是否拉伸")]
	public bool drag;

	[Header("偏移量")]
	public float offset;

	[Header("是否为横屏")]
	public bool isHeng = true;

	[Header("刘海大小")]
	public float bangSize;

	private void OnEnable()
	{
	}
}
