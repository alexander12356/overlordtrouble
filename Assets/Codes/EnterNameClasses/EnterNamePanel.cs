using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnterNamePanel : Panel
{
    private static EnterNamePanel m_Prefab;
    private InputField m_InputField = null;
    private ButtonList m_ButtonList = null;
    private bool m_IsButtonFocused = false;

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

        SaveDataBase.GetInstance().Parse();
    }

    public void Start()
    {
        m_InputField.Select();
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (moving)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_InputField.DeactivateInputField();
            m_ButtonList.isActive = true;
            m_IsButtonFocused = true;

            if (m_InputField.text == "")
            {
                m_InputField.text = LocalizationDataBase.GetInstance().GetText("GUI:EnterName:Standart");
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_InputField.ActivateInputField();
            m_InputField.Select();
            m_ButtonList.isActive = false;
            m_IsButtonFocused = false;
        }

        if(!m_IsButtonFocused && !m_InputField.isFocused)
        {
            EventSystem.current.SetSelectedGameObject(m_InputField.gameObject, null);
            m_InputField.OnPointerClick(new PointerEventData(EventSystem.current));
        }

        m_ButtonList.UpdateKey();
    }

    private void ShowConfirmPanel()
    {
        if (SaveDataBase.GetInstance().HasSave(m_InputField.text))
        {
            WarningPanel l_WarningPanel = Instantiate(WarningPanel.prefab);
            l_WarningPanel.SetText(LocalizationDataBase.GetInstance().GetText("GUI:EnterName:Warning"));

            EnterNameSystem.GetInstance().ShowPanel(l_WarningPanel, true);
        }
        else
        {
            YesNoPanel l_YesNoPanel = Instantiate(YesNoPanel.prefab);
            l_YesNoPanel.AddYesAction(ConfirmPanel);
            l_YesNoPanel.SetText(LocalizationDataBase.GetInstance().GetText("GUI:EnterName:AreYouShure"));

            EnterNameSystem.GetInstance().ShowPanel(l_YesNoPanel, true);
        }
    }

    private void ConfirmPanel()
    {
        PlayerData.GetInstance().NewGameDataInit();
        PlayerInventory.GetInstance().NewGameDataInit();
        GameManager.GetInstance().isTesting = false;
        PlayerData.GetInstance().SetPlayerName(m_InputField.text);
        EnterNameSystem.GetInstance().StartGame();
    }
}
