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

    public DialogPanel StartDialog(string p_DialogId, int p_SubDialogId)
    {
        Dialog l_Dialog = DialogDataBase.GetInstance().GetDialog(p_DialogId);
        SubDialog l_SubDialog = l_Dialog.subDialogs[p_SubDialogId];
        l_SubDialog.Init();

        DialogPanel l_DialogWindow = Instantiate(DialogPanel.prefab);
        l_DialogWindow.SetAvatar(l_Dialog.avatarImagePath);
        l_DialogWindow.SetDialog(l_SubDialog.phrases);
        JourneySystem.GetInstance().ShowPanel(l_DialogWindow, true);

        return l_DialogWindow;
    }

    public DialogQuestionPanel StartQuestionDialog(string p_DialogId, List<ActionStruct> p_ActionList)
    {
        Dialog l_Dialog = DialogDataBase.GetInstance().GetDialog(p_DialogId);
        SubDialog l_SubDialog = l_Dialog.subDialogs[0];

        DialogQuestionPanel l_DialogWindow = Instantiate(DialogQuestionPanel.prefab);
        l_DialogWindow.SetAvatar(l_Dialog.avatarImagePath);
        l_DialogWindow.SetDialog(l_SubDialog.phrases);
        l_DialogWindow.AddAnswers(p_ActionList);
        JourneySystem.GetInstance().ShowPanel(l_DialogWindow, true);

        return l_DialogWindow;
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