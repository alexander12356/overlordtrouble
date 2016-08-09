using System;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    private static DialogManager m_Instance = null;

    [SerializeField]
    private DialogWindow m_DialogWindow = null;

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
        m_DialogWindow.gameObject.SetActive(true);
        m_DialogWindow.SetText(DialogSystem.GetInstance().GetDialog(p_DialogId));
        Debug.Log("Dialog started!");
    }

    public void EndDialog()
    {
        Debug.Log("DialogEnded");
    }
}