using UnityEngine;

public class PlayerEnchancement
{
    private string m_CurrentEnchancement = string.Empty;

    public PlayerEnchancement()
    {
    }

    public void UpgradeClass(string p_Id)
    {
        ImproveData l_ImproveData = ImproveDataBase.GetInstance().GetImprove(p_Id);
        PlayerData.GetInstance().AddSkills(l_ImproveData.skills);

        m_CurrentEnchancement = p_Id;
    }

    public void SetEnchancementId(string p_Id)
    {
        m_CurrentEnchancement = p_Id;
    }

    public string GetCurrentEnchancement()
    {
        return m_CurrentEnchancement;
    }

    public Sprite GetProfileAvatar()
    {
        string l_AvatarPath = "Sprites/Creations/" + m_CurrentEnchancement + "/Profile";
        return Resources.Load<Sprite>(l_AvatarPath);
    }

    public RuntimeAnimatorController GetAnimatorController()
    {
        string l_Temp = "Sprites/Creations/" + m_CurrentEnchancement + "/" + m_CurrentEnchancement + "Animator";
        return Resources.Load<RuntimeAnimatorController>(l_Temp);
    }

    public Sprite GetBattleAvatar()
    {
        string l_Temp = "Sprites/Creations/" + m_CurrentEnchancement + "/Avatar";
        return Resources.Load<Sprite>(l_Temp);
    }

    public void Clear()
    {
        m_CurrentEnchancement = string.Empty;
    }
}
