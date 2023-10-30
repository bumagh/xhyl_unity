using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Language_image : MonoBehaviour
{
    public Sprite[] str;

    private void OnEnable()
    {
        transform.GetComponent<Image>().sprite = str[(int)ZH2_GVars.language_enum];
        transform.GetComponent<Image>().SetNativeSize();
    }
}
