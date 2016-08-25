using UnityEngine;
using System.Collections.Generic;

public class PlayerEnchancement : Singleton<PlayerEnchancement>
{
    private List<ImproveData> m_Enchancement = new List<ImproveData>();

    public List<ImproveData> enchancement
    {
        get { return m_Enchancement; }
    }

    public PlayerEnchancement()
    {
        AddEnchancement("DewElemental");
    }

    public void AddEnchancement(string p_Id)
    {
        ImproveData l_ImproveData = ImproveDataBase.GetInstance().GetImprove(p_Id);
        PlayerSkills.GetInstance().AddSkills(l_ImproveData.skills);
    }
}
