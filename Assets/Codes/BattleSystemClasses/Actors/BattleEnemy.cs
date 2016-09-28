using UnityEngine;
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
    private SpriteRenderer m_Renderer = null;
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
    public SpriteRenderer spriteRenderer
    {
        get { return m_Renderer; }
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
        m_Renderer.sprite = Resources.Load<Sprite>("Sprites/Creations/" + m_EnemyData.id + "/BattleProfile");
        m_Animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Sprites/Creations/" + m_EnemyData.id + "/" + m_EnemyData.id + "BattleAnimator");
    }

    public void Run()
    {
        Attack(BattlePlayer.GetInstance());
    }

    public override void Damage(float p_DamageValue)
    {
        base.Damage(p_DamageValue);

        health -= p_DamageValue;
    }

    public override void Attack(BattleActor p_Actor)
    {
        base.Attack(p_Actor);

        EnemyAttackData l_AttackData = m_EnemyData.attackList[Random.Range(0, m_EnemyData.attackList.Count)];
        float l_Damage = Random.Range(l_AttackData.damageValue[0], l_AttackData.damageValue[1]);
        string l_EnemyName = LocalizationDataBase.GetInstance().GetText("Enemy:" + m_EnemyData.id);
        string l_AttackName = LocalizationDataBase.GetInstance().GetText("Enemy:" + m_EnemyData.id + ":" + l_AttackData.id);
        string l_AttackText = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:EnemyAttack", new string[] { l_EnemyName, l_AttackName, l_Damage.ToString() });

        p_Actor.Damage(l_Damage);
        m_Animator.SetTrigger(l_AttackData.id);
        //TODO добавить возможность разных звуков
        m_AudioSource.PlayOneShot(AudioDataBase.GetInstance().GetAudioClip(m_EnemyData.id + "_Attack"));

        TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { l_AttackText });
        l_TextPanel.AddButtonAction(l_TextPanel.Close);
        l_TextPanel.AddButtonAction(EndTurn);
        BattleSystem.GetInstance().ShowPanel(l_TextPanel);
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

    public void PlayHitSound()
    {
        m_AudioSource.PlayOneShot(AudioDataBase.GetInstance().GetAudioClip(m_EnemyData.id + "_Hit"));
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
    #endregion

    #region Private
    private void InitComponents()
    {
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
        m_Renderer = transform.FindChild("Renderer").GetComponent<SpriteRenderer>();
        m_SelectedArrow = transform.FindChild("Selected").GetComponent<SpriteRenderer>();
    }
    #endregion
}