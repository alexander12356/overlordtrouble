using UnityEngine;

using System.Collections.Generic;

public class AttackEffectsSystem : MonoBehaviour
{
    private static AttackEffectsSystem m_Instance = null;

    private List<AttackEffect> m_AttackEffectList = new List<AttackEffect>();
    private int m_CurrentEffect = 0;
    private BattleActor m_TargetActor = null;
    private BattleActor m_TargetForEffect = null;

    public static AttackEffectsSystem GetInstance()
    {
        return m_Instance;
    }

    public void Awake()
    {
        m_Instance = this;
    }

    public void AddEffect(BattleActor p_Target, BattleActor p_TargetForEffect, string p_EffectPath)
    {
        m_TargetActor = p_Target;
        m_TargetForEffect = p_TargetForEffect;

        AttackEffect l_AttackEffectsPrefab = Resources.Load<AttackEffect>(p_EffectPath);//"Prefabs/BattleEffects/" + p_EffectPath);

        AttackEffect l_AttackEffect = Instantiate(l_AttackEffectsPrefab);
        l_AttackEffect.SetTarget(p_Target);
        l_AttackEffect.transform.SetParent(m_TargetForEffect.transform);
        l_AttackEffect.transform.localPosition = Vector3.zero;

        m_AttackEffectList.Add(l_AttackEffect);
    }

    public void PlayEffects()
    {
        m_TargetForEffect.spriteRenderer.transform.SetParent(m_AttackEffectList[m_CurrentEffect].enemyRendererTransform);
        m_AttackEffectList[m_CurrentEffect].PlayEffect();
    }

    public void EndAnimation()
    {
        PlayNextEffect();
    }

    public bool IsAllAnimationEnd()
    {
        return m_AttackEffectList.Count == 0;
    }

    private void PlayNextEffect()
    {
        m_CurrentEffect++;

        if (m_CurrentEffect > m_AttackEffectList.Count - 1)
        {
            Reset();
            return;
        }

        m_TargetForEffect.spriteRenderer.transform.SetParent(m_AttackEffectList[m_CurrentEffect].enemyRendererTransform);
        m_AttackEffectList[m_CurrentEffect].PlayEffect();
    }

    private void Reset()
    {
        m_CurrentEffect = 0;
        m_AttackEffectList.Clear();
        m_TargetForEffect.spriteRenderer.transform.SetParent(m_TargetForEffect.transform);
        m_TargetActor = null;
        m_TargetForEffect = null;
    }
}
