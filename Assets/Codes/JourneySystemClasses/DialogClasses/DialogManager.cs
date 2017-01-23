using System.Collections.Generic;

using UnityEngine;

public class DialogManager : MonoBehaviour
{
    private static DialogManager m_Instance = null;

    [SerializeField]
    private JourneyPlayer m_JourneyPlayer = null;

    public void Awake()
    {
        m_Instance = this;
    }

    public static DialogManager GetInstance()
    {
        return m_Instance;
    }

    public DialogPanel StartDialog(string p_DialogId, List<ActionStruct> m_AnswerActionList = null)
    {
        DialogData l_DialogData = DialogDataBase.GetInstance().GetDialog(p_DialogId);

        DialogPanel l_DialogPanel = Instantiate(DialogPanel.prefab);
        l_DialogPanel.SetDialog(l_DialogData);
        l_DialogPanel.SetAnswersActions(m_AnswerActionList);
        JourneySystem.GetInstance().ShowPanel(l_DialogPanel, true);

        return l_DialogPanel;
    }

    public void EndDialog()
    {
        if (JourneySystem.GetInstance().currentControlType != ControlType.Cutscene)
        {
            JourneySystem.GetInstance().SetControl(ControlType.Player);
            m_JourneyPlayer.PressDisactiveButtonAction();
        }
    }
}