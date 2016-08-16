using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections.Generic;

public class ImproveListPanel : Panel
{
    #region Variables
    private ButtonList m_ImproveButtonList = null;
    private Animator   m_Animator = null;

    [SerializeField]
    private List<Improve> m_ImproveList = null;

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
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_ImproveButtonList.UpdateKey();

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("DemoMainScene");
        }
    }

    public void ImproveComplete()
    {
        TextPanel m_TextPanel = Instantiate(TextPanel.prefab);

        m_TextPanel.SetText("Вы выбрали класс " + m_ImproveList[m_ImproveButtonList.currentButtonId].improveId);

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
            m_ImproveButtonList[i].AddAction(ShowYesNoPanel);
        }
    }

    private void ShowYesNoPanel()
    {
        YesNoPanel m_YesNoPanel = Instantiate(YesNoPanel.prefab);
        m_YesNoPanel.AddYesAction(Select);
        m_YesNoPanel.AddYesAction(m_YesNoPanel.Cancel);
        PanelManager.GetInstance().ShowPanel(m_YesNoPanel, true);
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
        m_ImproveCompleteImage.sprite = Resources.Load<Sprite>("Sprites/Creations/" + m_ImproveList[m_ImproveButtonList.currentButtonId].improveId + "/Profile");
        m_Animator.SetTrigger("Improve");
    }
    #endregion
}
