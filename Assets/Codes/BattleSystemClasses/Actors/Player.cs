using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Player : Actor
{
    #region Variables
    private static Player m_Instance = null;
    private int[] m_DamageValue = new int[2] { 5, 8};
    private Animator m_Animator = null;
    private AudioClip m_AudioHit = null;
    private AudioClip m_AudioAttack = null;
    private AudioSource m_AudioSource = null;

    [SerializeField]
    private Text m_HealthText = null;

    [SerializeField]
    private Text m_ManaText = null;
    #endregion

    #region Interface
    static public Player GetInstance()
    {
        return m_Instance;
    }
    
    public override void Attack(Actor p_Actor)
    {
        base.Attack(p_Actor);

        int l_Damage = Random.Range(m_DamageValue[0], m_DamageValue[1]);

        p_Actor.Damage(l_Damage);

        TextPanel l_NewTextPanel = Instantiate(TextPanel.prefab);
        l_NewTextPanel.SetText("Игрок нанес " + l_Damage + " урона врагу");
        l_NewTextPanel.AddButtonAction(EndTurn);
        PanelManager.GetInstance().ShowPanel(l_NewTextPanel);

        m_AudioSource.PlayOneShot(m_AudioAttack);
    }

    public override void Damage(int p_DamageValue)
    {
        base.Damage(p_DamageValue);

        health -= p_DamageValue;
        m_HealthText.text = "HP: " + health + "/" + baseHealth;

        Debug.Log("Player health: " + health);

        if (health <= 0.0f)
        {
            Died();

            Debug.Log("Player is died");
        }

        m_Animator.SetTrigger("Hit");
        m_AudioSource.PlayOneShot(m_AudioHit);
    }

    public void SpecialAttack(List<Special> m_SpecialList)
    {
        EndTurn();
    }

    public override void Died()
    {
        base.Died();

        BattleSystem.GetInstance().Died(BattleSystem.ActorID.Player);
    }

    public override void InitStats()
    {
        base.InitStats();

        health = baseHealth = 20;
        mana = baseMana = 20;
        isDead = true;
    }

    public override void Awake()
    {
        base.Awake();

        m_Instance = this;

        InitComponents();
        LoadSounds();
    }
    #endregion

    #region Private
    private void InitComponents()
    {
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();

        LoadSounds();
    }

    private void LoadSounds()
    {
        m_AudioHit = Resources.Load<AudioClip>("Sounds/Player/Hit");
        m_AudioAttack = Resources.Load<AudioClip>("Sounds/Player/Attack");
    }
    #endregion
}
