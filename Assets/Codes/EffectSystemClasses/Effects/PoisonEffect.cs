using System.Collections.Generic;

public class PoisonEffect : BaseEffect
{
    private int m_Chanse = 0;
    private float m_DamageValue = 0;
    private int m_Duration = 0;
    private int m_DurationCounter = 0;
    private BattleActor m_Target = null;
    private System.Random m_Random = new System.Random();

    public PoisonEffect(Special p_Special, int p_Chanse, int p_Duration) : base(p_Special)
    {
        id = "Poison";
        m_Chanse = p_Chanse;
        m_Duration = p_Duration;
    }

    public override void Run(IEffectInfluenced p_Sender, IEffectInfluenced p_Target)
    {
        base.Run(p_Sender, p_Target);

        if (m_Random.Next(0, 100) > m_Chanse)
        {
            return;
        }

        m_Target = p_Target as BattleActor;

        if (m_Target.HasSpecial(m_Special.id))
        {
            m_Target.StackEffect(m_Special.id, this);
        }
        else
        {
            m_Target.AddEffect(m_Special.id, this);
            m_DamageValue = m_Target.baseHealth * 7.0f / 100.0f;
        }

        DamageSystem.GetInstance().AddEffectSpecial(m_Target, m_Special);
    }

    public override void Upgrade()
    {
        base.Upgrade();
    }

    public override void Effective()
    {
        base.Effective();

        int l_IntDamageValue = System.Convert.ToInt32(System.Math.Ceiling(m_DamageValue));
        m_Target.health -= l_IntDamageValue;
        
        ShowPoisonedText(l_IntDamageValue);

        m_DurationCounter++;
    }

    public override bool CheckEnd()
    {
        if (m_Duration > m_DurationCounter)
        {
            return false;
        }

        EffectSystem.GetInstance().AddRemoveEffectSpecial(m_Target, m_Special);

        return true;
    }

    public override void Stack(BaseEffect p_Effect)
    {
        base.Stack(p_Effect);

        m_DurationCounter = 0;
    }

    private void ShowPoisonedText(int l_DamageValue)
    {
        string l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:DamageFromEffect", new string[] { m_Target.actorName, l_DamageValue.ToString(), m_Special.specialName });

        TextPanel l_TextPanel = UnityEngine.Object.Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string> { l_Text });
        l_TextPanel.AddButtonAction(l_TextPanel.Close);

        BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
        ResultSystem.GetInstance().AddStep(l_Step);
    }
}
