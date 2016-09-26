using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BattlePlayer : BattleActor
{
    #region Variables
    private static BattlePlayer m_Instance = null;
    private int[] m_AttackValue;
    private Animator m_Animator = null;
    private AudioSource m_AudioSource = null;
    private Text m_HealthText = null;
    private Text m_SpecialText = null;
    private Image m_HealthPointBar = null;
    private Image m_SpecialPointBar = null;
    #endregion

    #region Interface
    static public BattlePlayer GetInstance()
    {
        return m_Instance;
    }
    
    public override void Attack(BattleActor p_Actor)
    {
        base.Attack(p_Actor);

        int l_Damage = Random.Range(m_AttackValue[0], m_AttackValue[1]);

        p_Actor.Damage(l_Damage);
        AttackEffectsSystem.GetInstance().AddEffect((BattleEnemy)p_Actor, "Player_BaseAttack");
        AttackEffectsSystem.GetInstance().PlayEffects();

        TextPanel l_NewTextPanel = Instantiate(TextPanel.prefab);
        l_NewTextPanel.SetText(new List<string>() { "Игрок нанес " + l_Damage + " урона врагу" });
        l_NewTextPanel.AddButtonAction(EndTurn);
        BattleSystem.GetInstance().ShowPanel(l_NewTextPanel);
        BattleSystem.GetInstance().SetVisibleAvatarPanel(false);
    }

    public override void Damage(float p_DamageValue)
    {
        base.Damage(p_DamageValue);

        health -= p_DamageValue;
    }

    //TODO отрефакторить
    public void SpecialAttack(BattleEnemy p_Enemy, List<SpecialUpgradeIcon> p_SpecialUpgradeIconList)
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

            AttackEffectsSystem.GetInstance().AddEffect(p_Enemy, "Player_BaseAttack");
        }
        else if (l_UnbuffedSpecialCount == p_SpecialUpgradeIconList.Count)
        {
            SkillData p_SkillData = SkillDataBase.GetInstance().GetSkillData(p_SpecialUpgradeIconList[0].skillId);

            l_DamageValue = p_SkillData.damage - p_SkillData.damage * 0.25f;
            l_Text = "Плод твоих напрасных усилий был равен " + l_DamageValue + " очкам урона по " + p_Enemy.actorName;

            AttackEffectsSystem.GetInstance().AddEffect(p_Enemy, "Player_BaseAttack");
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

                AttackEffectsSystem.GetInstance().AddEffect(p_Enemy, l_BuffedSkills[i].id + "Monstyle");
            }
            
            l_Text = "ГГ использовал на \"" + p_Enemy.actorName + "\" " + l_UsedSpecialsName + " он нанес " + l_DamageValue + " урона";
        }

        p_Enemy.Damage(l_DamageValue);

        TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { l_Text });
        l_TextPanel.AddButtonAction(EndTurn);
        BattleSystem.GetInstance().ShowPanel(l_TextPanel);

        AttackEffectsSystem.GetInstance().PlayEffects();

        BattleSystem.GetInstance().SetVisibleAvatarPanel(false);
    }

    public override void Died()
    {
        base.Died();

        BattleSystem.GetInstance().Died(BattleSystem.ActorID.Player);
    }

    public override void InitStats()
    {
        base.InitStats();

        baseHealth = PlayerData.GetInstance().GetStatValue("HealthPoints");
        health = PlayerData.GetInstance().health;

        baseMana = PlayerData.GetInstance().GetStatValue("MonstylePoints");
        mana = PlayerData.GetInstance().monstylePoints;

        m_AttackValue = PlayerData.GetInstance().attackValue;
    }

    public override void Awake()
    {
        base.Awake();

        m_Instance = this;

        Image l_AvatarImage = transform.FindChild("AvatarImage").GetComponent<Image>();
        l_AvatarImage.sprite = PlayerData.GetInstance().GetBattleAvatar();

        InitComponents();
        InitStats();
    }

    public override void ChangeManaValue()
    {
        base.ChangeManaValue();

        m_SpecialText.text = "MP: " + mana + "/" + baseMana;

        Vector3 l_BarScale = m_SpecialPointBar.transform.localScale;
        l_BarScale.x = mana / baseMana;
        m_SpecialPointBar.transform.localScale = l_BarScale;
    }

    public override void ChangeHealthValue()
    {
        base.ChangeHealthValue();

        m_HealthText.text = "HP: " + health + "/" + baseHealth;
        PlayerData.GetInstance().health = (int)health;

        Vector3 l_BarScale = m_HealthPointBar.transform.localScale;
        l_BarScale.x = health / baseHealth;
        m_HealthPointBar.transform.localScale = l_BarScale;
    }
    #endregion

    #region Private
    private void InitComponents()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_AudioSource = GetComponent<AudioSource>();

        m_HealthText      = transform.FindChild("HealthText").GetComponent<Text>();
        m_HealthPointBar  = transform.FindChild("HealthBar").GetComponent<Image>();
        m_SpecialText     = transform.FindChild("SpecialText").GetComponent<Text>();
        m_SpecialPointBar = transform.FindChild("SpecialBar").GetComponent<Image>();
    }
    #endregion
}
