using M__M.HaiWang.Demo;
using UnityEngine;

public class TestSoundMgr : MonoBehaviour
{
	public void Start()
	{
		HW2_Singleton<SoundMgr>.Get().LoadSounds(LoadingLogic.Get().array);
	}

	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("Gun");
		}
		else if (Input.GetMouseButtonDown(1))
		{
			HW2_Singleton<SoundMgr>.Get().PlayClip("Powerup");
		}
	}

	public void OnDestroy()
	{
		HW2_Singleton<SoundMgr>.Get().UnloadAllSounds();
	}
}
