using UnityEngine;
using System.Collections.Generic;

public class SpecialSelectPanel : Panel
{
    [SerializeField]
    private Transform m_SelectedSpecialListTransform = null;

    [SerializeField]
    private SpecialIcon m_SpecialIconPrefab = null;

    private static SpecialSelectPanel m_Instance = null;
    private Dictionary<string, SpecialIcon> m_SpecialIcons = new Dictionary<string, SpecialIcon>();
    private List<Special> m_SpecialList = new List<Special>();

    private void Awake()
    {
        m_Instance = this;
    }

    static public SpecialSelectPanel GetInstance()
    {
        return m_Instance;
    }

    public void AddSpecial(string p_Id)
    {
        if (m_SpecialIcons.ContainsKey(p_Id))
        {
            Destroy(m_SpecialIcons[p_Id].gameObject);
            m_SpecialIcons.Remove(p_Id);
        }
        else if (m_SpecialIcons.Count < 4)
        {
            SpecialIcon l_NewSpecialIcon = Instantiate(m_SpecialIconPrefab);
            l_NewSpecialIcon.transform.SetParent(m_SelectedSpecialListTransform);
            l_NewSpecialIcon.id = p_Id;
            l_NewSpecialIcon.transform.localScale = Vector3.one;

            m_SpecialIcons.Add(p_Id, l_NewSpecialIcon);
        }
    }

    public void ButtonDepress(int p_Key)
    {
        switch (p_Key)
        {
            case 0:
                //PanelManager.GetInstance().Show(PanelEnum.SpecialUpdate);

                foreach (string p_Id in m_SpecialIcons.Keys)
                {
                    m_SpecialList.Add(new Special(p_Id, Special.Element.Fire));
                }
                SpecialUpgradePanel.GetInstance().SetSpecials(m_SpecialList);
                
                break;
            case 1:
                //PanelManager.GetInstance().Show(PanelEnum.Main);
                break;
        }
    }
}
