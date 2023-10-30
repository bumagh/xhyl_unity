using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class STOF_ErrorTipAnim : MonoBehaviour
{
	public Image img;

	public float delay = 1f;

	public bool bError = true;

	public void PlayTipAnim()
	{
		if (bError)
		{
			STOF_SoundManage.getInstance().playButtonMusic(STOF_ButtonMusicType.error);
		}
		img.color = Color.white;
		img.DOKill();
		img.DOFade(0f, 1f).SetDelay(delay);
	}
}
