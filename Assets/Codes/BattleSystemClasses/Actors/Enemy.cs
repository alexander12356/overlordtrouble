using UnityEngine;
using System.Collections.Generic;

public class Enemy : Actor
{
    #region Variables
    private static Enemy m_Prefab = null;
    private int[] m_DamageValue = new int[2];
    private Animator m_Animator = null;
    private AudioSource m_AudioSource = null;
    private AudioClip m_AudioHit = null;
    private AudioClip m_AudioAttack = null;
    private EnemyData m_EnemyData;
    private bool m_Selected = false;
    private SpriteRenderer m_SelectedArrow = null;
    private SpriteRenderer m_Renderer = null;
    #endregion

    #region Interface
    public static Enemy prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<Enemy>("Prefabs/BattleEnemy");
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
        LoadSounds();
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
        m_DamageValue = m_EnemyData.damageValue.ToArray();
        m_Renderer.sprite = Resources.Load<Sprite>("Sprites/Creations/" + m_EnemyData.id + "/BattleProfile");
    }

    public void Run()
    {
        Attack(Player.GetInstance());
    }

    public override void Damage(float p_DamageValue, string p_AttackType)
    {
        base.Damage(p_DamageValue, p_AttackType);

        health -= p_DamageValue;

        Debug.Log("Enemy health: " + health);
        
        m_Animator.SetTrigger(p_AttackType);
        m_AudioSource.PlayOneShot(m_AudioHit);
    }

    public override void Attack(Actor p_Actor)
    {
        base.Attack(p_Actor);

        float l_DamageValue = Random.Range(m_DamageValue[0], m_DamageValue[1]);
        p_Actor.Damage(l_DamageValue, "BaseAttack");

        TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { actorName + " использовал удар и нанес " + l_DamageValue + " урона" });
        l_TextPanel.AddButtonAction(EndTurn);
        PanelManager.GetInstance().ShowPanel(l_TextPanel);

        m_AudioSource.PlayOneShot(m_AudioAttack);
    }

    public override void Died()
    {
        base.Died();

        BattleSystem.GetInstance().Died(BattleSystem.ActorID.Enemy);
    }
    #endregion

    #region Private
    private void LoadSounds()
    {
        m_AudioHit = Resources.Load<AudioClip>("Sounds/Enemy/Nyashka/Hit");
        m_AudioAttack = Resources.Load<AudioClip>("Sounds/Enemy/Nyashka/Attack");
    }

    private void InitComponents()
    {
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
        m_Renderer = transform.FindChild("Renderer").GetComponent<SpriteRenderer>();
        m_SelectedArrow = transform.FindChild("Selected").GetComponent<SpriteRenderer>();
    }
    #endregion
}