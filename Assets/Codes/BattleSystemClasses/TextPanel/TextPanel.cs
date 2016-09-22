using System.Collections.Generic;

using UnityEngine;

public class TextPanel : Panel
{
    #region Variables
    private static TextPanel m_Prefab = null;
    private PanelButtonActionHandler m_ButtonAction = null;
    private TextBox m_TextBox;
    #endregion

    #region Interface
    public static TextPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<TextPanel>("Prefabs/Panels/TextPanel");
            }
            return m_Prefab;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_TextBox = GetComponent<TextBox>();
        m_TextBox.AddEndAction(CloseTextPanel);

        AddPushAction(ShowText);
    }

    public void AddButtonAction(PanelButtonActionHandler p_Action)
    {
        m_ButtonAction += p_Action;
    }

    public void RemoveButtonAction(PanelButtonActionHandler p_Action)
    {
        m_ButtonAction -= p_Action;
    }

    public void ButtonAction()
    {
        if (m_ButtonAction != null)
        {
            m_ButtonAction();
        }
    }

    public void SetText(List<string> p_Text)
    {
        m_TextBox.SetText(p_Text);
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (moving)
        {
            return;
        }

        m_TextBox.UpdateTextBox();
    }
    #endregion

    #region Private
    private void ShowText()
    {
        m_TextBox.ShowText();
    }

    private void CloseTextPanel()
    {
        Close();
        ButtonAction();
    }
    #endregion
}
