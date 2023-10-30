using UnityEngine;
using UnityEngine.UI;

public class STTF_FishAnimCtrl : MonoBehaviour
{
	[SerializeField]
	private Sprite[] spiSwim;

	[SerializeField]
	private Sprite[] spiDie;

	private Image img;

	private int lenSwim;

	private int lenDie;

	private int index;

	[SerializeField]
	private float intervelSwim;

	[SerializeField]
	private float intervelDie;

	private float fIntervel;

	public bool bDead;

	private float time;

	private void Awake()
	{
		img = GetComponent<Image>();
		lenSwim = spiSwim.Length;
		lenDie = spiDie.Length;
		fIntervel = intervelSwim;
	}

	private void Update()
	{
		time += Time.deltaTime;
		if (time >= fIntervel)
		{
			index++;
			time = 0f;
			if (!bDead)
			{
				index %= lenSwim;
				img.sprite = spiSwim[index];
			}
			else
			{
				index %= lenDie;
				img.sprite = spiDie[index];
			}
			img.SetNativeSize();
		}
	}

	public void Play(bool dead)
	{
		index = 0;
		bDead = dead;
		fIntervel = (bDead ? intervelDie : intervelSwim);
	}
}
