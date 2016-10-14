using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogPanel : Panel 
{
    private static DialogPanel m_Prefab;

    #region Variables
    private TextBox m_TextBox;
    private Image m_BackgroundImage = null;

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
    }

    public void SetDialog(List<string> p_Phrases)
    {
        m_TextBox.SetText(p_Phrases);
    }

    public void SetAvatar(string p_AvatarPath)
    {
        m_AvatarImage.sprite = Resources.Load<Sprite>(p_AvatarPath);

        if (m_AvatarImage.sprite == null)
        {
            m_AvatarImage.enabled = false;
            m_BackgroundImage.sprite = Resources.Load<Sprite>("Sprites/GUI/Dialogs/dialogue_without_portrait");
        }
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();

        if (moving)
        {
            return;
        }

        m_TextBox.UpdateTextBox();
    }
    #endregion

    #region Private
    private void Start()
    {
        AddPushAction(m_TextBox.ShowText);
        m_TextBox.AddEndAction(DialogClose);
    }

    private void DialogClose()
    {
        Close();
        DialogManager.GetInstance().EndDialog();
    }
    #endregion
}