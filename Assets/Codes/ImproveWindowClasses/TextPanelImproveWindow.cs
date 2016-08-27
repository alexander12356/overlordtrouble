using UnityEngine;
using System.Collections;

public class TextPanelImproveWindow : TextPanel
{
    private static TextPanelImproveWindow m_Prefab;

    public static TextPanelImproveWindow prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<TextPanelImproveWindow>("Prefabs/Panels/TextPanelImproveWindow");
            }
            return m_Prefab;
        }
    }
}
