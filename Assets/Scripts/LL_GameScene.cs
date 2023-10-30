using UnityEngine;

public class LL_GameScene : MonoBehaviour
{
	private void Awake()
	{
		string path = (LL_GameInfo.getInstance().Language == 0) ? "prefab/UI Root" : "prefab/UI Root_English";
		GameObject original = (GameObject)Resources.Load(path);
		GameObject gameObject = UnityEngine.Object.Instantiate(original);
	}
}
