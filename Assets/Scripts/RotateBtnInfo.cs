using UnityEngine;
using UnityEngine.UI;

public class RotateBtnInfo : MonoBehaviour
{
    public new string name = string.Empty;

    public string minGlod = string.Empty;

    public string onlinePeople = string.Empty;

    public int hallId;

    public int hallType = 1;

    public Text nameTextUp;

    public Text nameTextDown;

    public Text minGoldTextUp;

    public Text minGoldTextDown;

    public Text onlinePeopleTextUp;

    public Text onlinePeopleTextDown;

    public Text MinText;
    public Text MinText2;

    private void Awake()
    {
        MinText = transform.Find("Min").GetComponent<Text>();
        Destroy(MinText.GetComponent<Translation_Game>());
        MinText2 = transform.Find("Select/Min").GetComponent<Text>();
        Destroy(MinText2.GetComponent<Translation_Game>());
    }

    private void OnEnable()
    {
        MinText.text = ZH2_GVars.ShowTip("最少携带", "least", "การพกพาขั้นต่ำ", "Tối thiểu");
        MinText2.text = ZH2_GVars.ShowTip("最少携带", "least", "การพกพาขั้นต่ำ", "Tối thiểu");
    }

    private void Update()
    {
        nameTextUp.text = name;
        nameTextDown.text = name;
        minGoldTextUp.text = minGlod;
        minGoldTextDown.text = minGlod;
        onlinePeopleTextUp.text = onlinePeople;
        onlinePeopleTextDown.text = onlinePeople;
    }

    public void UpdateText()
    {
        nameTextUp.text = name;
        nameTextDown.text = name;
        minGoldTextUp.text = minGlod;
        minGoldTextDown.text = minGlod;
        onlinePeopleTextUp.text = onlinePeople;
        onlinePeopleTextDown.text = onlinePeople;
    }
}
