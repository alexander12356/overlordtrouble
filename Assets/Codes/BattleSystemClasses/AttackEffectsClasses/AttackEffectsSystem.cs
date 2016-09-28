using UnityEngine;

using System.Collections.Generic;

public class AttackEffectsSystem : MonoBehaviour
{
    private static AttackEffectsSystem m_Instance = null;

    private List<AttackEffect> m_AttackEffectList = new List<AttackEffect>();
    private int m_CurrentEffect = 0;
    private BattleEnemy m_BattleEnemy = null;

    public static AttackEffectsSystem GetInstance()
    {
        return m_Instance;
    }

    public void Awake()
    {
        m_Instance = this;
    }

    public void AddEffect(BattleEnemy p_Target, string p_EffectId)
    {
        m_BattleEnemy = p_Target;

        AttackEffect l_AttackEffectsPrefab = Resources.Load<AttackEffect>("Prefabs/BattleEffects/" + p_EffectId);

        AttackEffect l_AttackEffect = Instantiate(l_AttackEffectsPrefab);
        l_AttackEffect.SetData(p_EffectId, p_Target);
        l_AttackEffect.transform.SetParent(p_Target.transform);
        l_AttackEffect.transform.localPosition = Vector3.zero;

        m_AttackEffectList.Add(l_AttackEffect);
    }

    public void PlayEffects()
    {
        m_BattleEnemy.spriteRenderer.transform.SetParent(m_AttackEffectList[m_CurrentEffect].enemyRendererTransform);
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

        m_BattleEnemy.spriteRenderer.transform.SetParent(m_AttackEffectList[m_CurrentEffect].enemyRendererTransform);
        m_AttackEffectList[m_CurrentEffect].PlayEffect();
    }

    private void Reset()
    {
        m_CurrentEffect = 0;
        m_AttackEffectList.Clear();
        m_BattleEnemy.spriteRenderer.transform.SetParent(m_BattleEnemy.transform);
    }
}
