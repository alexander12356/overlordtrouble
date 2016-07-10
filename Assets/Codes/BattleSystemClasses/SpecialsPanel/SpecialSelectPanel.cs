using UnityEngine;
using System.Collections.Generic;

public class SpecialSelectPanel : Panel
{
    private enum ButtonListType
    {
        SpecialList,
        ConfirmList
    }

    #region Variables
    private static SpecialSelectPanel m_Instance = null;

    [SerializeField]
    private ButtonList m_SpecialButtonList = null;

    [SerializeField]
    private ButtonList m_ConfirmButtonList = null;

    [SerializeField]
    private ButtonList m_AddedSpecialButtonList = null;

    private ButtonListType m_CurrentButtonListType = ButtonListType.SpecialList;
    private List<Special> m_SpecialList = new List<Special>();
    private List<Special> m_AddedSpecialList = new List<Special>();
    #endregion

    #region Interface
    public static SpecialSelectPanel prefab
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = Resources.Load<SpecialSelectPanel>("Prefabs/Panels/SelectSpecialPanel");
            }
            return m_Instance;
        }
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (moving)
        {
            return;
        }

        if (m_CurrentButtonListType == ButtonListType.SpecialList && Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_CurrentButtonListType = ButtonListType.ConfirmList;
            m_SpecialButtonList.isActive = false;
            m_ConfirmButtonList.isActive = true;
        }
        if (m_CurrentButtonListType == ButtonListType.ConfirmList && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_CurrentButtonListType = ButtonListType.SpecialList;
            m_SpecialButtonList.isActive = true;
            m_ConfirmButtonList.isActive = false;
        }

        switch (m_CurrentButtonListType)
        {
            case ButtonListType.SpecialList:
                m_SpecialButtonList.UpdateKey();
                break;
            case ButtonListType.ConfirmList:
                m_ConfirmButtonList.UpdateKey();
                break;
        }
    }
    #endregion

    #region Private
    private void Awake()
    {
        m_ConfirmButtonList.isActive = false;
        m_AddedSpecialButtonList.isActive = false;

        m_ConfirmButtonList[1].AddAction(ReturnToMain);

        InitSpecialList();
        InitSpecialButtons();
    }

    private void ReturnToMain()
    {
        PanelManager.GetInstance().ClosePanel(this);
    }

    private void AddSpecial()
    {
        int l_AddedSpecialId = m_SpecialButtonList.currentButtonId;

        if (m_AddedSpecialList.Contains(m_SpecialList[l_AddedSpecialId]))
        {
            m_AddedSpecialList.RemoveAt(l_AddedSpecialId);
            m_AddedSpecialButtonList.RemoveButton(l_AddedSpecialId);
        }
        else
        {
            m_AddedSpecialList.Add(m_SpecialList[l_AddedSpecialId]);

            PanelButton l_PanelButton = Instantiate(PanelButton.prefab);
            l_PanelButton.title = "SP" + l_AddedSpecialId;
            m_AddedSpecialButtonList.AddButton(l_PanelButton);
        }
    }

    //TODO Kostil
    private void InitSpecialList()
    {
        for (int  i = 0; i < 4; i++)
        {
            Special l_Special = new Special("SP" + i, (Special.Element)i);
            m_SpecialList.Add(l_Special);
        }
    }

    //TODO Kostil
    private void InitSpecialButtons()
    {
        for (int i = 0; i < m_SpecialList.Count; i++)
        {
            m_SpecialButtonList[i].AddAction(AddSpecial);
            m_SpecialButtonList[i].title = m_SpecialList[i].title;
        }
    }
    #endregion
}
