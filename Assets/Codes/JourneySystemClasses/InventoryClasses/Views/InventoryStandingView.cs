using System;
using UnityEngine.UI;

public class InventoryStandingView : InventoryView
{
    private Text m_PlayerCoinsText = null;

    public Text playerCoinsText
    {
        get
        {
            if (m_PlayerCoinsText == null)
            {
                m_PlayerCoinsText = parent.transform.FindChild("Coins").GetComponentInChildren<Text>();
            }
            return m_PlayerCoinsText;
        }
    }

    public int playerCoins
    {
        get
        {
            return PlayerInventory.GetInstance().coins;
        }
        set
        {
            PlayerInventory.GetInstance().coins = value;
            playerCoinsText.text = PlayerInventory.GetInstance().coins.ToString();
        }
    }

    public InventoryStandingView(InventoryPanel p_Parent)
    {
        parent = p_Parent;
    }

    public override void Init()
    {
        playerCoins = PlayerInventory.GetInstance().coins;
        InitGroupData();
        InitGroupButtonList();
        InitEmptySlots();
    }

    public override bool Confrim()
    {
        return false;
    }

    public override bool isNull()
    {
        return false;
    }
}
