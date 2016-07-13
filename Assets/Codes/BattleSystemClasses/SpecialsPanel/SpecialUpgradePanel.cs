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
    private Dictionary<string, Special> m_AddedSpecialDictionary;
    private List<SpecialUpgradeIcon> m_SpecialUpgradeIconList = new List<SpecialUpgradeIcon>();
    private int   m_CurrentKeyCounter = 0;
    private float m_Timer = 0.0f;
    private float m_UpgradeTime = 10.0f;
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

    public void SetSpecials(Dictionary<string, Special> p_AddedSpecials)
    {
        m_AddedSpecialDictionary = p_AddedSpecials;
        CreateIcons(m_AddedSpecialDictionary);
        RandomizeSpecialKeys();
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (moving)
        {
            return;
        }

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
            }
            IncrementCurrentCounter();
        }
    }
    #endregion

    #region Private
    private void CreateIcons(Dictionary<string, Special> p_AddedSpecialDictionary)
    {
        foreach (Special l_Special in p_AddedSpecialDictionary.Values)
        {
            SpecialUpgradeIcon l_SpecialUpgradeIcon = Instantiate(SpecialUpgradeIcon.prefab);
            l_SpecialUpgradeIcon.SetTitle(l_Special.title);
            l_SpecialUpgradeIcon.SetSpecial(l_Special);
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

    private void IncrementCurrentCounter()
    {
        m_SpecialUpgradeIconList[m_CurrentKeyCounter].select = false;

        m_CurrentKeyCounter++;
        if (m_CurrentKeyCounter >= m_SpecialUpgradeIconList.Count)
        {
            m_CurrentKeyCounter = 0;
            RandomizeSpecialKeys();
        }

        m_SpecialUpgradeIconList[m_CurrentKeyCounter].select = true;

        if (m_SpecialUpgradeIconList[m_CurrentKeyCounter].isWrong)
        {
            IncrementCurrentCounter();
        }
    }
    #endregion
}
