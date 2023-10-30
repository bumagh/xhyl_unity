using UnityEngine;

public class JSYS_Alone : MonoBehaviour
{
	public enum Category : byte
	{
		飞禽,
		走兽,
		鲨鱼
	}

	[Header("种类枚举")]
	public Category category = Category.走兽;

	private void Start()
	{
	}

	private void Update()
	{
	}
}
