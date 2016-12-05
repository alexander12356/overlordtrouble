using System.Collections.Generic;

using UnityEngine;

public class ResultSystem : Singleton<ResultSystem>
{
    private Queue<TextPanel> m_TextPanelsQueue = new Queue<TextPanel>();
    private TextPanel m_LastAddedTextPanel = null;

    public void ShowResult()
    {
        if (m_TextPanelsQueue.Count == 0)
        {
            EndResult();
            return;
        }

        m_LastAddedTextPanel.RemovePopAction(ShowNextPanel);
        m_LastAddedTextPanel.AddButtonAction(EndResult);
        m_LastAddedTextPanel.RemoveButtonAction(m_LastAddedTextPanel.Close);

        ShowNextPanel();
    }

    public void AddTextPanel(TextPanel p_TextPanel)
    {
        m_LastAddedTextPanel = p_TextPanel;
        m_LastAddedTextPanel.AddPopAction(ShowNextPanel);

        m_TextPanelsQueue.Enqueue(m_LastAddedTextPanel);
    }

    private void ShowNextPanel()
    {
        TextPanel l_TextPanel = m_TextPanelsQueue.Dequeue();
        BattleSystem.GetInstance().ShowPanel(l_TextPanel);
    }

    private void EndResult()
    {
        if (!AttackEffectsSystem.GetInstance().IsAllAnimationEnd())
        {
            return;
        }

        if (m_LastAddedTextPanel != null)
        {
            m_LastAddedTextPanel.Close();
        }
        m_LastAddedTextPanel = null;
        m_TextPanelsQueue.Clear();
        DamageSystem.GetInstance().Reset();
        TurnSystem.GetInstance().EndTurn();
    }
}
