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
        m_ResultTextPanel.AddButtonAction(EndResult);
        m_TextPanelsQueue.Enqueue(m_ResultTextPanel);

        BattleSystem.GetInstance().ShowPanel(m_TextPanelsQueue.Dequeue());
    }

    public void AddTextPanel(TextPanel p_TextPanel)
    {
        p_TextPanel.AddButtonAction(p_TextPanel.Close);
        p_TextPanel.AddPopAction(ShowNextPanel);

        m_TextPanelsQueue.Enqueue(p_TextPanel);
    }

    private void ShowNextPanel()
    {
        BattleSystem.GetInstance().ShowPanel(m_TextPanelsQueue.Dequeue());
    }

    private void EndResult()
    {
        if (AttackEffectsSystem.GetInstance().IsAllAnimationEnd())
        {
            DamageSystem.GetInstance().Reset();
            m_DamageStatistic.target.CheckDeath();
            m_ResultTextPanel.Close();
            BattleSystem.GetInstance().EndTurn();
        }
    }
}
