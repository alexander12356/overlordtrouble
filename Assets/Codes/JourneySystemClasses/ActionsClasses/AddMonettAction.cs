using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMonettAction : MonoBehaviour
{
    [SerializeField]
    private int m_MonettCount = 0;
	
    public void Run()
    {
        PlayerInventory.GetInstance().coins += m_MonettCount;

        string l_Text = LocalizationDataBase.GetInstance().GetText("GUI:Journey:AddMonett", new string[] { m_MonettCount.ToString() });

        JourneyTextPanel l_TextPanel = Instantiate(JourneyTextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { l_Text });
        l_TextPanel.AddPopAction(SetControlPlayer);
        l_TextPanel.AddButtonAction(l_TextPanel.Close);

        JourneySystem.GetInstance().ShowPanel(l_TextPanel);

        JourneySystem.GetInstance().SetControl(ControlType.Panel);
    }

    private void SetControlPlayer()
    {
        JourneySystem.GetInstance().SetControl(ControlType.Player);
    }
}
