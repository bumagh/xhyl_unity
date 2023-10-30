using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Language_obj : MonoBehaviour
{
    public GameObject[] str;

    private void OnEnable()
    {
      for(int i = 0; i < str.Length; i++)
        {
            str[i].SetActive((int)ZH2_GVars.language_enum==i?true:false);
        }
    }
}
