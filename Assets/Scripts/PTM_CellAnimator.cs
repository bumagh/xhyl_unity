using UnityEngine;

public class PTM_CellAnimator : MonoBehaviour
{
	private STWM_SpiCell[] spiCells;

	private PTM_CellManager cSF_CellManager;

	public PTM_CellAnim Anim
	{
		get;
		set;
	}

	public PTM_ScrollAnim AnimScroll
	{
		get;
		set;
	}

	private void Awake()
	{
		Anim = base.transform.GetComponent<PTM_CellAnim>();
		AnimScroll = base.transform.Find("Scroll").GetComponent<PTM_ScrollAnim>();
		if (cSF_CellManager == null)
		{
			cSF_CellManager = base.transform.parent.GetComponent<PTM_CellManager>();
		}
		spiCells = cSF_CellManager.spiCells;
	}

	public void Play(int index)
	{
		if (index % 5 == 4)
		{
			AnimScroll.gameObject.SetActive(value: true);
			StartCoroutine(AnimScroll.AniStart2((index - 4) / 5 + 1));
		}
		else if (index % 5 == 3)
		{
			Anim.Play(spiCells[index].spis, 3f, loop: false);
		}
		else if (index % 5 == 2)
		{
			Anim.Play(spiCells[index].spis, 0.8f, loop: false);
		}
		else if (index % 5 == 1)
		{
			if (spiCells.Length > index)
			{
				Anim.Play(spiCells[index].spis, 3.5f, loop: false);
			}
			else
			{
				UnityEngine.Debug.LogError("索引超出数组长度");
			}
		}
		else
		{
			Anim.Play(spiCells[index].spis, 0.2f, loop: false);
		}
	}

	public void Stop(int index)
	{
		Anim.Play(spiCells[index / 5 * 5].spis, 0.2f, loop: false);
	}
}
