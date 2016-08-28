using UnityEngine;
using System.Collections.Generic;

public class Enemy : Actor
{
    #region Variables
    private static Enemy m_Instance = null;
    private int[] m_DamageValue = new int[2] { 5, 8 };
    private Animator m_Animator = null;
    private AudioSource m_AudioSource = null;
    private AudioClip m_AudioHit = null;
    private AudioClip m_AudioAttack = null;

    private bool m_Selected = false;

    [SerializeField]
    private SpriteRenderer m_SelectedArrow = null;
    #endregion

    #region Interface
    public bool selected
    {
        get { return m_Selected; }
        set
        {
            m_Selected = value;
            m_SelectedArrow.gameObject.SetActive(m_Selected);
        }
    }

    static public Enemy GetInstance()
    {
        return m_Instance;
    }

    public override void Awake()
    {
        base.Awake();

        m_Instance = this;

        InitComponents();
        LoadSounds();
    }

    public override void InitStats()
    {
        base.InitStats();

        actorName = "Гоповолк";
        health = baseHealth = 40;
        mana = baseMana = 0;
    }

    public void Run()
    {
        Attack(Player.GetInstance());
    }

    public override void Damage(float p_DamageValue)
    {
        base.Damage(p_DamageValue);

        health -= p_DamageValue;

        Debug.Log("Enemy health: " + health);
        
        m_Animator.SetTrigger("Hit");
        m_AudioSource.PlayOneShot(m_AudioHit);
    }

    public override void Attack(Actor p_Actor)
    {
        base.Attack(p_Actor);

        float l_DamageValue = Random.Range(m_DamageValue[0], m_DamageValue[1]);
        p_Actor.Damage(l_DamageValue);

        TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { "Гоповолк использовал удар и нанес " + l_DamageValue + " урона" });
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
    }
    #endregion
}