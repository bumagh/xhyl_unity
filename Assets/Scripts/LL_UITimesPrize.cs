using UnityEngine;

public class LL_UITimesPrize : MonoBehaviour
{
	public GameObject[] mAnimalSprite;

	private UISprite[] mAnimalSpriteIcon = new UISprite[12];

	private int mTotalAnimal;

	public static LL_UITimesPrize G_UITimesPrize;

	public static LL_UITimesPrize GetSingleton()
	{
		return G_UITimesPrize;
	}

	private void Awake()
	{
		if (G_UITimesPrize == null)
		{
			G_UITimesPrize = this;
		}
	}

	private void Start()
	{
		for (int i = 0; i < mAnimalSprite.Length; i++)
		{
			mAnimalSpriteIcon[i] = mAnimalSprite[i].GetComponent<UISprite>();
			mAnimalSpriteIcon[i].spriteName = "animalIcon0";
		}
		Reset();
	}

	private void Update()
	{
	}

	public void AddAnimal(int nTyp)
	{
		if (mTotalAnimal >= 6)
		{
			UnityEngine.Debug.Log("UITimesPrize AddAnimal nTyp exceeds 6!");
			return;
		}
		mAnimalSpriteIcon[mTotalAnimal].spriteName = "animalIcon" + nTyp.ToString();
		mAnimalSprite[mTotalAnimal].SetActiveRecursively(state: true);
		mTotalAnimal++;
	}

	public void Reset()
	{
		for (int i = 0; i < mAnimalSprite.Length; i++)
		{
			mAnimalSprite[i].SetActiveRecursively(state: false);
		}
		mTotalAnimal = 0;
	}
}
