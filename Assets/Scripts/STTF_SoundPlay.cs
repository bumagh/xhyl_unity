using UnityEngine;

public class STTF_SoundPlay : MonoBehaviour
{
	public enum SOUND_TYPE
	{
		SOUND_BULLET
	}

	private AudioSource mAudio;

	private void Awake()
	{
		mAudio = base.gameObject.GetComponent<AudioSource>();
	}

	public void Play(AudioClip clip, bool isLoop = false, float volume = 1f)
	{
		if (clip == null)
		{
			UnityEngine.Debug.Log("@SoundPlay, clip null!");
			return;
		}
		mAudio.clip = clip;
		GetComponent<AudioSource>().loop = isLoop;
		GetComponent<AudioSource>().volume = volume;
		GetComponent<AudioSource>().Play();
	}

	private void Update()
	{
		if (!mAudio.isPlaying)
		{
			ObjDestroy();
		}
	}

	public void OnDespawned()
	{
		mAudio.Stop();
	}

	public void ObjDestroy()
	{
		mAudio.Stop();
		STTF_MusicMngr.GetSingleton().DestroySound(this);
	}
}
