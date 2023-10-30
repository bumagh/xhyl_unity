using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Language_text : MonoBehaviour
{
    public string[] str;

    private void OnEnable()
    {
        transform.GetComponent<Text>().text = str[(int)ZH2_GVars.language_enum];
    }
}
