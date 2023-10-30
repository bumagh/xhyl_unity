using UnityEngine;

public class BCBM_GameScene : MonoBehaviour
{
	private void Awake()
	{
		string path = (BCBM_GameInfo.getInstance().Language == 0) ? "prefab/UI Root" : "prefab/UI Root_English";
		GameObject original = (GameObject)Resources.Load(path);
		GameObject gameObject = UnityEngine.Object.Instantiate(original);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
