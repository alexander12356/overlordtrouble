using UnityEngine;
using UnityEngine.UI;

public class ItemActionButton : PanelButton
{
    private static ItemActionButton m_Prefab = null;

    public static ItemActionButton prefab
    {
        get
        {
            if(m_Prefab == null)
            {
                m_Prefab = Resources.Load<ItemActionButton>("Prefabs/Button/ItemActionButton");
            }
            return m_Prefab;
        }
    }
}
