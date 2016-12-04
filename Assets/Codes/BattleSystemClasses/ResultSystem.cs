using System.Collections.Generic;

using UnityEngine;

public class ResultSystem : Singleton<ResultSystem>
{
    private Queue<TextPanel> m_TextPanelsQueue = new Queue<TextPanel>();
    private TextPanel l_LastAddedTextPanel = null;

    public void ShowResult()
    {
        l_LastAddedTextPanel.RemovePopAction(ShowNextPanel);
        l_LastAddedTextPanel.AddButtonAction(EndResult);
        l_LastAddedTextPanel.RemoveButtonAction(l_LastAddedTextPanel.Close);

        ShowNextPanel();
    }

    public void AddTextPanel(TextPanel p_TextPanel)
    {
        l_LastAddedTextPanel = p_TextPanel;
        l_LastAddedTextPanel.AddPopAction(ShowNextPanel);

        m_TextPanelsQueue.Enqueue(l_LastAddedTextPanel);
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

        l_LastAddedTextPanel.Close();
        DamageSystem.GetInstance().Reset();
        BattleSystem.GetInstance().EndTurn();
    }
}
