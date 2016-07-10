using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables
    private static Player m_Instance = null;
    private float m_Health = 20;
    private float m_Mana   = 10;
    private int[] m_DamageValue = new int[2] { 5, 8};
    private bool  m_IsDied = false;
    private Animator m_Animator = null;
    private AudioClip m_AudioHit = null;
    private AudioClip m_AudioAttack = null;
    private AudioSource m_AudioSource = null;
    #endregion

    #region Interface
    static public Player GetInstance()
    {
        return m_Instance;
    }
    
    public void Run()
    {
        Unblock();
    }
    
    public void Attack(Enemy p_Enemy)
    {
        int l_Damage = Random.Range(m_DamageValue[0], m_DamageValue[1]);

        p_Enemy.Damage(l_Damage);

        TextPanel l_NewTextPanel = Instantiate(TextPanel.prefab);
        l_NewTextPanel.SetText("Игрок нанес " + l_Damage + " урона врагу");
        l_NewTextPanel.AddButtonAction(EndTurn);
        PanelManager.GetInstance().ShowPanel(l_NewTextPanel);

        m_AudioSource.PlayOneShot(m_AudioAttack);
    }

    public void Damage(float p_Value)
    {
        m_Health -= p_Value;

        Debug.Log("Player health: " + m_Health);

        if (m_Health <= 0.0f)
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
    #endregion

    #region Private
    private void Awake()
    {
        m_Instance = this;

        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();

        LoadSounds();
    }
    
    private void Block()
    {
    }
    
    private void Unblock()
    {
    }
    
    private void Died()
    {
        BattleSystem.GetInstance().Died(BattleSystem.ActorID.Player);
        m_IsDied = true;
    }

    private void EndTurn()
    {
        TurnSystem.GetInstance().EndTurn();
    }

    private void LoadSounds()
    {
        m_AudioHit = Resources.Load<AudioClip>("Sounds/Player/Hit");
        m_AudioAttack = Resources.Load<AudioClip>("Sounds/Player/Attack");
    }
    #endregion
}
