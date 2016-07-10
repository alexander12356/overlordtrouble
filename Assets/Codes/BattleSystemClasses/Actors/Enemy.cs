using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    #region Variables
    private static Enemy m_Instance = null;
    private int m_CurrHealth = 20;
    private int m_MaxHealth = 20;
    private int m_Mana   = 10;
    private int[] m_DamageValue = new int[2] { 5, 8 };
    private bool m_IsDied = false;
    private Animator m_Animator = null;
    private AudioSource m_AudioSource = null;
    private AudioClip m_AudioHit = null;
    private AudioClip m_AudioAttack = null;

    [SerializeField]
    private string m_MobName = "Nyashka";

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

    public int maxHealth
    {
        get { return m_MaxHealth; }
    }

    public int currHealth
    {
        get { return m_CurrHealth; }
    }

    public string mobName
    {
        get { return m_MobName;  }
        set { m_MobName = value; }
    }

    public bool isDied
    {
        get { return m_IsDied; }
    }

    static public Enemy GetInstance()
    {
        return m_Instance;
    }
    
    public void Run()
    {
        Attack();
    }

    public void Damage(int p_Value)
    {
        m_CurrHealth -= p_Value;

        Debug.Log("Enemy health: " + m_CurrHealth);

        if (m_CurrHealth <= 0.0f)
        {
            Died();

            Debug.Log("Enemy is died");
        }
        m_Animator.SetTrigger("Hit");
        m_AudioSource.PlayOneShot(m_AudioHit);
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
    
    private void Attack()
    {
        int l_DamageValue = Random.Range(m_DamageValue[0], m_DamageValue[1]);
        Player.GetInstance().Damage(l_DamageValue);

        TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
        l_TextPanel.SetText("Няшка использовал удар и нанес " + l_DamageValue + " урона");
        l_TextPanel.AddButtonAction(EndTurn);
        PanelManager.GetInstance().ShowPanel(l_TextPanel);

        m_AudioSource.PlayOneShot(m_AudioAttack);
    }

    private void EndTurn()
    {
        TurnSystem.GetInstance().EndTurn();
    }

    private void Died()
    {
        BattleSystem.GetInstance().Died(BattleSystem.ActorID.Enemy);
        m_IsDied = true;
    }

    private void LoadSounds()
    {
        m_AudioHit = Resources.Load<AudioClip>("Sounds/Enemy/Nyashka/Hit");
        m_AudioAttack = Resources.Load<AudioClip>("Sounds/Enemy/Nyashka/Attack");
    }
    #endregion
}