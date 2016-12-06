using UnityEngine;

public class LeshiiAttackEffect : AttackEffect
{
    private static LeshiiAttackEffect m_Prefab = null;
    private PanelActionHandler m_PlayAction = null;

    public static LeshiiAttackEffect prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<LeshiiAttackEffect>("Prefabs/Bosses/Leshii/LeshiiAttackEffect");
            }
            return m_Prefab;
        }
    }

    public override void PlayEffect()
    {
        if (m_PlayAction != null)
        {
            m_PlayAction();
            m_PlayAction = null;
        }
    }

    public void AddPlayAction(PanelActionHandler p_Action)
    {
        m_PlayAction += p_Action;
    }
}
