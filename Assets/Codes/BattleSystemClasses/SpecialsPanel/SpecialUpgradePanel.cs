using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpecialUpgradePanel : Panel
{
    #region Variables
    private static SpecialUpgradePanel m_Prefab = null;

    [SerializeField]
    private Transform m_SpecialIconsConteiner = null;

    [SerializeField]
    private Image m_BarImage = null;

    private List<KeyCode> m_SpecialKeys = new List<KeyCode>();
    private List<string> m_AddedSkills;
    private List<SpecialUpgradeIcon> m_SpecialUpgradeIconList = new List<SpecialUpgradeIcon>();
    private int   m_CurrentKeyCounter = 0;
    private float m_Timer = 0.0f;
    private float m_UpgradeTime = 5.0f;
    private BattleEnemy m_Enemy = null;
    private int   m_WrongSpecialCounter = 0;
    private bool  m_IsAllWrong = false;
    #endregion

    #region Interface
    public static SpecialUpgradePanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<SpecialUpgradePanel>("Prefabs/Panels/SpecialUpgradePanel");
            }
            return m_Prefab;
        }
    }

    public void SetSkills(List<string> p_AddedSkills)
    {
        m_AddedSkills = p_AddedSkills;
        CreateIcons(m_AddedSkills);
        RandomizeSpecialKeys();
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (moving)
        {
            return;
        }

        CalculateTime();
        CheckKeyDown();
    }

    public void SetEnemy(BattleEnemy p_Enemy)
    {
        m_Enemy = p_Enemy;
    }
    #endregion

    #region Private
    private void CreateIcons(List<string> p_AddedSkills)
    {
        for (int i = 0; i < p_AddedSkills.Count; i++)
        {
            SkillData l_SkillData = SkillDataBase.GetInstance().GetSkillData(p_AddedSkills[i]);

            SpecialUpgradeIcon l_SpecialUpgradeIcon = Instantiate(SpecialUpgradeIcon.prefab);
            l_SpecialUpgradeIcon.SetTitle(LocalizationDataBase.GetInstance().GetText("Skill:" + l_SkillData.id));
            l_SpecialUpgradeIcon.skillId = p_AddedSkills[i];
            l_SpecialUpgradeIcon.transform.SetParent(m_SpecialIconsConteiner);

            l_SpecialUpgradeIcon.transform.localPosition = Vector3.zero;
            l_SpecialUpgradeIcon.transform.localScale    = Vector3.one;

            m_SpecialUpgradeIconList.Add(l_SpecialUpgradeIcon);
        }
        m_SpecialUpgradeIconList[0].select = true;
    }

    private void RandomizeSpecialKeys()
    {
        int l_KeyCode = 0;
        for (int i = 0; i < m_SpecialUpgradeIconList.Count; i++)
        {
            if (m_SpecialUpgradeIconList[i].isWrong)
            {
                continue;
            }

            l_KeyCode = Random.Range(0, 4);
            KeyCode l_Key = KeyCode.UpArrow;

            switch (l_KeyCode)
            {
                case 0:
                    l_Key = KeyCode.UpArrow;
                    break;
                case 1:
                    l_Key = KeyCode.DownArrow;
                    break;
                case 2:
                    l_Key = KeyCode.LeftArrow;
                    break;
                case 3:
                    l_Key = KeyCode.RightArrow;
                    break;
            }
            m_SpecialUpgradeIconList[i].arrowKey = l_Key;
        }
    }

    private void CheckKeyDown()
    {
        if (m_IsAllWrong)
        {
            return;
        }

        KeyCode l_CurrentKey = m_SpecialUpgradeIconList[m_CurrentKeyCounter].arrowKey;
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(l_CurrentKey))
            {
                m_SpecialUpgradeIconList[m_CurrentKeyCounter].Upgrade();
            }
            else
            {
                m_SpecialUpgradeIconList[m_CurrentKeyCounter].Wrong();
                m_WrongSpecialCounter++;
            }
            IncrementCurrentCounter();
        }
    }

    private void IncrementCurrentCounter()
    {
        m_SpecialUpgradeIconList[m_CurrentKeyCounter].select = false;

        m_CurrentKeyCounter++;
        if (m_CurrentKeyCounter >= m_SpecialUpgradeIconList.Count)
        {
            m_CurrentKeyCounter = 0;
            RandomizeSpecialKeys();

            if (m_WrongSpecialCounter >= m_SpecialUpgradeIconList.Count)
            {
                m_IsAllWrong = true;
                return;
            }
        }

        m_SpecialUpgradeIconList[m_CurrentKeyCounter].select = true;

        if (m_SpecialUpgradeIconList[m_CurrentKeyCounter].isWrong)
        {
            IncrementCurrentCounter();
        }
    }

    private void CalculateTime()
    {
        if (m_Timer < m_UpgradeTime)
        {
            m_Timer += Time.deltaTime;

            Vector3 l_BarImageScale = m_BarImage.transform.localScale;
            l_BarImageScale.x = 1.0f - (m_Timer / m_UpgradeTime);
            m_BarImage.transform.localScale = l_BarImageScale;
        }
        else
        {
            PanelManager.GetInstance().ClosePanel(this);
            BattlePlayer.GetInstance().SpecialAttack(m_Enemy, m_SpecialUpgradeIconList);
            return;
        }
    }
    #endregion
}
