using UnityEngine;
using UnityEngine.UI;

public class PanelButtonStat : PanelButton
{
    private static PanelButtonStat m_Prefab = null;
    private string m_StatId = string.Empty;
    private int m_StatValue = 0;
    private int m_AddedStatValue = 0;
    private Animator m_Animator = null;

    [SerializeField]
    private Text m_StatValueText = null;

    public static PanelButtonStat prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<PanelButtonStat>("Prefabs/Button/PanelButtonStat");
            }
            return m_Prefab;
        }
    }
    public string statId
    {
        get { return m_StatId;  }
        set { m_StatId = value; }
    }
    public int statValue
    {
        get { return m_StatValue;  }
        set
        {
            m_StatValue = value;

            if (m_StatId == "HealthPoints")
            {
                m_StatValueText.text = PlayerData.GetInstance().health + "/" + m_StatValue.ToString();
            }
            else if (m_StatId == "MonstylePoints")
            {
                m_StatValueText.text = PlayerData.GetInstance().specialPoints + "/" + m_StatValue.ToString();
            }
            else
            {
                m_StatValueText.text = m_StatValue.ToString();
            }
        }
    }
    public int addedStatValue
    {
        get { return m_AddedStatValue; }
        set
        {
            statValue -= m_AddedStatValue;

            m_AddedStatValue = value;
            statValue += m_AddedStatValue;

            if (m_AddedStatValue > 0)
            {
                m_TitleText.color = m_StatValueText.color = Color.green;
            }
            else
            {
                m_TitleText.color = m_StatValueText.color = Color.black;
            }
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_Animator = GetComponent<Animator>();
    }

    public void ConfirmAddedStatValue()
    {
        m_AddedStatValue = 0;
        m_TitleText.color = m_StatValueText.color = Color.black;
    }

    public int CancelAddedStatValue()
    {
        int l_AddedValue = addedStatValue;
        addedStatValue = 0;

        //TODO Kostil
        if (m_StatId == "HealthPoints")
        {
            return l_AddedValue / 3;
        }
        else
        {
            return l_AddedValue;
        }
    }

    public void PlayAnim(string p_TriggerId)
    {
        m_Animator.SetTrigger(p_TriggerId);
    }
}
