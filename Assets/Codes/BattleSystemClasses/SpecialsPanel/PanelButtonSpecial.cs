using UnityEngine;
using UnityEngine.UI;

using System;

public class PanelButtonSpecial : PanelButton
{
    private static PanelButtonSpecial m_Prefab = null;
    private bool  m_Chosen = false;
    private Text  m_SpecialPointsText = null;
    private Image m_Background = null;
    private string m_SkillId = string.Empty;

    public static PanelButtonSpecial prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<PanelButtonSpecial>("Prefabs/Button/PanelButtonSpecial");
            }
            return m_Prefab;
        }
    }
    public string skillId
    {
        get { return m_SkillId;  }
        set
        {
            m_SkillId = value;

            SkillData l_SkillData = SkillDataBase.GetInstance().GetSkillData(m_SkillId);

            string l_SkillName = LocalizationDataBase.GetInstance().GetText("Skill:" + m_SkillId);
            string l_Element = LocalizationDataBase.GetInstance().GetText("Elemental:" + l_SkillData.element);
            float l_Damage = l_SkillData.damage;
            float l_SpecialPoints = l_SkillData.mana;
            description   = l_SkillName + "\n" + LocalizationDataBase.GetInstance().GetText("Element") + ": " + l_Element + "\n" + LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:SelectMonstyle:Damage") + ": "+ l_Damage;
            specialPoints = l_SpecialPoints + "sp";
        }
    }
    public string description
    {
        get { return m_TitleText.text;  }
        set { m_TitleText.text = value; }
    }
    public string specialPoints
    {
        get { return m_SpecialPointsText.text;  }
        set { m_SpecialPointsText.text = value; }
    }
    public bool isChosen
    {
        get { return m_Chosen; }
    }

    public override void Awake()
    {
        base.Awake();

        m_Background = gameObject.transform.FindChild("Background").GetComponent<Image>();
        m_SelectedImage = gameObject.transform.FindChild("SelectImage").GetComponent<Image>();
        m_SpecialPointsText = gameObject.transform.FindChild("SpecialPointsText").GetComponent<Text>();

        m_Background.gameObject.SetActive(true);
    }

    public override void Select(bool p_Value)
    {
        base.Select(p_Value);

        if (m_Chosen)
        {
            
        }
    }

    public void Choose(bool p_Value)
    {
        m_Chosen = p_Value;
        if (p_Value)
        {
            m_SelectedImage.sprite = Resources.Load<Sprite>("Sprites/GUI/BattleSystem/SpecialSelectAndChoosenBackground");
            m_Background.sprite = Resources.Load<Sprite>("Sprites/GUI/BattleSystem/SpecialChoosenBackground");
        }
        else
        {
            m_SelectedImage.sprite = Resources.Load<Sprite>("Sprites/GUI/BattleSystem/SpecialSelectBackground");
            m_Background.sprite = Resources.Load<Sprite>("Sprites/GUI/BattleSystem/SpecialBackground");
        }
    }
}
