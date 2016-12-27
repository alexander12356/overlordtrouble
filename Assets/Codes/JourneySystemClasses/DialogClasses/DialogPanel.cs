using UnityEngine;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine.Events;

public class DialogPanel : Panel 
{
    private static DialogPanel m_Prefab;

    #region Variables
    protected DialogData m_DialogData = null;
    protected TextBox m_TextBox = null;
    private Image m_BackgroundImage = null;
    private DialogNode m_CurrentDialogNode;
    private ButtonList m_ButtonList = null;
    private bool m_IsShowAnswers = false;
    private Dictionary<string, UnityEvent> m_AnswerActions = new Dictionary<string, UnityEvent>();
    private float m_ButtonVerticalSize;

    [SerializeField]
    private Image m_AvatarImage = null;
    #endregion

    #region Interface
    public static DialogPanel prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<DialogPanel>("Prefabs/Windows/DialogPanel");
            }
            return m_Prefab;
        }
    }

    public override void Awake()
    {
        base.Awake();

        m_TextBox = GetComponentInChildren<TextBox>();
        m_BackgroundImage = GetComponentInChildren<Image>();
        m_ButtonList = GetComponentInChildren<ButtonList>(true);
        m_ButtonList.isActive = false;
    }

    public virtual void Start()
    {
        AddPushAction(m_TextBox.ShowText);
        m_TextBox.AddEndAction(CheckDialogEnd);

        m_CurrentDialogNode = m_DialogData.GetStartDialogNode();
        m_TextBox.SetText(m_CurrentDialogNode.textList);
        m_ButtonVerticalSize = m_ButtonList.rectTransform.sizeDelta.y;
    }

    public void SetDialog(DialogData p_DialogData)
    {
        m_DialogData = p_DialogData;

        SetAvatar(p_DialogData.avatarImagePath);
    }

    public void SetAnswersActions(List<ActionStruct> p_AnswerList)
    {
        for (int i = 0; i < p_AnswerList.Count; i++)
        {
            m_AnswerActions.Add(p_AnswerList[i].id, p_AnswerList[i].actionEvent);
        }
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

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
    #endregion

    #region Private
    private void CheckDialogEnd()
    {
        if (m_CurrentDialogNode.questionList.Count == 0)
        {
            DialogClose();
            return;
        }

        ShowAnswerList();
    }

    private void DialogClose()
    {
        Close();
        DialogManager.GetInstance().EndDialog();
    }

    private void SetAvatar(string p_AvatarPath)
    {
        m_AvatarImage.sprite = Resources.Load<Sprite>(p_AvatarPath);

        if (m_AvatarImage.sprite == null)
        {
            m_AvatarImage.enabled = false;
            m_BackgroundImage.sprite = Resources.Load<Sprite>("Sprites/GUI/Dialogs/dialogue_without_portrait");
        }
    }

    private void ShowAnswerList()
    {
        m_ButtonList.gameObject.SetActive(true);
        m_ButtonList.isActive = true;
        m_IsShowAnswers = true;

        for (int i = 0; i < m_CurrentDialogNode.questionList.Count; i++)
        {
            string l_AnswerId = m_CurrentDialogNode.questionList[i];
            DialogAnswerButton l_Button = Instantiate(DialogAnswerButton.prefab);
            l_Button.title = LocalizationDataBase.GetInstance().GetText("Dialog:" + m_DialogData.id + ":" + l_AnswerId);
            l_Button.answerId = l_AnswerId;
            l_Button.AddAction(ChooseAnswer);
            m_ButtonList.AddButton(l_Button);

            if (!m_AnswerActions.ContainsKey(l_AnswerId))
            {
                continue;
            }
            l_Button.AddAction(m_AnswerActions[l_AnswerId]);
        }

        Vector2 l_SizeDelta = m_ButtonList.rectTransform.sizeDelta;
        l_SizeDelta.y = m_ButtonVerticalSize * m_CurrentDialogNode.questionList.Count;
        m_ButtonList.rectTransform.sizeDelta = l_SizeDelta;
    }

    private void ChooseAnswer()
    {
        m_ButtonList.gameObject.SetActive(false);
        m_ButtonList.isActive = false;
        m_IsShowAnswers = false;

        string l_AnswerId = (m_ButtonList.currentButton as DialogAnswerButton).answerId;
        m_ButtonList.Clear();
        if (!m_DialogData.HasDialogNode(l_AnswerId))
        {
            DialogClose();
            return;
        }

        m_CurrentDialogNode = m_DialogData.GetDialogNode(l_AnswerId);
        m_TextBox.SetText(m_CurrentDialogNode.textList);
        m_TextBox.ShowText();
    }
    #endregion
}