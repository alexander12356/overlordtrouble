using UnityEngine;

public class PanelButtonChosenSpecial : PanelButton
{
    private static PanelButtonChosenSpecial m_Prefab = null;

    public static PanelButtonChosenSpecial prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<PanelButtonChosenSpecial>("Prefabs/Button/PanelButtonChosenSpecial");
            }
            return m_Prefab;
        }
    }
}
