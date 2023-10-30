using UnityEngine;

public class STWM_CellAnimator : MonoBehaviour
{
	private STWM_CellAnim anim;

	[SerializeField]
	private STWM_SpiRoll spiRoll;

	[SerializeField]
	private STWM_SpiCell[] spiCells;

	public STWM_ScrollAnim animScroll;

	[HideInInspector]
	public int curType;

	private void GetCellAnim()
	{
		if (anim == null)
		{
			anim = GetComponent<STWM_CellAnim>();
		}
		if (animScroll == null)
		{
			animScroll = base.transform.GetChild(0).GetComponent<STWM_ScrollAnim>();
		}
	}

	public void Play(int index)
	{
		GetCellAnim();
		curType = index / 5;
		if (index % 5 == 4)
		{
			animScroll.gameObject.SetActive(value: true);
			StartCoroutine(animScroll.AniStart2((index - 4) / 5 + 1));
		}
		else if (index % 5 == 3)
		{
			anim.Play(spiCells[index].spis, 3f, loop: false);
		}
		else if (index % 5 == 2)
		{
			anim.Play(spiCells[index].spis, 0.8f, loop: false);
		}
		else if (index % 5 == 1)
		{
			anim.Play(spiCells[index].spis, 3.5f, loop: false);
		}
		else
		{
			anim.Play(spiCells[index].spis, 0.2f, loop: false);
		}
	}

	public void Stop(int index)
	{
		GetCellAnim();
		anim.Play(spiCells[index / 5 * 5].spis, 0.2f, loop: false);
	}
}
