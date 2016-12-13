using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayManyEffectStep : BattleBaseStep
{
    private List<VisualEffect> m_EffectList;
    private int m_EndEffectCounter = 0;

    public BattlePlayManyEffectStep(List<VisualEffect> p_EffectList)
    {
        m_EffectList = p_EffectList;
    }

    public override void RunStep()
    {
        base.RunStep();
        
        for (int i = 0; i < m_EffectList.Count; i++)
        {
            m_EffectList[i].PlayEffect();
            m_EffectList[i].AddEndAnimationAction(CheckNextStep);
        }
    }

    public void CheckNextStep()
    {
        m_EndEffectCounter++;

        if (m_EndEffectCounter == m_EffectList.Count)
        {
            ResultSystem.GetInstance().NextStep();
        }
    }
}
