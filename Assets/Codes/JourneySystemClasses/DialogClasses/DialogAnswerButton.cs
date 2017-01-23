using UnityEngine;

public class DialogAnswerButton : PanelButton
{
    private static DialogAnswerButton m_Prefab = null;
    private string m_AnswerId = string.Empty;

    public static DialogAnswerButton prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<DialogAnswerButton>("Prefabs/Button/DialogAnswerButton");
            }
            return m_Prefab;
        }
    }
    public string answerId
    {
        get { return m_AnswerId; }
        set { m_AnswerId = value; }
    }
}
