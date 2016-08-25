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
	[SerializeField] private GameObject healthBar;
	[SerializeField] private GameObject manaBar;
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


    public override void Damage(float p_DamageValue)
    {
        base.Damage(p_DamageValue);

        health -= p_DamageValue;

        Debug.Log("Player health: " + health);

        if (health <= 0.0f)
        {
            Died();

            Debug.Log("Player is died");
        }

        m_Animator.SetTrigger("Hit");
        m_AudioSource.PlayOneShot(m_AudioHit);
    }

    //TODO отрефакторить
    public void SpecialAttack(Enemy p_Enemy, Dictionary<string, Special> p_SpecialDictionary)
    {
        int l_BrokenSpecialCount = 0;
        int l_UnbuffedSpecialCount = 0;
        float l_DamageValue = 0;

        List<Special> l_BuffedSpecialList = new List<Special>();
        List<Special> l_SpecialList = new List<Special>();
        foreach (string l_Key in p_SpecialDictionary.Keys)
        {
            if (p_SpecialDictionary[l_Key].level == - 1)
            {
                l_BrokenSpecialCount++;
            }
            else if (p_SpecialDictionary[l_Key].level == 0)
            {
                l_UnbuffedSpecialCount++;
            }
            else
            {
                l_BuffedSpecialList.Add(p_SpecialDictionary[l_Key]);
            }
            l_SpecialList.Add(p_SpecialDictionary[l_Key]);
        }
        if (l_BrokenSpecialCount == p_SpecialDictionary.Count)
        {
            l_DamageValue = 1.0f;

            TextPanel l_NewTextPanel = Instantiate(TextPanel.prefab);
            l_NewTextPanel.SetText("Плод твоих напрасных усилий был равен " + l_DamageValue + " очкам урона по " + p_Enemy.actorName);
            l_NewTextPanel.AddButtonAction(EndTurn);
            PanelManager.GetInstance().ShowPanel(l_NewTextPanel);
        }
        else if (l_UnbuffedSpecialCount == p_SpecialDictionary.Count)
        {
            l_DamageValue = l_SpecialList[0].damageValue - l_SpecialList[0].damageValue * 0.25f;

            TextPanel l_NewTextPanel = Instantiate(TextPanel.prefab);
            l_NewTextPanel.SetText("Плод твоих напрасных усилий был равен " + l_DamageValue + " очкам урона по " + p_Enemy.actorName);
            l_NewTextPanel.AddButtonAction(EndTurn);
            PanelManager.GetInstance().ShowPanel(l_NewTextPanel);
        }
        else
        {
            string l_UsedSpecialsName = "";
            for (int i = 0; i < l_BuffedSpecialList.Count; i++)
            {
                if (i == l_BuffedSpecialList.Count - 2)
                {
                    l_UsedSpecialsName += l_BuffedSpecialList[i].title + " и ";
                }
                else if (i == l_BuffedSpecialList.Count - 1)
                {
                    l_UsedSpecialsName += l_BuffedSpecialList[i].title;
                }
                else
                {
                    l_UsedSpecialsName += l_BuffedSpecialList[i].title + ", ";
                }

                l_DamageValue += l_BuffedSpecialList[i].damageValue + (l_BuffedSpecialList[i].damageValue * 0.1f) * l_BuffedSpecialList[i].level;
            }

            TextPanel l_NewTextPanel = Instantiate(TextPanel.prefab);
            l_NewTextPanel.SetText("ГГ использовал на \"" + p_Enemy.actorName + "\" " + l_UsedSpecialsName + " он нанес " + l_DamageValue + " урона");
            l_NewTextPanel.AddButtonAction(EndTurn);
            PanelManager.GetInstance().ShowPanel(l_NewTextPanel);
        }
        p_Enemy.Damage(l_DamageValue);
        m_AudioSource.PlayOneShot(m_AudioAttack);
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

    public override void ChangeManaValue()
    {
        base.ChangeManaValue();

        m_ManaText.text = "MP: " + mana + "/" + baseMana;
		float calc_mana = mana / baseMana;
		setManaBar (calc_mana);

    }

    public override void ChangeHealthValue()
    {
        base.ChangeHealthValue();

        m_HealthText.text = "HP: " + health + "/" + baseHealth;
		float calc_health = health / baseHealth;
		setHealthBar (calc_health);
    }
	void setHealthBar(float cur_health)
	{
		healthBar.transform.localScale = new Vector3 (cur_health, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}
	void setManaBar(float cur_mana)
	{
		manaBar.transform.localScale = new Vector3 (cur_mana, manaBar.transform.localScale.y, manaBar.transform.localScale.z);
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
