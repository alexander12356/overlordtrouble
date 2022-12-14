using UnityEngine;
using System.Collections.Generic;

public class BattleEnemy : BattleActor
{
    #region Variables
    private static BattleEnemy m_Prefab = null;
    private AudioSource m_AudioSource = null;
    private bool m_Selected = false;
    private SpriteRenderer m_SelectedArrow = null;
    private List<EnemyAttackData> m_AttackList = null;
    private SpriteRenderer m_Renderer = null;

    protected EnemyData m_EnemyData;
    protected Animator m_Animator = null;
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

        id = m_EnemyData.id;
        actorName = LocalizationDataBase.GetInstance().GetText("Enemy:" + m_EnemyData.id);
        health = baseHealth = m_EnemyData.health;
        attackStat = m_EnemyData.attackStat;
        defenseStat = m_EnemyData.defenseStat;
        speedStat = m_EnemyData.speedStat;
        level = m_EnemyData.level;
        element = m_EnemyData.element;
        m_AttackList = m_EnemyData.attackList;
        m_Renderer.sprite = Resources.Load<Sprite>("Sprites/Creations/" + m_EnemyData.id + "/BattleProfile");
        m_Animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Sprites/Creations/" + m_EnemyData.id + "/" + m_EnemyData.id + "BattleAnimator");
    }

    public override void RunTurn()
    {
        base.RunTurn();

        Attack(BattlePlayer.GetInstance());
        ResultSystem.GetInstance().ShowResult();
    }

    public override void Attack(BattleActor p_Actor)
    {
        base.Attack(p_Actor);

        EnemyAttackData l_AttackData = m_EnemyData.attackList[Random.Range(0, m_EnemyData.attackList.Count)];

        UsingAttack(p_Actor, l_AttackData);
    }

    public void UsingAttack(BattleActor p_Actor, EnemyAttackData p_EnemyAttackData)
    {
        string l_AttackEffectPrefabPath = "Prefabs/BattleEffects/" + m_EnemyData.id + "/" + p_EnemyAttackData.id;
        

        BattleBaseStep l_VisualEffectStep = null;
        switch (p_EnemyAttackData.targetId)
        {
            case EnemyAttackTarget.NONE:
                VisualEffect l_AttackEffect = Instantiate(Resources.Load<VisualEffect>(l_AttackEffectPrefabPath));
                l_AttackEffect.Init(p_Actor, rendererTransform);
                l_VisualEffectStep = new BattlePlayEffectStep(l_AttackEffect);
                break;
            case EnemyAttackTarget.Player:
                VisualEffect l_CanvasEffect = Instantiate(Resources.Load<VisualEffect>(l_AttackEffectPrefabPath));
                l_CanvasEffect.Init(BattlePlayer.GetInstance(), BattlePlayer.GetInstance().rendererTransform);
                l_VisualEffectStep = new BattlePlayEffectStep(l_CanvasEffect);
                break;
            case EnemyAttackTarget.Enemies:
                List<VisualEffect> l_VisualEffectList = new List<VisualEffect>();
                List<BattleEnemy> l_EnemyList = BattleSystem.GetInstance().GetEnemyList();
                for (int i = 0; i < l_EnemyList.Count; i++)
                {
                    if (l_EnemyList[i] != this)
                    {
                        VisualEffect l_AoeAttackEffect = Instantiate(Resources.Load<VisualEffect>(l_AttackEffectPrefabPath));
                        l_AoeAttackEffect.Init(l_EnemyList[i], l_EnemyList[i].rendererTransform);
                        l_VisualEffectList.Add(l_AoeAttackEffect);
                    }
                }
                l_VisualEffectStep = new BattlePlayManyEffectStep(l_VisualEffectList);
                break;
        }
        DamageSystem.GetInstance().AddVisualEffectStep(l_VisualEffectStep);

        Special l_Special = new Special(p_EnemyAttackData.id, p_EnemyAttackData.element, false, false);
        l_Special.specialName = LocalizationDataBase.GetInstance().GetText("Enemy:" + id + ":" + l_Special.id);

        List<BaseEffect> l_EffectList = new List<BaseEffect>();
        for (int i = 0; i < p_EnemyAttackData.effectList.Count; i++)
        {
            l_EffectList.Add(EffectSystem.GetInstance().CreateEffect(l_Special, p_EnemyAttackData.effectList[i]));
        }

        l_Special.SetEffects(l_EffectList);

        DamageSystem.GetInstance().EnemyAttack(this, p_Actor, l_Special);
    }

    public override void Die()
    {
        base.Die();
        
        LeshiiAttackEffect l_DieVisualEffect = Instantiate(LeshiiAttackEffect.prefab);
        l_DieVisualEffect.AddPlayAction(PlayDieAnimation);

        BattlePlayEffectStep l_PlayEffectStep = new BattlePlayEffectStep(l_DieVisualEffect);
        ResultSystem.GetInstance().AddStep(l_PlayEffectStep);

        string l_TextAboutDeath = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:EnemyDied", new string[] { actorName });
        TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { l_TextAboutDeath });
        l_TextPanel.AddButtonAction(l_TextPanel.Close);

        BattleShowPanelStep l_ShowPanelStep = new BattleShowPanelStep(l_TextPanel);
        ResultSystem.GetInstance().AddStep(l_ShowPanelStep);
    }

    // Called from Animation
    public override void Died()
    {
        base.Died();

        BattleSystem.GetInstance().AddExperience(m_EnemyData.experience);
        SetLoot();
        BattleSystem.GetInstance().EnemyDied(this);

        Destroy(gameObject);

        ResultSystem.GetInstance().NextStep();
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

        rendererTransform = transform.FindChild("Renderer");
        if (rendererTransform != null)
        {
            m_Renderer = rendererTransform.GetComponent<SpriteRenderer>();
        }

        Transform l_SelectedTransform = transform.FindChild("Selected");
        if (l_SelectedTransform != null)
        {
            m_SelectedArrow = l_SelectedTransform.GetComponent<SpriteRenderer>();
        }
    }

    private void PlayDieAnimation()
    {
        m_Animator.SetTrigger("Die");
    }

    private void SetLoot()
    {
        List<EnemyLootData> l_LootList = m_EnemyData.lootList;

        for (int i = 0; i < l_LootList.Count; i++)
        {
            BattleSystem.GetInstance().AddLoot(l_LootList[i]);
        }
    }
    #endregion
}