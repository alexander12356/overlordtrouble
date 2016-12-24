using UnityEngine;

public class PanelButtonEnemyChoise : PanelButton
{
    private static PanelButtonEnemyChoise m_Prefab = null;

    public static PanelButtonEnemyChoise prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<PanelButtonEnemyChoise>("Prefabs/Button/PanelButtonEnemyChoise");
            }
            return m_Prefab;
        }
    }
}
