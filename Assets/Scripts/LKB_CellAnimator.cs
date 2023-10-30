using UnityEngine;

public class LKB_CellAnimator : MonoBehaviour
{
	private STWM_SpiCell[] spiCells;

	private LKB_CellManager cSF_CellManager;

	public LKB_CellAnim Anim
	{
		get;
		set;
	}

	public LKB_ScrollAnim AnimScroll
	{
		get;
		set;
	}

	private void Awake()
	{
		Anim = base.transform.GetComponent<LKB_CellAnim>();
		AnimScroll = base.transform.Find("Scroll").GetComponent<LKB_ScrollAnim>();
		Init();
	}

	private void Init()
	{
		cSF_CellManager = base.transform.parent.GetComponent<LKB_CellManager>();
		spiCells = cSF_CellManager.spiCells;
	}

	public void Play(int index)
	{
		Init();
		if (spiCells.Length <= 0)
		{
			return;
		}
		if (index % 5 == 4)
		{
			AnimScroll.gameObject.SetActive(value: true);
			StartCoroutine(AnimScroll.AniStart2((index - 4) / 5 + 1));
		}
		else if (index % 5 == 3)
		{
			if (spiCells.Length > index)
			{
				Anim.Play(spiCells[index].spis, 3f, loop: false);
			}
			else
			{
				UnityEngine.Debug.LogError("索引超出数组长度: " + index);
			}
		}
		else if (index % 5 == 2)
		{
			if (spiCells.Length > index)
			{
				Anim.Play(spiCells[index].spis, 0.8f, loop: false);
			}
			else
			{
				UnityEngine.Debug.LogError("索引超出数组长度: " + index);
			}
		}
		else if (index % 5 == 1)
		{
			if (spiCells.Length > index)
			{
				Anim.Play(spiCells[index].spis, 3.5f, loop: false);
			}
			else
			{
				UnityEngine.Debug.LogError("索引超出数组长度: " + index);
			}
		}
		else if (spiCells.Length > index)
		{
			Anim.Play(spiCells[index].spis, 0.2f, loop: false);
		}
		else
		{
			UnityEngine.Debug.LogError("索引超出数组长度: " + index);
		}
	}

	public void Stop(int index)
	{
		Init();
		Anim.Play(spiCells[index / 5 * 5].spis, 0.2f, loop: false);
	}
}
