﻿using System.Collections.Generic;

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
    private BattleActor m_AttackTarget = null;
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

        AttackEffect l_AttackEffect = Instantiate(Resources.Load<AttackEffect>("Prefabs/BattleEffects/Player/Player_BaseAttack"));
        l_AttackEffect.Init(p_Actor, p_Actor.spriteRenderer.transform);

        BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_AttackEffect);
        ResultSystem.GetInstance().AddStep(l_Step);

        DamageSystem.GetInstance().Attack(this, p_Actor, l_Damage);
        ResultSystem.GetInstance().ShowResult();

        BattleSystem.GetInstance().SetVisibleAvatarPanel(false);
    }

    public override void Damage(float p_DamageValue)
    {
        base.Damage(p_DamageValue);

        health -= p_DamageValue;

        DamageSystem.GetInstance().AttackSuccess();
    }

    //TODO отрефакторить
    public void SpecialAttack(BattleActor p_Enemy, List<SpecialUpgradeIcon> p_SpecialUpgradeIconList)
    {
        m_AttackTarget = p_Enemy;
        m_AttackTarget.CheckPrevAttack();

        int l_BrokenSpecialCount = 0;
        int l_UnbuffedSpecialCount = 0;

        float l_DamageValue = 0;
        string l_Text = string.Empty;

        List<SkillData> l_BuffedSkills = new List<SkillData>();
        for (int i = 0; i < p_SpecialUpgradeIconList.Count; i++)
        {
            if (p_SpecialUpgradeIconList[i].GetBuffCount() == -1)
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

        string l_UsedSpecialsName = "";
        bool l_IsBadAttack = false;

        if (l_BrokenSpecialCount == p_SpecialUpgradeIconList.Count)
        {
            l_DamageValue = 1.0f;
            l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:BadAttack", new string[] { l_DamageValue.ToString(), p_Enemy.actorName });
            l_IsBadAttack = true;

            AttackEffect l_AttackEffect = Instantiate(Resources.Load<AttackEffect>("Prefabs/BattleEffects/Player/Player_BaseAttack"));
            l_AttackEffect.Init(p_Enemy, p_Enemy.spriteRenderer.transform);
            BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_AttackEffect);

            ResultSystem.GetInstance().AddStep(l_Step);
        }
        else if (l_UnbuffedSpecialCount == p_SpecialUpgradeIconList.Count)
        {
            SkillData p_SkillData = SkillDataBase.GetInstance().GetSkillData(p_SpecialUpgradeIconList[0].skillId);

            l_DamageValue = p_SkillData.damage - p_SkillData.damage * 0.25f;
            l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:BadAttack", new string[] { l_DamageValue.ToString(), p_Enemy.actorName });
            l_IsBadAttack = true;

            AttackEffect l_AttackEffect = Instantiate(Resources.Load<AttackEffect>("Prefabs/BattleEffects/Player/Player_BaseAttack"));
            l_AttackEffect.Init(p_Enemy, p_Enemy.spriteRenderer.transform);
            BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_AttackEffect);

            ResultSystem.GetInstance().AddStep(l_Step);
        }
        else
        {
            for (int i = 0; i < l_BuffedSkills.Count; i++)
            {
                string l_SkillName = LocalizationDataBase.GetInstance().GetText("Skill:" + l_BuffedSkills[i].id);
                if (i == l_BuffedSkills.Count - 2)
                {
                    l_UsedSpecialsName += l_SkillName + " " + LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:And") + " ";
                }
                else if (i == l_BuffedSkills.Count - 1)
                {
                    l_UsedSpecialsName += l_SkillName;
                }
                else
                {
                    l_UsedSpecialsName += l_SkillName + ", ";
                }
                l_DamageValue += l_BuffedSkills[i].damage;

                string l_PrefabPath = "Prefabs/BattleEffects/Monstyle/" + l_BuffedSkills[i].id + "Monstyle";

                AttackEffect l_AttackEffect = Instantiate(Resources.Load<AttackEffect>(l_PrefabPath));
                l_AttackEffect.Init(p_Enemy, p_Enemy.spriteRenderer.transform);
                BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_AttackEffect);

                ResultSystem.GetInstance().AddStep(l_Step);
            }

            l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:SpecialAttack", new string[] { p_Enemy.actorName, l_UsedSpecialsName, l_DamageValue.ToString() });
        }

        DamageSystem.GetInstance().SpecialAttack(this, p_Enemy, l_DamageValue, l_IsBadAttack, l_UsedSpecialsName);

        ResultSystem.GetInstance().ShowResult();
        BattleSystem.GetInstance().SetVisibleAvatarPanel(false);
    }

    public override void Die()
    {
        base.Die();

        BattleSystem.GetInstance().PlayerDied();
    }

    public override void InitStats()
    {
        base.InitStats();

        baseHealth = PlayerData.GetInstance().GetStatValue("HealthPoints");
        health = PlayerData.GetInstance().health;

        baseMana = PlayerData.GetInstance().GetStatValue("MonstylePoints");
        mana = PlayerData.GetInstance().monstylePoints;

        m_AttackValue = PlayerData.GetInstance().attackValue;

        actorName = PlayerData.GetInstance().GetPlayerName();
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
        PlayerData.GetInstance().monstylePoints = (int)mana;

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

    public override void PlayHitSound()
    {
        m_AudioSource.PlayOneShot(AudioDataBase.GetInstance().GetAudioClip("Player_Hit"));
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
