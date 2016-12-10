﻿using UnityEngine;
using System.Collections.Generic;

public class BattleEnemy : BattleActor
{
    #region Variables
    private static BattleEnemy m_Prefab = null;
    private Animator m_Animator = null;
    private AudioSource m_AudioSource = null;
    private EnemyData m_EnemyData;
    private bool m_Selected = false;
    private SpriteRenderer m_SelectedArrow = null;
    private List<EnemyAttackData> m_AttackList = null;
    #endregion

    #region Interface
    public static BattleEnemy prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<BattleEnemy>("Prefabs/BattleEnemy");
            }
            return m_Prefab;
        }
    }
    public bool selected
    {
        get { return m_Selected; }
        set
        {
            m_Selected = value;
            m_SelectedArrow.gameObject.SetActive(m_Selected);
        }
    }

    public override void Awake()
    {
        base.Awake();

        InitComponents();
    }

    public void SetData(EnemyData l_EnemyData)
    {
        m_EnemyData = l_EnemyData;
        InitStats();
    }

    public override void InitStats()
    {
        base.InitStats();

        actorName = LocalizationDataBase.GetInstance().GetText("Enemy:" + m_EnemyData.id);
        health = baseHealth = m_EnemyData.health;
        mana = baseMana = 0;
        m_AttackList = m_EnemyData.attackList;
        m_SpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Creations/" + m_EnemyData.id + "/BattleProfile");
        m_Animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Sprites/Creations/" + m_EnemyData.id + "/" + m_EnemyData.id + "BattleAnimator");
    }

    public override void RunTurn()
    {
        base.RunTurn();

        Attack(BattlePlayer.GetInstance());
        ResultSystem.GetInstance().ShowResult();
    }

    public override void Damage(float p_DamageValue)
    {
        base.Damage(p_DamageValue);

        health -= p_DamageValue;
        DamageSystem.GetInstance().AttackSuccess();
    }

    public override void Damage(List<AttackEffect> p_AttackEffectList)
    {
        base.Damage(p_AttackEffectList);

        float l_DamageValue = 0.0f;
        for (int i = 0; i < p_AttackEffectList.Count; i++)
        {
            l_DamageValue += p_AttackEffectList[i].attackValue;
        }

        Damage(l_DamageValue);
    }

    public override void Attack(BattleActor p_Actor)
    {
        base.Attack(p_Actor);

        EnemyAttackData l_AttackData = m_EnemyData.attackList[Random.Range(0, m_EnemyData.attackList.Count)];
        float l_Damage = Random.Range(l_AttackData.damageValue[0], l_AttackData.damageValue[1]);

        string l_AttackEffectPrefabPath = "Prefabs/BattleEffects/" + m_EnemyData.id + "/" + l_AttackData.id;
        VisualEffect l_AttackEffect = Instantiate(Resources.Load<VisualEffect>(l_AttackEffectPrefabPath));
        l_AttackEffect.Init(p_Actor, spriteRenderer.transform);

        BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_AttackEffect);
        ResultSystem.GetInstance().AddStep(l_Step);

        DamageSystem.GetInstance().Attack(this, p_Actor, l_Damage);
    }

    public override void Die()
    {
        base.Die();

        BattleSystem.GetInstance().AddExperience(m_EnemyData.experience);
        BattleSystem.GetInstance().EnemyDied(this);
        m_Animator.SetTrigger("Die");
    }

    // Called from Animation
    public override void Died()
    {
        base.Died();

        Destroy(gameObject);
    }

    public override void PlayHitSound()
    {
        m_AudioSource.PlayOneShot(AudioDataBase.GetInstance().GetAudioClip(m_EnemyData.id + "_Hit"));
    }
    #endregion

    #region Private
    private void InitComponents()
    {
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();

        Transform l_RendererTransform = transform.FindChild("Renderer");
        if (l_RendererTransform != null)
        {
            m_SpriteRenderer = l_RendererTransform.GetComponent<SpriteRenderer>();
        }

        Transform l_SelectedTransform = transform.FindChild("Selected");
        if (l_SelectedTransform != null)
        {
            m_SelectedArrow = l_SelectedTransform.GetComponent<SpriteRenderer>();
        }
    }
    #endregion
}