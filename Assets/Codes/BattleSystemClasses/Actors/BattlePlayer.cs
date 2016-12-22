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
    private int   m_MonstyleCapacity = 4;
    private Dictionary<string, EffectIcon> m_EffectIcons = new Dictionary<string, EffectIcon>();
    private Transform m_EffectsBar = null;
    private int m_BuffCount   = 0;
    private int m_DebuffCount = 0;
    #endregion

    #region Interface
    static public BattlePlayer GetInstance()
    {
        return m_Instance;
    }
    public int monstyleCapacity
    {
        get { return m_MonstyleCapacity;  }
        set { m_MonstyleCapacity = value; }
    }

    public override void Attack(BattleActor p_Actor)
    {
        base.Attack(p_Actor);

        int l_Damage = Random.Range(m_AttackValue[0], m_AttackValue[1]);

        VisualEffect l_AttackEffect = Instantiate(Resources.Load<VisualEffect>("Prefabs/BattleEffects/Player/Player_BaseAttack"));
        l_AttackEffect.Init(p_Actor, p_Actor.rendererTransform);

        BattlePlayEffectStep l_Step = new BattlePlayEffectStep(l_AttackEffect);
        DamageSystem.GetInstance().AddVisualEffectStep(l_Step);

        DamageSystem.GetInstance().Attack(this, p_Actor, Element.Physical, l_Damage);
        ResultSystem.GetInstance().ShowResult();

        BattleSystem.GetInstance().SetVisibleAvatarPanel(false);
    }
    
    public void SpecialAttack(BattleActor p_Target, List<Special> p_SpecialList)
    {
        if (p_SpecialList.Count == 0)
        {
            VisualEffect l_AttackEffect = Instantiate(Resources.Load<VisualEffect>("Prefabs/BattleEffects/Player/Player_BaseAttack"));
            l_AttackEffect.Init(p_Target, p_Target.rendererTransform);
            BattlePlayEffectStep l_PlayStep = new BattlePlayEffectStep(l_AttackEffect);
            DamageSystem.GetInstance().AddVisualEffectStep(l_PlayStep);

            string l_Text = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:BadAttack", new string[] { actorName });

            DamageSystem.GetInstance().Attack(this, p_Target, Element.Physical, 1.0f, l_Text);
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

        baseSpecialPoints = PlayerData.GetInstance().GetStatValue("MonstylePoints");
        specialPoints = PlayerData.GetInstance().monstylePoints;

        attackStat = PlayerData.GetInstance().GetStatValue("Attack");
        defenseStat = PlayerData.GetInstance().GetStatValue("Defense");
        level = PlayerData.GetInstance().GetLevel() + 1;

        m_AttackValue = PlayerData.GetInstance().attackValue;

        actorName = PlayerData.GetInstance().GetPlayerName();
    }

    public override void Awake()
    {
        base.Awake();

        m_Instance = this;

        Image l_AvatarImage = transform.FindChild("Renderer").GetComponent<Image>();
        l_AvatarImage.sprite = PlayerData.GetInstance().GetBattleAvatar();

        InitComponents();
        InitStats();
    }

    public override void ChangeManaValue()
    {
        base.ChangeManaValue();

        m_SpecialText.text = "MP: " + specialPoints + "/" + baseSpecialPoints;
        PlayerData.GetInstance().monstylePoints = (int)specialPoints;

        Vector3 l_BarScale = m_SpecialPointBar.transform.localScale;
        l_BarScale.x = specialPoints / baseSpecialPoints;
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

    public void RestoreSpecialPoints()
    {
        float l_RestoreSpecialPoints = 0.0f;

        if (baseSpecialPoints < 100)
        {
            l_RestoreSpecialPoints = baseSpecialPoints / 10.0f;
        }
        else
        {
            l_RestoreSpecialPoints = 10 + baseSpecialPoints / 10.0f;
        }
        specialPoints += l_RestoreSpecialPoints;
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
        m_EffectsBar      = transform.FindChild("EffectsBar");

        rendererTransform = transform.FindChild("Renderer");
    }
    
    public override void AddBuffIcon()
    {
        m_BuffCount++;

        if (m_BuffCount > 1)
        {
            return;
        }

        EffectIcon l_EffectIcon = Instantiate(EffectIcon.prefab);
        l_EffectIcon.SetIconId("Buff");
        l_EffectIcon.transform.SetParent(m_EffectsBar);
        l_EffectIcon.transform.localScale = Vector3.one;
        l_EffectIcon.transform.SetSiblingIndex(0);

        m_EffectIcons.Add("Buff", l_EffectIcon);
    }

    public override void RemoveBuffIcon()
    {
        m_BuffCount--;

        if (m_BuffCount == 0)
        {
            Destroy(m_EffectIcons["Buff"].gameObject);
            m_EffectIcons.Remove("Buff");
        }
    }

    public override void AddDebuffIcon()
    {
        base.AddDebuffIcon();

        m_DebuffCount++;

        if (m_DebuffCount > 1)
        {
            return;
        }

        EffectIcon l_EffectIcon = Instantiate(EffectIcon.prefab);
        l_EffectIcon.SetIconId("Debuff");
        l_EffectIcon.transform.SetParent(m_EffectsBar);
        l_EffectIcon.transform.localScale = Vector3.one;

        if (m_BuffCount > 0)
        {
            l_EffectIcon.transform.SetSiblingIndex(1);
        }
        else
        {
            l_EffectIcon.transform.SetSiblingIndex(0);
        }

        m_EffectIcons.Add("Debuff", l_EffectIcon);
    }

    public override void RemoveDebuffIcon()
    {
        base.RemoveDebuffIcon();

        m_DebuffCount--;

        if (m_DebuffCount == 0)
        {
            Destroy(m_EffectIcons["Debuff"].gameObject);
            m_EffectIcons.Remove("Debuff");
        }
    }

    public override void AddStatusEffectIcon(string p_EffectId)
    {
        base.AddStatusEffectIcon(p_EffectId);

        if (m_EffectIcons.ContainsKey(p_EffectId))
        {
            return;
        }

        EffectIcon l_EffectIcon = Instantiate(EffectIcon.prefab);
        l_EffectIcon.SetIconId(p_EffectId);
        l_EffectIcon.transform.SetParent(m_EffectsBar);
        l_EffectIcon.transform.localScale = Vector3.one;

        m_EffectIcons.Add(p_EffectId, l_EffectIcon);
    }

    public override void RemoveStatusEffectIcon(string p_EffectId)
    {
        base.RemoveStatusEffectIcon(p_EffectId);

        Destroy(m_EffectIcons[p_EffectId].gameObject);
        m_EffectIcons.Remove(p_EffectId);
    }
    #endregion
}
