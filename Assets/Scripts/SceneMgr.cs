using UnityEngine;

public class SceneMgr : SingletonBehaviour<SceneMgr>
{
	private Transform m_CurScene;

	private void Start()
	{
		LoadScene("Scene_bwx");
	}

	public void LoadScene(string strName)
	{
		if (m_CurScene != null)
		{
			Object.DestroyObject(m_CurScene.gameObject);
		}
		GameObject original = Resources.Load<GameObject>("Prefabs/scene/" + strName);
		m_CurScene = Object.Instantiate(original).transform;
		m_CurScene.SetParent(base.transform, worldPositionStays: false);
	}
}
