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

        VisualEffect l_AttackEffect = Instantiate(Resources.Load<VisualEffect>("Prefabs/BattleEffects/Player/Player_BaseAttack"));
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
    public void SpecialAttack(BattleActor p_Enemy, List<MonstyleUpgradeIcon> p_MonstyleUpgradeIconList)
    {
        m_AttackTarget = p_Enemy;
        m_AttackTarget.CheckPrevAttack();

        int l_BrokenMonstyleCount = 0;
        int l_UnbuffedMonstyleCount = 0;

        float l_DamageValue = 0;
        string l_Text = string.Empty;

        List<MonstyleData> l_BuffedMonstyle = new List<MonstyleData>();
        for (int i = 0; i < p_MonstyleUpgradeIconList.Count; i++)
        {
            if (p_MonstyleUpgradeIconList[i].GetBuffCount() == -1)
            {
                l_BrokenMonstyleCount++;
            }
            else if (p_MonstyleUpgradeIconList[i].GetBuffCount() == 0)
            {
                l_UnbuffedMonstyleCount++;
            }
            else
            {
                MonstyleData l_MonstyleDataData = MonstyleDataBase.GetInstance().GetMonstyleData(p_MonstyleUpgradeIconList[i].monstyleId);
                l_MonstyleDataData.damage = l_MonstyleDataData.damage + (l_MonstyleDataData.damage * 0.1f) * p_MonstyleUpgradeIconList[i].GetBuffCount();

                l_BuffedMonstyle.Add(l_MonstyleDataData);
            }
        }

        string l_UsedMonstylesName = "";
        bool l_IsBadAttack = false;

        if (l_BrokenMonstyleCount == p_MonstyleUpgradeIconList.Count)
        {
            l_DamageValue = 1.0f;
            l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:BadAttack", new string[] { l_DamageValue.ToString(), p_Enemy.actorName });
            l_IsBadAttack = true;

            VisualEffect l_AttackEffect = Instantiate(Resources.Load<VisualEffect>("Prefabs/BattleEffects/Player/Player_BaseAttack"));
            l_AttackEffect.Init(p_Enemy, p_Enemy.spriteRenderer.transform);
            BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_AttackEffect);

            ResultSystem.GetInstance().AddStep(l_Step);
        }
        else if (l_UnbuffedMonstyleCount == p_MonstyleUpgradeIconList.Count)
        {
            MonstyleData p_MonstyleData = MonstyleDataBase.GetInstance().GetMonstyleData(p_MonstyleUpgradeIconList[0].monstyleId);

            l_DamageValue = p_MonstyleData.damage - p_MonstyleData.damage * 0.25f;
            l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:BadAttack", new string[] { l_DamageValue.ToString(), p_Enemy.actorName });
            l_IsBadAttack = true;

            VisualEffect l_AttackEffect = Instantiate(Resources.Load<VisualEffect>("Prefabs/BattleEffects/Player/Player_BaseAttack"));
            l_AttackEffect.Init(p_Enemy, p_Enemy.spriteRenderer.transform);
            BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_AttackEffect);

            ResultSystem.GetInstance().AddStep(l_Step);
        }
        else
        {
            for (int i = 0; i < l_BuffedMonstyle.Count; i++)
            {
                string l_MonstyleName = LocalizationDataBase.GetInstance().GetText("Skill:" + l_BuffedMonstyle[i].id);
                if (i == l_BuffedMonstyle.Count - 2)
                {
                    l_UsedMonstylesName += l_MonstyleName + " " + LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:And") + " ";
                }
                else if (i == l_BuffedMonstyle.Count - 1)
                {
                    l_UsedMonstylesName += l_MonstyleName;
                }
                else
                {
                    l_UsedMonstylesName += l_MonstyleName + ", ";
                }
                l_DamageValue += l_BuffedMonstyle[i].damage;

                string l_PrefabPath = "Prefabs/BattleEffects/Monstyle/" + l_BuffedMonstyle[i].id + "Monstyle";

                VisualEffect l_AttackEffect = Instantiate(Resources.Load<VisualEffect>(l_PrefabPath));
                l_AttackEffect.Init(p_Enemy, p_Enemy.spriteRenderer.transform);
                BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_AttackEffect);

                ResultSystem.GetInstance().AddStep(l_Step);
            }

            l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:SpecialAttack", new string[] { p_Enemy.actorName, l_UsedMonstylesName, l_DamageValue.ToString() });
        }

        DamageSystem.GetInstance().MonstyleAttack(this, p_Enemy, l_DamageValue, l_IsBadAttack, l_UsedMonstylesName);

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
