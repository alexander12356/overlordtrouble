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
    
    public void SpecialAttack(BattleActor p_Target, List<Special> p_SpecialList)
    {
        p_Target.CheckPrevAttack();

        if (p_SpecialList.Count == 0)
        {
            VisualEffect l_AttackEffect = Instantiate(Resources.Load<VisualEffect>("Prefabs/BattleEffects/Player/Player_BaseAttack"));
            l_AttackEffect.Init(p_Target, p_Target.spriteRenderer.transform);
            BattlePlayEffectStep l_PlayStep = new BattlePlayEffectStep(l_AttackEffect);
            ResultSystem.GetInstance().AddStep(l_PlayStep);

            string l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:BadAttack", new string[] { "1", p_Target.actorName });

            TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
            l_TextPanel.SetText(new List<string>() { l_Text });
            l_TextPanel.AddButtonAction(l_TextPanel.Close);

            BattleShowPanelStep l_ShowStep = new BattleShowPanelStep(l_TextPanel);
            ResultSystem.GetInstance().AddStep(l_ShowStep);

            p_Target.Damage(1.0f);
        }
        else
        {
            DamageSystem.GetInstance().MonstyleAttack(this, p_Target, p_SpecialList);
        }

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
