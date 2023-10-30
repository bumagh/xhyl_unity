using M__M.HaiWang.Demo;
using UnityEngine;

public class FK3_TestSoundMgr : MonoBehaviour
{
	public void Start()
	{
		FK3_Singleton<FK3_SoundMgr>.Get().LoadSounds(FK3_LoadingLogic.Get().array);
	}

	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("Gun");
		}
		else if (Input.GetMouseButtonDown(1))
		{
			FK3_Singleton<FK3_SoundMgr>.Get().PlayClip("Powerup");
		}
	}

	public void OnDestroy()
	{
		FK3_Singleton<FK3_SoundMgr>.Get().UnloadAllSounds();
	}
}
