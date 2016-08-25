using UnityEngine;
using UnityEngine.UI;

public class YesNoPanel : Panel
{
    #region Variables
    private static YesNoPanel m_Prefab = null;
    private ButtonList m_ButtonList = null;
    private PanelActionHandler m_YesAction;
    private PanelActionHandler m_NoAction;
    private Text m_DescriptionText = null;
    #endregion

    #region Interface
    public static YesNoPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<YesNoPanel>("Prefabs/Panels/YesNoPanel");
            }
            return m_Prefab;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_DescriptionText = GetComponentInChildren<Text>();

        m_ButtonList = GetComponent<ButtonList>();
        m_ButtonList[0].AddAction(YesAction);
        m_ButtonList[1].AddAction(Cancel);
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_ButtonList.UpdateKey();
    }

    public void Cancel()
    {
        NoAction();
        PanelManager.GetInstance().ClosePanel(this);
    }

    public void AddYesAction(PanelActionHandler p_Action)
    {
        m_YesAction += p_Action;
    }

    public void AddNoAction(PanelActionHandler p_Action)
    {
        m_NoAction += p_Action;
    }

    public void SetText(string p_Text)
    {
        m_DescriptionText.text = p_Text;
    }
    #endregion

    #region Private
    private void YesAction()
    {
        if (m_YesAction != null)
        {
            m_YesAction();
        }
    }

    private void NoAction()
    {
        if (m_NoAction != null)
        {
            m_NoAction();
        }
    }
    #endregion
}
