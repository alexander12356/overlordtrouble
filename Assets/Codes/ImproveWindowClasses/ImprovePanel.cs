using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections.Generic;

public class ImprovePanel : Panel
{
    #region Variables
    private ButtonList m_ImproveButtonList = null;
    private Animator   m_Animator = null;

    [SerializeField]
    private List<ImproveItem> m_ImproveList = null;

    [SerializeField]
    private Image m_ImproveCompleteImage = null;
    #endregion

    #region Interface
    public override void Awake()
    {
        base.Awake();

        m_ImproveButtonList = GetComponent<ButtonList>();
        m_Animator = GetComponent<Animator>();
        InitButtonActions();

        PanelManager.GetInstance().ShowPanel(this);
        ImproveDataBase.GetInstance();
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_ImproveButtonList.UpdateKey();

        if (Input.GetKey(KeyCode.Escape))
        {
            ReturnToMainMenu();
        }
    }

    public void ImproveComplete()
    {
        TextPanel m_TextPanel = Instantiate(TextPanel.prefab);

        string l_ImproveName = m_ImproveList[m_ImproveButtonList.currentButtonId].improveId;
        string l_ImproveSkillsText = ", вы получили следующие приемы:\n";
        ImproveData l_ImproveData = ImproveDataBase.GetInstance().GetImprove(l_ImproveName);
        l_ImproveSkillsText += l_ImproveData.skills[0].id;
        for (int i = 1; i < l_ImproveData.skills.Count; i++)
        {
            l_ImproveSkillsText += ", " + l_ImproveData.skills[i].id;
        }

        string l_Text = "Вы выбрали класс " + l_ImproveName + l_ImproveSkillsText;
        m_TextPanel.SetText(new List<string>() { l_Text });
        m_TextPanel.AddPopAction(ReturnToMainMenu);

        PanelManager.GetInstance().ShowPanel(m_TextPanel, true);
        Vector3 m_TextPanelLocalPosition = m_TextPanel.myTransform.localPosition;
        m_TextPanelLocalPosition.y = -402;
        m_TextPanel.myTransform.localPosition = m_TextPanelLocalPosition;
    }
    #endregion

    #region Private
    private void InitButtonActions()
    {
        //TODO fix
        for (int i = 0; i < m_ImproveList.Count; i++)
        {
            m_ImproveButtonList.InsertButton(m_ImproveList[i].improveButton);

            if (i != m_ImproveList.Count - 1)
            {
                m_ImproveButtonList[i].AddAction(ShowYesNoPanel);
            }
            else
            {
                m_ImproveButtonList[i].AddAction(ReturnToMainMenu);
            }
            
        }
        
    }

    private void ShowYesNoPanel()
    {
        YesNoPanel l_YesNoPanel = Instantiate(YesNoPanel.prefab);
        l_YesNoPanel.AddYesAction(Select);
        l_YesNoPanel.AddYesAction(l_YesNoPanel.Cancel);
        PanelManager.GetInstance().ShowPanel(l_YesNoPanel, true);
    }

    private void Select()
    {
        m_ImproveList[m_ImproveButtonList.currentButtonId].Select();
        m_ImproveList[m_ImproveButtonList.currentButtonId].AddShowProfileAction(ShowProfile);
        for (int i = 0; i < m_ImproveList.Count; i++)
        {
            if (i != m_ImproveButtonList.currentButtonId)
            {
                m_ImproveList[i].Away();
            }            
        }
        m_ImproveButtonList.isActive = false;
    }

    private void ShowProfile()
    {
        string l_ImproveId = m_ImproveList[m_ImproveButtonList.currentButtonId].improveId;
        m_ImproveCompleteImage.sprite = Resources.Load<Sprite>(ImproveDataBase.GetInstance().GetImprove(l_ImproveId).profileImagePath);
        m_Animator.SetTrigger("Improve");
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("DemoMainScene");
    }
    #endregion
}
