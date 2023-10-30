using System.Collections;

using System.Collections.Generic;

using System.Runtime.InteropServices;

using UnityEditor;

using UnityEngine;

public class CalAnchorsEditor : EditorWindow

{

    const string TitleName = "UI设置/设置锚点适应自身";

    /// <summary>

    /// 是否设置子节点

    /// </summary>

  static  bool IsSetChildTransform = false;

    [MenuItem(TitleName)]

    private static void InitWindow()

    {

        Rect wr = new Rect(0, 0, 500, 500);

        var windows = EditorWindow.GetWindowWithRect(typeof(CalAnchorsEditor), wr, true, "设置锚点适应自身");

        windows.Show();

    }

    [MenuItem("UI设置/锚点适应 %#E")]
    static void OnCalAnchors()
    {
        SetSelectGameobjectItemAnchors();
    }

   

    void OnGUI()

    {

        IsSetChildTransform = GUILayout.Toggle(IsSetChildTransform, "是否设置子节点", GUILayout.Width(200));

        EditorGUILayout.LabelField("选择的物体数量", Selection.gameObjects.Length.ToString());

        if (GUILayout.Button("保存", GUILayout.Width(200)))

        {

            SetSelectGameobjectItemAnchors();

        }

    }

    /// <summary>

    /// 设置锚点

    /// </summary>

    private static void SetSelectGameobjectItemAnchors()

    {

        var uiItems = Selection.gameObjects;

        for (int i = 0; i < uiItems.Length; i++)

        {

            var item = uiItems[i];

            var rect = item.GetComponent<RectTransform>();

            CalAnchors(rect);

            if (IsSetChildTransform)

            {

                var rects = item.GetComponentsInChildren<RectTransform>();

                for (int j = 0; j < rects.Length; j++)

                {

                    var child = rects[j];

                    CalAnchors(child);

                }

            }

        }

    }

    /// <summary>

    /// 计算锚点

    /// 计算自身相对于父物体的锚点（也就是自身四个顶点所在位置相对于父物体自身大小的百分比）

    /// </summary>

    private static void CalAnchors(Transform transform)

    {

        var parent = transform.parent;

        RectTransform parentRect = null;

        if (parent)

        {

            parentRect = parent.GetComponent<RectTransform>();

        }

        RectTransform transformRect = transform.GetComponent<RectTransform>();

        Vector2 pivot = transformRect.pivot;

        if (parentRect)

        {

            float parentWidth = parentRect.rect.width;

            float parentHeight = parentRect.rect.height;

            float selfWidth = transformRect.rect.width;

            float selfHeight = transformRect.rect.height;

            float x = transformRect.localPosition.x;

            float y = transformRect.localPosition.y;

            float anchorsMinx = (parentWidth / 2 + x - (selfWidth * pivot.x)) / parentWidth;//

            float anchorsMaxx = (parentWidth / 2 + x + (selfWidth * (1 - pivot.x))) / parentWidth;

            float anchorsMiny = (parentHeight / 2 + y - (selfHeight * pivot.y)) / parentHeight;

            float anchorsMaxy = (parentHeight / 2 + y + (selfHeight * (1 - pivot.y))) / parentHeight;

            transformRect.anchorMin = new Vector2(anchorsMinx, anchorsMiny);

            transformRect.anchorMax = new Vector2(anchorsMaxx, anchorsMaxy);

            transformRect.offsetMin = Vector2.zero;

            transformRect.offsetMax = Vector2.zero;

        }

    }

}