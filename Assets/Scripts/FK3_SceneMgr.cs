using UnityEngine;

public class FK3_SceneMgr : FK3_SingletonBehaviour<FK3_SceneMgr>
{
	private Transform m_CurScene;

	private void Start()
	{
		LoadScene("FK3_Scene_bwx");
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
