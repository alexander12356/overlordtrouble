using System.Collections.Generic;

using UnityEngine;

public class ResultSystem : Singleton<ResultSystem>
{
    private Queue<TextPanel> m_TextPanelsQueue = new Queue<TextPanel>();
    private DamageStatistic m_DamageStatistic;
    private TextPanel m_ResultTextPanel = null;

    public void ShowResult()
    {
        m_DamageStatistic = DamageSystem.GetInstance().GetStatistics();

        m_ResultTextPanel = Object.Instantiate(TextPanel.prefab);
        m_ResultTextPanel.SetText(m_DamageStatistic.resultText);
        m_ResultTextPanel.AddButtonAction(CloseResult);

        BattleSystem.GetInstance().ShowPanel(m_ResultTextPanel);
    }

    private void CloseResult()
    {
        if (AttackEffectsSystem.GetInstance().IsAllAnimationEnd())
        {
            DamageSystem.GetInstance().Reset();
            m_ResultTextPanel.Close();
            m_DamageStatistic.target.CheckDeath();
            BattleSystem.GetInstance().EndTurn();
        }
    }
}
