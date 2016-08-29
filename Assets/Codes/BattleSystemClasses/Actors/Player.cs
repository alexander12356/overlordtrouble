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
        l_NewTextPanel.SetText(new List<string>() { "Игрок нанес " + l_Damage + " урона врагу" });
        l_NewTextPanel.AddButtonAction(EndTurn);
        PanelManager.GetInstance().ShowPanel(l_NewTextPanel);
        BattleSystem.GetInstance().SetVisibleAvatarPanel(false);

        m_AudioSource.PlayOneShot(m_AudioAttack);
    }

    public override void Damage(float p_DamageValue)
    {
        base.Damage(p_DamageValue);

        health -= p_DamageValue;

        Debug.Log("Player health: " + health);

        m_Animator.SetTrigger("Hit");
        m_AudioSource.PlayOneShot(m_AudioHit);
    }

    //TODO отрефакторить
    public void SpecialAttack(Enemy p_Enemy, List<SpecialUpgradeIcon> p_SpecialUpgradeIconList)
    {
        int l_BrokenSpecialCount = 0;
        int l_UnbuffedSpecialCount = 0;

        float l_DamageValue = 0;
        string l_Text = string.Empty;

        List<SkillData> l_BuffedSkills = new List<SkillData>();
        for (int i = 0; i < p_SpecialUpgradeIconList.Count; i++)
        {
            if (p_SpecialUpgradeIconList[i].GetBuffCount() == - 1)
            {
                l_BrokenSpecialCount++;
            }
            else if (p_SpecialUpgradeIconList[i].GetBuffCount() == 0)
            {
                l_UnbuffedSpecialCount++;
            }
            else
            {
                SkillData l_SkillData = SkillDataBase.GetInstance().GetSkillData(p_SpecialUpgradeIconList[i].skillId);
                l_SkillData.damage = l_SkillData.damage + (l_SkillData.damage * 0.1f) * p_SpecialUpgradeIconList[i].GetBuffCount();

                l_BuffedSkills.Add(l_SkillData);
            }
        }
        if (l_BrokenSpecialCount == p_SpecialUpgradeIconList.Count)
        {
            l_DamageValue = 1.0f;
            l_Text = "Плод твоих напрасных усилий был равен " + l_DamageValue + " очкам урона по " + p_Enemy.actorName;
        }
        else if (l_UnbuffedSpecialCount == p_SpecialUpgradeIconList.Count)
        {
            SkillData p_SkillData = SkillDataBase.GetInstance().GetSkillData(p_SpecialUpgradeIconList[0].skillId);

            l_DamageValue = p_SkillData.damage - p_SkillData.damage * 0.25f;
            l_Text = "Плод твоих напрасных усилий был равен " + l_DamageValue + " очкам урона по " + p_Enemy.actorName;
        }
        else
        {
            string l_UsedSpecialsName = "";

            for (int i = 0; i < l_BuffedSkills.Count; i++)
            {
                string l_SkillLocalization = LocalizationDataBase.GetInstance().GetText("Skill:" + l_BuffedSkills[i].id);
                if (i == l_BuffedSkills.Count - 2)
                {
                    l_UsedSpecialsName += l_SkillLocalization + " и ";
                }
                else if (i == l_BuffedSkills.Count - 1)
                {
                    l_UsedSpecialsName += l_SkillLocalization;
                }
                else
                {
                    l_UsedSpecialsName += l_SkillLocalization + ", ";
                }
                l_DamageValue += l_BuffedSkills[i].damage;
            }
            
            l_Text = "ГГ использовал на \"" + p_Enemy.actorName + "\" " + l_UsedSpecialsName + " он нанес " + l_DamageValue + " урона";
        }

        TextPanel l_TextNewPanel = Instantiate(TextPanel.prefab);
        l_TextNewPanel.SetText(new List<string>() { l_Text });
        l_TextNewPanel.AddButtonAction(EndTurn);
        PanelManager.GetInstance().ShowPanel(l_TextNewPanel);
        BattleSystem.GetInstance().SetVisibleAvatarPanel(false);

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
    }

    public override void ChangeHealthValue()
    {
        base.ChangeHealthValue();

        m_HealthText.text = "HP: " + health + "/" + baseHealth;
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
