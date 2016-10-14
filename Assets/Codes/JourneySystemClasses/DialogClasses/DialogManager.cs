using System;
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

    public void StartDialog(string p_DialogId, int p_SubDialogId)
    {
        Dialog l_Dialog = DialogDataBase.GetInstance().GetDialog(p_DialogId);
        SubDialog l_SubDialog = l_Dialog.subDialogs[p_SubDialogId];

        DialogPanel l_DialogWindow = Instantiate(DialogPanel.prefab);
        l_DialogWindow.SetAvatar(l_Dialog.avatarImagePath);
        l_DialogWindow.SetDialog(l_SubDialog.phrases);
        JourneySystem.GetInstance().ShowPanel(l_DialogWindow, true);
    }

    public void EndDialog()
    {
        JourneySystem.GetInstance().SetControl(ControlType.Player);
        m_JourneyPlayer.PressDisactiveButtonAction();
    }
}