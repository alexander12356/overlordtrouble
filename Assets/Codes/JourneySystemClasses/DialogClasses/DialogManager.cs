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

    public void StartDialog(string p_DialogId)
    {
        DialogPanel l_DialogWindow = Instantiate(DialogPanel.prefab);
        l_DialogWindow.SetDialog(DialogDataBase.GetInstance().GetDialog(p_DialogId));

        JourneySystem.GetInstance().ShowPanel(l_DialogWindow, true);
        l_DialogWindow.myTransform.localPosition = new Vector3(0.0f, -457.0f, 0);
    }

    public void EndDialog()
    {
        JourneySystem.GetInstance().SetControl(ControlType.Player);
        m_JourneyPlayer.DisactiveButtonAction();
    }
}