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
        string l_ItemsName = CalculateItemList(m_ItemList);
        string l_PanelText = LocalizationDataBase.GetInstance().GetText("GUI:Journey:ItemCatched", new string[] { l_ItemsName });

        ShowTextPanel(new List<string>() { l_PanelText });
    }

    private void SetControlPlayer()
    {
        JourneySystem.GetInstance().SetControl(ControlType.Player);
    }

    private void ShowTextPanel(List<string> p_TextList)
    {
        JourneyTextPanel l_TextPanel = Instantiate(JourneyTextPanel.prefab);
        l_TextPanel.SetText(p_TextList);
        l_TextPanel.AddButtonAction(l_TextPanel.Close);
        l_TextPanel.AddPopAction(SetControlPlayer);

        JourneySystem.GetInstance().ShowPanel(l_TextPanel);
        JourneySystem.GetInstance().SetControl(ControlType.Panel);
    }

    private string CalculateItemList(List<AddedItem> p_ItemIds)
    {
        string l_ItemsName = string.Empty;
        for (int i = 0; i < p_ItemIds.Count; i++)
        {
            l_ItemsName += LocalizationDataBase.GetInstance().GetText("Item:" + p_ItemIds[i].id);

            if (p_ItemIds[i].count > 1)
            {
                l_ItemsName += " x" + p_ItemIds[i].count;
            }

            if (i == p_ItemIds.Count - 1)
            {
                l_ItemsName += ".";
            }
            else if (i == p_ItemIds.Count - 2)
            {
                l_ItemsName += " " + LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:And") + " ";
            }
            else
            {
                l_ItemsName += ", ";

            }

            PlayerInventory.GetInstance().AddItem(p_ItemIds[i].id, p_ItemIds[i].count);
        }

        return l_ItemsName;
    }
}
