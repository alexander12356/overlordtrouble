using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstyleSystem : Singleton<MonstyleSystem>
{
    private string m_UsesSpecialNames = string.Empty;

    public string usesSpecialNames
    {
        get { return m_UsesSpecialNames; }
    }
    
    public void UsingMonstyle(BattleActor p_Sender, BattleActor p_Target, List<Special> p_SpecialList)
    {
        m_UsesSpecialNames = GenerateUsesSpecialNames(p_SpecialList);
        
        for (int i = 0; i < p_SpecialList.Count; i++)
        {
            if (p_SpecialList[i].isAoe)
            {
                RunAoeSpecial(p_Sender, p_Target, p_SpecialList[i]);
            }
            else
            {
                RunSpecial(p_Sender, p_Target, p_SpecialList[i]);
            }
        }
    }
    
    private void RunAoeSpecial(BattleActor p_Sender, BattleActor p_Target, Special p_Special)
    {
        List<VisualEffect> l_VisualEffectList = new List<VisualEffect>();
        List<BattleEnemy> l_EnemyList = BattleSystem.GetInstance().GetEnemyList();
        for (int i = 0; i < l_EnemyList.Count; i++)
        {
            string l_PrefabPath = "Prefabs/BattleEffects/Monstyle/" + p_Special.id + "Monstyle";

            VisualEffect l_AttackEffect = Object.Instantiate(Resources.Load<VisualEffect>(l_PrefabPath));
            l_AttackEffect.Init(l_EnemyList[i], l_EnemyList[i].rendererTransform);
            l_VisualEffectList.Add(l_AttackEffect);
            
            p_Special.Run(p_Sender, l_EnemyList[i]);

            if (p_Target != l_EnemyList[i])
            {
                DamageSystem.GetInstance().AddAoeSpecial(l_EnemyList[i], p_Special);
            }
        }

        BattlePlayManyEffectStep l_Step = new BattlePlayManyEffectStep(l_VisualEffectList);
        DamageSystem.GetInstance().AddVisualEffectStep(l_Step);
    }
    
    private void RunSpecial(BattleActor p_Sender, BattleActor p_Target, Special p_Special)
    {
        string l_PrefabPath = "Prefabs/BattleEffects/Monstyle/" + p_Special.id + "Monstyle";

        VisualEffect l_Prefab = Resources.Load<VisualEffect>(l_PrefabPath);

        if (l_Prefab != null)
        {
            VisualEffect l_AttackEffect = Object.Instantiate(l_Prefab);
            l_AttackEffect.Init(p_Target, p_Target.rendererTransform);

            BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_AttackEffect);
            DamageSystem.GetInstance().AddVisualEffectStep(l_Step);
        }
        
        p_Special.Run(p_Sender, p_Target);
    }
    
    private string GenerateUsesSpecialNames(List<Special> m_SpecialList)
    {
        string l_Text = string.Empty;

        for (int i = 0; i < m_SpecialList.Count; i++)
        {
            string l_SpecialName = LocalizationDataBase.GetInstance().GetText("Special:" + m_SpecialList[i].id);
            l_Text += l_SpecialName;
            if (i == m_SpecialList.Count - 2)
            {
                l_Text += " " + LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:And") + " ";
            }
            else if (i == m_SpecialList.Count - 1)
            {
                l_Text += "";
            }
            else
            {
                l_Text += ", ";
            }
        }

        return l_Text;
    }
}
