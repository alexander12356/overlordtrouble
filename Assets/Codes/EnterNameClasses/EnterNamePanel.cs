using UnityEngine;
using UnityEngine.UI;

public class EnterNamePanel : Panel
{
    private static EnterNamePanel m_Prefab;
    private InputField m_InputField = null;
    private ButtonList m_ButtonList = null;

    public static EnterNamePanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<EnterNamePanel>("Prefabs/Panels/EnterNamePanel");
            }
            return m_Prefab;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_InputField = GetComponentInChildren<InputField>();
        m_ButtonList = GetComponentInChildren<ButtonList>();

        m_ButtonList[0].AddAction(ShowConfirmPanel);
    }

    public void Start()
    {
        m_InputField.Select();
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_InputField.DeactivateInputField();
            m_ButtonList.isActive = true;

            if (m_InputField.text == "")
            {
                m_InputField.text = LocalizationDataBase.GetInstance().GetText("GUI:EnterName:Standart");
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_InputField.ActivateInputField();
            m_InputField.Select();
            m_ButtonList.isActive = false;
        }

        m_ButtonList.UpdateKey();
    }

    private void ShowConfirmPanel()
    {
        YesNoPanel l_YesNoPanel = Instantiate(YesNoPanel.prefab);
        l_YesNoPanel.AddYesAction(ConfirmPanel);
        l_YesNoPanel.SetText(LocalizationDataBase.GetInstance().GetText("GUI:EnterName:AreYouShure"));

        EnterNameSystem.GetInstance().ShowPanel(l_YesNoPanel, true);
    }

    private void ConfirmPanel()
    {
        PlayerData.GetInstance().ResetData();
        GameManager.GetInstance().isTesting = false;
        PlayerData.GetInstance().SetPlayerName(m_InputField.text);
        EnterNameSystem.GetInstance().StartGame();
    }
}
