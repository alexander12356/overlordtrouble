using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UsingMonstylePanel : Panel
{
    #region Variables
    private static UsingMonstylePanel m_Prefab = null;

    [SerializeField]
    private Transform m_SpecialIconsConteiner = null;

    [SerializeField]
    private Image m_BarImage = null;
    
    private List<string> m_AddedMonstyles;
    private List<SpecialUpgradeIcon> m_SpecialUpgradeIconList = new List<SpecialUpgradeIcon>();
    private int   m_CurrentKeyCounter = 0;
    private float m_Timer = 0.0f;
    private float m_UpgradeTime = 5.0f;
    private BattleEnemy m_Enemy = null;
    private int   m_WrongSpecialCounter = 0;
    private bool  m_IsAllWrong = false;
    #endregion

    #region Interface
    public static UsingMonstylePanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<UsingMonstylePanel>("Prefabs/Panels/UsingMonstylePanel");
            }
            return m_Prefab;
        }
    }

    public void SetSkills(List<string> p_AddedMonstyle)
    {
        m_AddedMonstyles = p_AddedMonstyle;
        CreateIcons(m_AddedMonstyles);
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
            SpecialData l_MonstyleData = SpecialDataBase.GetInstance().GetSpecialData(p_AddedSkills[i]);

            SpecialUpgradeIcon l_MonstyleUpgradeIcon = Instantiate(SpecialUpgradeIcon.prefab);
            l_MonstyleUpgradeIcon.SetTitle(LocalizationDataBase.GetInstance().GetText("Skill:" + l_MonstyleData.id));
            l_MonstyleUpgradeIcon.specialId = p_AddedSkills[i];
            l_MonstyleUpgradeIcon.transform.SetParent(m_SpecialIconsConteiner);

            l_MonstyleUpgradeIcon.transform.localPosition = Vector3.zero;
            l_MonstyleUpgradeIcon.transform.localScale    = Vector3.one;

            m_SpecialUpgradeIconList.Add(l_MonstyleUpgradeIcon);
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
        if (IsArrowDown())
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
            Close();
            AddPopAction(SpecialAttack);
            return;
        }
    }

    private void SpecialAttack()
    {
        BattlePlayer.GetInstance().SpecialAttack(m_Enemy, m_SpecialUpgradeIconList);
    }

    private bool IsArrowDown()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            return true;
        }
        return false;
    }
    #endregion
}
