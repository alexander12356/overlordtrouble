using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PanelButtonEncyclopediaEnemy : PanelButton
{
    private static PanelButtonEncyclopediaEnemy m_Prefab;
    private string m_EnemyId = string.Empty;
    private bool m_Chosen = false;

    public static PanelButtonEncyclopediaEnemy prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<PanelButtonEncyclopediaEnemy>("Prefabs/Button/PanelButtonEncyclopediaEnemy");
            }
            return m_Prefab;
        }
    }

    public bool chosen
    {
        get { return m_Chosen; }
        set
        {
            m_Chosen = value;
            if (m_Chosen)
            {
                text.color = Color.yellow;
            }
            else
            {
                text.color = Color.black;
            }
        }
    }

    public string enemyId
    {
        get
        {
            return m_EnemyId;
        }
        set
        {
            m_EnemyId = value;
            title = LocalizationDataBase.GetInstance().GetText("Enemy:" + m_EnemyId);
        }
    }
}
