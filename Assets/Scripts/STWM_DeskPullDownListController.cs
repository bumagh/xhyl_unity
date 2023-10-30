using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STWM_DeskPullDownListController : MonoBehaviour
{
	public static STWM_DeskPullDownListController Instance;

	public Sprite[] arrListArrows;

	public Sprite[] arrImgButton;

	public bool bMoveFinished = true;

	private bool bListShow;

	private Image imgListArrow;

	private GameObject goDeskButton;

	private GameObject goScrollView;

	private RectTransform rtfScrollView;

	private RectTransform rtfContent;

	private List<GameObject> buttons = new List<GameObject>();

	private List<Image> imgBtns = new List<Image>();

	private List<Text> txtBtns = new List<Text>();

	private bool _isUnfold;

	private bool _btnListLock;

	private float width;

	private float height;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		imgListArrow = base.transform.Find("ImgArrow").GetComponent<Image>();
		goScrollView = base.transform.Find("ScrollView").gameObject;
		rtfScrollView = goScrollView.GetComponent<RectTransform>();
		Vector2 offsetMin = rtfScrollView.offsetMin;
		width = offsetMin.x;
		rtfContent = rtfScrollView.Find("Viewport/Content").GetComponent<RectTransform>();
		goDeskButton = rtfContent.Find("TableListItem").gameObject;
	}

	private void Start()
	{
		goScrollView.SetActive(value: false);
		_isUnfold = true;
	}

	public void OnListButtonDown()
	{
		if (!bMoveFinished)
		{
			return;
		}
		int curDeskIndex = STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().curDeskIndex;
		if (curDeskIndex < imgBtns.Count && curDeskIndex >= 0)
		{
			imgBtns[STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().curDeskIndex].sprite = arrImgButton[1];
		}
		STWM_SoundManager.Instance.PlayClickAudio();
		if (!_btnListLock)
		{
			_btnListLock = true;
			if (!bListShow)
			{
				goScrollView.SetActive(value: true);
				imgListArrow.sprite = arrListArrows[1];
				StartCoroutine(ListRollDown());
				bListShow = true;
			}
			else
			{
				imgListArrow.sprite = arrListArrows[0];
				StartCoroutine(ListRollUp());
				bListShow = false;
			}
		}
	}

	public void RollUpListByClickBG()
	{
		if (bListShow)
		{
			imgListArrow.sprite = arrListArrows[0];
		}
		StartCoroutine(ListRollUp());
		bListShow = false;
	}

	public void ChangeListHeightByDeskNum(int desknum, List<STWM_Desk> desklist)
	{
		goDeskButton.SetActive(value: false);
		if (buttons.Count > 0)
		{
			foreach (GameObject button in buttons)
			{
				UnityEngine.Object.Destroy(button);
			}
			buttons.Clear();
			imgBtns.Clear();
			txtBtns.Clear();
		}
		RectTransform rectTransform = rtfScrollView;
		Vector2 offsetMax = rtfScrollView.offsetMax;
		rectTransform.offsetMax = new Vector2(offsetMax.x, 22.5f);
		if (desknum <= 8)
		{
			height = -80f - (float)(desknum * 50);
		}
		else
		{
			height = -450f;
		}
		float d = (float)desknum * 55f;
		rtfContent.sizeDelta = Vector2.up * d;
		for (int i = 0; i < desknum; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(goDeskButton, Vector3.zero, Quaternion.identity);
			gameObject.transform.SetParent(rtfContent, worldPositionStays: false);
			gameObject.transform.localPosition = Vector3.right * 110f - Vector3.up * (27.5f + (float)(i * 55));
			gameObject.SetActive(value: true);
			int num = (desklist[i].userId != -1) ? 1 : 0;
			txtBtns.Add(gameObject.transform.Find("Text").GetComponent<Text>());
			txtBtns[i].text = desklist[i].name + " (" + num + "/" + 1 + ")";
			buttons.Add(gameObject);
			int curIndex = i;
			gameObject.GetComponent<Button>().onClick.AddListener(delegate
			{
				SelectDeskByButton(curIndex);
			});
			imgBtns.Add(gameObject.GetComponent<Image>());
		}
		ForceRollDown();
	}

	public void UpdateDeskInList(List<STWM_Desk> desks)
	{
		for (int i = 0; i < desks.Count; i++)
		{
			int num = (desks[i].userId != -1) ? 1 : 0;
			txtBtns[i].text = desks[i].name + " (" + num + "/" + 1 + ")";
		}
	}

	public void SelectDeskByButton(int index)
	{
		if (bMoveFinished)
		{
			if (index > STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().curDeskIndex)
			{
				STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().ForceMoveDesksToRight(index);
			}
			else if (index < STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().curDeskIndex)
			{
				STWM_MB_Singleton<STWM_LobbyViewController>.GetInstance().ForceMoveDesksToLeft(index);
			}
			for (int i = 0; i < buttons.Count; i++)
			{
				imgBtns[i].sprite = arrImgButton[(i == index) ? 1 : 0];
			}
		}
	}

	private IEnumerator ListRollDown()
	{
		_isUnfold = false;
		float timer = 0f;
		float duration = 0.3f;
		while (timer < duration)
		{
			timer += Time.deltaTime;
			float y = Mathf.Lerp(0f, 1f, timer / duration);
			rtfScrollView.offsetMin = Vector2.right * width + Vector2.up * y * (height + 80f) + Vector2.up * -80f;
			yield return null;
		}
		_btnListLock = false;
	}

	private IEnumerator ListRollUp()
	{
		_isUnfold = true;
		float timer = 0f;
		float duration = 0.3f;
		while (timer < duration)
		{
			timer += Time.deltaTime;
			Mathf.Lerp(1f, 0f, (duration - timer) / duration);
			rtfScrollView.offsetMin = Vector2.right * width + Vector2.up * (duration - timer) / duration * (height + 80f) + Vector2.up * -80f;
			yield return null;
		}
		if (duration - timer != 0f)
		{
			rtfScrollView.offsetMin = Vector2.right * width + Vector2.up * -80f;
		}
		goScrollView.SetActive(value: false);
		_btnListLock = false;
	}

	public void ForceRollDown()
	{
		goScrollView.gameObject.SetActive(value: false);
		imgListArrow.sprite = arrListArrows[0];
		bListShow = false;
	}

	public bool IsUnfold()
	{
		return _isUnfold;
	}

	public void ShowListButtonHighBG(bool _bIsLeft, int _icurIndex)
	{
		if (_bIsLeft && buttons.Count > 0)
		{
			if (_icurIndex > 0)
			{
				imgBtns[_icurIndex].sprite = arrImgButton[0];
				imgBtns[_icurIndex - 1].sprite = arrImgButton[1];
			}
			else if (_icurIndex == 0)
			{
				imgBtns[0].sprite = arrImgButton[0];
				imgBtns[buttons.Count - 1].sprite = arrImgButton[1];
			}
		}
		else if (!_bIsLeft && buttons.Count > 0)
		{
			if (_icurIndex < buttons.Count - 1)
			{
				imgBtns[_icurIndex].sprite = arrImgButton[0];
				imgBtns[_icurIndex + 1].sprite = arrImgButton[1];
			}
			else if (_icurIndex == buttons.Count - 1)
			{
				imgBtns[0].sprite = arrImgButton[1];
				imgBtns[buttons.Count - 1].sprite = arrImgButton[0];
			}
		}
	}
}
