using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogQuestionPanel : DialogPanel
{
    private static DialogQuestionPanel m_Prefab;
    private ButtonList m_ButtonList = null;
    private bool m_IsShowAnswers = false;

    public static DialogQuestionPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<DialogQuestionPanel>("Prefabs/Windows/DialogQuestionPanel");
            }
            return m_Prefab;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_ButtonList = GetComponentInChildren<ButtonList>();
        m_ButtonList.isActive = false;
    }

    public override void Start()
    {
        AddPushAction(m_TextBox.ShowText);
        m_TextBox.AddEndAction(ShowAnswers);
    }

    public void AddAnswers(List<ActionStruct> p_AnswerList)
    {
        float m_ButtonVerticalSize = m_ButtonList.rectTransform.sizeDelta.y;

        for (int i = 0; i < p_AnswerList.Count; i++)
        {
            PanelButton l_Button = Instantiate(PanelButton.prefab);
            l_Button.AddAction(p_AnswerList[i].actionEvent);
            l_Button.title = LocalizationDataBase.GetInstance().GetText(p_AnswerList[i].id);

            m_ButtonList.AddButton(l_Button);
        }

        Vector2 l_SizeDelta = m_ButtonList.rectTransform.sizeDelta;
        l_SizeDelta.y = m_ButtonVerticalSize * p_AnswerList.Count;
        m_ButtonList.rectTransform.sizeDelta = l_SizeDelta;

        m_ButtonList.rectTransform.sizeDelta = l_SizeDelta;
        m_ButtonList.gameObject.SetActive(false);
    }

    public override void UpdatePanel()
    {
        if (moving)
        {
            return;
        }

        if (m_IsShowAnswers)
        {
            m_ButtonList.UpdateKey();
        }
        else
        {
            m_TextBox.UpdateTextBox();
        }
    }

    public void DialogClose()
    {
        Close();
        DialogManager.GetInstance().EndDialog();
    }

    private void ShowAnswers()
    {
        m_ButtonList.gameObject.SetActive(true);
        m_ButtonList.isActive = true;
        m_IsShowAnswers = true;
        m_TextBox.isActiveButton = false;
    }
}
