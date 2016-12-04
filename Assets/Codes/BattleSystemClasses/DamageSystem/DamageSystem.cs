using System.Collections.Generic;

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
    }

    public void AttackFail()
    {
        m_ResultText.Add(LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:AttackFail"));
    }

    public DamageStatistic GetStatistics()
    {
        DamageStatistic l_DamageStatistic = new DamageStatistic(m_Target, m_ResultText);
        return l_DamageStatistic;
    }

    public void Reset()
    {
        m_ResultText.Clear();
        m_AttackNames = "";
    }
}
