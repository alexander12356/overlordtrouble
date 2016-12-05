using UnityEngine;

using System.Collections.Generic;

public enum AttackEffectType
{
    Instance,
    Animation
}

public class AttackEffectsSystem : MonoBehaviour
{
    private static AttackEffectsSystem m_Instance = null;

    private Queue<AttackEffect> m_AttackEffectQueue = new Queue<AttackEffect>();
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
        l_AttackEffect.type = AttackEffectType.Instance;
        l_AttackEffect.SetTarget(p_Target);
        l_AttackEffect.transform.SetParent(m_TargetForEffect.transform);
        l_AttackEffect.transform.localPosition = Vector3.zero;

        m_AttackEffectQueue.Enqueue(l_AttackEffect);
    }

    public void AddEffect(AttackEffect l_AttackEffect)
    {
        l_AttackEffect.type = AttackEffectType.Animation;
        m_AttackEffectQueue.Enqueue(l_AttackEffect);
    }

    public void PlayEffects()
    {
        if (m_AttackEffectQueue.Peek().type == AttackEffectType.Instance)
        {
            m_TargetForEffect.spriteRenderer.transform.SetParent(m_AttackEffectQueue.Peek().enemyRendererTransform);
        }
        m_AttackEffectQueue.Peek().PlayEffect();
    }

    public void EndAnimation()
    {
        PlayNextEffect();
    }

    public bool IsAllAnimationEnd()
    {
        return m_AttackEffectQueue.Count == 0;
    }

    private void PlayNextEffect()
    {
        AttackEffect l_AttackEffect = m_AttackEffectQueue.Dequeue();
        Destroy(l_AttackEffect.gameObject);

        if (m_AttackEffectQueue.Count == 0)
        {
            Reset();
            return;
        }

        PlayEffects();
    }

    private void Reset()
    {
        m_AttackEffectQueue.Clear();
        if (m_TargetForEffect != null)
        {
            m_TargetForEffect.spriteRenderer.transform.SetParent(m_TargetForEffect.transform);
        }
        m_TargetActor = null;
        m_TargetForEffect = null;
    }
}
