﻿using UnityEngine;
using UnityEngine.UI;

public class SpecialStatus : MonoBehaviour
{
    private Text m_Text = null;
    private Image m_Image = null;

	public void Awake ()
    {
        m_Text  = GetComponentInChildren<Text>();
        m_Image = GetComponentInChildren<Image>();
	}

    public void Selected(bool p_Value)
    {
        if (p_Value)
        {
            m_Image.color = Color.green;
            m_Text.text = "Выбрано";
        }
        else
        {
            m_Image.color = Color.grey;
            m_Text.text = "Не выбрано";
        }
    }
}
