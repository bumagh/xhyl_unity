using DG.Tweening;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class SelectionManger : MonoBehaviour
{
    [Header("是否打开分类游戏")]
    public bool isShowSelectionPanel = false;

    private Transform m_xformGrid;

    public Color pitchOnColor = new Color(1f, 1f, 1f, 1f);

    public Color unselectedColor = new Color(0.47f, 0.47f, 0.47f, 1f);

    public List<Image> images;

    public List<Button> btns;

    public Material material;

    private void Awake()
    {
        ZH2_GVars.isShowSelectionPanel=isShowSelectionPanel;
        int index = PlayerPrefs.GetInt(ZH2_GVars.selectionNum, 0);
        m_xformGrid = base.transform.Find("InnerGames");
        images = new List<Image>();
        btns = new List<Button>();
        for (int i = 0; i < m_xformGrid.childCount; i++)
        {
            Image component = m_xformGrid.Find(i + "/selection").GetComponent<Image>();
            images.Add(component);
        }
        for (int j = 0; j < m_xformGrid.childCount; j++)
        {
            Button component2 = m_xformGrid.GetChild(j).GetComponent<Button>();
            btns.Add(component2);
        }
        for (int k = 0; k < btns.Count; k++)
        {
            int tempIndex = k;
            btns[k].onClick.AddListener(delegate
            {
                ChangeHaed(tempIndex);
            });
        }
        if (!isShowSelectionPanel)
        {
            base.transform.localScale = Vector3.zero;
            index = 0;
        }
        else
        {
            base.transform.localScale = Vector3.one;
        }
        ShowSelectionNum(index);
    }

    public void ChangeHaed(int index)
    {
        Debug.Log("按钮:" + index);
        PlayerPrefs.SetInt(ZH2_GVars.selectionNum, index);
        PlayerPrefs.Save();
        MB_Singleton<SoundManager>.Get().PlaySound(SoundType.Common_Click);
        ShowSelectionNum(index);
    }

    private void ShowSelectionNum(int index)
    {
        for (int i = 0; i < images.Count; i++)
        {
            if (images[i].color != unselectedColor)
            {
                images[i].DOColor(unselectedColor, 0.25f);
            }
            images[i].transform.parent.Find("Image").gameObject.SetActive(value: false);
            images[i].material = null;
        }
        images[index].color = pitchOnColor;
        images[index].material = material;
        images[index].transform.parent.Find("Image").gameObject.SetActive(value: true);
    }
}
