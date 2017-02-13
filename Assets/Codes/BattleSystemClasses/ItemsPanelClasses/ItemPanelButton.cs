using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ItemPanelButton : PanelButton
{
    private static ItemPanelButton m_Prefab;
    private Image m_Background = null;
    private Text m_DescriptionText = null;
    private bool m_Chosen = false;
    private string m_ItemId = string.Empty;

    public static ItemPanelButton prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<ItemPanelButton>("Prefabs/Button/BattleSystem/ItemPanelButton");
            }
            return m_Prefab;
        }
    }
    public bool isChosen
    {
        get { return m_Chosen; }
    }
    public string itemId
    {
        get { return m_ItemId; }
        set { m_ItemId = value; }
    }

    public string description
    {
        get { return m_DescriptionText.text; }
        set { m_DescriptionText.text = value; }
    }

    public override void Awake()
    {
        base.Awake();

        m_Background = gameObject.transform.FindChild("Background").GetComponent<Image>();
        m_SelectedImage = gameObject.transform.FindChild("SelectImage").GetComponent<Image>();
        m_DescriptionText = gameObject.transform.FindChild("SpecialPointsText").GetComponent<Text>();

        m_Background.gameObject.SetActive(true);
    }

    public void Choose(bool p_Value)
    {
        m_Chosen = p_Value;
        if (p_Value)
        {
            m_SelectedImage.sprite = Resources.Load<Sprite>("Sprites/GUI/BattleSystem/CreateMonstyle/SpecialSelectAndChoosenBackground");
            m_Background.sprite = Resources.Load<Sprite>("Sprites/GUI/BattleSystem/CreateMonstyle/SpecialChoosenBackground");
        }
        else
        {
            m_SelectedImage.sprite = Resources.Load<Sprite>("Sprites/GUI/BattleSystem/CreateMonstyle/SpecialSelectBackground");
            m_Background.sprite = Resources.Load<Sprite>("Sprites/GUI/BattleSystem/CreateMonstyle/SpecialBackground");
        }
    }
}
