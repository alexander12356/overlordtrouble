using UnityEngine;

public class MainPanel : Panel
{
    private static MainPanel m_Instance = null;

    public static MainPanel GetInstance()
    {
        return m_Instance;
    }

    [SerializeField]
    private CanvasGroup m_ButtonsCanvasGroup = null;

    public void ButtonDepress(int p_Key)
    {
        switch (p_Key)
        {
            case 1:
                Player.GetInstance().Attack();
                break;
            case 2:
                PanelManager.GetInstance().Show(PanelEnum.SpecialSelect);
                break;
            case 3:
                Application.Quit();
                break;
        }
    }

    private void Awake()
    {
        m_Instance = this;
    }

    // Блокировка кнопок()
    public void Unblock()
    {
        m_ButtonsCanvasGroup.interactable = true;
    }

    public void Block()
    {
        m_ButtonsCanvasGroup.interactable = false;
    }
}