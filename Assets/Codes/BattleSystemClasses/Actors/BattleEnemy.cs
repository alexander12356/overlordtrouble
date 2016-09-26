using UnityEngine;
using System.Collections.Generic;

public class BattleEnemy : BattleActor
{
    #region Variables
    private static BattleEnemy m_Prefab = null;
    private int[] m_DamageValue = new int[2];
    private Animator m_Animator = null;
    private AudioSource m_AudioSource = null;
    private EnemyData m_EnemyData;
    private bool m_Selected = false;
    private SpriteRenderer m_SelectedArrow = null;
    private SpriteRenderer m_Renderer = null;
    #endregion

    #region Interface
    public static BattleEnemy prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<BattleEnemy>("Prefabs/BattleEnemy");
            }
            return m_Prefab;
        }
    }
    public bool selected
    {
        get { return m_Selected; }
        set
        {
            m_Selected = value;
            m_SelectedArrow.gameObject.SetActive(m_Selected);
        }
    }
    public SpriteRenderer spriteRenderer
    {
        get { return m_Renderer; }
    }

    public override void Awake()
    {
        base.Awake();

        InitComponents();
    }

    public void SetData(EnemyData l_EnemyData)
    {
        m_EnemyData = l_EnemyData;
        InitStats();
    }

    public override void InitStats()
    {
        base.InitStats();

        actorName = LocalizationDataBase.GetInstance().GetText("Enemy:" + m_EnemyData.id);
        health = baseHealth = m_EnemyData.health;
        mana = baseMana = 0;
        m_DamageValue = m_EnemyData.damageValue.ToArray();
        m_Renderer.sprite = Resources.Load<Sprite>("Sprites/Creations/" + m_EnemyData.id + "/BattleProfile");
    }

    public void Run()
    {
        Attack(BattlePlayer.GetInstance());
    }

    public override void Damage(float p_DamageValue)
    {
        base.Damage(p_DamageValue);

        health -= p_DamageValue;
    }

    public override void Attack(BattleActor p_Actor)
    {
        base.Attack(p_Actor);

        float l_DamageValue = Random.Range(m_DamageValue[0], m_DamageValue[1]);
        p_Actor.Damage(l_DamageValue);

        TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { actorName + " использовал удар и нанес " + l_DamageValue + " урона" });
        l_TextPanel.AddButtonAction(EndTurn);
        BattleSystem.GetInstance().ShowPanel(l_TextPanel);

        m_AudioSource.PlayOneShot(AudioDataBase.GetInstance().GetAudioClip(m_EnemyData.id + "_Attack"));
    }

    public override void Died()
    {
        base.Died();

        BattleSystem.GetInstance().AddExperience(m_EnemyData.experience);
        BattleSystem.GetInstance().Died(BattleSystem.ActorID.Enemy);
    }

    public void PlayHitSound()
    {
        m_AudioSource.PlayOneShot(AudioDataBase.GetInstance().GetAudioClip(m_EnemyData.id + "_Hit"));
    }
    #endregion

    #region Private
    private void InitComponents()
    {
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
        m_Renderer = transform.FindChild("Renderer").GetComponent<SpriteRenderer>();
        m_SelectedArrow = transform.FindChild("Selected").GetComponent<SpriteRenderer>();
    }
    #endregion
}