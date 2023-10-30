using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TF_ErrorTipAnim : MonoBehaviour
{
	public Image img;

	public float delay = 1f;

	public bool bError = true;

	public void PlayTipAnim()
	{
		if (bError)
		{
			TF_SoundManage.getInstance().playButtonMusic(TF_ButtonMusicType.error);
		}
		img.color = Color.white;
		img.DOKill();
		img.DOFade(0f, 1f).SetDelay(delay);
	}
}
