using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LanguagePanel : MonoBehaviour
{
    public Button CH;
    public Button EN;
    public Button VN;

    public GameObject btnShow;
    private GameObject Panel;
    private Button Btn;
    private RectTransform rect;

    private void Awake()
    {
        CH.onClick.AddListener(OnButtonCH);
        EN.onClick.AddListener(OnButtonEN);
        VN.onClick.AddListener(OnButtonVN);

        Panel = transform.Find("Panel").gameObject;
        Btn = transform.Find("Btn").GetComponent<Button>();
        Btn.onClick.AddListener(OnBtn);
        rect=Btn.transform.Find("Down/A").GetComponent<RectTransform>();

        transform.Find("Close").GetComponent<Button>().onClick.AddListener(() => {
            MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
            transform.gameObject.SetActive(false);
        });
    }


    private void OnEnable()
    {
        rect.localScale = new Vector3(1, 1, 1);
        Panel.SetActive(false);
    }

    void OnBtn()
    {
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        if (Panel.activeSelf)
        {
            //收回去
            rect.localScale = new Vector3(1, 1, 1);
            Panel.SetActive(false);
        }
        else
        {
            //弹出来
            rect.localScale = new Vector3(1, -1, 1);
            Panel.SetActive(true);
        }
    }

    private void OnButtonCH()
    {
        btnShow.gameObject.SetActive(false);
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
           ZH2_GVars.language_enum = ZH2_GVars.LanguageType.Chinese;
        SwitchLang(ZH2_GVars.language_enum);
    }

    private void OnButtonEN()
    {
        btnShow.gameObject.SetActive(false);
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        ZH2_GVars.language_enum = ZH2_GVars.LanguageType.English;
        SwitchLang(ZH2_GVars.language_enum);
    }
    private void OnButtonVN()
    {
        btnShow.gameObject.SetActive(false);
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        ZH2_GVars.language_enum = ZH2_GVars.LanguageType.Vietnam;
        SwitchLang(ZH2_GVars.language_enum);
    }

    private void SwitchLang(ZH2_GVars.LanguageType lang)
    {
        btnShow.gameObject.SetActive(true);
        Debug.LogError("当前语言: " + ZH2_GVars.language_enum);
        PlayerPrefs.SetInt("GVarsLanguage", (int)ZH2_GVars.language_enum);
        ZH2_GVars.language_enum = lang;
        FindAllObjectsInScene.RefreshAllTxt();
        transform.gameObject.SetActive(false);
    }
}
