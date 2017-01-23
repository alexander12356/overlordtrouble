using UnityEngine;
using UnityEngine.UI;

public class WarningPanel : Panel
{
    private static WarningPanel m_Prefab;
    private ButtonList m_ButtonList = null;
    private Text m_Text = null;

    public static WarningPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<WarningPanel>("Prefabs/Panels/WarningPanel");
            }

            return m_Prefab;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_ButtonList = GetComponent<ButtonList>();
        m_Text = GetComponentInChildren<Text>();
    }

    public void Start()
    {
        m_ButtonList[0].AddAction(Close);
        m_ButtonList[0].title = LocalizationDataBase.GetInstance().GetText("GUI:Ok");
    }

    public void SetText(string p_Text)
    {
        m_Text.text = p_Text;
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        m_ButtonList.UpdateKey();
    }
}
