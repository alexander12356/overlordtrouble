using System;

public class EffectSystem : Singleton<EffectSystem>
{
    public BaseEffect CreateEffect(EffectData p_EffectData)
    {
        BaseEffect l_Effect = null;

        switch (p_EffectData.type)
        {
            case EffectType.Attack:
                float l_AttackValue = Convert.ToSingle(p_EffectData.parameters[0]);
                Element l_Element = (Element)Enum.Parse(typeof(Element), p_EffectData.parameters[1]);

                l_Effect = new AttackEffect(l_AttackValue, l_Element);
                break;
        }

        return l_Effect;
    }
}
