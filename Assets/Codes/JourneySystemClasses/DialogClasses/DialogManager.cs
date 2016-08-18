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
        DialogWindow l_DialogWindow = Instantiate(DialogWindow.prefab);
        l_DialogWindow.transform.SetParent(transform);
        l_DialogWindow.transform.localScale = Vector3.one;

        l_DialogWindow.gameObject.SetActive(true);
        l_DialogWindow.SetDialog(DialogSystem.GetInstance().GetDialog(p_DialogId));
        l_DialogWindow.Show();
    }

    public void EndDialog()
    {
        m_JourneyPlayer.DisactiveButtonAction();
        JourneySystem.GetInstance().SetControl(ControlType.Player);
    }
}