using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AddedItem
{
    public string id;
    public int count;
}

public class AddItemAction : MonoBehaviour
{
    [SerializeField]
    List<AddedItem> m_ItemList = null;
	
    public void Run()
    {
        string l_ItemsName = string.Empty;
        for (int i = 0; i < m_ItemList.Count; i++)
        {
            l_ItemsName += LocalizationDataBase.GetInstance().GetText("Item:" + m_ItemList[i].id) + " ";

            PlayerInventory.GetInstance().AddItem(m_ItemList[i].id, m_ItemList[i].count);
        }

        string l_Text = LocalizationDataBase.GetInstance().GetText("GUI:Journey:ItemCatched", new string[] { l_ItemsName });

        JourneyTextPanel l_TextPanel = Instantiate(JourneyTextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { l_Text });
        l_TextPanel.AddButtonAction(l_TextPanel.Close);
        l_TextPanel.AddPopAction(SetControlPlayer);

        JourneySystem.GetInstance().ShowPanel(l_TextPanel);
        JourneySystem.GetInstance().SetControl(ControlType.Panel);
    }

    private void SetControlPlayer()
    {
        JourneySystem.GetInstance().SetControl(ControlType.Player);
    }
}
