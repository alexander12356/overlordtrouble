using System;

public class EffectSystem : Singleton<EffectSystem>
{
    public BaseEffect CreateEffect(Special p_Special, EffectData p_EffectData)
    {
        BaseEffect l_Effect = null;

        switch (p_EffectData.type)
        {
            case EffectType.Attack:
                float l_AttackValue = Convert.ToSingle(p_EffectData.parameters[0]);

                l_Effect = new AttackEffect(p_Special, l_AttackValue);
                break;
            case EffectType.Defense:

                float l_DefenseValue = Convert.ToSingle(p_EffectData.parameters[0]);
                int l_Duration = Convert.ToInt32(p_EffectData.parameters[1]);

                l_Effect = new DefenseEffect(p_Special, l_DefenseValue, l_Duration);
                break;
        }

        return l_Effect;
    }
}
