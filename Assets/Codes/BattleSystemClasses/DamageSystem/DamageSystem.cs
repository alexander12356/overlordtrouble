using System.Collections.Generic;

using UnityEngine;

public class DamageSystem : Singleton<DamageSystem>
{
    private enum AttackType
    {
        BaseAttack,
        SpecialAttack
    }

    private BattleActor m_Target = null;
    private BattleActor m_Sender = null;
    private string m_AttackNames = string.Empty;
    private List<string> m_ResultText = new List<string>();
    private float m_DamageValue = 0.0f;
    private Queue<TextPanel> m_TextPanelsQueue = new Queue<TextPanel>();
    private TextPanel m_LastAddedPanel = null;
    private AttackType m_AttackType = AttackType.BaseAttack;
    private bool m_IsBadAttack = false;

    public void Attack(BattleActor p_Sender, BattleActor p_Target, float p_DamageValue)
    {
        m_Sender = p_Sender;
        m_Target = p_Target;
        m_DamageValue = p_DamageValue;
        m_AttackType = AttackType.BaseAttack;

        p_Target.Damage(p_DamageValue);
    }

    public void SpecialAttack(BattleActor p_Sender, BattleActor p_Target, float p_DamageValue, bool p_IsBadAttack, string p_AttackNames = "")
    {
        m_Sender = p_Sender;
        m_Target = p_Target;
        m_DamageValue = p_DamageValue;
        m_AttackNames = p_AttackNames;
        m_IsBadAttack = p_IsBadAttack;
        m_AttackType = AttackType.SpecialAttack;

        p_Target.Damage(p_DamageValue);
    }

    public void AttackSuccess()
    {
        string l_SenderName = m_Sender.actorName;
        string l_TargetName = m_Target.actorName;

        string l_StatisticText = string.Empty;

        switch (m_AttackType)
        {
            case AttackType.BaseAttack:
                l_StatisticText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:PlayerAttack", new string[] { l_SenderName, l_TargetName, m_DamageValue.ToString() });
                break;
            case AttackType.SpecialAttack:
                if (m_IsBadAttack)
                {
                    l_StatisticText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:BadAttack", new string[] { m_DamageValue.ToString(), l_TargetName });
                }
                else
                {
                    l_StatisticText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:SpecialAttack", new string[] { l_SenderName, l_TargetName, m_AttackNames, m_DamageValue.ToString() });
                }
                break;
        }

        m_ResultText.Add(l_StatisticText);

        //TextPanel l_TextPanel = Object.Instantiate(TextPanel.prefab);
        //l_TextPanel.SetText(m_ResultText);
        //m_TextPanelsQueue.Enqueue(l_TextPanel);
        //m_LastAddedPanel = l_TextPanel;
    }

    public void AttackFail()
    {
        m_ResultText.Add(LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:AttackFail"));

        //TextPanel l_TextPanel = Object.Instantiate(TextPanel.prefab);
        //l_TextPanel.SetText(new List<string>() { l_FailText });
        //m_TextPanelsQueue.Enqueue(l_TextPanel);
        //m_LastAddedPanel = l_TextPanel;
    }

    public DamageStatistic GetStatistics()
    {
        DamageStatistic l_DamageStatistic = new DamageStatistic(m_Target, m_ResultText);
        return l_DamageStatistic;
    }

    //public void AddTextPanel(TextPanel p_TextPanel)
    //{
    //    m_TextPanelsQueue.Peek().AddButtonAction(m_TextPanelsQueue.Peek().Close);
    //    m_TextPanelsQueue.Peek().AddPopAction(ShowNextPanel);

    //    m_LastAddedPanel = p_TextPanel;
    //    m_TextPanelsQueue.Enqueue(m_LastAddedPanel);
    //}

    //public void ShowResult()
    //{
    //    m_LastAddedPanel.AddButtonAction(CloseTextPanel);
    //    BattleSystem.GetInstance().ShowPanel(m_TextPanelsQueue.Dequeue());
    //    BattleSystem.GetInstance().SetVisibleAvatarPanel(false);
    //}

    //private void ShowNextPanel()
    //{
    //    BattleSystem.GetInstance().ShowPanel(m_TextPanelsQueue.Dequeue());
    //}

    //private void CloseTextPanel()
    //{
    //    if (AttackEffectsSystem.GetInstance().IsAllAnimationEnd())
    //    {
    //        m_Target.CheckDeath();
    //        m_LastAddedPanel.Close();
    //        BattleSystem.GetInstance().EndTurn();
    //        Reset();
    //    }
    //}

    public void Reset()
    {
        m_TextPanelsQueue.Clear();
        m_LastAddedPanel = null;
        m_ResultText.Clear();
        m_AttackNames = "";
    }
}
