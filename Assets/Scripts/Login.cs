using UnityEngine;

public class Login : MonoBehaviour
{
	private void Awake()
	{
		if (AssetBundleManager.GetInstance() == null)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "AssetBundleManager";
			gameObject.AddComponent<AssetBundleManager>();
		}
		if (!(All_TipCanvas.GetInstance() == null))
		{
		}
	}

	public void Test()
	{
		OpenWeb.intance.OnOpenWebUrl("https://www.cali7777.net");
	}
}
