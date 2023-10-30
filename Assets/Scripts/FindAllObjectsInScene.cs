using System.Collections.Generic;
using UnityEngine;

public class FindAllObjectsInScene : MonoBehaviour
{
	public static List<Translation> list = new List<Translation>();

	public static void InitAllTL(Transform tf)
	{
		for (int i = 0; i < tf.childCount; i++)
		{
			Transform child = tf.GetChild(i);
			Translation component = child.GetComponent<Translation>();
			if (component != null)
			{
				child.GetComponent<Translation>().Init();
				list.Add(component);
			}
			if (child.childCount > 0)
			{
				InitAllTL(child);
			}
		}
	}

	public static void RefreshAllTxt()
	{
		for (int i = 0; i < list.Count; i++)
		{
			Translation translation = list[i];
			int language_enum = (int)ZH2_GVars.language_enum;
			translation.Refresh(language_enum.ToString());
		}
	}
}
