using UnityEngine;
using System.Collections.Generic;

public class PlayerEnchancement : Singleton<PlayerEnchancement>
{
    private List<ImproveData> m_Enchancement = new List<ImproveData>();
    private string m_CurrentEnchancement = string.Empty;

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

        m_CurrentEnchancement = p_Id;
    }

    public RuntimeAnimatorController GetAnimatorController()
    {
        string l_Temp = "Sprites/Creations/" + m_CurrentEnchancement + "/" + m_CurrentEnchancement + "Animator";
        return Resources.Load<RuntimeAnimatorController>(l_Temp);
    }

    public Sprite GetBattleAvatar()
    {
        string l_Temp = "Sprites/Creations/" + m_CurrentEnchancement + "/" + "Avatar";
        return Resources.Load<Sprite>(l_Temp);
    }
}
